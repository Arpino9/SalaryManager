﻿using System;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.StaticValues;
using SalaryManager.Domain.Modules.Logics;
using SalaryManager.Domain.ValueObjects;
using SalaryManager.Infrastructure.Interface;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.ViewModels;
using SalaryManager.Infrastructure.XML;
using SalaryManager.Domain.Repositories;

namespace SalaryManager.WPF.Models
{
    /// <summary>
    /// Model - 勤務備考
    /// </summary>
    public class Model_WorkingReference : IInputPayslip
    {

        #region Get Instance

        private static Model_WorkingReference model = null;

        public static Model_WorkingReference GetInstance(IWorkingReferencesRepository repository)
        {
            if (model == null)
            {
                model = new Model_WorkingReference(repository);
            }

            return model;
        }

        #endregion

        /// <summary> Repository </summary>
        private IWorkingReferencesRepository _repository;

        public Model_WorkingReference(IWorkingReferencesRepository repository)
        {
            _repository = repository;
        }

        /// <summary> ViewModel - メイン画面 </summary>
        internal ViewModel_MainWindow MainWindow { get; set; }

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
        /// <remarks>
        /// 画面起動時に、項目を初期化する。
        /// </remarks>
        public void Initialize(DateTime entityDate)
        {
            WorkingReferences.Create(_repository);
            Careers.Create(new CareerSQLite());

            this.ViewModel.Entity          = WorkingReferences.Fetch(entityDate.Year, entityDate.Month);
            this.ViewModel.Entity_LastYear = WorkingReferences.Fetch(entityDate.Year, entityDate.Month - 1);

            var showDefaultPayslip = XMLLoader.FetchShowDefaultPayslip();

            if (this.ViewModel.Entity is null && showDefaultPayslip)
            {
                // デフォルト明細
                this.ViewModel.Entity = WorkingReferences.FetchDefault();
            }

            this.Refresh();
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
                WorkingReferences.Create(_repository);

                this.ViewModel.Entity          = WorkingReferences.Fetch(this.Header.Year_Value,     this.Header.Month_Value);
                this.ViewModel.Entity_LastYear = WorkingReferences.Fetch(this.Header.Year_Value - 1, this.Header.Month_Value);

                this.Refresh();
            }   
        }

        /// <summary>
        /// クリア
        /// </summary>
        /// <remarks>
        /// 各項目を初期化する。
        /// </remarks>
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
            // 備考
            this.ViewModel.Remarks_Text            = default(string);
        }

        /// <summary>
        /// 再描画
        /// </summary>
        /// <remarks>
        /// 該当月に控除額が存在すれば、各項目を再描画する。
        /// </remarks>
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
            // 備考
            this.ViewModel.Remarks_Text            = entity.Remarks;
        }

        /// <summary>
        /// エディットバリデーションチェック
        /// </summary>
        /// <returns>判定可否</returns>
        public bool EditValidationCheck()
        {
            if (this.ViewModel.Entity is null)
            {
                // 今月の明細の新規登録
                return true;
            }

            var paidVacation = this.ViewModel.Entity.PaidVacation;

            if (paidVacation.Value < PaidVacationDaysValue.Minimum ||
                paidVacation.Value > PaidVacationDaysValue.Maximum)
            {
                Message.ShowErrorMessage(
                    $"有給休暇は{PaidVacationDaysValue.Minimum}から{PaidVacationDaysValue.Maximum}までの日数で入力して下さい。",
                    this.MainWindow.Window_Title);

                return false;
            }

            return true;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="transaction">トランザクション</param>
        /// <remarks>
        /// SQLiteに接続し、入力項目を保存する。
        /// </remarks>
        public void Save(ITransactionRepository transaction)
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

            _repository.Save(transaction, entity);
        }
    }
}
