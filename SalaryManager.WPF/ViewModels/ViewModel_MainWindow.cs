using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.Converter;
using SalaryManager.WPF.Models;

namespace SalaryManager.WPF.ViewModels
{
    /// <summary>
    /// ViewModel - メイン画面
    /// </summary>
    public class ViewModel_MainWindow : INotifyPropertyChanged
    {
        #region Property Changed

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var d = PropertyChanged;
            d?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public ViewModel_MainWindow()
        {
            _windowAction += this.Model.Window_Activated;
            _windowAction += this.Header.Window_Activated;
            _windowAction += this.WorkPlace.Window_Activated;
            _windowAction += this.AnnualChart.Window_Activated;
            this.Window_Activated = new RelayCommand(_windowAction);

            // 保存
            this._save_Action += this.Model.Save;
            this._save_Action += this.AnnualChart.Reload;

            this._createBackup_Action += this.Model.CreateBackup;

            // デフォルトに設定
            this._setDefault_Action += this.Header.SetDefault;
            this._setDefault_Action += this.Header.Save;

            this.Model.ViewModel = this;

            this.Model.Initialize();
        }

        /// <summary> Model - ヘッダー </summary>
        public Model_MainWindow Model { get; set; } = Model_MainWindow.GetInstance();

        /// <summary> Model - ヘッダ </summary>
        public Model_Header Header { get; set; } = Model_Header.GetInstance(new HeaderSQLite());

        /// <summary> Model - 勤務場所 </summary>
        private Model_WorkPlace WorkPlace { get; set; } = Model_WorkPlace.GetInstance();

        /// <summary> Model - 月収一覧 </summary>
        private Model_AnnualChart AnnualChart { get; set; } = Model_AnnualChart.GetInstance();

        #region タイトル

        /// <summary>
        /// タイトル
        /// </summary>
        public string Title
        {
            get => "給与明細管理(仮)";
        }

        #endregion

        #region 画面遷移時

        /// <summary>
        /// Window - Activated
        /// </summary>
        public RelayCommand Window_Activated { get; private set; }

        private Action _windowAction;

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

        #region フォントサイズ

        private decimal _FontSize;

        /// <summary>
        /// フォントサイズ - FontSize
        /// </summary>
        public decimal FontSize
        {
            get => this._FontSize;
            set
            {
                this._FontSize = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 背景色

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

        #endregion

        #region CSV読込

        private RelayCommand _readCSV_Command;

        /// <summary>
        /// CSV読込ボタン
        /// </summary>
        public RelayCommand ReadCSV_Command
        {
            get
            {
                if (this._readCSV_Command == null)
                {
                    this._readCSV_Command = new RelayCommand(this.Model.ReadCSV);
                }
                return this._readCSV_Command;
            }
        }

        #endregion

        #region デフォルトに設定

        private RelayCommand _setDefault_Command;

        private Action _setDefault_Action;

        /// <summary>
        /// デフォルトに設定 - Command
        /// </summary>
        public RelayCommand SetDefault_Command
        {
            get
            {
                if (this._setDefault_Command == null)
                {
                    this._setDefault_Command = new RelayCommand(this._setDefault_Action);
                }
                return this._setDefault_Command;
            }
        }

        #endregion

        #region デフォルトから取得

        private RelayCommand _getDefault_Command;

        /// <summary>
        /// デフォルトに設定 - Command
        /// </summary>
        public RelayCommand GetDefault_Command
        {
            get
            {
                if (this._getDefault_Command == null)
                {
                    this._getDefault_Command = new RelayCommand(this.Model.FetchDefault);
                }
                return this._getDefault_Command;
            }
        }

        #endregion

        #region 経歴管理画面を開く

        private RelayCommand _showCareerManager_Command;

        /// <summary>
        /// 経歴管理画面を開く - Command
        /// </summary>
        public RelayCommand ShowCareerManager_Command
        {
            get
            {
                if (this._showCareerManager_Command == null)
                {
                    this._showCareerManager_Command = new RelayCommand(this.Model.ShowCareerManager);
                }
                return this._showCareerManager_Command;
            }
        }

        #endregion

        #region 添付ファイル管理画面を開く

        private RelayCommand _showFileStorage_Command;

        /// <summary>
        /// 添付ファイル画面を開く - Command
        /// </summary>
        public RelayCommand ShowFileStorage_Command
        {
            get
            {
                if (this._showFileStorage_Command == null)
                {
                    this._showFileStorage_Command = new RelayCommand(this.Model.ShowFileSotrage);
                }
                return this._showFileStorage_Command;
            }
        }

        #endregion

        #region オプション

        private RelayCommand _showOption_Command;

        /// <summary>
        /// 今月の明細を表示 - Command
        /// </summary>
        public RelayCommand ShowOption_Command
        {
            get
            {
                if (this._showOption_Command == null)
                {
                    this._showOption_Command = new RelayCommand(this.Model.ShowOption);
                }
                return this._showOption_Command;
            }
        }

        #endregion

        #region 今月の明細を表示

        private RelayCommand _showCurrentPayslip_Command;

        /// <summary>
        /// 今月の明細を表示 - Command
        /// </summary>
        public RelayCommand ShowCurrentPayslip_Command
        {
            get
            {
                if (this._showCurrentPayslip_Command == null)
                {
                    this._showCurrentPayslip_Command = new RelayCommand(this.Model.ShowCurrentPayslip);
                }
                return this._showCurrentPayslip_Command;
            }
        }

        #endregion

        #region 保存

        private RelayCommand _save_Command;

        private Action _save_Action;

        /// <summary>
        /// 保存 - Command
        /// </summary>
        public RelayCommand Save_Command
        {
            get
            {
                if (this._save_Command == null)
                {
                    this._save_Command = new RelayCommand(this._save_Action);
                }
                return this._save_Command;
            }
        }

        #endregion

        #region 保存

        private RelayCommand _createBackup_Command;

        private Action _createBackup_Action;

        /// <summary>
        /// 保存 - Command
        /// </summary>
        public RelayCommand CreateBackup_Command
        {
            get
            {
                if (this._createBackup_Command == null)
                {
                    this._createBackup_Command = new RelayCommand(this._createBackup_Action);
                }
                return this._createBackup_Command;
            }
        }

        #endregion

        #region Excel出力

        private RelayCommand _outputExcel_Command;

        /// <summary>
        /// Excel出力 - Command
        /// </summary>
        public RelayCommand OutputExcel_Command
        {
            get
            {
                if (this._outputExcel_Command == null)
                {
                    this._outputExcel_Command = new RelayCommand(this.Model.OutputExcel);
                }
                return this._outputExcel_Command;
            }
        }

        #endregion

        #region 金額の比較用

        private Brush _priceUpDown_Foreground;

        /// <summary>
        /// 金額の比較用 - Foreground
        /// </summary>
        public Brush PriceUpdown_Foreground
        {
            get => this._priceUpDown_Foreground;
            set
            {
                this._priceUpDown_Foreground = value;
                this.RaisePropertyChanged();
            }
        }

        private string _priceUpDown_Content;

        /// <summary>
        /// 金額の比較用 - Content
        /// </summary>
        public string PriceUpdown_Content
        {
            get => this._priceUpDown_Content;
            set
            {
                this._priceUpDown_Content = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

    }
}
