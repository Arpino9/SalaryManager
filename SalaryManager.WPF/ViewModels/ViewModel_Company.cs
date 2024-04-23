using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.Domain.ValueObjects;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.Converter;
using SalaryManager.WPF.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SalaryManager.WPF.ViewModels
{
    [EditorBrowsable(EditorBrowsableState.Always)]
    public class ViewModel_Company : INotifyPropertyChanged
    {

        #region Property Changed

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var d = PropertyChanged;
            d?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public ViewModel_Company()
        {
            this.Model.ViewModel = this;

            this.Companies_ItemSource = new ObservableCollection<CompanyEntity>();

            this.BindEvent();

            this.Model.Initialize();
        }

        /// <summary>
        /// イベント登録
        /// </summary>
        private void BindEvent()
        {
            this.CompanyName_TextChanged    = new RelayCommand(this.Model.EnableAddButton);
            this.Address_Google_TextChanged = new RelayCommand(this.Model.EnableAddButton);

            // 就業中
            this.BusinessCategory_Large_SelectionChanged = new RelayCommand(this.Model.BusinessCategory_Large_SelectionChanged);
            this.BusinessCategory_Middle_SelectionChanged = new RelayCommand(this.Model.BusinessCategory_Middle_SelectionChanged);

            // 会社一覧
            this.Companies_SelectionChanged = new RelayCommand(this.Model.Companies_SelectionChanged);
        }

        public Model_Company Model = Model_Company.GetInstance(new CompanySQLite());

        #region タイトル

        /// <summary> 
        /// タイトル 
        /// </summary>
        public string Title
            => "会社マスタ";

        #endregion

        #region 会社一覧

        private ObservableCollection<CompanyEntity> _companies_itemSource;

        /// <summary>
        /// 会社一覧 - ItemSource
        /// </summary>
        public ObservableCollection<CompanyEntity> Companies_ItemSource
        {
            get => this._companies_itemSource;
            set
            {
                this._companies_itemSource = value;
                this.RaisePropertyChanged();
            }
        }

        private int _companies_SelectedIndex;

        /// <summary>
        /// 会社一覧 - SelectedIndex
        /// </summary>
        public int Companies_SelectedIndex
        {
            get => this._companies_SelectedIndex;
            set
            {
                this._companies_SelectedIndex = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 会社一覧 - SelectionChanged
        /// </summary>
        public RelayCommand Companies_SelectionChanged { get; private set; }

        #endregion

        #region 業種 (大区分)

        /// <summary> 
        /// 業種 (大区分) - ItemsSource
        /// </summary>
        /// <remarks>
        /// 固定なので、値オブジェクトのリストを流用。
        /// </remarks>
        public ObservableCollection<BusinessCategoryValue> BusinessCategory_Large_ItemsSource
            => ListUtils.ToObservableCollection(BusinessCategoryValue.LargeCategory);

        private string _businessCategory_Large_SelectedItem;

        /// <summary>
        /// 業種 (大区分) - SelectedItem
        /// </summary>
        public string BusinessCategory_Large_SelectedItem
        {
            get => this._businessCategory_Large_SelectedItem;
            set
            {
                this._businessCategory_Large_SelectedItem = value;
                this.RaisePropertyChanged();
            }
        }

        private int _businessCategory_Large_SelectedIndex;

        /// <summary>
        /// 業種 (大区分) - SelectedIndex
        /// </summary>
        public int BusinessCategory_Large_SelectedIndex
        {
            get => this._businessCategory_Large_SelectedIndex;
            set
            {
                this._businessCategory_Large_SelectedIndex = value;
                this.RaisePropertyChanged();
            }
        }

        private string _businessCategory_Large_Text;

        /// <summary>
        /// 業種 (大区分) - Text
        /// </summary>
        public string BusinessCategory_Large_Text
        {
            get => this._businessCategory_Large_Text;
            set
            {
                this._businessCategory_Large_Text = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 業種 (大区分) - TextChanged
        /// </summary>
        public RelayCommand BusinessCategory_Large_SelectionChanged { get; private set; }

        #endregion

        #region 業種 (中区分)

        private ObservableCollection<string> _BusinessCategory_Middle_ItemSource;

        /// <summary>
        /// 業種 (中区分) - TextChanged
        /// </summary>
        public ObservableCollection<string> BusinessCategory_Middle_ItemSource
        {
            get => this._BusinessCategory_Middle_ItemSource;
            set
            {
                this._BusinessCategory_Middle_ItemSource = value;
                this.RaisePropertyChanged();
            }
        }

        private int _businessCategory_Middle_SelectedIndex;

        /// <summary>
        /// 業種 (中区分) - SelectedIndex
        /// </summary>
        public int BusinessCategory_Middle_SelectedIndex
        {
            get => this._businessCategory_Middle_SelectedIndex;
            set
            {
                this._businessCategory_Middle_SelectedIndex = value;
                this.RaisePropertyChanged();
            }
        }

        private string _businessCategory_Middle_SelectedItem;

        /// <summary>
        /// 業種 (中区分) - SelectedItem
        /// </summary>
        public string BusinessCategory_Middle_SelectedItem
        {
            get => this._businessCategory_Middle_SelectedItem;
            set
            {
                this._businessCategory_Middle_SelectedItem = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary> 業種 (中区分) No </summary>
        public string BusinessCategory_MiddleNo;

        private string _businessCategory_Middle_Text;

        /// <summary>
        /// 業種 (中区分) - Text
        /// </summary>
        public string BusinessCategory_Middle_Text
        {
            get => this._businessCategory_Middle_Text;
            set
            {
                this._businessCategory_Middle_Text = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 業種 (中区分) - TextChanged
        /// </summary>
        public RelayCommand BusinessCategory_Middle_SelectionChanged { get; private set; }

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

        #region 郵便番号

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

        #endregion

        #region 住所

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
        /// 住所(Googleカレンダー登録用) - Text
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
        /// 住所(Googleカレンダー登録用) - TextChanged
        /// </summary>
        public RelayCommand Address_Google_TextChanged { get; private set; }

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
