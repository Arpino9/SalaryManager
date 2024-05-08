using System;
using System.Collections.ObjectModel;
using Reactive.Bindings;
using SalaryManager.Domain.Entities;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.Models;

namespace SalaryManager.WPF.ViewModels
{
    /// <summary>
    /// ViewModel - 自宅
    /// </summary>
    public class ViewModel_Home
    {
        public ViewModel_Home()
        {
            this.Model.ViewModel = this;

            this.Model.Initialize();
            
            this.BindEvent();
        }

        /// <summary>
        /// イベント登録
        /// </summary>
        /// <remarks>
        /// Viewの指定したイベントと、発火させるメソッドを紐付ける。
        /// Subscribe()メソッドのオーバーロードが正しく呼ばれないので、
        /// 名前空間に「using System;」を必ず入れること。
        /// </remarks>
        private void BindEvent()
        {
            this.Homes_SelectionChanged.Subscribe(_ => this.Model.Homes_SelectionChanged());
            this.IsLiving_Checked.Subscribe(_ => this.Model.IsLiving_Checked());
            this.Address_Google_TextChanged.Subscribe(_ => this.Model.EnableAddButton());

            this.Add_Command.Subscribe(_ => this.Model.Add());
            this.Update_Command.Subscribe(_ => this.Model.Update());
            this.Delete_Command.Subscribe(_ => this.Model.Delete());
        }

        /// <summary> Model - 自宅 </summary>
        public Model_Home Model { get; set; } 
            = Model_Home.GetInstance(new HomeSQLite());

        #region Window

        /// <summary> Window - Title </summary>
        public ReactiveProperty<string> Window_Title { get; }
            = new ReactiveProperty<string>("自宅マスタ");

        #endregion

        #region 自宅一覧

        /// <summary> 自宅一覧 - ItemSource </summary>
        public ReactiveProperty<ObservableCollection<HomeEntity>> Homes_ItemSource { get; set; }
            = new ReactiveProperty<ObservableCollection<HomeEntity>>();

        /// <summary> 自宅一覧 - SelectionChanged </summary>
        public ReactiveCommand Homes_SelectionChanged { get; private set; }
            = new ReactiveCommand();

        /// <summary> 自宅 - SelectedIndex </summary>
        public ReactiveProperty<int> Homes_SelectedIndex { get; set; }
            = new ReactiveProperty<int>();

        #endregion

        #region 名称

        /// <summary> 名称 - Text </summary>
        public ReactiveProperty<string> DisplayName_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 郵便番号

        /// <summary> 郵便番号 - Text </summary>
        public ReactiveProperty<string> PostCode_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 在住期間

        /// <summary> 在住期間 - 開始日 - Text </summary>
        public ReactiveProperty<DateTime> LivingStart_SelectedDate { get; set; }
            = new ReactiveProperty<DateTime>();

        /// <summary> 在住期間 - 終了日 - Text </summary>
        public ReactiveProperty<DateTime> LivingEnd_SelectedDate { get; set; }
            = new ReactiveProperty<DateTime>();

        #endregion

        #region 就業中

        /// <summary> 就業中 - IsChecked </summary>
        public ReactiveProperty<bool> IsLiving_IsChecked { get; set; }
            = new ReactiveProperty<bool>();

        /// <summary> 就業中 - Checked </summary>
        public ReactiveCommand IsLiving_Checked { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 住所

        /// <summary> 住所 - Text </summary>
        public ReactiveProperty<string> Address_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 住所 (Google) - Text </summary>
        public ReactiveProperty<string> Address_Google_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 住所 (Google) - TextChanged </summary>
        public ReactiveProperty<bool> Address_Google_TextChanged { get; set; }
            = new ReactiveProperty<bool>();

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
