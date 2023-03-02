using ClosedXML.Excel;
using SalaryManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryManager.Domain.Logics
{
    /// <summary>
    /// Utility - Excel
    /// </summary>
    public sealed class Excel
    {
        public Excel()
        {
            
        }

        /// <summary> Workbook </summary>
        public XLWorkbook Workbook { get; } = new XLWorkbook(Shared.PathOutputPayslip);

        /// <summary> Worksheet </summary>
        public IXLWorksheet Worksheet => this.Workbook.Worksheet("Sheet1");

        /// <summary> 拡張子 </summary>
        public static readonly string FileExtension = "xlsx";

        /// <summary>
        /// Workbook保存
        /// </summary>
        /// <param name="directory">保存先ディレクトリ</param>
        public void CopyAsWorkbook(string directory)
        {
            if (string.IsNullOrEmpty(directory))
            {
                return;
            }

            var year  = DateTime.Today.ToString("yyyy");
            var month = DateTime.Today.ToString("MM");
            var day   = DateTime.Today.ToString("dd");

            this.Workbook.SaveAs($@"{directory}\Payslips_{year}{month}{day}.{Excel.FileExtension}");
        }

        /// <summary>
        /// Write - ヘッダ
        /// </summary>
        /// <param name="entities">エンティティ</param>
        /// <param name="row">行</param>
        /// <returns>void</returns>
        public async System.Threading.Tasks.Task WriteAllHeader(IReadOnlyList<HeaderEntity> entities, int row)
        {
            if (!entities.Any())
            {
                return;
            }

            foreach (var entity in entities)
            {
                // 年月
                this.Worksheet.Cell(row, 3).Value = entity.YearMonth;

                row++;
            }

            await System.Threading.Tasks.Task.CompletedTask;
        }

        /// <summary>
        /// Write - 支給額
        /// </summary>
        /// <param name="entities">エンティティ</param>
        /// <param name="row">行</param>
        /// <returns>void</returns>
        public async System.Threading.Tasks.Task WriteAllAllowance(IReadOnlyList<AllowanceEntity> entities, int row)
        {
            if (!entities.Any())
            {
                return;
            }

            foreach (var entity in entities ) 
            {
                // 基本給
                this.Worksheet.Cell(row, 4).Value = entity.BasicSalary.Value;
                // 役職手当
                this.Worksheet.Cell(row, 5).Value = entity.ExecutiveAllowance.Value;
                // 扶養手当
                this.Worksheet.Cell(row, 6).Value = entity.DependencyAllowance.Value;
                // 時間外手当
                this.Worksheet.Cell(row, 7).Value = entity.OvertimeAllowance.Value;
                // 休日割増
                this.Worksheet.Cell(row, 8).Value = entity.DaysoffIncreased.Value;
                // 深夜割増
                this.Worksheet.Cell(row, 9).Value = entity.NightworkIncreased.Value;
                // 住宅手当
                this.Worksheet.Cell(row, 10).Value = entity.HousingAllowance.Value;
                // 遅刻早退欠勤
                this.Worksheet.Cell(row, 11).Value = entity.LateAbsent;
                // 交通費
                this.Worksheet.Cell(row, 12).Value = entity.TransportationExpenses.Value;
                // 在宅手当
                this.Worksheet.Cell(row, 13).Value = entity.ElectricityAllowance.Value;
                // 特別手当
                this.Worksheet.Cell(row, 14).Value = entity.SpecialAllowance.Value;
                // 予備
                this.Worksheet.Cell(row, 15).Value = entity.SpareAllowance.Value;
                // 備考
                this.Worksheet.Cell(row, 16).Value = entity.Remarks;
                // 支給総計
                this.Worksheet.Cell(row, 40).Value = entity.TotalSalary;
                // 差引支給額
                this.Worksheet.Cell(row, 43).Value = entity.TotalDeductedSalary;

                row++;
            }

            await System.Threading.Tasks.Task.CompletedTask;
        }

        /// <summary>
        /// Write - 控除額
        /// </summary>
        /// <param name="entities">エンティティ</param>
        /// <param name="row">行</param>
        /// <returns>void</returns>
        public async System.Threading.Tasks.Task WriteAllDeduction(IReadOnlyList<DeductionEntity> entities, int row)
        {
            if (!entities.Any())
            {
                return;
            }

            foreach (var entity in entities)
            {
                // 健康保険
                this.Worksheet.Cell(row, 17).Value = entity.HealthInsurance.Value;
                // 介護保険
                this.Worksheet.Cell(row, 18).Value = entity.NursingInsurance.Value;
                // 厚生年金
                this.Worksheet.Cell(row, 19).Value = entity.WelfareAnnuity.Value;
                // 雇用保険
                this.Worksheet.Cell(row, 20).Value = entity.EmploymentInsurance.Value;
                // 所得税
                this.Worksheet.Cell(row, 21).Value = entity.IncomeTax.Value;
                // 市町村税
                this.Worksheet.Cell(row, 22).Value = entity.MunicipalTax.Value;
                // 互助会
                this.Worksheet.Cell(row, 23).Value = entity.FriendshipAssociation.Value;
                // 年末調整他
                this.Worksheet.Cell(row, 24).Value = entity.YearEndTaxAdjustment;
                // 備考
                this.Worksheet.Cell(row, 25).Value = entity.Remarks;
                // 控除額計
                this.Worksheet.Cell(row, 41).Value = entity.TotalDeduct;

                row++;
            }

            await System.Threading.Tasks.Task.CompletedTask;
        }

        /// <summary>
        /// Write - 勤務備考
        /// </summary>
        /// <param name="entities">エンティティ</param>
        /// <param name="row">行</param>
        /// <returns>void</returns>
        public async System.Threading.Tasks.Task WriteAllWorkingReferences(IReadOnlyList<WorkingReferencesEntity> entities, int row)
        {
            if (!entities.Any())
            {
                return;
            }

            foreach (var entity in entities)
            {
                // 時間外時間
                this.Worksheet.Cell(row, 26).Value = entity.OvertimeTime;
                // 休出時間
                this.Worksheet.Cell(row, 27).Value = entity.WeekendWorktime;
                // 深夜時間
                this.Worksheet.Cell(row, 28).Value = entity.MidnightWorktime;
                // 遅刻早退欠勤H
                this.Worksheet.Cell(row, 29).Value = entity.LateAbsentH;
                // 支給額-保険
                this.Worksheet.Cell(row, 30).Value = entity.Insurance.Value;
                // 標準月額千円
                this.Worksheet.Cell(row, 31).Value = entity.Norm;
                // 扶養人数
                this.Worksheet.Cell(row, 32).Value = entity.NumberOfDependent;
                // 有給残日数
                this.Worksheet.Cell(row, 33).Value = entity.PaidVacation.Value;
                // 勤務時間
                this.Worksheet.Cell(row, 34).Value = entity.WorkingHours;
                // 勤務時間
                this.Worksheet.Cell(row, 35).Value = entity.Remarks;
                // 勤務先
                this.Worksheet.Cell(row, 2).Value  = entity.WorkPlace;

                row++;
            }

            await System.Threading.Tasks.Task.CompletedTask;
        }

        /// <summary>
        /// Write - 副業
        /// </summary>
        /// <param name="entities">エンティティ</param>
        /// <param name="row">行</param>
        public async System.Threading.Tasks.Task WriteAllSideBusiness(IReadOnlyList<SideBusinessEntity> entities, int row)
        {
            if (!entities.Any())
            {
                return;
            }

            foreach (var entity in entities)
            {
                // 副業収入
                this.Worksheet.Cell(row, 36).Value = entity.SideBusiness;
                // 臨時収入
                this.Worksheet.Cell(row, 37).Value = entity.Perquisite;
                // その他
                this.Worksheet.Cell(row, 38).Value = entity.Others;
                // 備考
                this.Worksheet.Cell(row, 39).Value = entity.Remarks;

                row++;
            }

            await System.Threading.Tasks.Task.CompletedTask;
        }
    }
}
