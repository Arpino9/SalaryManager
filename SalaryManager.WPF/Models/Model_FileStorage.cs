using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.Domain.Modules.Logics;
using SalaryManager.Domain.Repositories;
using SalaryManager.Domain.StaticValues;
using SalaryManager.Domain.ValueObjects;
using SalaryManager.Infrastructure.Interface;
using SalaryManager.Infrastructure.PDF;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.ViewModels;
using SalaryManager.WPF.Window;
using System;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Message = SalaryManager.Domain.Modules.Logics.Message;

namespace SalaryManager.WPF.Models
{
    public sealed class Model_FileStorage : IMaster
    {

        #region Get Instance

        private static Model_FileStorage model = null;

        public static Model_FileStorage GetInstance(IFileStorageRepository repository)
        {
            if (model == null)
            {
                model = new Model_FileStorage(repository);
            }

            return model;
        }

        #endregion

        /// <summary> Repository </summary>
        private IFileStorageRepository _repository;

        public Model_FileStorage(IFileStorageRepository repository)
        {
            _repository  = repository;
        }

        /// <summary> PDF変換 </summary>
        private PDFConverter PDFConverter { get; set; } = new PDFConverter();

        /// <summary> ViewModel - 添付ファイル管理 </summary>
        public ViewModel_FileStorage ViewModel { get; set; }

        /// <summary> ViewModel - イメージビューアー </summary>
        public ViewModel_ImageViewer ViewModel_ImageViewer { get; set; }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            FileStorages.Create(_repository);

            this.ViewModel.Entities = FileStorages.FetchByDescending();

            this.Reflesh_ListView();

            this.ViewModel.AttachedFile_SelectedIndex = -1;
            this.Clear_InputForm();
        }

        /// <summary>
        /// Enable - 操作ボタン
        /// </summary>
        private void EnableControlButton()
        {
            var selected = this.ViewModel.AttachedFile_ItemSource.Any()
                        && this.ViewModel.AttachedFile_SelectedIndex >= 0;

            // 更新ボタン
            this.ViewModel.Update_IsEnabled = selected;
            // 削除ボタン
            this.ViewModel.Delete_IsEnabled = selected;
        }

        #region ファイルを開く

        /// <summary>
        /// ファイルを開く - SelectionChanged
        /// </summary>
        internal void AttachedFile_SelectionChanged()
        {
            if (this.ViewModel.AttachedFile_SelectedIndex == -1)
            {
                return;
            }

            this.EnableControlButton();

            if (!this.ViewModel.AttachedFile_ItemSource.Any())
            {
                return;
            }

            var entity = this.ViewModel.AttachedFile_ItemSource[this.ViewModel.AttachedFile_SelectedIndex];

            // サムネイル
            this.ViewModel.FileImage_Image = ImageUtils.ConvertBytesToImage(entity.Image);
            // 画像を拡大表示するボタン
            this.ViewModel.OpenImageViewer_IsEnabled = true;

            // タイトル
            this.ViewModel.Title_IsEnabled = true;
            this.ViewModel.Title_Text      = entity.Title;
            // ファイル名
            this.ViewModel.FileName_Text = entity.FileName;
            // 備考
            this.ViewModel.Remarks_IsEnabled = true;
            this.ViewModel.Remarks_Text      = entity.Remarks;

            // 追加
            this.ViewModel.Update_IsEnabled = true;
            // 削除
            this.ViewModel.Delete_IsEnabled = true;
        }

        /// <summary>
        /// ファイルを開く
        /// </summary>
        internal void OpenFile()
        {
            //TODO: PDF, 画像ファイルに限定する
            var filter = "すべてのファイル(*.*)|*.*";
            var path = DialogUtils.SelectFile(string.Empty, filter);

            if (string.IsNullOrEmpty(path)) 
            {
                // キャンセル
                return;
            }

            var extension = new FileExtensionValue(ImageUtils.ExtractFileExtension(path));

            if (extension.IsPDF) 
            {
                // PDF
                if (this.ConvertPDFToPNG(path))
                {
                    // 追加ボタン
                    this.ViewModel.Add_IsEnabled = true;
                }
                
                return;
            }

            // サムネイル
            this.ViewModel.ByteImage       = ImageUtils.ConvertPathToBytes(path, extension.ImageFormat);
            this.ViewModel.FileImage_Image = ImageUtils.ConvertPathToImage(path, extension.ImageFormat);
            // 画像を拡大表示するボタン
            this.ViewModel.OpenImageViewer_IsEnabled = true;

            // タイトル
            this.ViewModel.Title_IsEnabled = true;
            this.ViewModel.Title_Text      = ImageUtils.ExtractFileNameWithoutExtension(path);
            // ファイル名
            this.ViewModel.FileName_Text   = ImageUtils.ExtractFileNameWithExtension(path);
            // 備考
            this.ViewModel.Remarks_IsEnabled = true;
            // 追加ボタン
            this.ViewModel.Add_IsEnabled   = true;
        }

