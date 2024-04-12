using SalaryManager.Domain;
using SalaryManager.WPF.Converter;
using SalaryManager.WPF.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace SalaryManager.WPF.ViewModels
{
    public class ViewModel_GeneralOption : INotifyPropertyChanged
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

        public ViewModel_GeneralOption()
        {
            this.Model.GeneralOption = this;
            this.Model.Initialize_General();

            this.BindEvents();
        }

        /// <summary>
        /// イベント登録
        /// </summary>
        private void BindEvents()
        {
            // フォントファミリ
            this.FontFamily_SelectionChanged = new RelayCommand(this.Model.FontFamily_SelectionChanged);
            // 画像の保存方法
            this.HowToSaveImage_Checked      = new RelayCommand(this.Model.HowToSaveImage_SelectionChanged);
        }

        /// <summary> Model - オプション </summary>
        public Model_Option Model = Model_Option.GetInstance();

        #region タイトル

        /// <summary>
        /// タイトル
        /// </summary>
        public string Window_Title
        {
            get => "オプション";
        }

        #endregion

        #region フォントファミリ

        private System.Windows.Media.FontFamily _FontFamily;

        /// <summary>
        /// フォントファミリ - FontFamily
        /// </summary>
        public System.Windows.Media.FontFamily FontFamily
        {
            get => this._FontFamily;
            set
            {
                this._FontFamily = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region SQLite

        private string _selectSQLite_Text;

        /// <summary>
        /// SQLite - Text
        /// </summary>
        public string SelectSQLite_Text
        {
            get => this._selectSQLite_Text;
            set
            {
                this._selectSQLite_Text = value;
                this.RaisePropertyChanged();
            }
        }

        private RelayCommand _selectSQLite_Command;

        /// <summary>
        /// SQLite - Command
        /// </summary>
        /// <remarks>
        /// 開く
        /// </remarks>
        public RelayCommand SelectSQLite_Command
        {
            get
            {
                if (this._selectSQLite_Command == null)
                {
                    this._selectSQLite_Command = new RelayCommand(this.Model.SelectSQLitePath);
                }
                return this._selectSQLite_Command;
            }
        }

        #endregion

        #region Excelテンプレート

        private string _selectExcelTempletePath_Text;

        /// <summary>
        /// Excelテンプレート - Text
        /// </summary>
        public string SelectExcelTempletePath_Text
        {
            get => this._selectExcelTempletePath_Text;
            set
            {
                this._selectExcelTempletePath_Text = value;
                this.RaisePropertyChanged();
            }
        }

        private RelayCommand _selectExcelTempletePath_Command;

        /// <summary>
        /// Excelテンプレート - Command
        /// </summary>
        /// <remarks>
        /// 開く
        /// </remarks>
        public RelayCommand SelectExcelTemplatePath_Command
        {
            get
            {
                if (this._selectExcelTempletePath_Command == null)
                {
                    this._selectExcelTempletePath_Command = new RelayCommand(this.Model.SelectExcelTemplatePath);
                }
                return this._selectExcelTempletePath_Command;
            }
        }
        
        #endregion

        #region フォントファミリ

        /// <summary> FontFamily - SelectionChanged </summary>
        public RelayCommand FontFamily_SelectionChanged { get; private set; }

        private ObservableCollection<string> _fontFamily_ItemSource;

        /// <summary>
        /// フォントファミリ - ItemSource
        /// </summary>
        public ObservableCollection<string> FontFamily_ItemSource
        {
            get => this._fontFamily_ItemSource;
            set
            {
                this._fontFamily_ItemSource = value;
                this.RaisePropertyChanged();
            }
        }

        private string _fontFamily_Text;

        /// <summary>
        /// フォントファミリ - Text
        /// </summary>
        public string FontFamily_Text
        {
            get => this._fontFamily_Text;
            set
            {
                this._fontFamily_Text = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region フォントサイズ

        private decimal _fontSize_Value;

        /// <summary>
        /// フォントサイズ - Value
        /// </summary>
        public decimal FontSize_Value
        {
            get => this._fontSize_Value;
            set
            {
                this._fontSize_Value = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 背景色

        internal System.Drawing.Color Window_BackgroundColor = SystemColors.ControlLight;

        private System.Windows.Media.Brush _window_Background;

        /// <summary>
        /// 背景色 - Background
        /// </summary>
        public System.Windows.Media.Brush Window_Background
        {
            get => this._window_Background;
            set
            {
                this._window_Background = value;
                this.RaisePropertyChanged();
            }
        }

        private RelayCommand _changeWindowBackground_Command;

        /// <summary>
        /// 背景色 - Command
        /// </summary>
        public RelayCommand ChangeWindowBackground_Command
        {
            get
            {
                if (this._changeWindowBackground_Command == null)
                {
                    this._changeWindowBackground_Command = new RelayCommand(this.Model.ChangeWindowBackground);
                }
                return this._changeWindowBackground_Command;
            }
        }

        #endregion

        #region プレビュー

        private System.Windows.Media.FontFamily _Preview_FontFamily;

        /// <summary>
        /// プレビュー - FontFamily
        /// </summary>
        public System.Windows.Media.FontFamily Preview_FontFamily
        {
            get => this._Preview_FontFamily;
            set
            {
                this._Preview_FontFamily = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 初期表示時にデフォルト明細を表示する

        private bool _showdefaultPayslip_IsChecked;

        /// <summary>
        /// 初期表示時にデフォルト明細を表示する - IsChecked
        /// </summary>
        public bool ShowDefaultPayslip_IsChecked
        {
            get => this._showdefaultPayslip_IsChecked;
            set
            {
                this._showdefaultPayslip_IsChecked = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 画像の保存方法

        /// <summary> 画像の保存方法 - Checked </summary>
        public RelayCommand HowToSaveImage_Checked { get; private set; }

        /// <summary>
        /// 画像の保存方法
        /// </summary>
        public enum HowToSaveImage
        {
            /// <summary> 画像パス </summary>
            SavePath,

            /// <summary> 画像データ </summary>
            SaveImage,
        }

        private HowToSaveImage _howToSaveImage_IsChecked = HowToSaveImage.SavePath;

        /// <summary>
        /// 画像の保存方法 - IsChecked
        /// </summary>
        public HowToSaveImage HowToSaveImage_IsChecked
        {
            get => this._howToSaveImage_IsChecked;
            set
            {
                this._howToSaveImage_IsChecked = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region フォルダを開く

        private string _selectfolder_Text;

        /// <summary>
        /// フォルダを開く - Text
        /// </summary>
        public string ImageFolderPath_Text
        {
            get => this._selectfolder_Text;
            set
            {
                this._selectfolder_Text = value;
                this.RaisePropertyChanged(); ;
            }
        }

        private RelayCommand _selectfolder_Command;

        /// <summary>
        /// フォルダを開く - Command
        /// </summary>
        public RelayCommand SelectFolder_Command
        {
            get
            {
                if (this._selectfolder_Command == null)
                {
                    this._selectfolder_Command = new RelayCommand(this.Model.OpenFolder);
                }
                return this._selectfolder_Command;
            }
        }

        private bool _selectfolder_IsEnabled;

        /// <summary>
        /// フォルダを開く - IsEnabled
        /// </summary>
        public bool SelectFolder_IsEnabled
        {
            get => this._selectfolder_IsEnabled;
            set
            {
                this._selectfolder_IsEnabled = value;
                this.RaisePropertyChanged(); ;
            }
        }

        #endregion

        #region 保存

        private RelayCommand _save_Command;

        /// <summary>
        /// 保存 - Command
        /// </summary>
        public RelayCommand Save_Command
        {
            get
            {
                if (this._save_Command == null)
                {
                    if (Shared.SavingExtension == "XML")
                    {
                        this._save_Command = new RelayCommand(this.Model.SaveXML);
                    }
                    else
                    {
                        this._save_Command = new RelayCommand(this.Model.SaveJSON);
                    }
                }
                return this._save_Command;
            }
        }

        #endregion

        #region 初期値に戻す

        private RelayCommand _setDefault_Command;

        /// <summary>
        /// 初期値に戻す - Command
        /// </summary>
        public RelayCommand SetDefault_Command
        {
            get
            {
                if (this._setDefault_Command == null)
                {
                    this._setDefault_Command = new RelayCommand(this.Model.SetDefault);
                }
                return this._setDefault_Command;
            }
        }

        #endregion

    }
}
