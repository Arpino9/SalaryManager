using System;
using System.Linq;
using System.Collections.ObjectModel;
using SalaryManager.Domain.Entities;
using SalaryManager.WPF.ViewModels;
using SalaryManager.Domain.Modules.Logics;
using SalaryManager.Domain.StaticValues;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.Infrastructure.Interface;
using SalaryManager.Infrastructure.XML;

namespace SalaryManager.WPF.Models
{
    /// <summary>
    /// Model - 職歴
    /// </summary>
    public class Model_Career : IMaster
    {
        #region Get Instance

        private static Model_Career model = null;

        public static Model_Career GetInstance()
        {
            if (model == null)
            {
                model = new Model_Career();
            }

            return model;
        }

        #endregion

        public Model_Career()
        {
            
        }
        
        /// <summary> ViewModel - 職歴 </summary>
        public ViewModel_Career ViewModel { get; set; }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <remarks>
        /// 未選択状態、かつ新規登録が可能な状態にする。
        /// </remarks>
        public void Initialize()
        {
            Careers.Create(new CareerSQLite());

            this.ViewModel.FontFamily = XMLLoader.FetchFontFamily();
            this.ViewModel.FontSize   = XMLLoader.FetchFontSize();

            this.ViewModel.Window_Background = XMLLoader.FetchBackgroundColorBrush();

            this.ViewModel.Entities = Careers.FetchByDescending();

            this.Reflesh_ListView();

            this.ViewModel.Careers_SelectedIndex = -1;
            this.Clear_InputForm();
        }

        /// <summary>
        /// 再描画
        /// </summary>
        /// <remarks>
        /// 該当月に経歴情報が存在すれば、各項目を再描画する。
        /// </remarks>
        public void Refresh()
        {
            // ListView
            this.Reflesh_ListView();
            // 入力用フォーム
            this.Reflesh_InputForm();
        }

        /// <summary>
        /// 再描画 - ListView
        /// </summary>
        private void Reflesh_ListView()
        {
            this.ViewModel.Careers_ItemSource.Clear();

            var entities = this.ViewModel.Entities;

            if (!entities.Any())
            {
                this.Clear_InputForm();
                return;
            }

            foreach (var entity in entities)
            {
                this.ViewModel.Careers_ItemSource.Add(entity);
            }
        }

        /// <summary>
        /// Enable - 操作ボタン
        /// </summary>
        /// <remarks>
        /// 追加ボタンは「会社名」に値があれば押下可能。
        /// </remarks>
        private void EnableControlButton()
        {
            var selected = this.ViewModel.Careers_ItemSource.Any() 
                        && this.ViewModel.Careers_SelectedIndex >= 0;

            // 更新ボタン
            this.ViewModel.Update_IsEnabled = selected;
            // 削除ボタン
            this.ViewModel.Delete_IsEnabled = selected;
        }

        /// <summary>
        /// 経歴 - SelectionChanged
        /// </summary>
        public void Careers_SelectionChanged()
        {
            if (this.ViewModel.Careers_SelectedIndex == -1)
            {
                return;
            }

            this.EnableControlButton();

            if (!this.ViewModel.Careers_ItemSource.Any())
            {
                return;
            }

            var entity = this.ViewModel.Careers_ItemSource[this.ViewModel.Careers_SelectedIndex];
            // 雇用形態
            this.ViewModel.WorkingStatus_Text = entity.WorkingStatus;
            // 会社名
            this.ViewModel.CompanyName_Text   = entity.CompanyName.Text;
            // 社員番号
            this.ViewModel.EmployeeNumber     = entity.EmployeeNumber;
            // 勤務開始日
            this.ViewModel.WorkingStartDate   = entity.WorkingStartDate.Value;
            // 勤務終了日
            this.ViewModel.WorkingEndDate     = entity.WorkingEndDate.IsWorking ? DateTime.Today : entity.WorkingEndDate.Value;
            // 就業中か
            this.ViewModel.IsWorking          = entity.WorkingEndDate.IsWorking;
            // 備考
            this.ViewModel.Remarks            = entity.Remarks;

            var allowance = entity.AllowanceExistence;
            // 皆勤手当
            this.ViewModel.PerfectAttendanceAllowance_IsChecked = allowance.PerfectAttendance.Value;
            // 教育手当
            this.ViewModel.EducationAllowance_IsChecked         = allowance.Education.Value;
            // 在宅手当
            this.ViewModel.ElectricityAllowance_IsChecked       = allowance.Electricity.Value;
            // 資格手当
            this.ViewModel.CertificationAllowance_IsChecked     = allowance.Certification.Value;
            // 時間外手当
            this.ViewModel.OvertimeAllowance_IsChecked          = allowance.Overtime.Value;
            // 出張手当
            this.ViewModel.TravelAllowance_IsChecked            = allowance.Travel.Value;
            // 住宅手当
            this.ViewModel.HousingAllowance_IsChecked           = allowance.Housing.Value;
            // 食事手当
            this.ViewModel.FoodAllowance_IsChecked              = allowance.Food.Value;
            // 深夜手当
            this.ViewModel.LateNightAllowance_IsChecked         = allowance.LateNight.Value;
            // 地域手当
            this.ViewModel.AreaAllowance_IsChecked              = allowance.Area.Value;
            // 通勤手当
            this.ViewModel.CommutingAllowance_IsChecked         = allowance.Commution.Value;
            // 扶養手当
            this.ViewModel.DependencyAllowance_IsChecked        = allowance.Dependency.Value;
            // 役職手当
            this.ViewModel.ExecutiveAllowance_IsChecked         = allowance.Executive.Value;
            // 特別手当
            this.ViewModel.SpecialAllowance_IsChecked           = allowance.Special.Value;
        }

