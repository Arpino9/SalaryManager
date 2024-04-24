using SalaryManager.Domain.Entities;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.Converter;
using SalaryManager.WPF.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SalaryManager.WPF.ViewModels
{
    /// <summary>
    /// ViewModel - 自宅
    /// </summary>
    public class ViewModel_Home : INotifyPropertyChanged
    {

        #region Property Changed

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var d = PropertyChanged;
            d?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public ViewModel_Home()
        {
            this.Model.ViewModel = this;

            this.Homes_ItemSource = new ObservableCollection<HomeEntity>();

            BindEvent();

            this.Model.Initialize();
        }

        /// <summary>
        /// イベント登録
        /// </summary>
        private void BindEvent()
        {
            this.Address_Google_TextChanged = new RelayCommand(this.Model.EnableAddButton);

            // 会社一覧
            this.Homes_SelectionChanged = new RelayCommand(this.Model.Homes_SelectionChanged);
            this.IsLiving_Checked       = new RelayCommand(this.Model.IsLiving_Checked);
        }

        /// <summary> Model - 自宅 </summary>
        public Model_Home Model { get; set; } = Model_Home.GetInstance(new HomeSQLite());

        /// <summary> タイトル </summary>
        public string Title => "自宅マスタ";

        private ObservableCollection<HomeEntity> _home_itemSource;

        /// <summary>
        /// 自宅一覧 - ItemSource
        /// </summary>
        public ObservableCollection<HomeEntity> Homes_ItemSource
        {
            get => this._home_itemSource;
            set
            {
                this._home_itemSource = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 自宅一覧 - SelectionChanged
        /// </summary>
        public RelayCommand Homes_SelectionChanged { get; private set; }

        private int _homes_SelectedIndex;

        /// <summary>
        /// 自宅 - SelectedIndex
        /// </summary>
        public int Homes_SelectedIndex
        {
            get => this._homes_SelectedIndex;
            set
            {
                this._homes_SelectedIndex = value;
                this.RaisePropertyChanged();
            }
        }

        private string _displayName_Text;

        /// <summary>
        /// 名称 - Text
        /// </summary>
        public string DisplayName_Text
        {
            get => this._displayName_Text;
            set
            {
                this._displayName_Text = value;
                this.RaisePropertyChanged();
            }
        }

        private string _postCode_Text;

        /// <summary>
        /// 郵便番号 - Text
        /// </summary>
        public string PostCode_Text
        {
            get => this._postCode_Text;
            set
            {
                this._postCode_Text = value;
                this.RaisePropertyChanged();
            }
        }

        private DateTime _livingStart_SelectedDate;

        /// <summary>
        /// 在住期間 - 開始日 - Text
        /// </summary>
        public DateTime LivingStart_SelectedDate
        {
            get => this._livingStart_SelectedDate;
            set
            {
                this._livingStart_SelectedDate = value;
                this.RaisePropertyChanged();
            }
        }

        private DateTime _livingEnd_SelectedDate;

        /// <summary>
        /// 在住期間 - 終了日 - Text
        /// </summary>
        public DateTime LivingEnd_SelectedDate
        {
            get => this._livingEnd_SelectedDate;
            set
            {
                this._livingEnd_SelectedDate = value;
                this.RaisePropertyChanged();
            }
        }

        private bool _isLiving_IsChecked;

        /// <summary>
        /// 住所 - IsChecked
        /// </summary>
        public bool IsLiving_IsChecked
        {
            get => this._isLiving_IsChecked;
            set
            {
                this._isLiving_IsChecked = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 就業中 - Checked
        /// </summary>
        public RelayCommand IsLiving_Checked { get; private set; }

        private string _address_Text;

        /// <summary>
        /// 住所 - Text
        /// </summary>
        public string Address_Text
        {
            get => this._address_Text;
            set
            {
                this._address_Text = value;
                this.RaisePropertyChanged();
            }
        }

        private string _address_Google_Text;

        /// <summary>
        /// 住所 (Google) - Text
        /// </summary>
        public string Address_Google_Text
        {
            get => this._address_Google_Text;
            set
            {
                this._address_Google_Text = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 会社名 - TextChanged
        /// </summary>
        public RelayCommand Address_Google_TextChanged { get; private set; }

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
