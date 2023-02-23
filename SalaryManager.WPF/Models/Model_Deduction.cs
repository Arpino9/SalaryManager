using System;
using System.Linq;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Interface;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.ViewModels;

namespace SalaryManager.WPF.Models
{
    /// <summary>
    /// Model - 控除額
    /// </summary>
    public class Model_Deduction : IInputPayroll
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

        /// <summary> Entity - 控除額 </summary>
        private DeductionEntity Entity { get; set; }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            var sqlite = new DeductionSQLite();
            var records = sqlite.GetEntities();

            this.Entity = records.Where(record => record.YearMonth.Year  == DateTime.Today.Year
                                               && record.YearMonth.Month == DateTime.Today.Month)
                                 .FirstOrDefault();

            if (this.Entity is null)
            {
                // レコードなし
                var header        = new HeaderSQLite();
                var defaultEntity = header.GetDefaultEntity();

                if (defaultEntity != null)
                {
                    this.Entity = records.Where(record => record.ID == defaultEntity.ID)
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
            if (this.Entity is null)
            {
                this.Clear();
                return;
            }

            // 健康保険
            this.ViewModel.HealthInsurance       = this.Entity.HealthInsurance;
            // 介護保険
            this.ViewModel.NursingInsurance      = this.Entity.NursingInsurance;
            // 厚生年金
            this.ViewModel.WelfareAnnuity        = this.Entity.WelfareAnnuity;
            // 雇用保険
            this.ViewModel.EmploymentInsurance   = this.Entity.EmploymentInsurance;
            // 所得税
            this.ViewModel.IncomeTax             = this.Entity.IncomeTax;
            // 市町村税
            this.ViewModel.MunicipalTax          = this.Entity.MunicipalTax;
            // 互助会
            this.ViewModel.FriendshipAssociation = this.Entity.FriendshipAssociation;
            // 年末調整他
            this.ViewModel.YearEndTaxAdjustment  = this.Entity.YearEndTaxAdjustment;
            // 備考
            this.ViewModel.Remarks               = this.Entity.Remarks;
            // 控除額計
            this.ViewModel.TotalDeduct           = this.Entity.TotalDeduct;
        }

        /// <summary>
        /// リロード
        /// </summary>
        public void Reload()
        {
            var deductionTable = new DeductionSQLite();
            this.Entity = deductionTable.GetEntity(this.Header.Year, this.Header.Month);
            
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
        /// 登録
        /// </summary>
        public void Register()
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
            deduction.Save(entity);
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
        }
    }
}