        /// <summary>
        /// PDFをPNGに変換する
        /// </summary>
        /// <param name="path">ファイルパス</param>
        /// <returns>追加可否</returns>
        /// <remarks>
        /// 一時的にPNGを出力し、リスト追加後に削除している。
        /// </remarks>
        private bool ConvertPDFToPNG(string path)
        {
            var pngPaths = this.PDFConverter.ConvertPDFIntoImage(path);

            if (pngPaths.Count == 1)
            {
                // 1枚
                // タイトル
                this.ViewModel.Title_Text    = ImageUtils.ExtractFileNameWithoutExtension(pngPaths.First());
                // ファイル名
                this.ViewModel.FileName_Text = ImageUtils.ExtractFileNameWithExtension(pngPaths.First());
                // 表示する画像
                this.ViewModel.ByteImage     = ImageUtils.ConvertPathToBytes(pngPaths.First(), ImageFormat.Png);

                this.AddFile();

                File.Delete(pngPaths.First());
            }
            else
            {
                // 複数枚
                if (Message.ShowConfirmingMessage("PDFが複数枚選択されています。全て追加しますか？\n(「いいえ」で中断)", this.ViewModel.Window_Title) == false)
                {
                    return false;
                }

                foreach (var pngPath in pngPaths)
                {
                    // タイトル
                    this.ViewModel.Title_Text    = ImageUtils.ExtractFileNameWithoutExtension(pngPath);
                    // ファイル名
                    this.ViewModel.FileName_Text = ImageUtils.ExtractFileNameWithExtension(pngPath);
                    // 表示する画像
                    this.ViewModel.ByteImage     = ImageUtils.ConvertPathToBytes(pngPath, ImageFormat.Png);

                    this.AddFile();

                    File.Delete(pngPath);
                }
            }

            return true;
        }

        #endregion

        #region 画像を拡大表示する

        /// <summary>
        /// イメージビューアーを開く
        /// </summary>
        internal void OpenImageViewer()
        {
            this.ViewModel_ImageViewer = new ViewModel_ImageViewer();

            var viewer = new ImageViewer();

            this.ViewModel_ImageViewer.FileImage_Height = this.ViewModel.FileImage_Image.Height;
            this.ViewModel_ImageViewer.FileImage_Width  = this.ViewModel.FileImage_Image.Width;
            this.ViewModel_ImageViewer.FileImage_Image  = this.ViewModel.FileImage_Image;
            
            viewer.Show();
        }

        #endregion

        #region 追加

        /// <summary>
        /// 追加
        /// </summary>
        internal void Add()
        {
            if (!Message.ShowConfirmingMessage($"画像情報を追加しますか？", this.ViewModel.Window_Title))
            {
                // キャンセル
                return;
            }

            if (string.IsNullOrEmpty(this.ViewModel.Title_Text))
            {
                Message.ShowErrorMessage("タイトルは入力必須です", this.ViewModel.Window_Title);
                return;
            }

            this.AddFile();
        }

        /// <summary>
        /// 添付画像をリストに追加する
        /// </summary>
        /// <remarks>
        /// 重複を防止するため、登録されたレコードの最大ID + 1のIDを割り当てる。
        /// </remarks>
        private void AddFile()
        {
            if (this.ViewModel.FileImage_Image is null)
            {
                throw new Domain.Exceptions.FormatException("画像情報が定義されていません。");
            }

            using (var cursor = new CursorWaiting())
            {
                this.ViewModel.CreateDate = DateTime.Today;
                this.ViewModel.UpdateDate = DateTime.Today;

                var id = default(int);

                if (this.ViewModel.AttachedFile_ItemSource.Any())
                {
                    id = this.ViewModel.AttachedFile_ItemSource.Max(x => x.ID) + 1;
                }
                else
                {
                    id = 1;
                }
                
                this.ViewModel.AttachedFile_ItemSource.Add(this.CreateEntity(id));
                this.Save();

                // 並び変え
                this.ViewModel.AttachedFile_ItemSource = new ObservableCollection<FileStorageEntity>(this.ViewModel.AttachedFile_ItemSource.OrderByDescending(x => x.FileName));

                // 追加ボタン
                this.ViewModel.Add_IsEnabled = false;
            }
        }

