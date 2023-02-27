using System;
using System.Linq;
using System.Windows.Media;
using SalaryManager.Domain.Entities;
using SalaryManager.Infrastructure.Interface;
using SalaryManager.Infrastructure.SQLite;
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

        public static Model_Deduction GetInstance()
        {
            if (model == null)
            {
                model = new Model_Deduction();
            }

            return model;
        }

        #endregion

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
        public void Initialize(DateTime entityDate)
        {
            var sqlite = new DeductionSQLite();
            var records = sqlite.GetEntities();

            this.ViewModel.Entity = records.Where(record => record.YearMonth.Year  == entityDate.Year
                                                         && record.YearMonth.Month == entityDate.Month)
                                           .FirstOrDefault();

            this.ViewModel.Entity_LastYear = records.Where(record => record.YearMonth.Year  == entityDate.Year - 1
                                                                  && record.YearMonth.Month == entityDate.Month)
                                                    .FirstOrDefault();

            if (this.ViewModel.Entity is null)
            {
                // レコードなし
                var header        = new HeaderSQLite();
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

            // 健康保険
            this.ViewModel.HealthInsurance       = entity.HealthInsurance.Value;
            // 介護保険
            this.ViewModel.NursingInsurance      = entity.NursingInsurance.Value;
            // 厚生年金
            this.ViewModel.WelfareAnnuity        = entity.WelfareAnnuity.Value;
            // 雇用保険
            this.ViewModel.EmploymentInsurance   = entity.EmploymentInsurance.Value;
            // 所得税
            this.ViewModel.IncomeTax             = entity.IncomeTax.Value;
            // 市町村税
            this.ViewModel.MunicipalTax          = entity.MunicipalTax.Value;
            // 互助会
            this.ViewModel.FriendshipAssociation = entity.FriendshipAssociation.Value;
            // 年末調整他
            this.ViewModel.YearEndTaxAdjustment  = entity.YearEndTaxAdjustment;
            // 備考
            this.ViewModel.Remarks               = entity.Remarks;
            // 控除額計
            this.ViewModel.TotalDeduct           = entity.TotalDeduct.Value;
        }

        /// <summary>
        /// リロード
        /// </summary>
        public void Reload()
        {
            var deductionTable = new DeductionSQLite();
            this.ViewModel.Entity          = deductionTable.GetEntity(this.Header.Year,     this.Header.Month);
            this.ViewModel.Entity_LastYear = deductionTable.GetEntity(this.Header.Year - 1, this.Header.Month);

            this.Refresh();
        }

        /// <summary>
        /// クリア
        /// </summary>
        public void Clear()
        {
            // 健康保険
            this.ViewModel.HealthInsurance       = default(double);
            // 介護保険
            this.ViewModel.NursingInsurance      = default(double);
            // 厚生年金
            this.ViewModel.WelfareAnnuity        = default(double);
            // 雇用保険
            this.ViewModel.EmploymentInsurance   = default(double);
            // 所得税
            this.ViewModel.IncomeTax             = default(double);
            // 市町村税
            this.ViewModel.MunicipalTax          = default(double);
            // 互助会
            this.ViewModel.FriendshipAssociation = default(double);
            // 年末調整他
            this.ViewModel.YearEndTaxAdjustment  = default(double);
            // 備考
            this.ViewModel.Remarks               = default(string);
            // 控除額計
            this.ViewModel.TotalDeduct           = default(double);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="transaction">トランザクション</param>
        public void Save(SQLiteTransaction transaction)
        {
            var entity = new DeductionEntity(
                            this.Header.ID,
                            this.Header.YearMonth,
                            this.ViewModel.HealthInsurance,
                            this.ViewModel.NursingInsurance,
                            this.ViewModel.WelfareAnnuity,
                            this.ViewModel.EmploymentInsurance,
                            this.ViewModel.IncomeTax,
                            this.ViewModel.MunicipalTax,
                            this.ViewModel.FriendshipAssociation,
                            this.ViewModel.YearEndTaxAdjustment,
                            this.ViewModel.Remarks,
                            this.ViewModel.TotalDeduct);

            var deduction = new DeductionSQLite();
            deduction.Save(transaction, entity);
        }

        /// <summary>
        /// 再計算
        /// </summary>
        internal void ReCaluculate()
        {
            if (this.ViewModel is null)
            {
                return;
            }

            this.ViewModel.TotalDeduct = this.ViewModel.HealthInsurance
                                       + this.ViewModel.NursingInsurance
                                       + this.ViewModel.WelfareAnnuity
                                       + this.ViewModel.EmploymentInsurance
                                       + this.ViewModel.IncomeTax
                                       + this.ViewModel.MunicipalTax
                                       + this.ViewModel.FriendshipAssociation
                                       + this.ViewModel.YearEndTaxAdjustment;

            this.Allowance.ReCaluculate();
        }
    }
}