        /// <summary>
        /// 就業中か - Checked
        /// </summary>
        public void IsWorking_Checked()
        {
            this.ViewModel.WorkingEndDate_IsEnabled = this.ViewModel.IsWorking ? false : true;

            if (this.ViewModel.IsWorking)
            {
                this.ViewModel.WorkingEndDate = DateTime.Today;
            }            
        }

        /// <summary>
        /// 会社名 - TextChanged
        /// </summary>
        public void EnableAddButton()
        {
            var inputted = !string.IsNullOrEmpty(this.ViewModel.CompanyName_Text);

            this.ViewModel.Add_IsEnabled = inputted;
        }

        /// <summary>
        /// 再描画 - 入力用フォーム
        /// </summary>
        private void Reflesh_InputForm()
        {
            this.IsWorking_Checked();

            this.Careers_SelectionChanged();

            // 追加ボタン
            this.EnableAddButton();
            // 更新、削除ボタン
            this.EnableControlButton();
        }

        /// <summary>
        /// リロード
        /// </summary>
        /// <remarks>
        /// 年月の変更時などに、該当月の項目を取得する。
        /// </remarks>
        public void Reload()
        {
            using (var cursor = new CursorWaiting())
            {
                Careers.Create(new CareerSQLite());

                this.ViewModel.Entities = Careers.FetchByDescending();

                this.Refresh();
            }
        }

        /// <summary>
        /// クリア
        /// </summary>
        /// <remarks>
        /// 各項目を初期化する。
        /// </remarks>
        public void Clear_InputForm()
        {
            // 雇用形態
            this.ViewModel.WorkingStatus_Text = this.ViewModel.WorkingStatus_ItemSource.First();
            // 会社名
            this.ViewModel.CompanyName_Text   = default(string);
            // 勤務開始日
            this.ViewModel.WorkingStartDate   = DateTime.Now;
            // 勤務終了日
            this.ViewModel.WorkingEndDate     = DateTime.Now;
            this.IsWorking_Checked();
            // 社員番号
            this.ViewModel.EmployeeNumber     = default(string);
            // 備考
            this.ViewModel.Remarks            = default(string);

            // 皆勤手当
            this.ViewModel.PerfectAttendanceAllowance_IsChecked = default(bool);
            // 教育手当
            this.ViewModel.EducationAllowance_IsChecked         = default(bool);
            // 在宅手当
            this.ViewModel.ElectricityAllowance_IsChecked       = default(bool);
            // 資格手当
            this.ViewModel.CertificationAllowance_IsChecked     = default(bool);
            // 時間外手当
            this.ViewModel.OvertimeAllowance_IsChecked          = default(bool);
            // 出張手当
            this.ViewModel.TravelAllowance_IsChecked            = default(bool);
            // 住宅手当
            this.ViewModel.HousingAllowance_IsChecked           = default(bool);
            // 食事手当
            this.ViewModel.FoodAllowance_IsChecked              = default(bool);
            // 深夜手当
            this.ViewModel.LateNightAllowance_IsChecked         = default(bool);
            // 地域手当
            this.ViewModel.AreaAllowance_IsChecked              = default(bool);
            // 通勤手当
            this.ViewModel.CommutingAllowance_IsChecked         = default(bool);
            // 扶養手当
            this.ViewModel.DependencyAllowance_IsChecked         = default(bool);
            // 役職手当
            this.ViewModel.ExecutiveAllowance_IsChecked         = default(bool);
            // 特別手当
            this.ViewModel.SpecialAllowance_IsChecked           = default(bool);

            // 追加ボタン
            this.ViewModel.Add_IsEnabled    = false;
            // 更新ボタン
            this.ViewModel.Update_IsEnabled = false;
            // 削除ボタン
            this.ViewModel.Delete_IsEnabled = false;
        }

