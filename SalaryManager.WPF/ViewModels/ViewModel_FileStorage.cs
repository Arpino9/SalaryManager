using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using SalaryManager.Domain.Entities;
using SalaryManager.WPF.Converter;
using SalaryManager.WPF.Models;

namespace SalaryManager.WPF.ViewModels
{
    public class ViewModel_FileStorage : INotifyPropertyChanged
    {

        #region Property Changed

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            var d = PropertyChanged;
            d?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public ViewModel_FileStorage()
        {
            this.Model.ViewModel = this;
            this.AttachedFile_ItemSource = new ObservableCollection<FileStorageEntity>();

            _attachedFileAction += this.Model.AttachedFile_SelectionChanged;
            this.AttachedFile_SelectionChanged = new RelayCommand(_attachedFileAction);

            this.Model.Initialize();
        }

        /// <summary> Model - 支給額 </summary>
        public Model_FileStorage Model = Model_FileStorage.GetInstance();

        public IReadOnlyList<FileStorageEntity> Entities { get; internal set; }

        #region 添付ファイル一覧

        private ObservableCollection<FileStorageEntity> _file_ItemSource;

        /// <summary>
        /// 添付ファイル一覧 - ItemSource
        /// </summary>
        public ObservableCollection<FileStorageEntity> AttachedFile_ItemSource
        {
            get => _file_ItemSource;
            set
            {
                _file_ItemSource = value;
                this.RaisePropertyChanged();
            }
        }

        private int _attachedFile_SelectedIndex;

        /// <summary>
        /// 添付ファイル一覧 - SelectedIndex
        /// </summary>
        public int AttachedFile_SelectedIndex
        {
            get => this._attachedFile_SelectedIndex;
            set
            {
                this._attachedFile_SelectedIndex = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 添付ファイル一覧 - SelectionChanged
        /// </summary>
        public RelayCommand AttachedFile_SelectionChanged { get; private set; }

        private Action _attachedFileAction;

        #endregion

        #region ID

        private int _id_Text;

        /// <summary>
        /// ID - Text
        /// </summary>
        public int ID_Text
        {
            get => this._id_Text;
            set
            {
                this._id_Text = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 開く

        private string _filePath_Text;

        /// <summary>
        /// ファイルのパス - Text
        /// </summary>
        public string FilePath_Text
        {
            get => this._filePath_Text;
            set
            {
                this._filePath_Text = value;
                this.RaisePropertyChanged();
            }
        }

        private RelayCommand _selectfile_Command;

        /// <summary>
        /// ファイルのパス - Command
        /// </summary>
        /// <remarks>
        /// 開く
        /// </remarks>
        public RelayCommand SelectFile_Command
        {
            get
            {
                if (this._selectfile_Command == null)
                {
                    this._selectfile_Command = new RelayCommand(this.Model.OpenFile);
                }
                return this._selectfile_Command;
            }
        }

        #endregion

        #region 画像

        public byte[] ByteImage { get; set; }

        private ImageSource _fileImage_Image;

        /// <summary>
        /// ファイルのパス - Text
        /// </summary>
        public ImageSource FileImage_Image
        {
            get => this._fileImage_Image;
            set
            {
                this._fileImage_Image = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 画像を拡大表示する

        private bool _openImageViewer_IsEnabled;

        /// <summary>
        /// 画像を拡大表示する - IsEnabled
        /// </summary>
        public bool OpenImageViewer_IsEnabled
        {
            get => this._openImageViewer_IsEnabled;
            set
            {
                this._openImageViewer_IsEnabled = value;
                this.RaisePropertyChanged();
            }
        }

        private RelayCommand _openImageViewer_Command;

        /// <summary>
        /// 画像を拡大表示する - Command
        /// </summary>
        public RelayCommand OpenImageViewer_Command
        {
            get
            {
                if (this._openImageViewer_Command == null)
                {
                    this._openImageViewer_Command = new RelayCommand(this.Model.OpenImageViewer);
                }
                return this._openImageViewer_Command;
            }
        }

        #endregion

        #region タイトル

        private string _title_Text;

        /// <summary>
        /// タイトル - Text
        /// </summary>
        public string Title_Text
        {
            get => this._title_Text;
            set
            {
                this._title_Text = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region ファイル名

        private string _fileName_Text;

        /// <summary>
        /// ファイル名 - Text
        /// </summary>
        public string FileName_Text
        {
            get => this._fileName_Text;
            set
            {
                this._fileName_Text = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 備考

        private string _remarks_Text;

        /// <summary>
        /// 備考 - Text
        /// </summary>
        public string Remarks_Text
        {
            get => this._remarks_Text;
            set
            {
                this._remarks_Text = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 追加日付

        private DateTime _createDate;

        /// <summary>
        /// 追加日付 - Text
        /// </summary>
        public DateTime CreateDate
        {
            get => this._createDate;
            set
            {
                this._createDate = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 更新日付

        private DateTime _updateDate;

        /// <summary>
        /// 更新日付 - Text
        /// </summary>
        public DateTime UpdateDate
        {
            get => this._updateDate;
            set
            {
                this._updateDate = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 追加

        private RelayCommand _add_Command;

        /// <summary>
        /// 追加 - Command
        /// </summary>
        public RelayCommand Add_Command
        {
            get
            {
                if (this._add_Command == null)
                {
                    this._add_Command = new RelayCommand(this.Model.Add);
                }
                return this._add_Command;
            }
        }

        private bool _add_IsEnabled;

        /// <summary>
        /// 追加 - IsEnabled
        /// </summary>
        public bool Add_IsEnabled
        {
            get => this._add_IsEnabled;
            set
            {
                this._add_IsEnabled = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 更新

        private RelayCommand _update_Command;

        /// <summary>
        /// 更新 - Command
        /// </summary>
        public RelayCommand Update_Command
        {
            get
            {
                if (this._update_Command == null)
                {
                    this._update_Command = new RelayCommand(this.Model.Update);
                }
                return this._update_Command;
            }
        }

        private bool _update_IsEnabled;

        /// <summary>
        /// 更新 - IsEnabled
        /// </summary>
        public bool Update_IsEnabled
        {
            get => this._update_IsEnabled;
            set
            {
                this._update_IsEnabled = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 削除

        private RelayCommand _deleteCommand;

        /// <summary>
        /// 削除 - Command
        /// </summary>
        public RelayCommand Delete_Command
        {
            get
            {
                if (this._deleteCommand == null)
                {
                    this._deleteCommand = new RelayCommand(this.Model.Delete);
                }
                return this._deleteCommand;
            }
        }

        private bool _delete_IsEnabled;

        /// <summary>
        /// 削除 - IsEnabled
        /// </summary>
        public bool Delete_IsEnabled
        {
            get => this._delete_IsEnabled;
            set
            {
                this._delete_IsEnabled = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

    }
}
