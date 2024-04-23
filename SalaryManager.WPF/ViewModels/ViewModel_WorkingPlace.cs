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
    public class ViewModel_WorkingPlace : INotifyPropertyChanged
    {
        #region Property Changed

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var d = PropertyChanged;
            d?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public ViewModel_WorkingPlace()
        {
            this.Model.ViewModel = this;

            this.WorkingPlaces_ItemSource = new ObservableCollection<WorkingPlaceEntity>();

            this.Model.Initialize();

            this.BindEvent();
        }

        /// <summary>
        /// イベント登録
        /// </summary>
        private void BindEvent()
        {
            // 就業場所
            this.WorkingPlace_TextChanged       = new RelayCommand(this.Model.SearchAddress);
            // 住所
            this.Address_TextChanged            = new RelayCommand(this.Model.EnableAddButton);
            // 経歴一覧
            this.WorkingPlaces_SelectionChanged = new RelayCommand(this.Model.Careers_SelectionChanged);
        }

        public Model_WorkingPlace Model { get; set; } = Model_WorkingPlace.GetInstance(new WorkingPlaceSQLite());

        public IReadOnlyList<WorkingPlaceEntity> Entities { get; internal set; }

        #region タイトル

        /// <summary>
        /// タイトル
        /// </summary>
        public string Title => "就業場所登録";

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

        #region 就業場所一覧

        private ObservableCollection<WorkingPlaceEntity> _workingPlaces_itemSource;

        /// <summary>
        /// 就業場所一覧 - ItemSource
        /// </summary>
        public ObservableCollection<WorkingPlaceEntity> WorkingPlaces_ItemSource
        {
            get => this._workingPlaces_itemSource;
            set
            {
                this._workingPlaces_itemSource = value;
                this.RaisePropertyChanged();
            }
        }

        private int _workingPlaces_SelectedIndex;

        /// <summary>
        /// 就業場所一覧 - SelectedIndex
        /// </summary>
        public int WorkingPlaces_SelectedIndex
        {
            get => this._workingPlaces_SelectedIndex;
            set
            {
                this._workingPlaces_SelectedIndex = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 就業場所一覧 - SelectionChanged
        /// </summary>
        public RelayCommand WorkingPlaces_SelectionChanged { get; private set; }

        #endregion

        #region 会社名

        private ObservableCollection<string> _companyName_ItemSource;

        /// <summary>
        /// 会社名 - ItemSource
        /// </summary>
        public ObservableCollection<string> CompanyName_ItemSource
        {
            get => this._companyName_ItemSource;
            set
            {
                this._companyName_ItemSource = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 派遣元会社名

        private string _dispacthingCompanyName_Text;

        /// <summary>
        /// 派遣元会社名 - Text
        /// </summary>
        public string DispatchingCompanyName_Text
        {
            get => this._dispacthingCompanyName_Text;
            set
            {
                this._dispacthingCompanyName_Text = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 会社名 - TextChanged
        /// </summary>
        public RelayCommand CompanyName_TextChanged { get; private set; }

        #endregion

        #region 派遣先会社名

        private string _dispacthedCompanyName_Text;

        /// <summary>
        /// 派遣先会社名 - Text
        /// </summary>
        public string DispatchedCompanyName_Text
        {
            get => this._dispacthedCompanyName_Text;
            set
            {
                this._dispacthedCompanyName_Text = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 会社名

        private ObservableCollection<string> _workingPlace_ItemSource;

        /// <summary>
        /// 就業場所 - ItemSource
        /// </summary>
        public ObservableCollection<string> WorkingPlace_ItemSource
        {
            get => this._workingPlace_ItemSource;
            set
            {
                this._workingPlace_ItemSource = value;
                this.RaisePropertyChanged();
            }
        }

        private string _workingPlace_Name_SelectedItem;

        /// <summary>
        /// 就業場所 - SelectedItem
        /// </summary>
        public string WorkingPlace_Name_SelectedItem
        {
            get => this._workingPlace_Name_SelectedItem;
            set
            {
                this._workingPlace_Name_SelectedItem = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 就業場所 - TextChanged
        /// </summary>
        public RelayCommand WorkingPlace_TextChanged { get; private set; }

        #endregion

        #region 住所

        private string _workingPlace_Address_Text;

        /// <summary>
        /// 住所 - Text
        /// </summary>
        public string WorkingPlace_Address_Text
        {
            get => this._workingPlace_Address_Text;
            set
            {
                this._workingPlace_Address_Text = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 住所 - TextChanged
        /// </summary>
        public RelayCommand Address_TextChanged { get; private set; }

        #endregion

        #region 労働 - 開始

        private int _workingTime_Start_Hour;

        /// <summary>
        /// 労働 - 開始 - 時
        /// </summary>
        public int WorkingTime_Start_Hour
        {
            get => this._workingTime_Start_Hour;
            set
            {
                this._workingTime_Start_Hour = value;
                this.RaisePropertyChanged();
            }
        }

        private int _workingTime_Start_Minute;

        /// <summary>
        /// 労働時間 - 開始 - 分
        /// </summary>
        public int WorkingTime_Start_Minute
        {
            get => this._workingTime_Start_Minute;
            set
            {
                this._workingTime_Start_Minute = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 労働 - 終了

        private int _workingTime_End_Hour;

        /// <summary>
        /// 労働時間 - 終了 - 時
        /// </summary>
        public int WorkingTime_End_Hour
        {
            get => this._workingTime_End_Hour;
            set
            {
                this._workingTime_End_Hour = value;
                this.RaisePropertyChanged();
            }
        }

        private int _workingTime_End_Minute;

        /// <summary>
        /// 労働時間 - 終了 - 分
        /// </summary>
        public int WorkingTime_End_Minute
        {
            get => this._workingTime_End_Minute;
            set
            {
                this._workingTime_End_Minute = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 昼休憩 - 開始

        private int _lunchTime_Start_Hour;

        /// <summary>
        /// 昼休憩 - 開始 - 時
        /// </summary>
        public int LunchTime_Start_Hour
        {
            get => this._lunchTime_Start_Hour;
            set
            {
                this._lunchTime_Start_Hour = value;
                this.RaisePropertyChanged();
            }
        }

        private int _lunchTime_Start_Minute;

        /// <summary>
        /// 昼休憩 - 開始 - 分
        /// </summary>
        public int LunchTime_Start_Minute
        {
            get => this._lunchTime_Start_Minute;
            set
            {
                this._lunchTime_Start_Minute = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 昼休憩 - 終了

        private int _lunchTime_End_Hour;

        /// <summary>
        /// 昼休憩 - 終了 - 時
        /// </summary>
        public int LunchTime_End_Hour
        {
            get => this._lunchTime_End_Hour;
            set
            {
                this._lunchTime_End_Hour = value;
                this.RaisePropertyChanged();
            }
        }

        private int _lunchTime_End_Minute;

        /// <summary>
        /// 昼休憩 - 終了 - 分
        /// </summary>
        public int LunchTime_End_Minute
        {
            get => this._lunchTime_End_Minute;
            set
            {
                this._lunchTime_End_Minute = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 休憩 - 開始

        private int _breakTime_Start_Hour;

        /// <summary>
        /// 昼休憩 - 開始 - 時
        /// </summary>
        public int BreakTime_Start_Hour
        {
            get => this._breakTime_Start_Hour;
            set
            {
                this._breakTime_Start_Hour = value;
                this.RaisePropertyChanged();
            }
        }

        private int _breakTime_Start_Minute;

        /// <summary>
        /// 昼休憩 - 開始 - 時
        /// </summary>
        public int BreakTime_Start_Minute
        {
            get => this._breakTime_Start_Minute;
            set
            {
                this._breakTime_Start_Minute = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 休憩 - 終了

        private int _breakTime_End_Hour;

        /// <summary>
        /// 休憩 - 終了 - 時
        /// </summary>
        public int BreakTime_End_Hour
        {
            get => this._breakTime_End_Hour;
            set
            {
                this._breakTime_End_Hour = value;
                this.RaisePropertyChanged();
            }
        }

        private int _breakTime_End_Minute;

        /// <summary>
        /// 休憩 - 終了 - 分
        /// </summary>
        public int BreakTime_End_Minute
        {
            get => this._breakTime_End_Minute;
            set
            {
                this._breakTime_End_Minute = value;
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