        /// <summary>
        /// 追加
        /// </summary>
        public void Add()
        {
            if (!Message.ShowConfirmingMessage($"入力された職歴を追加しますか？", this.ViewModel.Title))
            {
                // キャンセル
                return;
            }

            using (var cursor = new CursorWaiting())
            {
                this.ViewModel.Delete_IsEnabled = true;

                var entity = this.CreateEntity(this.ViewModel.Entities.Count + 1);
                this.ViewModel.Careers_ItemSource.Add(entity);
                this.Save();

                // 並び変え
                this.ViewModel.Careers_ItemSource = new ObservableCollection<CareerEntity>(this.ViewModel.Careers_ItemSource.OrderByDescending(x => x.WorkingStartDate.ToString()));
            }   
        }

        /// <summary>
        /// Create Entity
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>職歴</returns>
        private CareerEntity CreateEntity(int id)
        {
            var workingEndDate = this.ViewModel.IsWorking ? DateTime.MaxValue : this.ViewModel.WorkingEndDate;

            return new CareerEntity(
                id,
                this.ViewModel.WorkingStatus_Text,
                this.ViewModel.CompanyName_Text,
                this.ViewModel.EmployeeNumber,
                this.ViewModel.WorkingStartDate,
                workingEndDate,
                CreateAllowanceExistenceEntity(),
                this.ViewModel.Remarks);

            // 手当有無の作成
            AllowanceExistenceEntity CreateAllowanceExistenceEntity()
            {
                return new AllowanceExistenceEntity(
                    this.ViewModel.PerfectAttendanceAllowance_IsChecked,
                    this.ViewModel.EducationAllowance_IsChecked,
                    this.ViewModel.ElectricityAllowance_IsChecked,
                    this.ViewModel.CertificationAllowance_IsChecked,
                    this.ViewModel.OvertimeAllowance_IsChecked,
                    this.ViewModel.TravelAllowance_IsChecked,
                    this.ViewModel.HousingAllowance_IsChecked,
                    this.ViewModel.FoodAllowance_IsChecked,
                    this.ViewModel.LateNightAllowance_IsChecked,
                    this.ViewModel.AreaAllowance_IsChecked,
                    this.ViewModel.CommutingAllowance_IsChecked,
                    this.ViewModel.DependencyAllowance_IsChecked,
                    this.ViewModel.ExecutiveAllowance_IsChecked,
                    this.ViewModel.SpecialAllowance_IsChecked);
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            if (!Message.ShowConfirmingMessage($"選択中の職歴を更新しますか？", this.ViewModel.Title))
            {
                // キャンセル
                return;
            }

            using (var cursor = new CursorWaiting())
            {
                var id = this.ViewModel.Careers_ItemSource[this.ViewModel.Careers_SelectedIndex].ID;

                var entity = this.CreateEntity(id);
                this.ViewModel.Careers_ItemSource[this.ViewModel.Careers_SelectedIndex] = entity;

                this.Save();
            }   
        }

        /// <summary>
        /// 削除
        /// </summary>
        public void Delete()
        {
            if (this.ViewModel.Careers_SelectedIndex == -1 ||
                !this.ViewModel.Careers_ItemSource.Any()) 
            {
                return;
            }

            if (!Message.ShowConfirmingMessage($"選択中の職歴を削除しますか？", this.ViewModel.Title))
            {
                // キャンセル
                return;
            }

            using (var cursor = new CursorWaiting())
            {
                var career = new CareerSQLite();
                career.Delete(this.ViewModel.Careers_SelectedIndex + 1);

                this.ViewModel.Careers_ItemSource.RemoveAt(this.ViewModel.Careers_SelectedIndex);

                this.Reload();
                this.EnableControlButton();
            }   
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            foreach (var entity in this.ViewModel.Careers_ItemSource)
            {
                var career = new CareerSQLite();
                career.Save(entity);
            }

            this.Reload();
        }
    }
}
