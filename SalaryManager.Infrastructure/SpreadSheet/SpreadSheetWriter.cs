using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Modules.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SalaryManager.Infrastructure.SpreadSheet
{
    /// <summary>
    /// スプレッドシート - Writer
    /// </summary>
    public class SpreadSheetWriter
    {
        /// <summary> 全ての給与明細 </summary>
        private List<SpreadSheetDefinition> Payslips { get; set; } = new List<SpreadSheetDefinition>();

        /// <summary> ヘッダ </summary>
        private IReadOnlyList<HeaderEntity> Headers { get; set; }

        /// <summary> 支給額 </summary>
        private IReadOnlyList<AllowanceValueEntity> Allowances { get; set; }

        /// <summary> 控除額 </summary>
        private IReadOnlyList<DeductionEntity> Deductions { get; set; }

        /// <summary> 勤務備考 </summary>
        private IReadOnlyList<WorkingReferencesEntity> WorkingReferenceses { get; set; }

        /// <summary> 副業額 </summary>
        private IReadOnlyList<SideBusinessEntity> SideBusinesses { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="headers">ヘッダ</param>
        /// <param name="allowances">支給額</param>
        /// <param name="deductionEntity">控除額</param>
        /// <param name="workingReferencesEntity">勤務備考</param>
        /// <param name="sideBusinessEntity">副業額</param>
        public SpreadSheetWriter(
            IReadOnlyList<HeaderEntity> headers,
            IReadOnlyList<AllowanceValueEntity> allowances,
            IReadOnlyList<DeductionEntity> deductionEntity,
            IReadOnlyList<WorkingReferencesEntity> workingReferencesEntity,
            IReadOnlyList<SideBusinessEntity> sideBusinessEntity)
        {
            this.Headers             = headers;
            this.Allowances          = allowances;
            this.Deductions          = deductionEntity;
            this.WorkingReferenceses = workingReferencesEntity;
            this.SideBusinesses      = sideBusinessEntity;
        }

        /// <summary>
        /// 書き込み
        /// </summary>
        /// <remarks>
        /// 行単位で、明細データを書き込む
        /// </remarks>
        private void Write()
        {
            using (var fileStream = new FileStream(SpreadSheetDefinition.PrivateKey, FileMode.Open, FileAccess.Read))
            {
                var googleCredential = GoogleCredential.FromStream(fileStream)
                                                       .CreateScoped(SheetsService.Scope.Spreadsheets);

                var initializer = new BaseClientService.Initializer() { HttpClientInitializer = googleCredential };
                var sheetsService = new SheetsService(initializer);

                this.WriteOutline(sheetsService);
                this.WriteDetail(sheetsService);
            }
        }

        /// <summary>
        /// 書き込み - 概要
        /// </summary>
        /// <param name="sheetsService">Googleドライブのサービス定義</param>
        private void WriteOutline(SheetsService sheetsService)
        {
            SpreadSheetReader.ReadOutlineHeader(SpreadSheetDefinition.SheetName.Outline);

            var columns = SpreadSheetReader.CellValues;

            foreach (var payslip in this.Payslips)
            {
                var date = DateUtils.ConvertToSQLiteValue(payslip.HeaderEntity.YearMonth);

                columns.Add(new List<object>() { date,
                                                 payslip.WorkingReferencesEntity.WorkPlace,
                                                 payslip.AllowanceValueEntity.TotalSalary.ToString(),
                                                 payslip.DeductionEntity.TotalDeduct.ToString(),
                                                 payslip.AllowanceValueEntity.TotalDeductedSalary.ToString(),
                                                 payslip.SideBusinessEntity.SideBusiness});
            }

            var body = new ValueRange() { Values = columns };

            var request = sheetsService.Spreadsheets.Values.Update(body, SpreadSheetDefinition.SheetId, SpreadSheetDefinition.OutlineRange);
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            request.Execute();
        }

        /// <summary>
        /// 書き込み - 明細
        /// </summary>
        /// <param name="sheetsService">Googleドライブのサービス定義</param>
        private void WriteDetail(SheetsService sheetsService)
        {
            SpreadSheetReader.ReadOutlineHeader(SpreadSheetDefinition.SheetName.Detail);

            var columns = SpreadSheetReader.CellValues;

            foreach (var payslip in this.Payslips)
            {
                var allowance = payslip.AllowanceValueEntity;
                var deduction = payslip.DeductionEntity;
                var workingReference = payslip.WorkingReferencesEntity;
                var sideBusiness = payslip.SideBusinessEntity;

                var date = DateUtils.ConvertToSQLiteValue(payslip.HeaderEntity.YearMonth);

                columns.Add(new List<object>() { // 勤務先と日付
                                                 payslip.WorkingReferencesEntity.WorkPlace,
                                                 date,

                                                 // 支給額
                                                 allowance.BasicSalary.ToString(),
                                                 allowance.ExecutiveAllowance.ToString(),
                                                 allowance.DependencyAllowance.ToString(),
                                                 allowance.OvertimeAllowance.ToString(),
                                                 allowance.DaysoffIncreased.ToString(),
                                                 allowance.NightworkIncreased.ToString(),
                                                 allowance.HousingAllowance.ToString(),
                                                 allowance.LateAbsent,
                                                 allowance.TransportationExpenses.ToString(),
                                                 allowance.ElectricityAllowance.ToString(),
                                                 allowance.SpecialAllowance.ToString(),
                                                 allowance.SpareAllowance.ToString(),
                                                 allowance.Remarks,

                                                 // 控除額
                                                 deduction.HealthInsurance.ToString(),
                                                 deduction.NursingInsurance.ToString(),
                                                 deduction.WelfareAnnuity.ToString(),
                                                 deduction.EmploymentInsurance.ToString(),
                                                 deduction.IncomeTax.ToString(),
                                                 deduction.MunicipalTax.ToString(),
                                                 deduction.FriendshipAssociation.ToString(),
                                                 deduction.YearEndTaxAdjustment.ToString(),
                                                 deduction.Remarks,

                                                 // 勤務備考
                                                 workingReference.OvertimeTime,
                                                 workingReference.WeekendWorktime,
                                                 workingReference.MidnightWorktime,
                                                 workingReference.LateAbsentH,
                                                 workingReference.Insurance.ToString(),
                                                 workingReference.Norm,
                                                 workingReference.NumberOfDependent,
                                                 workingReference.PaidVacation.ToString(),
                                                 workingReference.WorkingHours,
                                                 workingReference.Remarks,

                                                 // 副業
                                                 sideBusiness.SideBusiness.ToString(),
                                                 sideBusiness.Perquisite,
                                                 sideBusiness.Others.ToString(),
                                                 sideBusiness.Remarks,

                                                 // 合計額
                                                 allowance.TotalSalary.ToString(),
                                                 payslip.DeductionEntity.TotalDeduct.ToString(),
                                                 payslip.SideBusinessEntity.SideBusiness,
                                                 payslip.AllowanceValueEntity.TotalDeductedSalary.ToString()});                
            }

            var body = new ValueRange() { Values = columns };

            var request = sheetsService.Spreadsheets.Values.Update(body, SpreadSheetDefinition.SheetId, SpreadSheetDefinition.DetailRange);
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            request.Execute();
        }

        /// <summary>
        /// Write - 明細
        /// </summary>
        public void WritePayslips()
        {
            if (this.Headers is null)
            {
                return;
            }

            foreach (var entity in this.Headers)
            {
                var payslip = new SpreadSheetDefinition();

                // 年月
                payslip.HeaderEntity = entity;
                // 支給額
                payslip.AllowanceValueEntity    = this.Allowances.ToList().Find(x => x.YearMonth == entity.YearMonth);
                // 控除額
                payslip.DeductionEntity         = this.Deductions.ToList().Find(x => x.YearMonth == entity.YearMonth);
                // 勤怠備考
                payslip.WorkingReferencesEntity = this.WorkingReferenceses.ToList().Find(x => x.YearMonth == entity.YearMonth);
                // 副業
                payslip.SideBusinessEntity      = this.SideBusinesses.ToList().Find(x => x.YearMonth == entity.YearMonth);

                this.Payslips.Add(payslip);
            }

            this.Write();
        }
    }
}
