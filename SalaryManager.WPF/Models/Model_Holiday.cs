using System;
using System.Collections.Generic;
using System.Linq;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.Domain.Modules.Logics;
using SalaryManager.Domain.StaticValues;
using SalaryManager.Infrastructure.Interface;
using SalaryManager.Infrastructure.JSON;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.ViewModels;

namespace SalaryManager.WPF.Models
{
    public class Model_Holiday : IMaster
    {
        #region Get Instance

        public Model_Holiday()
        {
            
        }

        #endregion

        /// <summary> ViewModel - 職歴 </summary>
        public ViewModel_Holiday ViewModel { get; set; }

        public void Clear_InputForm()
        {
            this.ViewModel.Date_SelectedDate.Value = DateTime.Today;
            this.ViewModel.Name_Text.Value         = string.Empty;
            this.ViewModel.CompanyHoliday_IsChecked.Value = false;
            this.ViewModel.CompanyName_SelectedIndex.Value = 0;
            this.ViewModel.Remarks_Text.Value      = string.Empty;
        }

        public void Initialize()
        {
            this.Clear_InputForm();
            this.Reload();

            Companies.Create(new CompanySQLite());
            this.ViewModel.CompanyName_ItemSource = Companies.FetchByAscending().ToReactiveCollection();

            this.ViewModel.CompanyName_SelectedIndex.Value = 0;

            if (this.ViewModel.Holidays_ItemSource.Any())
            {
                var holiday = this.ViewModel.Holidays_ItemSource[this.ViewModel.Holidays_SelectedIndex.Value];

                this.ViewModel.Date_SelectedDate.Value = holiday.Date;
                this.ViewModel.Name_Text.Value         = holiday.Name;
                this.ViewModel.CompanyHoliday_IsChecked.Value = string.IsNullOrEmpty(holiday.CompanyName) == false;
                this.ViewModel.CompanyName_Text.Value  = holiday.CompanyName;
                this.ViewModel.Remarks_Text.Value      = holiday.Remarks;
            }
        }

        public void Refresh()
        {
            // 追加ボタン
            this.ViewModel.Add_IsEnabled.Value    = false;
            // 更新ボタン
            this.ViewModel.Update_IsEnabled.Value = false;
            // 削除ボタン
            this.ViewModel.Delete_IsEnabled.Value = false;
        }

        /// <summary>
        /// 祝日 - SelectionChanged
        /// </summary>
        public void Holidays_SelectionChanged()
        {
            if (this.ViewModel.Holidays_SelectedIndex.Value == -1)
            {
                return;
            }

            this.EnableControlButton();

            if (!this.ViewModel.Holidays_ItemSource.Any())
            {
                return;
            }

            var entity = this.ViewModel.Holidays_ItemSource[this.ViewModel.Holidays_SelectedIndex.Value];

            // 日付
            this.ViewModel.Date_SelectedDate.Value = entity.Date;
            // 名称
            this.ViewModel.Name_Text.Value         = entity.Name;
            // 会社休日
            this.ViewModel.CompanyHoliday_IsChecked.Value = string.IsNullOrEmpty(entity.CompanyName) == false;
            // 会社名
            this.ViewModel.CompanyName_Text.Value  = entity.CompanyName;
            // 備考
            this.ViewModel.Remarks_Text.Value      = entity.Remarks;
        }

        // <summary>
        /// Enable - 操作ボタン
        /// </summary>
        /// <remarks>
        /// 追加ボタンは「会社名」に値があれば押下可能。
        /// </remarks>
        private void EnableControlButton()
        {
            var selected = this.ViewModel.Holidays_ItemSource.Any()
                        && this.ViewModel.Holidays_SelectedIndex.Value >= 0;

            // 更新ボタン
            this.ViewModel.Update_IsEnabled.Value = selected;
            // 削除ボタン
            this.ViewModel.Delete_IsEnabled.Value = selected;
        }

