using System;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Modules.Logics;
using SalaryManager.Domain.Repositories;
using SalaryManager.Domain.StaticValues;
using SalaryManager.Infrastructure.Interface;
using SalaryManager.Infrastructure.XML;
using SalaryManager.WPF.ViewModels;

namespace SalaryManager.WPF.Models
{
    /// <summary>
    /// Model - 控除額
    /// </summary>
    public class Model_Deduction : IInputPayslip
    {

        #region Get Instance

        private static Model_Deduction model = null;

        public static Model_Deduction GetInstance(IDeductionRepository repository)
        {
            if (model == null)
            {
                model = new Model_Deduction(repository);
            }

            return model;
        }

        #endregion
        
        /// <summary> Repository </summary>
        private IDeductionRepository _repository;

        public Model_Deduction(IDeductionRepository repository)
        {
            _repository = repository;
        }

        /// <summary> ViewModel - ヘッダ </summary>
        internal ViewModel_Header Header { get; set; }

        /// <summary> ViewModel - 控除額 </summary>
        internal ViewModel_Deduction ViewModel { get; set; }

        /// <summary> ViewModel - 支給額 </summary>
        internal Model_Allowance Allowance { get; set; }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="entityDate">取得する日付</param>
        /// <remarks>
        /// 画面起動時に、項目を初期化する。
        /// </remarks>
        public void Initialize(DateTime entityDate)
        {
            Deductions.Create(_repository);

            this.ViewModel.Entity          = Deductions.Fetch(entityDate.Year, entityDate.Month);
            this.ViewModel.Entity_LastYear = Deductions.Fetch(entityDate.Year, entityDate.Month - 1);

            var showDefaultPayslip = XMLLoader.FetchShowDefaultPayslip();

            if (this.ViewModel.Entity is null && showDefaultPayslip)
            {
                // デフォルト明細
                this.ViewModel.Entity = Deductions.FetchDefault();
            }

            this.Refresh();
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

            // 健康保険
            this.ViewModel.HealthInsurance_Value       = entity.HealthInsurance.Value;
            // 介護保険
            this.ViewModel.NursingInsurance_Value      = entity.NursingInsurance.Value;
            // 厚生年金
            this.ViewModel.WelfareAnnuity_Value        = entity.WelfareAnnuity.Value;
            // 雇用保険
            this.ViewModel.EmploymentInsurance_Value   = entity.EmploymentInsurance.Value;
            // 所得税
            this.ViewModel.IncomeTax_Value             = entity.IncomeTax.Value;
            // 市町村税
            this.ViewModel.MunicipalTax_Value          = entity.MunicipalTax.Value;
            // 互助会
            this.ViewModel.FriendshipAssociation_Value = entity.FriendshipAssociation.Value;
            // 年末調整他
            this.ViewModel.YearEndTaxAdjustment_Value  = entity.YearEndTaxAdjustment;
            // 備考
            this.ViewModel.Remarks_Text                = entity.Remarks;
            // 控除額計
            this.ViewModel.TotalDeduct_Value           = entity.TotalDeduct.Value;
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
                Deductions.Create(_repository);

                this.ViewModel.Entity          = Deductions.Fetch(this.Header.Year_Value,     this.Header.Month_Value);
                this.ViewModel.Entity_LastYear = Deductions.Fetch(this.Header.Year_Value - 1, this.Header.Month_Value);

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
            // 健康保険
            this.ViewModel.HealthInsurance_Value       = default(double);
            // 介護保険
            this.ViewModel.NursingInsurance_Value      = default(double);
            // 厚生年金
            this.ViewModel.WelfareAnnuity_Value        = default(double);
            // 雇用保険
            this.ViewModel.EmploymentInsurance_Value   = default(double);
            // 所得税
            this.ViewModel.IncomeTax_Value             = default(double);
            // 市町村税
            this.ViewModel.MunicipalTax_Value          = default(double);
            // 互助会
            this.ViewModel.FriendshipAssociation_Value = default(double);
            // 年末調整他
            this.ViewModel.YearEndTaxAdjustment_Value  = default(double);
            // 備考
            this.ViewModel.Remarks_Text                = default(string);
            // 控除額計
            this.ViewModel.TotalDeduct_Value           = default(double);
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
            var entity = new DeductionEntity(
                            this.Header.ID,
                            this.Header.YearMonth,
                            this.ViewModel.HealthInsurance_Value,
                            this.ViewModel.NursingInsurance_Value,
                            this.ViewModel.WelfareAnnuity_Value,
                            this.ViewModel.EmploymentInsurance_Value,
                            this.ViewModel.IncomeTax_Value,
                            this.ViewModel.MunicipalTax_Value,
                            this.ViewModel.FriendshipAssociation_Value,
                            this.ViewModel.YearEndTaxAdjustment_Value,
                            this.ViewModel.Remarks_Text,
                            this.ViewModel.TotalDeduct_Value);

            _repository.Save(transaction, entity);
        }

        /// <summary>
        /// 再計算
        /// </summary>
        /// <remarks>
        /// 該当項目の変更時に、支給総計と差引支給額を再計算する。
        /// </remarks>
        internal void ReCaluculate()
        {
            if (this.ViewModel is null)
            {
                return;
            }

            this.ViewModel.TotalDeduct_Value = this.ViewModel.HealthInsurance_Value
                                       + this.ViewModel.NursingInsurance_Value
                                       + this.ViewModel.WelfareAnnuity_Value
                                       + this.ViewModel.EmploymentInsurance_Value
                                       + this.ViewModel.IncomeTax_Value
                                       + this.ViewModel.MunicipalTax_Value
                                       + this.ViewModel.FriendshipAssociation_Value
                                       + this.ViewModel.YearEndTaxAdjustment_Value;

            this.Allowance.ReCaluculate();
        }
    }
}
