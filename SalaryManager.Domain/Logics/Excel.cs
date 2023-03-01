using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing;
using SalaryManager.Domain.Helpers;
using System;

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

        /// <summary> 拡張子 </summary>
        public static readonly string FileExtension = "xlsx";

        public void Copy()
        {
            var selector  = new DirectorySelector();
            var directory = selector.Select("出力先のフォルダを選択してください。");

            if (string.IsNullOrEmpty(directory)) 
            {
                return;
            }

            var year  = DateTime.Today.ToString("yyyy");
            var month = DateTime.Today.ToString("MM");
            var day   = DateTime.Today.ToString("dd");

            this.Workbook.SaveAs($@"{directory}\Payslips_{year}{month}{day}.{Excel.FileExtension}");
        }
    }
}
