using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.Domain.Modules.Logics;
using SalaryManager.Infrastructure.Interface;
using SalaryManager.Infrastructure.JSON;
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
            this.ViewModel.Date_SelectedDate = DateTime.Today;
            this.ViewModel.Name_Text         = string.Empty;
            this.ViewModel.Remarks_Text      = string.Empty;
        }

        public void Initialize()
        {
            this.Clear_InputForm();
            this.Reload();

            if (this.ViewModel.Holidays_ItemSource.Any())
            {
                var holiday = this.ViewModel.Holidays_ItemSource[this.ViewModel.Holidays_SelectedIndex];

                this.ViewModel.Date_SelectedDate = holiday.Date;
                this.ViewModel.Name_Text         = holiday.Name;
                this.ViewModel.Remarks_Text      = holiday.Remarks;
            }
        }

        public void Refresh()
        {
            // 追加ボタン
            this.ViewModel.Add_IsEnabled    = false;
            // 更新ボタン
            this.ViewModel.Update_IsEnabled = false;
            // 削除ボタン
            this.ViewModel.Delete_IsEnabled = false;
        }

        /// <summary>
        /// 祝日 - SelectionChanged
        /// </summary>
        public void Holidays_SelectionChanged()
        {
            if (this.ViewModel.Holidays_SelectedIndex == -1)
            {
                return;
            }

            this.EnableControlButton();

            if (!this.ViewModel.Holidays_ItemSource.Any())
            {
                return;
            }

            var entity = this.ViewModel.Holidays_ItemSource[this.ViewModel.Holidays_SelectedIndex];

            // 日付
            this.ViewModel.Date_SelectedDate = entity.Date;
            // 名称
            this.ViewModel.Name_Text         = entity.Name;
            // 備考
            this.ViewModel.Remarks_Text      = entity.Remarks;
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
                        && this.ViewModel.Holidays_SelectedIndex >= 0;

            // 更新ボタン
            this.ViewModel.Update_IsEnabled = selected;
            // 削除ボタン
            this.ViewModel.Delete_IsEnabled = selected;
        }

        /// <summary>
        /// 祝日名 - TextChanged
        /// </summary>
        public void EnableAddButton()
        {
            var inputted = !string.IsNullOrEmpty(this.ViewModel.Name_Text);

            this.ViewModel.Add_IsEnabled = inputted;
        }

        public void Reload()
        {
            var holidays = JSONExtension.DeserializeSettings<IReadOnlyList<JSONProperty_Holiday>>(FilePath.GetJSONHolidayDefaultPath());

            if (holidays.Any() == false)
            {
                return;
            }

            this.ViewModel.Holidays_ItemSource.Clear();

            var list = new List<HolidayEntity>();

            foreach (var holiday in holidays)
            {
                list.Add(new HolidayEntity(holiday.Date, holiday.Name, holiday.Remarks));
            }

            this.ViewModel.Holidays_ItemSource = ListUtils.ToObservableCollection(list);

            // 並び変え
            this.ViewModel.Holidays_ItemSource = new ObservableCollection<HolidayEntity>(this.ViewModel.Holidays_ItemSource.OrderBy(x => x.Date.ToString()));
        }

        public void Save()
        {
            var list = new List<JSONProperty_Holiday>();

            foreach (var holiday in this.ViewModel.Holidays_ItemSource)
            {
                var json = new JSONProperty_Holiday { 
                                                        Date    = holiday.Date, 
                                                        Name    = holiday.Name, 
                                                        Remarks = holiday.Remarks 
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
                this.ViewModel.Date_SelectedDate,
                this.ViewModel.Name_Text,
                this.ViewModel.Remarks_Text);
        }

        /// <summary>
        /// 追加
        /// </summary>
        public void Add()
        {
            if (!Message.ShowConfirmingMessage($"入力された祝日を追加しますか？", this.ViewModel.Title))
            {
                // キャンセル
                return;
            }

            using (var cursor = new CursorWaiting())
            {
                this.ViewModel.Delete_IsEnabled = true;

                var entity = this.CreateEntity();

                this.ViewModel.Holidays_ItemSource.Add(entity);
                this.Save();

                this.Reload();

                this.ViewModel.Holidays_SelectedIndex = this.ViewModel.Holidays_ItemSource.Count;

                this.Refresh();
                this.Clear_InputForm();
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            if (!Message.ShowConfirmingMessage($"選択中の祝日を更新しますか？", this.ViewModel.Title))
            {
                // キャンセル
                return;
            }

            using (var cursor = new CursorWaiting())
            {
                var entity = this.CreateEntity();
                this.ViewModel.Holidays_ItemSource[this.ViewModel.Holidays_SelectedIndex] = entity;

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
            if (this.ViewModel.Holidays_SelectedIndex == -1 ||
                !this.ViewModel.Holidays_ItemSource.Any())
            {
                return;
            }

            if (!Message.ShowConfirmingMessage($"選択中の祝日を削除しますか？", this.ViewModel.Title))
            {
                // キャンセル
                return;
            }

            using (var cursor = new CursorWaiting())
            {
                this.ViewModel.Holidays_ItemSource.RemoveAt(this.ViewModel.Holidays_SelectedIndex);

                this.Save();
                this.EnableControlButton();

                this.Reload();

                this.Refresh();
                this.Clear_InputForm();
            }
        }
    }
}
