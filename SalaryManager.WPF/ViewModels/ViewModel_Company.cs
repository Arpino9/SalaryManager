using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.Domain.ValueObjects;
using SalaryManager.WPF.Converter;
using SalaryManager.WPF.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

            this.BindEvent();

            this.Model.Initialize();
        }

        /// <summary>
        /// イベント登録
        /// </summary>
        private void BindEvent()
        {
            // 就業中
            this.BusinessCategory_Large_SelectionChanged = new RelayCommand(this.Model.BusinessCategory_Large_SelectionChanged);
        }

        public Model_Company Model = new Model_Company();

        #region 業種 (大区分)

        /// <summary> 
        /// 業種 (大区分) - ItemsSource
        /// </summary>
        public ObservableCollection<BusinessCategoryValue> BusinessCategory_Large_ItemsSource
            => ListUtils.ToObservableCollection(BusinessCategoryValue.LargeCategory);

        private string _BusinessCategory_Large_SelectedItem;

        /// <summary>
        /// 業種 (大区分) - SelectedItem
        /// </summary>
        public string BusinessCategory_Large_SelectedItem
        {
            get => this._BusinessCategory_Large_SelectedItem;
            set
            {
                this._BusinessCategory_Large_SelectedItem = value;
                this.RaisePropertyChanged();
            }
        }

        private int _BusinessCategory_Large_SelectedIndex;

        /// <summary>
        /// 業種 (大区分) - SelectedIndex
        /// </summary>
        public int BusinessCategory_Large_SelectedIndex
        {
            get => this._BusinessCategory_Large_SelectedIndex;
            set
            {
                this._BusinessCategory_Large_SelectedIndex = value;
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

        private int _BusinessCategory_Middle_SelectedIndex;

        /// <summary>
        /// 業種 (中区分) - SelectedIndex
        /// </summary>
        public int BusinessCategory_Middle_SelectedIndex
        {
            get => this._BusinessCategory_Middle_SelectedIndex;
            set
            {
                this._BusinessCategory_Middle_SelectedIndex = value;
                this.RaisePropertyChanged();
            }
        }

        private string _BusinessCategory_Middle_Text;

        /// <summary>
        /// 業種 (中区分) - Text
        /// </summary>
        public string BusinessCategory_Middle_Text
        {
            get => this._BusinessCategory_Middle_Text;
            set
            {
                this._BusinessCategory_Middle_Text = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary> 業種 (中区分) No </summary>
        public string BusinessCategory_MiddleNo;

        #endregion

    }
}
