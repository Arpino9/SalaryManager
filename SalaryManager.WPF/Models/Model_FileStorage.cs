using Reactive.Bindings;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Exceptions;
using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.Domain.Modules.Logics;
using SalaryManager.Domain.Repositories;
using SalaryManager.Domain.StaticValues;
using SalaryManager.Domain.ValueObjects;
using SalaryManager.Infrastructure.Interface;
using SalaryManager.Infrastructure.PDF;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.Infrastructure.XML;
using SalaryManager.WPF.ViewModels;
using SalaryManager.WPF.Window;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Media;
using static SalaryManager.WPF.ViewModels.ViewModel_GeneralOption;
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

        /// <summary> Entities - 添付ファイル管理 </summary>
        public IReadOnlyList<FileStorageEntity> Entities { get; internal set; }

        /// <summary> 画像の保存方法 </summary>
        internal HowToSaveImage HowToSave { get; private set; }

        /// <summary> イメージ </summary>
        public byte[] ByteImage { get; set; }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            FileStorages.Create(_repository);

            this.Entities = FileStorages.FetchByDescending();

            this.ViewModel.AttachedFile_SelectedIndex.Value = -1;
            this.Clear_InputForm();

            var obj = EnumUtils.ToEnum(this.HowToSave.GetType(), XMLLoader.FetchHowToSaveImage());
            if (obj is null)
            {
                this.ViewModel.SelectFile_IsEnabled.Value   = false;
                this.ViewModel.SelectFolder_IsEnabled.Value = true;

                this.Reflesh_ListView();
            }
            else
            {
                var howToSave = (HowToSaveImage)obj;
                this.ViewModel.SelectFile_IsEnabled.Value   = (howToSave == HowToSaveImage.SaveImage);
                this.ViewModel.SelectFolder_IsEnabled.Value = (howToSave == HowToSaveImage.SavePath);

                if (howToSave == HowToSaveImage.SavePath)
                {
                    this.SelectFolder();
                }
                else
                {
                    this.Reflesh_ListView();
                }
            }
        }

        /// <summary>
        /// Enable - 操作ボタン
        /// </summary>
        private void EnableControlButton()
        {
            var selected = this.ViewModel.AttachedFile_ItemSource.Any()
                        && this.ViewModel.AttachedFile_SelectedIndex.Value >= 0;

            // 更新ボタン
            this.ViewModel.Update_IsEnabled.Value = selected;
            // 削除ボタン
            this.ViewModel.Delete_IsEnabled.Value = selected;
        }

        #region ファイルを開く

        /// <summary>
        /// ファイルを開く - SelectionChanged
        /// </summary>
        internal void AttachedFile_SelectionChanged()
        {
            if (this.ViewModel.AttachedFile_SelectedIndex.Value.IsUnSelected())
            {
                return;
            }

            this.EnableControlButton();

            if (!this.ViewModel.AttachedFile_ItemSource.Any())
            {
                return;
            }

            var entity = this.ViewModel.AttachedFile_ItemSource[this.ViewModel.AttachedFile_SelectedIndex.Value];

            // サムネイル
            this.ViewModel.FileImage_Image.Value = ImageUtils.ConvertBytesToImage(entity.Image);
            // 画像を拡大表示するボタン
            this.ViewModel.OpenImageViewer_IsEnabled.Value = true;

            var selectedSaveImage = (this.HowToSave == HowToSaveImage.SaveImage);

            // タイトル
            this.ViewModel.Title_IsEnabled.Value = selectedSaveImage;
            this.ViewModel.Title_Text.Value      = entity.Title;
            // ファイル名
            this.ViewModel.FileName_Text.Value = entity.FileName;
            // 備考
            this.ViewModel.Remarks_IsEnabled.Value = selectedSaveImage;
            this.ViewModel.Remarks_Text.Value      = entity.Remarks;

            // 追加
            this.ViewModel.Update_IsEnabled.Value = selectedSaveImage;
            // 削除
            this.ViewModel.Delete_IsEnabled.Value = selectedSaveImage;
        }

        /// <summary>
        /// ファイルを開く
        /// </summary>
        internal void SelectFile()
        {
            //TODO: PDF, 画像ファイルに限定する
            var filter = "すべてのファイル(*.*)|*.*";
            var path = DialogUtils.SelectFile(string.Empty, filter);

            if (string.IsNullOrEmpty(path)) 
            {
                // キャンセル
                return;
            }

            var extension = new FileExtensionValue(path);

            if (extension.IsPDF) 
            {
                // PDF
                if (this.ConvertPDFToPNG(path))
                {
                    // 追加ボタン
                    this.ViewModel.Add_IsEnabled.Value = true;
                }
                
                return;
            }

            // サムネイル
            this.ByteImage       = ImageUtils.ConvertPathToBytes(path, extension.ImageFormat);
            this.ViewModel.FileImage_Image.Value = ImageUtils.ConvertPathToImage(path, extension.ImageFormat);
            // 画像を拡大表示するボタン
            this.ViewModel.OpenImageViewer_IsEnabled.Value = true;

            // タイトル
            this.ViewModel.Title_IsEnabled.Value = true;
            this.ViewModel.Title_Text.Value      = ImageUtils.ExtractFileNameWithoutExtension(path);
            // ファイル名
            this.ViewModel.FileName_Text.Value   = ImageUtils.ExtractFileNameWithExtension(path);
            // 備考
            this.ViewModel.Remarks_IsEnabled.Value = true;
            // 追加ボタン
            this.ViewModel.Add_IsEnabled.Value   = true;
        }

        /// <summary>
        /// フォルダを開く
        /// </summary>
        internal void SelectFolder()
        {
            var folderPath = XMLLoader.FetchImageFolder();
            
            if (Directory.Exists(folderPath) == false)
            {
                Message.ShowErrorMessage("フォルダが存在しません。設定画面から画像ファイルの格納先を指定してください。", this.ViewModel.Window_Title.Value);
                return;
            }

            using (var cursor = new CursorWaiting())
            {
                foreach (var filePath in Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories))
                {
                    try
                    {
                        this.AddFileFromFolder(filePath);
                    }
                    catch(Exception ex) when (ex.Message == "Invalid password")
                    {
                        throw new FileReaderException("PDFの読込パスワードが不正です。\nオプションから正しいパスワードを設定してください。");
                    } 
                    catch(Exception ex) 
                    {
                        throw new FileReaderException("添付ファイルの読み込みに失敗しました。", ex);
                    }
                    
                }

                var orderedList = this.ViewModel.AttachedFile_ItemSource.OrderByDescending(x => x.Title).ToList();
                this.ViewModel.AttachedFile_ItemSource = orderedList.ToReactiveCollection();

                this.ViewModel.Title_IsEnabled.Value   = false;
                this.ViewModel.Remarks_IsEnabled.Value = false;
                this.ViewModel.Update_IsEnabled.Value  = false;
                this.ViewModel.Delete_IsEnabled.Value  = false;
            }   
        }

        /// <summary>
        /// フォルダから画像・PDFファイルを追加する
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        private void AddFileFromFolder(string filePath)
        {
            var extension = new FileExtensionValue(filePath);

            // ID
            var id = this.GetID();

            // 追加日
            this.ViewModel.CreateDate = File.GetCreationTime(filePath);
            // 更新日
            this.ViewModel.UpdateDate = File.GetLastWriteTime(filePath);

            if (extension.IsPDF)
            {
                var pngPaths = this.PDFConverter.ConvertPDFIntoImage(filePath);

                var count = 1;

                foreach (var pngPath in pngPaths)
                {
                    // タイトル
                    this.ViewModel.Title_Text.Value = $"{ImageUtils.ExtractFileNameWithoutExtension(filePath)}_{count.ToString("D2")}";
                    // ファイル名
                    this.ViewModel.FileName_Text.Value = ImageUtils.ExtractFileNameWithExtension(filePath);

                    // 表示する画像
                    this.ByteImage = ImageUtils.ConvertPathToBytes(pngPath, ImageFormat.Png);
                    this.ViewModel.FileImage_Image.Value = ImageUtils.ConvertPathToImage(pngPath, ImageFormat.Png);

                    File.Delete(pngPath);

                    this.ViewModel.AttachedFile_ItemSource.Add(this.CreateEntity(id));

                    count++;
                }
            }

            if (extension.IsImage)
            {
                // タイトル
                this.ViewModel.Title_Text.Value = ImageUtils.ExtractFileNameWithoutExtension(filePath);
                // ファイル名
                this.ViewModel.FileName_Text.Value = ImageUtils.ExtractFileNameWithExtension(filePath);

                // 表示する画像
                this.ByteImage       = ImageUtils.ConvertPathToBytes(filePath, ImageFormat.Png);

                this.ViewModel.FileImage_Image = new ReactiveProperty<ImageSource>();
                this.ViewModel.FileImage_Image.Value = ImageUtils.ConvertPathToImage(filePath, extension.ImageFormat);

                this.ViewModel.AttachedFile_ItemSource.Add(this.CreateEntity(id));
            }
        }

        /// <summary>
        /// IDを取得する
        /// </summary>
        /// <returns>ID</returns>
        private int GetID()
        {
            if (this.ViewModel.AttachedFile_ItemSource.Any())
            {
                return this.ViewModel.AttachedFile_ItemSource.Max(x => x.ID) + 1;
            }

            return 1;
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
                this.ViewModel.Title_Text.Value    = ImageUtils.ExtractFileNameWithoutExtension(pngPaths.First());
                // ファイル名
                this.ViewModel.FileName_Text.Value = ImageUtils.ExtractFileNameWithExtension(pngPaths.First());
                // 表示する画像
                this.ByteImage     = ImageUtils.ConvertPathToBytes(pngPaths.First(), ImageFormat.Png);

                this.AddFile();

                File.Delete(pngPaths.First());
            }
            else
            {
                // 複数枚
                if (Message.ShowConfirmingMessage("PDFが複数枚選択されています。全て追加しますか？\n(「いいえ」で中断)", this.ViewModel.Window_Title.Value) == false)
                {
                    return false;
                }

                foreach (var pngPath in pngPaths)
                {
                    // タイトル
                    this.ViewModel.Title_Text.Value    = ImageUtils.ExtractFileNameWithoutExtension(pngPath);
                    // ファイル名
                    this.ViewModel.FileName_Text.Value = ImageUtils.ExtractFileNameWithExtension(pngPath);
                    // 表示する画像
                    this.ByteImage     = ImageUtils.ConvertPathToBytes(pngPath, ImageFormat.Png);

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

            this.ViewModel_ImageViewer.FileImage_Height.Value = this.ViewModel.FileImage_Image.Value.Height;
            this.ViewModel_ImageViewer.FileImage_Width.Value  = this.ViewModel.FileImage_Image.Value.Width;
            this.ViewModel_ImageViewer.FileImage_Image.Value  = this.ViewModel.FileImage_Image.Value;
            
            viewer.Show();
        }

        #endregion

        #region 追加

        /// <summary>
        /// 追加
        /// </summary>
        internal void Add()
        {
            if (!Message.ShowConfirmingMessage($"画像情報を追加しますか？", this.ViewModel.Window_Title.Value))
            {
                // キャンセル
                return;
            }

            if (string.IsNullOrEmpty(this.ViewModel.Title_Text.Value))
            {
                Message.ShowErrorMessage("タイトルは入力必須です", this.ViewModel.Window_Title.Value);
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

                var id = this.GetID();
                this.ViewModel.AttachedFile_ItemSource.Add(this.CreateEntity(id));

                this.Save();

                // 並び変え
                var orderedList = new ObservableCollection<FileStorageEntity>(this.ViewModel.AttachedFile_ItemSource.OrderByDescending(x => x.FileName)); ;
                this.ViewModel.AttachedFile_ItemSource = orderedList.ToReactiveCollection();

                // 追加ボタン
                this.ViewModel.Add_IsEnabled.Value = false;
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
                        this.ViewModel.Title_Text.Value,
                        this.ViewModel.FileName_Text.Value,
                        this.ByteImage,
                        this.ViewModel.Remarks_Text.Value,
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
            if (this.Entities.Any())
            {
                // 既存の添付画像あり
                this.ViewModel.AttachedFile_ItemSource.Clear();

                foreach (var entity in this.Entities)
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

                this.Entities = FileStorages.FetchByDescending();

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
            // ファイル or フォルダを開くボタン
            this.ViewModel.SelectFile_IsEnabled.Value   = false;
            this.ViewModel.SelectFolder_IsEnabled.Value = true;

            // サムネイル
            this.ViewModel.FileImage_Image = null;
            // 画像を拡大表示するボタン
            this.ViewModel.OpenImageViewer_IsEnabled.Value = false;

            // タイトル
            this.ViewModel.Title_IsEnabled.Value = false;
            this.ViewModel.Title_Text.Value      = string.Empty;
            // ファイル名
            this.ViewModel.FileName_Text.Value   = string.Empty;
            // 備考
            this.ViewModel.Remarks_Text.Value    = string.Empty;
            // 作成日
            this.ViewModel.CreateDate      = DateTime.Today;
            // 更新日
            this.ViewModel.UpdateDate      = DateTime.Today;

            // 追加ボタン
            this.ViewModel.Add_IsEnabled.Value = false;
        }

        #region 更新

        /// <summary>
        /// 更新
        /// </summary>
        internal void Update()
        {
            if (!Message.ShowConfirmingMessage("画像情報を更新しますか？", this.ViewModel.Window_Title.Value))
            {
                // キャンセル
                return;
            }

            if (string.IsNullOrEmpty(this.ViewModel.Title_Text.Value))
            {
                Message.ShowErrorMessage("タイトルは入力必須です", this.ViewModel.Window_Title.Value);
                return;
            }

            this.ViewModel.UpdateDate = DateTime.Today;

            using (var cursor = new CursorWaiting())
            {
                var id = this.ViewModel.AttachedFile_ItemSource[this.ViewModel.AttachedFile_SelectedIndex.Value].ID;

                var entity = this.CreateEntity(id);
                this.ViewModel.AttachedFile_ItemSource[this.ViewModel.AttachedFile_SelectedIndex.Value] = entity;

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
            if (!Message.ShowConfirmingMessage("画像情報を削除しますか？", this.ViewModel.Window_Title.Value))
            {
                // キャンセル
                return;
            }

            using (var cursor = new CursorWaiting())
            {
                var id = this.ViewModel.AttachedFile_ItemSource[this.ViewModel.AttachedFile_SelectedIndex.Value].ID;
                _repository.Delete(id);

                this.ViewModel.AttachedFile_ItemSource.RemoveAt(this.ViewModel.AttachedFile_SelectedIndex.Value);

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
