using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SalaryManager.Domain.Entities;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.Converter;
using SalaryManager.WPF.Models;

namespace SalaryManager.WPF.ViewModels
{
    [EditorBrowsable(EditorBrowsableState.Always)]
    public class ViewModel_Career : INotifyPropertyChanged
    {
        #region Property Changed

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var d = PropertyChanged;
            d?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public ViewModel_Career()
        {
            this.Model.ViewModel = this;

            this.Careers_ItemSource = new ObservableCollection<CareerEntity>();

            this.Model.Initialize();

            this.BindEvent();
        }

        /// <summary>
        /// イベント登録
        /// </summary>
        private void BindEvent()
        {
            // 就業中
            this.IsWorking_Checked        = new RelayCommand(this.Model.IsWorking_Checked);
            // 会社名
            this.CompanyName_TextChanged  = new RelayCommand(this.Model.EnableAddButton);
            // 経歴一覧
            this.Careers_SelectionChanged = new RelayCommand(this.Model.Careers_SelectionChanged);
        }

        public Model_Career Model { get; set; } = Model_Career.GetInstance(new CareerSQLite());

        public IReadOnlyList<CareerEntity> Entities { get; internal set; }

        #region タイトル

        /// <summary>
        /// タイトル
        /// </summary>
        public string Title
        {
            get => "経歴編集";
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

        #region 職歴一覧

        private ObservableCollection<CareerEntity> _careers_itemSource;

        /// <summary>
        /// 職歴一覧 - ItemSource
        /// </summary>
        public ObservableCollection<CareerEntity> Careers_ItemSource
        {
            get => this._careers_itemSource;
            set
            {
                this._careers_itemSource = value;
                this.RaisePropertyChanged();
            }
        }

        private int _careers_SelectedIndex;

        /// <summary>
        /// 職歴一覧 - SelectedIndex
        /// </summary>
        public int Careers_SelectedIndex
        {
            get => this._careers_SelectedIndex;
            set
            {
                this._careers_SelectedIndex = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 職歴一覧 - SelectionChanged
        /// </summary>
        public RelayCommand Careers_SelectionChanged { get; private set; }

        #endregion

        #region 雇用形態

        /// <summary>
        /// 雇用形態 - ItemSource
        /// </summary>
        public ObservableCollection<string> WorkingStatus_ItemSource
        {
            get => new ObservableCollection<string> {"正社員", "契約社員", "派遣社員", "業務委託", "アルバイト"};
        }

        private int _workingStatus_SelectedIndex;

        /// <summary>
        /// 雇用形態 - SelectedIndex
        /// </summary>
        public int WorkingStatus_SelectedIndex
        {
            get => this._workingStatus_SelectedIndex;
            set
            {
                this._workingStatus_SelectedIndex = value;
                this.RaisePropertyChanged();
            }
        }

        private string _workingStatus_Text;

        /// <summary>
        /// 雇用形態 - Text
        /// </summary>
        public string WorkingStatus_Text
        {
            get => this._workingStatus_Text;
            set
            {
                this._workingStatus_Text = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 会社名

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

        /// <summary>
        /// 会社名 - TextChanged
        /// </summary>
        public RelayCommand CompanyName_TextChanged { get; private set; }

        #endregion

        #region 勤務開始日

        private DateTime _workingStartDate;

        /// <summary>
        /// 勤務開始日
        /// </summary>
        public DateTime WorkingStartDate
        {
            get => this._workingStartDate;
            set
            {
                this._workingStartDate = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 勤務終了日

        private DateTime _workingEndDate;

        /// <summary>
        /// 勤務開始日
        /// </summary>
        public DateTime WorkingEndDate
        {
            get => this._workingEndDate;
            set
            {
                this._workingEndDate = value;
                this.RaisePropertyChanged();
            }
        }

        private bool _isWorking_IsEnabled;

        /// <summary>
        /// 就業中 - IsEnabled
        /// </summary>
        public bool WorkingEndDate_IsEnabled
        {
            get => this._isWorking_IsEnabled;
            set
            {
                this._isWorking_IsEnabled = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 就業中

        private bool _isWorking;

        /// <summary>
        /// 就業中
        /// </summary>
        public bool IsWorking
        {
            get => this._isWorking;
            set
            {
                this._isWorking = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 就業中 - Checked
        /// </summary>
        public RelayCommand IsWorking_Checked { get; private set; }

        #endregion

        #region 社員番号

        private string _employeeNumber;

        /// <summary>
        /// 社員番号
        /// </summary>
        public string EmployeeNumber
        {
            get => this._employeeNumber;
            set
            {
                this._employeeNumber = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 皆勤手当
        
        private bool _perfectAttendanceAllowance_IsChecked;

        /// <summary>
        /// 皆勤手当 - IsChecked
        /// </summary>
        public bool PerfectAttendanceAllowance_IsChecked
        {
            get => this._perfectAttendanceAllowance_IsChecked;
            set
            {
                this._perfectAttendanceAllowance_IsChecked = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 教育手当

        private bool _educationAllowance_IsChecked;

        /// <summary>
        /// 教育手当 - IsChecked
        /// </summary>
        public bool EducationAllowance_IsChecked
        {
            get => this._educationAllowance_IsChecked;
            set
            {
                this._educationAllowance_IsChecked = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 在宅手当

        private bool _electricityAllowance_IsChecked;

        /// <summary>
        /// 在宅手当 - IsChecked
        /// </summary>
        public bool ElectricityAllowance_IsChecked
        {
            get => this._electricityAllowance_IsChecked;
            set
            {
                this._electricityAllowance_IsChecked = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 資格手当

        private bool _certificationAllowance_IsChecked;

        /// <summary>
        /// 資格手当 - IsChecked
        /// </summary>
        public bool CertificationAllowance_IsChecked
        {
            get => this._certificationAllowance_IsChecked;
            set
            {
                this._certificationAllowance_IsChecked = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 時間外手当

        private bool _overtimeAllowance_IsChecked;

        /// <summary>
        /// 時間外手当 - IsChecked
        /// </summary>
        public bool OvertimeAllowance_IsChecked
        {
            get => this._overtimeAllowance_IsChecked;
            set
            {
                this._overtimeAllowance_IsChecked = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 出張手当

        private bool _travelAllowance_IsChecked;

        /// <summary>
        /// 出張手当 - IsChecked
        /// </summary>
        public bool TravelAllowance_IsChecked
        {
            get => this._travelAllowance_IsChecked;
            set
            {
                this._travelAllowance_IsChecked = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 住宅手当

        private bool _housingAllowance_IsChecked;

        /// <summary>
        /// 住宅手当 - IsChecked
        /// </summary>
        public bool HousingAllowance_IsChecked
        {
            get => this._housingAllowance_IsChecked;
            set
            {
                this._housingAllowance_IsChecked = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 食事手当

        private bool _foodAllowance_IsChecked;

        /// <summary>
        /// 食事手当 - IsChecked
        /// </summary>
        public bool FoodAllowance_IsChecked
        {
            get => this._foodAllowance_IsChecked;
            set
            {
                this._foodAllowance_IsChecked = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 深夜手当

        private bool _lateNightAllowance_IsChecked;

        /// <summary>
        /// 深夜手当 - IsChecked
        /// </summary>
        public bool LateNightAllowance_IsChecked
        {
            get => this._lateNightAllowance_IsChecked;
            set
            {
                this._lateNightAllowance_IsChecked = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 地域手当

        private bool _areaAllowance_IsChecked;

        /// <summary>
        /// 地域手当 - IsChecked
        /// </summary>
        public bool AreaAllowance_IsChecked
        {
            get => this._areaAllowance_IsChecked;
            set
            {
                this._areaAllowance_IsChecked = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 通勤手当

        private bool _commutionAllowance_IsChecked;

        /// <summary>
        /// 通勤手当 - IsChecked
        /// </summary>
        public bool CommutingAllowance_IsChecked
        {
            get => this._commutionAllowance_IsChecked;
            set
            {
                this._commutionAllowance_IsChecked = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 扶養手当

        private bool _dependencyAllowance_IsChecked;

        /// <summary>
        /// 扶養手当 - IsCecked
        /// </summary>
        public bool DependencyAllowance_IsChecked
        {
            get => this._dependencyAllowance_IsChecked;
            set
            {
                this._dependencyAllowance_IsChecked = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 役職手当

        private bool _executiveAllowance_IsChecked;

        /// <summary>
        /// 役職手当 - IsChecked
        /// </summary>
        public bool ExecutiveAllowance_IsChecked
        {
            get => this._executiveAllowance_IsChecked;
            set
            {
                this._executiveAllowance_IsChecked = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 特別手当

        private bool _specialAllowance_IsChecked;

        /// <summary>
        /// 特別手当 - IsChecked
        /// </summary>
        public bool SpecialAllowance_IsChecked
        {
            get => this._specialAllowance_IsChecked;
            set
            {
                this._specialAllowance_IsChecked = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 備考

        private string _remarks;

        /// <summary>
        /// 備考
        /// </summary>
        public string Remarks
        {
            get => this._remarks;
            set
            {
                this._remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

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
