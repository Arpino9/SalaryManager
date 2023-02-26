using System;
using System.Linq;
using SalaryManager.Domain.Entities;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.Infrastructure.Interface;
using SalaryManager.WPF.ViewModels;

namespace SalaryManager.WPF.Models
{
    /// <summary>
    /// Model - 勤務備考
    /// </summary>
    public class Model_WorkingReference : IInputPayroll
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
            var sqlite = new WorkingReferenceSQLite();
            var records = sqlite.GetEntities();

            this.ViewModel.Entity = records.Where(record => record.YearMonth.Year  == entityDate.Year
                                                         && record.YearMonth.Month == entityDate.Month)
                                           .FirstOrDefault();

            this.ViewModel.Entity_LastYear = records.Where(record => record.YearMonth.Year  == entityDate.Year - 1
                                                                  && record.YearMonth.Month == entityDate.Month)
                                                    .FirstOrDefault();

            if (this.ViewModel.Entity is null)
            {
                var header = new HeaderSQLite();
                var defaultEntity = header.GetDefaultEntity();

                if (defaultEntity != null)
                {
                    this.ViewModel.Entity = records.Where(record => record.ID == defaultEntity.ID)
                                                   .FirstOrDefault();
                }
            }

            this.Refresh();
        }

        /// <summary>
        /// リロード
        /// </summary>
        public void Reload()
        {
            var workingReferenceTable = new WorkingReferenceSQLite();

            this.ViewModel.Entity          = workingReferenceTable.GetEntity(this.Header.Year,     this.Header.Month);
            this.ViewModel.Entity_LastYear = workingReferenceTable.GetEntity(this.Header.Year - 1, this.Header.Month);
            
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
            if (this.ViewModel.Entity is null)
            {
                this.Clear();
                return;
            }

            // 時間外時間
            this.ViewModel.OvertimeTime      = this.ViewModel.Entity.OvertimeTime;
            // 休出時間
            this.ViewModel.WeekendWorktime   = this.ViewModel.Entity.WeekendWorktime;
            // 深夜時間
            this.ViewModel.MidnightWorktime  = this.ViewModel.Entity.MidnightWorktime;
            // 遅刻早退欠勤H
            this.ViewModel.LateAbsentH       = this.ViewModel.Entity.LateAbsentH;
            // 支給額-保険
            this.ViewModel.Insurance         = this.ViewModel.Entity.Insurance.Value;
            // 標準月額千円
            this.ViewModel.Norm              = this.ViewModel.Entity.Norm;
            // 扶養人数
            this.ViewModel.NumberOfDependent = this.ViewModel.Entity.NumberOfDependent;
            // 有給残日数
            this.ViewModel.PaidVacation      = this.ViewModel.Entity.PaidVacation.Value;
            // 勤務時間
            this.ViewModel.WorkingHours      = this.ViewModel.Entity.WorkingHours;
            // 勤務先
            this.WorkPlace.WorkPlace         = this.ViewModel.Entity.WorkPlace;
            // 備考
            this.ViewModel.Remarks           = this.ViewModel.Entity.Remarks;
        }

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="transaction">トランザクション</param>
        public void Register(SQLiteTransaction transaction)
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
