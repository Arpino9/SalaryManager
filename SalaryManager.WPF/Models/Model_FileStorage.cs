using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Vml.Office;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.Domain.Modules.Logics;
using SalaryManager.Infrastructure.Interface;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

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

        public ViewModel_FileStorage ViewModel { get; set; }

        public void Initialize()
        {
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
            var path = DialogUtils.SelectFile("a", filter);

            if (string.IsNullOrEmpty(path)) 
            {
                return;
            }

            // 表示する画像
            this.ViewModel.ByteImage       = ImageUtils.ConvertPathToBytes(path, ImageFormat.Jpeg);
            this.ViewModel.FileImage_Image = ImageUtils.ConvertPathToImage(path, ImageFormat.Jpeg);

            var fileName = StringUtils.ExtractFileName(path);
            // タイトル
            this.ViewModel.Title_Text    = fileName;
            // ファイル名
            this.ViewModel.FileName_Text = fileName;
            // 追加ボタン
            this.ViewModel.Add_IsEnabled = true;
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

            using (var cursor = new CursorWaiting())
            {
                this.ViewModel.CreateDate = DateTime.Today;
                this.ViewModel.UpdateDate = DateTime.Today;

                var id = this.ViewModel.AttachedFile_ItemSource.Count + 1;

                this.ViewModel.AttachedFile_ItemSource.Add(this.CreateEntity(id));

                // 並び変え
                this.ViewModel.AttachedFile_ItemSource = new ObservableCollection<FileStorageEntity>(this.ViewModel.AttachedFile_ItemSource.OrderByDescending(x => x.FileName));
            }
        }

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
            }
        }

        #endregion

        public void Refresh()
        {
            this.Reflesh_InputForm();
        }

        /// <summary>
        /// 再描画 - 入力用フォーム
        /// </summary>
        private void Reflesh_InputForm()
        {
            // 更新、削除ボタン
            this.EnableControlButton();
        }

        public void Reload()
        {
            throw new NotImplementedException();
        }

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

        public void Save()
        {
            throw new NotImplementedException();
        }


    }
}
