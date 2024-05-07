using System;
using Reactive.Bindings;
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

            this.Companies_ItemSource.Value = new ObservableCollection<CompanyEntity>();

            this.BindEvent();

            this.Model.Initialize();
        }

        /// <summary>
        /// イベント登録
        /// </summary>
        private void BindEvent()
        {
            // 会社名
            this.CompanyName_TextChanged.Subscribe(_ => this.Model.EnableAddButton());
            // 住所
            this.Address_Google_TextChanged.Subscribe(_ => this.Model.EnableAddButton());
            // 業種
            this.BusinessCategory_Large_SelectionChanged.Subscribe(_ => this.Model.BusinessCategory_Large_SelectionChanged());
            
            this.Add_Command.Subscribe(_ => this.Model.Add());
            this.Update_Command.Subscribe(_ => this.Model.Update());
            this.Delete_Command.Subscribe(_ => this.Model.Delete());
            
            // 会社一覧
            this.Companies_SelectionChanged = new RelayCommand(this.Model.Companies_SelectionChanged);
        }

        /// <summary> Model - 会社 </summary>
        public Model_Company Model = Model_Company.GetInstance(new CompanySQLite());

        #region タイトル

        /// <summary> Window - Title </summary>
        public ReactiveProperty<string> Window_Title { get; }
            = new ReactiveProperty<string>("会社マスタ");

        #endregion

        #region 会社一覧

        /// <summary> 会社一覧 - ItemSource </summary>
        public ReactiveProperty<ObservableCollection<CompanyEntity>> Companies_ItemSource { get; set; }
            = new ReactiveProperty<ObservableCollection<CompanyEntity>>();

        /// <summary> 会社一覧 - SelectedIndex </summary>
        public ReactiveProperty<int> Companies_SelectedIndex { get; set; }
            = new ReactiveProperty<int>();

        /// <summary>
        /// 会社一覧 - SelectionChanged
        /// </summary>
        public RelayCommand Companies_SelectionChanged { get; private set; }

        #endregion

        #region 業種 (大区分)

        // TODO:
        /// <summary> 
        /// 業種 (大区分) - ItemsSource
        /// </summary>
        /// <remarks>
        /// 固定なので、値オブジェクトのリストを流用。
        /// </remarks>
        public ObservableCollection<BusinessCategoryValue> BusinessCategory_Large_ItemsSource
            => ListUtils.ToObservableCollection(BusinessCategoryValue.LargeCategory);

        /// <summary> 業種 (大区分) - Text </summary>
        public ReactiveProperty<string> BusinessCategory_Large_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 業種 (大区分) - SelectionChanged </summary>
        public ReactiveCommand BusinessCategory_Large_SelectionChanged { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 業種 (中区分)

        // TODO:
        /*/// <summary> 業種 (中区分) - ItemSource </summary>
        public ReactiveProperty<ObservableCollection<string>> BusinessCategory_Middle_ItemSource { get; set; }
            = new ReactiveProperty<ObservableCollection<string>>();*/

        private ObservableCollection<string> _BusinessCategory_Middle_ItemSource;

        /// <summary>
        /// 業種 (中区分) - ItemSource
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

        /// <summary> 業種 (中区分) - Text </summary>
        public ReactiveProperty<string> BusinessCategory_Middle_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 会社名

        /// <summary> 会社名 - Text </summary>
        public ReactiveProperty<string> CompanyName_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 会社名 - TextChanged </summary>
        public ReactiveCommand CompanyName_TextChanged { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 郵便番号

        /// <summary> 郵便番号 - Text </summary>
        public ReactiveProperty<string> PostCode_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 住所

        /// <summary> 住所 - Text </summary>
        public ReactiveProperty<string> Address_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 住所(Googleカレンダー登録用) - Text </summary>
        public ReactiveProperty<string> Address_Google_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary>
        /// 住所(Googleカレンダー登録用) - TextChanged
        /// </summary>
        //public RelayCommand Address_Google_TextChanged { get; private set; }

        /// <summary> 住所(Googleカレンダー登録用) - TextChanged </summary>
        public ReactiveCommand Address_Google_TextChanged { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 備考

        /// <summary> 備考 - Text </summary>
        public ReactiveProperty<string> Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 追加

        /// <summary> 追加 - IsEnabled </summary>
        public ReactiveProperty<bool> Add_IsEnabled { get; set; }
            = new ReactiveProperty<bool>();

        /// <summary> 追加 - Command </summary>
        public ReactiveCommand Add_Command { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 更新

        /// <summary> 更新 - IsEnabled </summary>
        public ReactiveProperty<bool> Update_IsEnabled { get; set; }
            = new ReactiveProperty<bool>();

        /// <summary> 更新 - Command </summary>
        public ReactiveCommand Update_Command { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 削除

        /// <summary> 削除 - IsEnabled </summary>
        public ReactiveProperty<bool> Delete_IsEnabled { get; set; }
            = new ReactiveProperty<bool>();

        /// <summary> 削除 - Command </summary>
        public ReactiveCommand Delete_Command { get; private set; }
            = new ReactiveCommand();

        #endregion

    }
}
