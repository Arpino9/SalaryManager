using SalaryManager.WPF.Converter;
using SalaryManager.WPF.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            this.Model.ViewModel = this;
            this.Model.Initialize();

            // フォントファミリ
            _fontFamily_Action += this.Model.FontFamily_SelectionChanged;
            this.FontFamily_SelectionChanged = new RelayCommand(_fontFamily_Action);
        }

        /// <summary> Model - 支給額 </summary>
        public Model_Option Model = Model_Option.GetInstance();

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

        private RelayCommand _setDefault_selectSQLite_Command;

        /// <summary>
        /// SQLite - Command
        /// </summary>
        /// <remarks>
        /// 初期値に戻す
        /// </remarks>
        public RelayCommand SetDefault_SelectSQLite_Command
        {
            get
            {
                if (this._setDefault_selectSQLite_Command == null)
                {
                    this._setDefault_selectSQLite_Command = new RelayCommand(this.Model.SetDefault_SelectSQLitePath);
                }
                return this._setDefault_selectSQLite_Command;
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

        private RelayCommand _setDefault_SelectExcelTemplatePath_Command;

        /// <summary>
        /// Excelテンプレート - Command
        /// </summary>
        /// <remarks>
        /// 初期値に戻す
        /// </remarks>
        public RelayCommand SetDefault_SelectExcelTemplatePath_Command
        {
            get
            {
                if (this._setDefault_SelectExcelTemplatePath_Command == null)
                {
                    this._setDefault_SelectExcelTemplatePath_Command = new RelayCommand(this.Model.SetDefault_SelectExcelTemplatePath);
                }
                return this._setDefault_SelectExcelTemplatePath_Command;
            }
        }
        
        #endregion

        #region フォントファミリ

        public Action _fontFamily_Action;

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

        private RelayCommand _setDefault_FontFamily_Command;

        /// <summary>
        /// フォントファミリ - Command
        /// </summary>
        /// <remarks>
        /// 初期値に戻す
        /// </remarks>
        public RelayCommand SetDefault_FontFamily_Command
        {
            get
            {
                if (this._setDefault_FontFamily_Command == null)
                {
                    this._setDefault_FontFamily_Command = new RelayCommand(this.Model.SetDefault_FontFamily);
                }
                return this._setDefault_FontFamily_Command;
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
                    this._save_Command = new RelayCommand(this.Model.Save);
                }
                return this._save_Command;
            }
        }

        #endregion

    }
}
