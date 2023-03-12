using System;
using SalaryManager.Domain.Entities;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.Infrastructure.Interface;
using SalaryManager.WPF.ViewModels;
using SalaryManager.Domain.StaticValues;
using SalaryManager.Domain.Modules.Logics;
using SalaryManager.Domain.ValueObjects;
using System.Windows.Media;

namespace SalaryManager.WPF.Models
{
    /// <summary>
    /// Model - 勤務備考
    /// </summary>
    public class Model_WorkingReference : IInputPayslip
    {
        #region Get Instance

        private static Model_WorkingReference model = null;

        public static Model_WorkingReference GetInstance()
        {
            if (model == null)
            {
                model = new Model_WorkingReference();
            }

            return model;
        }

        #endregion

        /// <summary> ViewModel - ヘッダ </summary>
        internal ViewModel_Header Header { get; set; }

        /// <summary> ViewModel - 勤務先 </summary>
        internal ViewModel_WorkPlace WorkPlace { get; set; }

        /// <summary> ViewModel - 勤務備考 </summary>
        internal ViewModel_WorkingReference ViewModel { get; set; }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="entityDate">取得する日付</param>
        public void Initialize(DateTime entityDate)
        {
            WorkingReferences.Create(new WorkingReferenceSQLite());
            Careers.Create(new CareerSQLite());

            this.ViewModel.Entity          = WorkingReferences.Fetch(entityDate.Year, entityDate.Month);
            this.ViewModel.Entity_LastYear = WorkingReferences.Fetch(entityDate.Year, entityDate.Month - 1);

            if (this.ViewModel.Entity is null)
            {
                // デフォルト明細
                this.ViewModel.Entity = WorkingReferences.FetchDefault();
            }

            this.Refresh();
        }

        /// <summary>
        /// リロード
        /// </summary>
        public void Reload()
        {
            using (var cursor = new CursorWaiting())
            {
                WorkingReferences.Create(new WorkingReferenceSQLite());

                this.ViewModel.Entity          = WorkingReferences.Fetch(this.Header.Year_Value,     this.Header.Month_Value);
                this.ViewModel.Entity_LastYear = WorkingReferences.Fetch(this.Header.Year_Value - 1, this.Header.Month_Value);

                this.Refresh();
            }   
        }

        /// <summary>
        /// クリア
        /// </summary>
        public void Clear()
        {
            // 時間外時間
            this.ViewModel.OvertimeTime_Value      = default(double);
            // 休出時間
            this.ViewModel.WeekendWorktime_Value   = default(double);
            // 深夜時間
            this.ViewModel.MidnightWorktime_Value  = default(double);
            // 遅刻早退欠勤H
            this.ViewModel.LateAbsentH_Value       = default(double);
            // 支給額-保険
            this.ViewModel.Insurance_Value         = default(double);
            // 標準月額千円
            this.ViewModel.Norm_Value              = default(double);
            // 扶養人数
            this.ViewModel.NumberOfDependent_Value = default(double);
            // 有給残日数
            this.ViewModel.PaidVacation_Value      = default(double);
            // 勤務時間
            this.ViewModel.WorkingHours_Value      = default(double);
            // 所属会社名
            this.WorkPlace.CompanyName       = CompanyValue.Undefined.DisplayValue;
            this.WorkPlace.CompanyName_Foreground = new SolidColorBrush(Colors.Gray);
            // 勤務先
            this.WorkPlace.WorkPlace         = default(string);
            // 備考
            this.ViewModel.Remarks_Text           = default(string);
        }

        /// <summary>
        /// 再描画
        /// </summary>
        public void Refresh()
        {
            var entity = this.ViewModel.Entity;

            if (entity is null)
            {
                this.Clear();
                return;
            }

            // 時間外時間
            this.ViewModel.OvertimeTime_Value      = entity.OvertimeTime;
            // 休出時間
            this.ViewModel.WeekendWorktime_Value   = entity.WeekendWorktime;
            // 深夜時間
            this.ViewModel.MidnightWorktime_Value  = entity.MidnightWorktime;
            // 遅刻早退欠勤H
            this.ViewModel.LateAbsentH_Value       = entity.LateAbsentH;
            // 支給額-保険
            this.ViewModel.Insurance_Value         = entity.Insurance.Value;
            // 標準月額千円
            this.ViewModel.Norm_Value              = entity.Norm;
            // 扶養人数
            this.ViewModel.NumberOfDependent_Value = entity.NumberOfDependent;
            // 有給残日数
            this.ViewModel.PaidVacation_Value      = entity.PaidVacation.Value;
            // 勤務時間
            this.ViewModel.WorkingHours_Value      = entity.WorkingHours;
            // 勤務先
            this.WorkPlace.WorkPlace         = entity.WorkPlace;
            // 備考
            this.ViewModel.Remarks_Text           = entity.Remarks;

            // 所属会社名
            var company = Careers.FetchCompany(new DateTime(this.Header.Year_Value, this.Header.Month_Value, 1));
            this.WorkPlace.CompanyName = company;

            if (company == CompanyValue.Undefined.DisplayValue)
            {
                this.WorkPlace.CompanyName_Foreground = new SolidColorBrush(Colors.Gray);
            }
            else
            {
                this.WorkPlace.CompanyName_Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="transaction">トランザクション</param>
        public void Save(SQLiteTransaction transaction)
        {
            var entity = new WorkingReferencesEntity(
                this.Header.ID,
                this.Header.YearMonth,
                this.ViewModel.OvertimeTime_Value,
                this.ViewModel.WeekendWorktime_Value,
                this.ViewModel.MidnightWorktime_Value,
                this.ViewModel.LateAbsentH_Value,
                this.ViewModel.Insurance_Value,
                this.ViewModel.Norm_Value,
                this.ViewModel.NumberOfDependent_Value,
                this.ViewModel.PaidVacation_Value,
                this.ViewModel.WorkingHours_Value,
                this.WorkPlace.WorkPlace,
                this.ViewModel.Remarks_Text);

            var workingReference = new WorkingReferenceSQLite();
            workingReference.Save(transaction, entity);
        }
    }
}