        /// <summary>
        /// 祝日名 - TextChanged
        /// </summary>
        public void EnableAddButton()
        {
            var inputted = !string.IsNullOrEmpty(this.ViewModel.Name_Text.Value);

            this.ViewModel.Add_IsEnabled.Value = inputted;
        }

        /// <summary>
        /// 会社休日 - Checked
        /// </summary>
        public void EnableCompanyNameComboBox()
        {
            this.ViewModel.CompanyName_IsEnabled.Value = this.ViewModel.CompanyHoliday_IsChecked.Value;
        }

        public void Reload()
        {
            var holidays = JSONExtension.DeserializeSettings<IReadOnlyList<JSONProperty_Holiday>>(FilePath.GetJSONHolidayDefaultPath());

            if (holidays.Any() == false)
            {
                return;
            }

            var list = new List<HolidayEntity>();

            foreach (var holiday in holidays)
            {
                list.Add(new HolidayEntity(holiday.Date, holiday.Name, holiday.CompanyName, holiday.Remarks));
            }

            this.ViewModel.Holidays_ItemSource = list.ToReactiveCollection(this.ViewModel.Holidays_ItemSource);
        }

        public void Save()
        {
            var list = new List<JSONProperty_Holiday>();

            foreach (var holiday in this.ViewModel.Holidays_ItemSource)
            {
                var json = new JSONProperty_Holiday { 
                                                        Date        = holiday.Date, 
                                                        Name        = holiday.Name, 
                                                        CompanyName = holiday.CompanyName,
                                                        Remarks     = holiday.Remarks 
                                                    };

                list.Add(json);
            }

            list.ToArray().SerializeToFile(FilePath.GetJSONHolidayDefaultPath());
        }

        /// <summary>
        /// Create Entity
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>職歴</returns>
        private HolidayEntity CreateEntity()
        {
            return new HolidayEntity(
                this.ViewModel.Date_SelectedDate.Value,
                this.ViewModel.Name_Text.Value,
                this.ViewModel.CompanyName_Text.Value,
                this.ViewModel.Remarks_Text.Value);
        }

        /// <summary>
        /// 追加
        /// </summary>
        public void Add()
        {
            if (!Message.ShowConfirmingMessage($"入力された祝日を追加しますか？", this.ViewModel.Window_Title.Value))
            {
                // キャンセル
                return;
            }

            using (var cursor = new CursorWaiting())
            {
                this.ViewModel.Delete_IsEnabled.Value = true;

                var entity = this.CreateEntity();

                this.ViewModel.Holidays_ItemSource.Add(entity);
                this.Save();

                this.Reload();

                this.ViewModel.Holidays_SelectedIndex.Value = this.ViewModel.Holidays_ItemSource.Count;

                this.Refresh();
                this.Clear_InputForm();
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            if (!Message.ShowConfirmingMessage($"選択中の祝日を更新しますか？", this.ViewModel.Window_Title.Value))
            {
                // キャンセル
                return;
            }

            using (var cursor = new CursorWaiting())
            {
                var entity = this.CreateEntity();
                this.ViewModel.Holidays_ItemSource[this.ViewModel.Holidays_SelectedIndex.Value] = entity;

                this.Save();

                this.Reload();

                this.Refresh();
                this.Clear_InputForm();
            }
        }

        /// <summary>
        /// 削除
        /// </summary>
        public void Delete()
        {
            if (this.ViewModel.Holidays_SelectedIndex.Value == -1 ||
                !this.ViewModel.Holidays_ItemSource.Any())
            {
                return;
            }

            if (!Message.ShowConfirmingMessage($"選択中の祝日を削除しますか？", this.ViewModel.Window_Title.Value))
            {
                // キャンセル
                return;
            }

            using (var cursor = new CursorWaiting())
            {
                this.ViewModel.Holidays_ItemSource.RemoveAt(this.ViewModel.Holidays_SelectedIndex.Value);

                this.Save();
                this.EnableControlButton();

                this.Reload();

                this.Refresh();
                this.Clear_InputForm();
            }
        }
    }
}
