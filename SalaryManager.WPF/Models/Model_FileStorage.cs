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

        public static Model_FileStorage GetInstance()
        {
            if (model == null)
            {
                model = new Model_FileStorage();
            }

            return model;
        }

        #endregion

        public Model_FileStorage()
        {
            _repositoryPDFConverter = new PDFConverter();
        }

        /// <summary> ViewModel - 添付ファイル管理 </summary>
        public ViewModel_FileStorage ViewModel { get; set; }

        /// <summary> PDF変換リポジトリ </summary>
        private IPDFConverterRepository _repositoryPDFConverter;

        public void Initialize()
        {
            FileStorages.Create(new FileStorageSQLite());

            this.ViewModel.Entities = FileStorages.FetchByDescending();

            this.Reflesh_ListView();

            this.ViewModel.AttachedFile_SelectedIndex = -1;
            this.Clear_InputForm();
        }

        /// <summary>
        /// Enable - 操作ボタン
        /// </summary>
        /// <remarks>
        /// 追加ボタンは「会社名」に値があれば押下可能。
        /// </remarks>
        private void EnableControlButton()
        {
            var selected = this.ViewModel.AttachedFile_ItemSource.Any()
                        && this.ViewModel.AttachedFile_SelectedIndex >= 0;

            // 更新ボタン
            this.ViewModel.Update_IsEnabled = selected;
            // 削除ボタン
            this.ViewModel.Delete_IsEnabled = selected;
        }

        #region MyRegion

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

            this.ViewModel.FileImage_Image = ImageUtils.ConvertBytesToImage(entity.Image);

            this.ViewModel.Title_Text    = entity.Title;
            this.ViewModel.FileName_Text = entity.FileName;
            this.ViewModel.Remarks_Text  = entity.Remarks;
        }

        #endregion

        #region ファイルを開く

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
                return;
            }

            var extension = new FileExtensionValue(ImageUtils.ExtractFileExtension(path));

            if (extension.IsPDF) 
            {
                if (this.ConvertPDFToPNG(path))
                {
                    // 追加ボタン
                    this.ViewModel.Add_IsEnabled = true;
                }
                
                return;
            }

            // 表示する画像
            this.ViewModel.ByteImage       = ImageUtils.ConvertPathToBytes(path, extension.ImageFormat);
            this.ViewModel.FileImage_Image = ImageUtils.ConvertPathToImage(path, extension.ImageFormat);

            var fileName = ImageUtils.ExtractFileName(path);
            // タイトル
            this.ViewModel.Title_Text    = fileName;
            // ファイル名
            this.ViewModel.FileName_Text = fileName;
            // 追加ボタン
            this.ViewModel.Add_IsEnabled = true;
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
            var pngPaths = _repositoryPDFConverter.ConvertPDFIntoImage(path);

            if (pngPaths.Count == 1)
            {
                // 1枚
                var fileName = ImageUtils.ExtractFileName(pngPaths.First());
                // タイトル
                this.ViewModel.Title_Text    = fileName;
                // ファイル名
                this.ViewModel.FileName_Text = fileName;
                // 表示する画像
                this.ViewModel.ByteImage     = ImageUtils.ConvertPathToBytes(pngPaths.First(), ImageFormat.Png);

                this.AddFile();

                File.Delete(pngPaths.First());
            }
            else
            {
                // 複数枚
                if (Message.ShowConfirmingMessage("PDFが複数枚選択されています。全て追加しますか？\n(「いいえ」で中断)", "確認") == false)
                {
                    return false;
                }

                foreach (var pngPath in pngPaths)
                {
                    var fileName = ImageUtils.ExtractFileName(pngPath);
                    // タイトル
                    this.ViewModel.Title_Text    = fileName;
                    // ファイル名
                    this.ViewModel.FileName_Text = fileName;
                    // 表示する画像
                    this.ViewModel.ByteImage     = ImageUtils.ConvertPathToBytes(pngPath, ImageFormat.Png);

                    this.AddFile();

                    File.Delete(pngPath);
                }
            }

            return true;
        }

        #endregion

        #region 追加

        /// <summary>
        /// 追加
        /// </summary>
        internal void Add()
        {
            if (!Message.ShowConfirmingMessage($"画像情報を追加しますか？", "添付ファイル管理"))
            {
                // キャンセル
                return;
            }

            this.AddFile();
        }

        /// <summary>
        /// 添付画像をリストに追加する
        /// </summary>
        private void AddFile()
        {
            using (var cursor = new CursorWaiting())
            {
                this.ViewModel.CreateDate = DateTime.Today;
                this.ViewModel.UpdateDate = DateTime.Today;

                var id = this.ViewModel.AttachedFile_ItemSource.Count + 1;

                this.ViewModel.AttachedFile_ItemSource.Add(this.CreateEntity(id));
                this.Save();

                // 並び変え
                this.ViewModel.AttachedFile_ItemSource = new ObservableCollection<FileStorageEntity>(this.ViewModel.AttachedFile_ItemSource.OrderByDescending(x => x.FileName));
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
        /// <remarks>
        /// 該当月に添付ファイル情報が存在すれば、各項目を再描画する。
        /// </remarks>
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
        private void Reflesh_ListView()
        {
            this.ViewModel.AttachedFile_ItemSource.Clear();

            var entities = this.ViewModel.Entities;

            if (!entities.Any())
            {
                this.Clear_InputForm();
                return;
            }

            foreach (var entity in entities)
            {
                this.ViewModel.AttachedFile_ItemSource.Add(entity);
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
        /// <remarks>
        /// 年月の変更時などに、該当月の項目を取得する。
        /// </remarks>
        public void Reload()
        {
            using (var cursor = new CursorWaiting())
            {
                Careers.Create(new CareerSQLite());

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
            this.ViewModel.FileImage_Image = null;

            this.ViewModel.Title_Text    = string.Empty;
            this.ViewModel.FileName_Text = string.Empty;
            this.ViewModel.Remarks_Text  = string.Empty;

            this.ViewModel.CreateDate = DateTime.Today;
            this.ViewModel.UpdateDate = DateTime.Today;

            // 追加ボタン
            this.ViewModel.Add_IsEnabled = false;
        }

        #region 更新

        /// <summary>
        /// 更新
        /// </summary>
        internal void Update()
        {
            if (!Message.ShowConfirmingMessage("画像情報を更新しますか？", "添付ファイル管理"))
            {
                // キャンセル
                return;
            }

            this.ViewModel.UpdateDate = DateTime.Today;

            using (var cursor = new CursorWaiting())
            {
                var id = this.ViewModel.AttachedFile_ItemSource[this.ViewModel.AttachedFile_SelectedIndex].ID;

                var entity = this.CreateEntity(id);
                this.ViewModel.AttachedFile_ItemSource[this.ViewModel.AttachedFile_SelectedIndex] = entity;

                var sqlite = new FileStorageSQLite();
                sqlite.Save(entity);
            }
        }

        #endregion

        #region 削除

        /// <summary>
        /// 削除
        /// </summary>
        internal void Delete()
        {
            using (var cursor = new CursorWaiting())
            {
                this.ViewModel.AttachedFile_ItemSource.RemoveAt(this.ViewModel.AttachedFile_SelectedIndex);

                this.EnableControlButton();

                var sqlite = new FileStorageSQLite();
                sqlite.Delete(this.ViewModel.AttachedFile_SelectedIndex + 1);

                this.Reload();
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
                var sqlite = new FileStorageSQLite();
                sqlite.Save(entity);
            }

            this.Reload();
        }
    }
}
