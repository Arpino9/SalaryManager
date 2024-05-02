using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SalaryManager.Domain.Entities;
using SalaryManager.WPF.Converter;
using SalaryManager.WPF.Models;

namespace SalaryManager.WPF.ViewModels
{
    public class ViewModel_Holiday : INotifyPropertyChanged
    {

        #region Property Changed

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var d = PropertyChanged;
            d?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public ViewModel_Holiday()
        {
            this.Model.ViewModel = this;

            this.Holidays_ItemSource = new ObservableCollection<HolidayEntity>();
            this.CompanyName_ItemSource = new ObservableCollection<CompanyEntity>();

            this.BindEvent();
            this.Model.Initialize();
        }

        /// <summary>
        /// イベント登録
        /// </summary>
        private void BindEvent()
        {
            // 祝日名
            this.Name_TextChanged = new RelayCommand(this.Model.EnableAddButton);
            // 会社休日
            this.CompanyHoliday_Checked = new RelayCommand(this.Model.EnableCompanyNameComboBox);
            // 祝日一覧
            this.Holidays_SelectionChanged = new RelayCommand(this.Model.Holidays_SelectionChanged);
        }

        /// <summary> Model - 自宅 </summary>
        public Model_Holiday Model { get; set; } = new Model_Holiday();

        /// <summary> タイトル </summary>
        public string Title => "祝日マスタ";

        private ObservableCollection<HolidayEntity> _holidays_itemSource;

        /// <summary>
        /// 祝日一覧 - ItemSource
        /// </summary>
        public ObservableCollection<HolidayEntity> Holidays_ItemSource
        {
            get => this._holidays_itemSource;
            set
            {
                this._holidays_itemSource = value;
                this.RaisePropertyChanged();
            }
        }

        private int _holidays_SelectedIndex;

        /// <summary>
        /// 祝日一覧 - SelectedIndex
        /// </summary>
        public int Holidays_SelectedIndex
        {
            get => this._holidays_SelectedIndex;
            set
            {
                this._holidays_SelectedIndex = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 祝日一覧 - SelectionChanged
        /// </summary>
        public RelayCommand Holidays_SelectionChanged { get; private set; }

        private DateTime _date_SelectedDate;

        /// <summary>
        /// 日付 - Text
        /// </summary>
        public DateTime Date_SelectedDate
        {
            get => this._date_SelectedDate;
            set
            {
                this._date_SelectedDate = value;
                this.RaisePropertyChanged();
            }
        }

        private string _name_Text;

        /// <summary>
        /// 祝日名 - Text
        /// </summary>
        public string Name_Text
        {
            get => this._name_Text;
            set
            {
                this._name_Text = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 祝日名 - TextChanged
        /// </summary>
        public RelayCommand Name_TextChanged { get; private set; }

        private bool _companyHoliday_IsChecked;

        /// <summary>
        /// 会社休日 - IsChecked
        /// </summary>
        public bool CompanyHoliday_IsChecked
        {
            get => this._companyHoliday_IsChecked;
            set
            {
                this._companyHoliday_IsChecked = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 会社休日 - Checked
        /// </summary>
        public RelayCommand CompanyHoliday_Checked { get; private set; }

        private bool _companyName_IsEnabled;

        /// <summary>
        /// 会社名 - ItemSource
        /// </summary>
        public bool CompanyName_IsEnabled
        {
            get => this._companyName_IsEnabled;
            set
            {
                this._companyName_IsEnabled = value;
                this.RaisePropertyChanged();
            }
        }

        private ObservableCollection<CompanyEntity> _companyName_ItemSource;

        /// <summary>
        /// 会社名 - ItemSource
        /// </summary>
        public ObservableCollection<CompanyEntity> CompanyName_ItemSource
        {
            get => this._companyName_ItemSource;
            set
            {
                this._companyName_ItemSource = value;
                this.RaisePropertyChanged();
            }
        }

        private int _companyName_SelectedIndex;

        /// <summary>
        /// 会社名 - SelectedIndex
        /// </summary>
        public int CompanyName_SelectedIndex
        {
            get => this._companyName_SelectedIndex;
            set
            {
                this._companyName_SelectedIndex = value;
                this.RaisePropertyChanged();
            }
        }

        private string _companyName_Text;

        /// <summary>
        /// 会社名 - Text
        /// </summary>
        public string CompanyName_Text
        {
            get => this._companyName_Text;
            set
            {
                this._companyName_Text = value;
                this.RaisePropertyChanged();
            }
        }

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

        #region 追加

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

        private RelayCommand _add_Command;

        /// <summary>
        /// 追加ボタン - Command
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

        #endregion

        #region 更新

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

        private RelayCommand _update_Command;

        /// <summary>
        /// 更新ボタン - Command
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

        #endregion

        #region 削除

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

        private RelayCommand _delete_Command;

        /// <summary>
        /// 削除 - Command
        /// </summary>
        public RelayCommand Delete_Command
        {
            get
            {
                if (this._delete_Command == null)
                {
                    this._delete_Command = new RelayCommand(this.Model.Delete);
                }
                return this._delete_Command;
            }
        }

        #endregion
    }
}
