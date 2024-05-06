using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Reactive.Bindings;
using SalaryManager.Domain.Entities;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.Models;

namespace SalaryManager.WPF.ViewModels
{
    /// <summary>
    /// ViewModel - 経歴
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Always)]
    public class ViewModel_Career : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel_Career()
        {
            this.Model.ViewModel = this;

            this.Careers_ItemSource.Value = new ObservableCollection<CareerEntity>();

            this.Model.Initialize();

            this.BindEvent();
        }

        /// <summary>
        /// イベント登録
        /// </summary>
        /// <remarks>
        /// Viewの指定したイベントと、発火させるメソッドを紐付ける。
        /// </remarks>
        private void BindEvent()
        {
            // 就業中
            this.Working_Checked.Subscribe(_ => this.Model.IsWorking_Checked());
            // 会社名
            this.CompanyName_TextChanged.Subscribe(_ => this.Model.EnableAddButton());
            // 経歴一覧
            this.Careers_SelectionChanged.Subscribe(_ => this.Model.Careers_SelectionChanged());

            this.Add_Command.Subscribe(_ => this.Model.Add());
            this.Update_Command.Subscribe(_ => this.Model.Update());
            this.Delete_Command.Subscribe(_ => this.Model.Delete());
        }

        /// <summary> Model - 経歴 </summary>
        public Model_Career Model { get; set; } = Model_Career.GetInstance(new CareerSQLite());

        /// <summary> Entity - 経歴 </summary>
        public IReadOnlyList<CareerEntity> Entities { get; internal set; }

        #region Window

        /// <summary> Window - FontFamily </summary>
        public ReactiveProperty<System.Windows.Media.FontFamily> Window_FontFamily { get; set; }
            = new ReactiveProperty<System.Windows.Media.FontFamily>();

        /// <summary> Window - FontSize </summary>
        public ReactiveProperty<decimal> Window_FontSize { get; set; }
            = new ReactiveProperty<decimal>();

        /// <summary> Window - Background </summary>
        public ReactiveProperty<System.Windows.Media.Brush> Window_Background { get; set; }
            = new ReactiveProperty<System.Windows.Media.Brush>();

        /// <summary> Window - Title </summary>
        public ReactiveProperty<string> Window_Title { get; }
            = new ReactiveProperty<string>("経歴編集");

        #endregion

        #region 職歴一覧

        /// <summary> 職歴一覧 - ItemSource </summary>
        public ReactiveProperty<ObservableCollection<CareerEntity>> Careers_ItemSource { get; set; }
            = new ReactiveProperty<ObservableCollection<CareerEntity>>();

        /// <summary> 職歴一覧 - SelectedIndex </summary>
        public ReactiveProperty<int> Careers_SelectedIndex { get; set; }
            = new ReactiveProperty<int>();

        /// <summary> 特別手当 - MouseMove </summary>
        public ReactiveCommand Careers_SelectionChanged { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 雇用形態

        /// <summary> 雇用形態 - ItemSource </summary>
        public ReactiveProperty<ObservableCollection<string>> WorkingStatus_ItemSource { get; }
            = new ReactiveProperty<ObservableCollection<string>>(
                new ObservableCollection<string> { "正社員", "契約社員", "派遣社員", "業務委託", "アルバイト" });

        /// <summary> 雇用形態 - SelectedIndex </summary>
        public ReactiveProperty<int> WorkingStatus_SelectedIndex { get; set; }
            = new ReactiveProperty<int>();

        /// <summary> 雇用形態 - Text </summary>
        public ReactiveProperty<string> WorkingStatus_Text { get; set; }
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

        #region 勤務期間

        /// <summary> 勤務開始日 - SelectedDate </summary>
        public ReactiveProperty<DateTime> WorkingStart_SelectedDate { get; set; }
            = new ReactiveProperty<DateTime>();

        /// <summary> 勤務終了日 - SelectedDate </summary>
        public ReactiveProperty<DateTime> WorkingEnd_SelectedDate { get; set; }
            = new ReactiveProperty<DateTime>();

        /// <summary> 勤務終了日 - IsEnabled </summary>
        public ReactiveProperty<bool> WorkingEnd_IsEnabled { get; set; }
            = new ReactiveProperty<bool>();

        /// <summary> 就業中 - IsChecked </summary>
        public ReactiveProperty<bool> Working_IsChecked { get; set; }
            = new ReactiveProperty<bool>();

        /// <summary> 就業中 - Checked </summary>
        public ReactiveCommand Working_Checked { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 社員番号

        /// <summary> 社員番号 - Text </summary>
        public ReactiveProperty<string> EmployeeNumber_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 手当

        /// <summary> 皆勤手当 - IsChecked </summary>
        public ReactiveProperty<bool> PerfectAttendanceAllowance_IsChecked { get; set; }
            = new ReactiveProperty<bool>();

        /// <summary> 教育手当 - IsChecked </summary>
        public ReactiveProperty<bool> EducationAllowance_IsChecked { get; set; }
            = new ReactiveProperty<bool>();

        /// <summary> 在宅手当 - IsChecked </summary>
        public ReactiveProperty<bool> ElectricityAllowance_IsChecked { get; set; }
            = new ReactiveProperty<bool>();

        /// <summary> 資格手当 - IsChecked </summary>
        public ReactiveProperty<bool> CertificationAllowance_IsChecked { get; set; }
            = new ReactiveProperty<bool>();

        /// <summary> 時間外手当 - IsChecked </summary>
        public ReactiveProperty<bool> OvertimeAllowance_IsChecked { get; set; }
            = new ReactiveProperty<bool>();

        /// <summary> 出張手当 - IsChecked </summary>
        public ReactiveProperty<bool> TravelAllowance_IsChecked { get; set; }
            = new ReactiveProperty<bool>();

        /// <summary> 住宅手当 - IsChecked </summary>
        public ReactiveProperty<bool> HousingAllowance_IsChecked { get; set; }
            = new ReactiveProperty<bool>();

        /// <summary> 食事手当 - IsChecked </summary>
        public ReactiveProperty<bool> FoodAllowance_IsChecked { get; set; }
            = new ReactiveProperty<bool>();

        /// <summary> 深夜手当 - IsChecked </summary>
        public ReactiveProperty<bool> LateNightAllowance_IsChecked { get; set; }
            = new ReactiveProperty<bool>();

        /// <summary> 地域手当 - IsChecked </summary>
        public ReactiveProperty<bool> AreaAllowance_IsChecked { get; set; }
            = new ReactiveProperty<bool>();

        /// <summary> 通勤手当 - IsChecked </summary>
        public ReactiveProperty<bool> CommutingAllowance_IsChecked { get; set; }
            = new ReactiveProperty<bool>();

        /// <summary> 前払退職金 - IsChecked </summary>
        public ReactiveProperty<bool> PrepaidRetirementPayment_IsChecked { get; set; }
            = new ReactiveProperty<bool>();

        /// <summary> 扶養手当 - IsChecked </summary>
        public ReactiveProperty<bool> DependencyAllowance_IsChecked { get; set; }
            = new ReactiveProperty<bool>();

        /// <summary> 役職手当 - IsChecked </summary>
        public ReactiveProperty<bool> ExecutiveAllowance_IsChecked { get; set; }
            = new ReactiveProperty<bool>();

        /// <summary> 特別手当 - IsChecked </summary>
        public ReactiveProperty<bool> SpecialAllowance_IsChecked { get; set; }
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