        /// <summary>
        /// エンティティ生成
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>エンティティ</returns>
        private FileStorageEntity CreateEntity(int id)
        {
            return new FileStorageEntity(
                        id,
                        this.ViewModel.Title_Text,
                        this.ViewModel.FileName_Text,
                        this.ViewModel.ByteImage,
                        this.ViewModel.Remarks_Text,
                        this.ViewModel.CreateDate,
                        this.ViewModel.UpdateDate);
        }

        #endregion

        /// <summary>
        /// 再描画
        /// </summary>
        public void Refresh()
        {
            // ListView
            this.Reflesh_ListView();
            // 入力用フォーム
            this.Reflesh_InputForm();
        }

        /// <summary>
        /// 再描画 - ListView
        /// </summary>
        /// <remarks>
        /// リストのデータを更新する。
        /// </remarks>
        private void Reflesh_ListView()
        {
            var entities = this.ViewModel.Entities;

            if (entities.Any())
            {
                // 既存の添付画像あり
                this.ViewModel.AttachedFile_ItemSource.Clear();

                foreach (var entity in entities)
                {
                    this.ViewModel.AttachedFile_ItemSource.Add(entity);
                }
            }
            else
            {
                // 既存の添付画像なし
                this.ViewModel.AttachedFile_ItemSource.Clear();
                this.Clear_InputForm();
                return;
            }
        }

        /// <summary>
        /// 再描画 - 入力用フォーム
        /// </summary>
        private void Reflesh_InputForm()
        {
            // ListView
            this.Reflesh_ListView();
            // 更新、削除ボタン
            this.EnableControlButton();
        }

        /// <summary>
        /// リロード
        /// </summary>
        public void Reload()
        {
            using (var cursor = new CursorWaiting())
            {
                FileStorages.Create(new FileStorageSQLite());

                this.ViewModel.Entities = FileStorages.FetchByDescending();

                this.Refresh();
            }
        }

        /// <summary>
        /// クリア
        /// </summary>
        /// <remarks>
        /// 各項目を初期化する。
        /// </remarks>
        public void Clear_InputForm()
        {
            // サムネイル
            this.ViewModel.FileImage_Image = null;
            // 画像を拡大表示するボタン
            this.ViewModel.OpenImageViewer_IsEnabled = false;

            // タイトル
            this.ViewModel.Title_IsEnabled = false;
            this.ViewModel.Title_Text      = string.Empty;
            // ファイル名
            this.ViewModel.FileName_Text   = string.Empty;
            // 備考
            this.ViewModel.Remarks_Text    = string.Empty;
            // 作成日
            this.ViewModel.CreateDate      = DateTime.Today;
            // 更新日
            this.ViewModel.UpdateDate      = DateTime.Today;

            // 追加ボタン
            this.ViewModel.Add_IsEnabled = false;
        }

        #region 更新

        /// <summary>
        /// 更新
        /// </summary>
        internal void Update()
        {
            if (!Message.ShowConfirmingMessage("画像情報を更新しますか？", this.ViewModel.Window_Title))
            {
                // キャンセル
                return;
            }

            if (string.IsNullOrEmpty(this.ViewModel.Title_Text))
            {
                Message.ShowErrorMessage("タイトルは入力必須です", this.ViewModel.Window_Title);
                return;
            }

            this.ViewModel.UpdateDate = DateTime.Today;

            using (var cursor = new CursorWaiting())
            {
                var id = this.ViewModel.AttachedFile_ItemSource[this.ViewModel.AttachedFile_SelectedIndex].ID;

                var entity = this.CreateEntity(id);
                this.ViewModel.AttachedFile_ItemSource[this.ViewModel.AttachedFile_SelectedIndex] = entity;

                _repository.Save(entity);

                this.EnableControlButton();
                this.Clear_InputForm();
            }
        }

        #endregion

        #region 削除

        /// <summary>
        /// 削除
        /// </summary>
        internal void Delete()
        {
            if (!Message.ShowConfirmingMessage("画像情報を削除しますか？", this.ViewModel.Window_Title))
            {
                // キャンセル
                return;
            }

            using (var cursor = new CursorWaiting())
            {
                var id = this.ViewModel.AttachedFile_ItemSource[this.ViewModel.AttachedFile_SelectedIndex].ID;
                _repository.Delete(id);

                this.ViewModel.AttachedFile_ItemSource.RemoveAt(this.ViewModel.AttachedFile_SelectedIndex);

                this.EnableControlButton();
                this.Clear_InputForm();
            }
        }

        #endregion

        /// <summary>
        /// 保存
        /// </summary>
        public void Save()        
        {
            foreach(var entity in this.ViewModel.AttachedFile_ItemSource)
            {
                _repository.Save(entity);
            }

            this.Reload();
        }
    }
}
