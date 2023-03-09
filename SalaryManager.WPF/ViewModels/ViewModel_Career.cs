using SalaryManager.Domain.Entities;
using SalaryManager.WPF.Converter;
using SalaryManager.WPF.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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

            this.WorkingStatus_ItemSource = new ObservableCollection<string>();
            this.Careers_ItemSource       = new ObservableCollection<CareerEntity>();

            this.Model.Initialize();
        }

        public Model_Career Model { get; set; } = Model_Career.GetInstance();

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

        #endregion

        #region 雇用形態

        private ObservableCollection<string> _workingStatus_itemSource;

        /// <summary>
        /// 雇用形態 - ItemSource
        /// </summary>
        public ObservableCollection<string> WorkingStatus_ItemSource
        {
            get => this._workingStatus_itemSource;
            set
            {
                this._workingStatus_itemSource = value;
                this.RaisePropertyChanged();
            }
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

        private string _companyName;

        /// <summary>
        /// 会社名
        /// </summary>
        public string CompanyName
        {
            get => this._companyName;
            set
            {
                this._companyName = value;
                this.RaisePropertyChanged();
            }
        }

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
        /// 就業中
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

                this.Model.IsWorking_Checked();
            }
        }

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
        /// 皆勤手当
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
        /// 教育手当
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
        /// 在宅手当
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
        /// 資格手当
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
        /// 時間外手当
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
        /// 出張手当
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
        /// 住宅手当
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
        /// 食事手当
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
        /// 深夜手当
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
        /// 地域手当
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
        /// 通勤手当
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
        /// 扶養手当
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
        /// 役職手当
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
        /// 特別手当
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

        private RelayCommand _add;

        /// <summary>
        /// 追加ボタン
        /// </summary>
        public RelayCommand Add_Command
        {
            get
            {
                if (this._add == null)
                {
                    this._add = new RelayCommand(this.Model.Add);
                }
                return this._add;
            }
        }

        #endregion

        #region 削除

        private bool _remove_IsEnabled;

        /// <summary>
        /// 削除 - IsEnabled
        /// </summary>
        public bool Remove_IsEnabled
        {
            get => this._remove_IsEnabled;
            set
            {
                this._remove_IsEnabled = value;
                this.RaisePropertyChanged();
            }
        }

        private RelayCommand _remove;

        /// <summary>
        /// 削除ボタン
        /// </summary>
        public RelayCommand Remove_Command
        {
            get
            {
                if (this._remove == null)
                {
                    this._remove = new RelayCommand(this.Model.Remove);
                }
                return this._remove;
            }
        }

        #endregion

    }
}
