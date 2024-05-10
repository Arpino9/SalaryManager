using System;
using System.ComponentModel;
using System.Windows.Media;
using Reactive.Bindings;
using SalaryManager.Domain.Entities;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.Models;

namespace SalaryManager.WPF.ViewModels
{
    /// <summary>
    /// ViewModel - 添付ファイル
    /// </summary>
    public class ViewModel_FileStorage : ViewModelBase
    {
        public override event PropertyChangedEventHandler PropertyChanged;

        public ViewModel_FileStorage()
        {
            this.Model.ViewModel = this;

            this.Model.Initialize();

            this.BindEvents();
        }

        protected override void BindEvents()
        {
            this.AttachedFile_SelectionChanged.Subscribe(_ => this.Model.AttachedFile_SelectionChanged());
            this.SelectFile_Command.Subscribe(_ => this.Model.SelectFile());
            this.SelectFolder_Command.Subscribe(_ => this.Model.SelectFolder());
            this.OpenImageViewer_Command.Subscribe(_ => this.Model.OpenImageViewer());

            this.Add_Command.Subscribe(_ => this.Model.Add());
            this.Update_Command.Subscribe(_ => this.Model.Update());
            this.Delete_Command.Subscribe(_ => this.Model.Delete());
        }

        /// <summary> Model - 支給額 </summary>
        public Model_FileStorage Model 
            = Model_FileStorage.GetInstance(new FileStorageSQLite());


        #region タイトル

        /// <summary> Window - Title </summary>
        public ReactiveProperty<string> Window_Title { get; }
            = new ReactiveProperty<string>("添付ファイル管理");

        #endregion

        #region 添付ファイル一覧

        /// <summary> 添付ファイル一覧 - ItemSource </summary>
        public ReactiveCollection<FileStorageEntity> AttachedFile_ItemSource { get; set; }
            = new ReactiveCollection<FileStorageEntity>();

        /// <summary> 添付ファイル一覧 - SelectedIndex </summary>
        public ReactiveProperty<int> AttachedFile_SelectedIndex { get; set; }
            = new ReactiveProperty<int>();

        /// <summary> 添付ファイル一覧 - SelectionChanged </summary>
        public ReactiveCommand AttachedFile_SelectionChanged { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region ID

        /// <summary> ID - Text </summary>
        public ReactiveProperty<int> ID_Text { get; set; }
            = new ReactiveProperty<int>();

        #endregion

        #region ファイルのパス

        /// <summary> ファイルのパス - Text </summary>
        public ReactiveProperty<string> FilePath_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region ファイルを開く

        /// <summary> ファイルを開く - IsEnabled </summary>
        public ReactiveProperty<bool> SelectFile_IsEnabled { get; set; }
            = new ReactiveProperty<bool>();

        /// <summary> ファイルを開く - Command </summary>
        public ReactiveCommand SelectFile_Command { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region フォルダを開く

        /// <summary> フォルダを開く - IsEnabled </summary>
        public ReactiveProperty<bool> SelectFolder_IsEnabled { get; set; }
            = new ReactiveProperty<bool>();

        /// <summary> フォルダを開く - Command </summary>
        public ReactiveCommand SelectFolder_Command { get; private set; }
            = new ReactiveCommand();

        #endregion
        
        #region 画像

        /// <summary> ファイルのパス - Image </summary>
        public ReactiveProperty<ImageSource> FileImage_Image { get; set; }
            = new ReactiveProperty<ImageSource>();

        #endregion

        #region 画像を拡大表示する

        /// <summary> 画像を拡大表示する - IsEnabled </summary>
        public ReactiveProperty<bool> OpenImageViewer_IsEnabled { get; set; }
            = new ReactiveProperty<bool>();

        /// <summary> 画像を拡大表示する - Command </summary>
        public ReactiveCommand OpenImageViewer_Command { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region タイトル

        /// <summary> タイトル - IsEnabled </summary>
        public ReactiveProperty<bool> Title_IsEnabled { get; set; }
            = new ReactiveProperty<bool>();

        /// <summary> タイトル - Text </summary>
        public ReactiveProperty<string> Title_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region ファイル名

        /// <summary> ファイル名 - Text </summary>
        public ReactiveProperty<string> FileName_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 備考

        /// <summary> 備考 - IsEnabled </summary>
        public ReactiveProperty<bool> Remarks_IsEnabled { get; set; }
            = new ReactiveProperty<bool>();

        /// <summary> 備考 - Text </summary>
        public ReactiveProperty<string> Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 追加日付

        /// <summary> 追加日付 </summary>
        public DateTime CreateDate { get; set; }

        #endregion

        #region 更新日付

        /// <summary> 更新日付  </summary>
        public DateTime UpdateDate { get; set; }

        #endregion

        #region 追加

        /// <summary> 追加 - IsEnabled </summary>
        public ReactiveProperty<bool> Add_IsEnabled { get; set; }
            = new ReactiveProperty<bool>();

        /// <summary> 追加 - Command </summary>
        public ReactiveCommand Add_Command { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 更新

        /// <summary> 更新 - IsEnabled </summary>
        public ReactiveProperty<bool> Update_IsEnabled { get; set; }
            = new ReactiveProperty<bool>();

        /// <summary> 更新 - Command </summary>
        public ReactiveCommand Update_Command { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 削除

        /// <summary> 削除 - IsEnabled </summary>
        public ReactiveProperty<bool> Delete_IsEnabled { get; set; }
            = new ReactiveProperty<bool>();

        /// <summary> 削除 - Command </summary>
        public ReactiveCommand Delete_Command { get; private set; }
            = new ReactiveCommand();

        #endregion

    }
}
