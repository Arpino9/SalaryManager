using System;
using SalaryManager.Domain.Entities;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.Infrastructure.Interface;
using SalaryManager.WPF.ViewModels;
using SalaryManager.Domain.StaticValues;

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
            WorkingReferences.Create(new WorkingReferenceSQLite());

            this.ViewModel.Entity          = WorkingReferences.Fetch(this.Header.Year,     this.Header.Month);
            this.ViewModel.Entity_LastYear = WorkingReferences.Fetch(this.Header.Year - 1, this.Header.Month);
            
            this.Refresh();
        }

        /// <summary>
        /// クリア
        /// </summary>
        public void Clear()
        {
            // 時間外時間
            this.ViewModel.OvertimeTime      = default(double);
            // 休出時間
            this.ViewModel.WeekendWorktime   = default(double);
            // 深夜時間
            this.ViewModel.MidnightWorktime  = default(double);
            // 遅刻早退欠勤H
            this.ViewModel.LateAbsentH       = default(double);
            // 支給額-保険
            this.ViewModel.Insurance         = default(double);
            // 標準月額千円
            this.ViewModel.Norm              = default(double);
            // 扶養人数
            this.ViewModel.NumberOfDependent = default(double);
            // 有給残日数
            this.ViewModel.PaidVacation      = default(double);
            // 勤務時間
            this.ViewModel.WorkingHours      = default(double);
            // 勤務先
            this.WorkPlace.WorkPlace         = default(string);
            // 備考
            this.ViewModel.Remarks           = default(string);
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
            this.ViewModel.OvertimeTime      = entity.OvertimeTime;
            // 休出時間
            this.ViewModel.WeekendWorktime   = entity.WeekendWorktime;
            // 深夜時間
            this.ViewModel.MidnightWorktime  = entity.MidnightWorktime;
            // 遅刻早退欠勤H
            this.ViewModel.LateAbsentH       = entity.LateAbsentH;
            // 支給額-保険
            this.ViewModel.Insurance         = entity.Insurance.Value;
            // 標準月額千円
            this.ViewModel.Norm              = entity.Norm;
            // 扶養人数
            this.ViewModel.NumberOfDependent = entity.NumberOfDependent;
            // 有給残日数
            this.ViewModel.PaidVacation      = entity.PaidVacation.Value;
            // 勤務時間
            this.ViewModel.WorkingHours      = entity.WorkingHours;
            // 勤務先
            this.WorkPlace.WorkPlace         = entity.WorkPlace;
            // 備考
            this.ViewModel.Remarks           = entity.Remarks;
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
                this.ViewModel.OvertimeTime,
                this.ViewModel.WeekendWorktime,
                this.ViewModel.MidnightWorktime,
                this.ViewModel.LateAbsentH,
                this.ViewModel.Insurance,
                this.ViewModel.Norm,
                this.ViewModel.NumberOfDependent,
                this.ViewModel.PaidVacation,
                this.ViewModel.WorkingHours,
                this.WorkPlace.WorkPlace,
                this.ViewModel.Remarks);

            var workingReference = new WorkingReferenceSQLite();
            workingReference.Save(transaction, entity);
        }
    }
}
