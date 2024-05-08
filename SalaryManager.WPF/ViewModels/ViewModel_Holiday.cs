﻿using System;
using System.Collections.ObjectModel;
using Reactive.Bindings;
using SalaryManager.Domain.Entities;
using SalaryManager.WPF.Models;

namespace SalaryManager.WPF.ViewModels
{
    public class ViewModel_Holiday
    {
        public ViewModel_Holiday()
        {
            this.Model.ViewModel = this;

            this.Holidays_ItemSource.Value    = new ObservableCollection<HolidayEntity>();
            this.CompanyName_ItemSource.Value = new ObservableCollection<CompanyEntity>();

            this.BindEvent();
            this.Model.Initialize();
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
            // 祝日名
            this.Name_TextChanged.Subscribe(_ => Model.EnableAddButton());
            // 会社休日
            this.CompanyHoliday_Checked.Subscribe(_ => Model.EnableCompanyNameComboBox());
            // 祝日一覧
            this.Holidays_SelectionChanged.Subscribe(_ => Model.Holidays_SelectionChanged());

            this.Add_Command.Subscribe(_ => Model.Add());
            this.Update_Command.Subscribe(_ => Model.Update());
            this.Delete_Command.Subscribe(_ => Model.Delete());
        }

        /// <summary> Model - 自宅 </summary>
        public Model_Holiday Model { get; set; } 
            = new Model_Holiday();

        #region Window

        /// <summary> Window - Title </summary>
        public ReactiveProperty<string> Window_Title { get; }
            = new ReactiveProperty<string>("祝日マスタ");

        #endregion

        #region 祝日一覧

        /// <summary> 祝日一覧 - ItemSource </summary>
        public ReactiveProperty<ObservableCollection<HolidayEntity>> Holidays_ItemSource { get; }
            = new ReactiveProperty<ObservableCollection<HolidayEntity>>();

        /// <summary> 祝日一覧 - SelectedIndex </summary>
        public ReactiveProperty<int> Holidays_SelectedIndex { get; }
            = new ReactiveProperty<int>();

        /// <summary> 祝日一覧 - SelectionChanged </summary>
        public ReactiveCommand Holidays_SelectionChanged { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 日付

        /// <summary> 日付 - SelectedDate </summary>
        public ReactiveProperty<DateTime> Date_SelectedDate { get; }
            = new ReactiveProperty<DateTime>();

        #endregion

        #region 祝日名

        /// <summary> 祝日名 - Text </summary>
        public ReactiveProperty<string> Name_Text { get; }
            = new ReactiveProperty<string>();

        /// <summary> 祝日名 - TextChanged </summary>
        public ReactiveCommand Name_TextChanged { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 会社休日設定

        /// <summary> 会社休日 - IsChecked </summary>
        public ReactiveProperty<bool> CompanyHoliday_IsChecked { get; }
            = new ReactiveProperty<bool>();

        /// <summary> 会社休日 - Checked </summary>
        public ReactiveCommand CompanyHoliday_Checked { get; private set; }
            = new ReactiveCommand();

        /// <summary> 会社名 - IsEnabled </summary>
        public ReactiveProperty<bool> CompanyName_IsEnabled { get; }
            = new ReactiveProperty<bool>();

        /// <summary> 会社名 - ItemSource </summary>
        public ReactiveProperty<ObservableCollection<CompanyEntity>> CompanyName_ItemSource { get; }
            = new ReactiveProperty<ObservableCollection<CompanyEntity>>();

        /// <summary> 会社名 - SelectedIndex </summary>
        public ReactiveProperty<int> CompanyName_SelectedIndex { get; }
            = new ReactiveProperty<int>();

        /// <summary> 会社名 - Text </summary>
        public ReactiveProperty<string> CompanyName_Text { get; }
            = new ReactiveProperty<string>();

        #endregion

        #region 備考

        /// <summary> 備考 - Text </summary>
        public ReactiveProperty<string> Remarks_Text { get; }
            = new ReactiveProperty<string>();

        #endregion

        #region 追加

        /// <summary> 追加 - IsEnabled </summary>
        public ReactiveProperty<bool> Add_IsEnabled { get; }
            = new ReactiveProperty<bool>();

        /// <summary> 追加 - Command </summary>
        public ReactiveCommand Add_Command { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 更新

        /// <summary> 更新 - IsEnabled </summary>
        public ReactiveProperty<bool> Update_IsEnabled { get; }
            = new ReactiveProperty<bool>();

        /// <summary> 更新 - Command </summary>
        public ReactiveCommand Update_Command { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 削除

        /// <summary> 削除 - IsEnabled </summary>
        public ReactiveProperty<bool> Delete_IsEnabled { get; }
            = new ReactiveProperty<bool>();

        /// <summary> 削除 - Command </summary>
        public ReactiveCommand Delete_Command { get; private set; }
            = new ReactiveCommand();

        #endregion

    }
}
