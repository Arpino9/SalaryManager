﻿using ClosedXML.Excel;
using SalaryManager.Domain;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Modules.Logics;
using SalaryManager.Domain.Repositories;
using SalaryManager.Infrastructure.XML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryManager.Infrastructure.Excel
{
    /// <summary>
    /// Excel書き込み
    /// </summary>
    public sealed class ExcelWriter : IExcelWriterRepository
    {
        public ExcelWriter()
        {
            if (Shared.XMLName is null)
            {
                return;
            }

            this.Workbook = new XLWorkbook(XMLLoader.FetchExcelTemplatePath());
        }

        /// <summary> Workbook </summary>
        public XLWorkbook Workbook { get; }

        /// <summary> Worksheet - 給与明細 </summary>
        public IXLWorksheet Worksheet_Payslip => this.Workbook.Worksheet("給与明細");

        /// <summary> Worksheet - 収支推移 </summary>
        public IXLWorksheet Worksheet_Budget => this.Workbook.Worksheet("収支推移");

        /// <summary> デフォルト列 </summary>
        public static readonly int DefaultColumn = 2;
        /// <summary> デフォルト行 </summary>
        /// <remarks> Excelのヘッダ行 </remarks>
        public static readonly int DefaultRow = 5;

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

            try
            {
                this.Workbook.SaveAs($@"{directory}\Payslips_{year}{month}{day}.xlsx");
            }
            catch (IOException ex)
            {
                Message.ShowErrorMessage("更新先のExcelが開いたままです。Excelを閉じて再度出力してください。", "Excel出力エラー");
            }
        }

        /// <summary>
        /// Write - ヘッダ
        /// </summary>
        /// <param name="entities">エンティティ</param>
        /// <returns>void</returns>
        public async Task WriteAllHeader(IReadOnlyList<HeaderEntity> entities)
        {
            if (!entities.Any())
            {
                return;
            }

            var row = ExcelWriter.DefaultRow;

            foreach (var entity in entities)
            {
                // 年月
                this.Worksheet_Budget.Cell(row - 1, 2).Value = entity.YearMonth;
                this.Worksheet_Payslip.Cell(row, 3).Value = entity.YearMonth;

                row++;
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// Write - 支給額
        /// </summary>
        /// <param name="entities">エンティティ</param>
        /// <returns>void</returns>
        public async Task WriteAllAllowance(IReadOnlyList<AllowanceValueEntity> entities)
        {
            if (!entities.Any())
            {
                return;
            }

            var row = ExcelWriter.DefaultRow;

            foreach (var entity in entities)
            {
                // 基本給
                this.Worksheet_Payslip.Cell(row, 4).Value = entity.BasicSalary;
                // 役職手当
                this.Worksheet_Payslip.Cell(row, 5).Value = entity.ExecutiveAllowance;
                // 扶養手当
                this.Worksheet_Payslip.Cell(row, 6).Value = entity.DependencyAllowance;
                // 時間外手当
                this.Worksheet_Payslip.Cell(row, 7).Value = entity.OvertimeAllowance;
                // 休日割増
                this.Worksheet_Payslip.Cell(row, 8).Value = entity.DaysoffIncreased;
                // 深夜割増
                this.Worksheet_Payslip.Cell(row, 9).Value = entity.NightworkIncreased;
                // 住宅手当
                this.Worksheet_Payslip.Cell(row, 10).Value = entity.HousingAllowance;
                // 遅刻早退欠勤
                this.Worksheet_Payslip.Cell(row, 11).Value = entity.LateAbsent;
                // 交通費
                this.Worksheet_Payslip.Cell(row, 12).Value = entity.TransportationExpenses;
                // 在宅手当
                this.Worksheet_Payslip.Cell(row, 13).Value = entity.ElectricityAllowance;
                // 特別手当
                this.Worksheet_Payslip.Cell(row, 14).Value = entity.SpecialAllowance;
                // 予備
                this.Worksheet_Payslip.Cell(row, 15).Value = entity.SpareAllowance;
                // 備考
                this.Worksheet_Payslip.Cell(row, 16).Value = entity.Remarks;
                // 支給総計
                this.Worksheet_Budget.Cell(row - 1, 4).Value = entity.TotalSalary;
                this.Worksheet_Payslip.Cell(row, 40).Value = entity.TotalSalary;
                // 差引支給額
                this.Worksheet_Budget.Cell(row - 1, 6).Value = entity.TotalDeductedSalary;
                this.Worksheet_Payslip.Cell(row, 43).Value = entity.TotalDeductedSalary;

                row++;
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// Write - 控除額
        /// </summary>
        /// <param name="entities">エンティティ</param>
        /// <returns>void</returns>
        public async Task WriteAllDeduction(IReadOnlyList<DeductionEntity> entities)
        {
            if (!entities.Any())
            {
                return;
            }

            var row = ExcelWriter.DefaultRow;

            foreach (var entity in entities)
            {
                // 健康保険
                this.Worksheet_Payslip.Cell(row, 17).Value = entity.HealthInsurance;
                // 介護保険
                this.Worksheet_Payslip.Cell(row, 18).Value = entity.NursingInsurance;
                // 厚生年金
                this.Worksheet_Payslip.Cell(row, 19).Value = entity.WelfareAnnuity;
                // 雇用保険
                this.Worksheet_Payslip.Cell(row, 20).Value = entity.EmploymentInsurance;
                // 所得税
                this.Worksheet_Payslip.Cell(row, 21).Value = entity.IncomeTax;
                // 市町村税
                this.Worksheet_Payslip.Cell(row, 22).Value = entity.MunicipalTax;
                // 互助会
                this.Worksheet_Payslip.Cell(row, 23).Value = entity.FriendshipAssociation;
                // 年末調整他
                this.Worksheet_Payslip.Cell(row, 24).Value = entity.YearEndTaxAdjustment;
                // 備考
                this.Worksheet_Payslip.Cell(row, 25).Value = entity.Remarks;
                // 控除額計
                this.Worksheet_Budget.Cell(row - 1, 5).Value = entity.TotalDeduct;
                this.Worksheet_Payslip.Cell(row, 41).Value = entity.TotalDeduct;

                row++;
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// Write - 勤務備考
        /// </summary>
        /// <param name="entities">エンティティ</param>
        /// <returns>void</returns>
        public async Task WriteAllWorkingReferences(IReadOnlyList<WorkingReferencesEntity> entities)
        {
            if (!entities.Any())
            {
                return;
            }

            var row = ExcelWriter.DefaultRow;

            foreach (var entity in entities)
            {
                // 時間外時間
                this.Worksheet_Payslip.Cell(row, 26).Value = entity.OvertimeTime;
                // 休出時間
                this.Worksheet_Payslip.Cell(row, 27).Value = entity.WeekendWorktime;
                // 深夜時間
                this.Worksheet_Payslip.Cell(row, 28).Value = entity.MidnightWorktime;
                // 遅刻早退欠勤H
                this.Worksheet_Payslip.Cell(row, 29).Value = entity.LateAbsentH;
                // 支給額-保険
                this.Worksheet_Payslip.Cell(row, 30).Value = entity.Insurance;
                // 標準月額千円
                this.Worksheet_Payslip.Cell(row, 31).Value = entity.Norm;
                // 扶養人数
                this.Worksheet_Payslip.Cell(row, 32).Value = entity.NumberOfDependent;
                // 有給残日数
                this.Worksheet_Payslip.Cell(row, 33).Value = entity.PaidVacation.Value;
                // 勤務時間
                this.Worksheet_Payslip.Cell(row, 34).Value = entity.WorkingHours;
                // 勤務時間
                this.Worksheet_Payslip.Cell(row, 35).Value = entity.Remarks;
                // 勤務先
                this.Worksheet_Budget.Cell(row - 1, 3).Value = entity.WorkPlace;
                this.Worksheet_Payslip.Cell(row, 2).Value = entity.WorkPlace;

                row++;
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// Write - 副業
        /// </summary>
        /// <param name="entities">エンティティ</param>
        /// <returns>void</returns>
        public async Task WriteAllSideBusiness(IReadOnlyList<SideBusinessEntity> entities)
        {
            if (!entities.Any())
            {
                return;
            }

            var row = ExcelWriter.DefaultRow;

            foreach (var entity in entities)
            {
                // 副業収入
                this.Worksheet_Payslip.Cell(row, 36).Value = entity.SideBusiness;
                // 臨時収入
                this.Worksheet_Payslip.Cell(row, 37).Value = entity.Perquisite;
                // その他
                this.Worksheet_Payslip.Cell(row, 38).Value = entity.Others;
                // 備考
                this.Worksheet_Payslip.Cell(row, 39).Value = entity.Remarks;

                row++;
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// スタイルの設定
        /// </summary>
        /// <returns>void</returns>
        public async Task SetStyle()
        {
            var date = Domain.StaticValues.Headers.FetchByDescending();

            // 罫線
            var style_Payslip = this.Worksheet_Payslip.Range(ExcelWriter.DefaultRow, ExcelWriter.DefaultColumn, date.Count + ExcelWriter.DefaultRow - 1, 43).Style;
            style_Payslip.Border.InsideBorder = XLBorderStyleValues.Thin;
            style_Payslip.Border.OutsideBorder = XLBorderStyleValues.Thin;

            var style_Budget = this.Worksheet_Budget.Range(ExcelWriter.DefaultRow - 1, ExcelWriter.DefaultColumn, date.Count + ExcelWriter.DefaultRow - 2, 7).Style;
            style_Budget.Border.InsideBorder = XLBorderStyleValues.Thin;
            style_Budget.Border.OutsideBorder = XLBorderStyleValues.Thin;

            // 行間の自動調整
            for (var column = ExcelWriter.DefaultColumn; column < 43; column++)
            {
                this.Worksheet_Payslip.Column(column).AdjustToContents();
                this.Worksheet_Budget.Column(column).AdjustToContents();
            }

            // 揃え方向
            style_Payslip.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            style_Budget.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

            await Task.CompletedTask;
        }
    }
}
