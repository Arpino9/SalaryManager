﻿using System.Configuration;

namespace SalaryManager.Domain
{
    /// <summary>
    /// Shread
    /// </summary>
    /// <remarks>
    /// configファイルとDomain, Infrastructure層の橋渡しクラス。
    /// </remarks>
    public static class Shared
    {
        /// <summary> 日付フォーマット </summary>
        public static string YearFormat = ConfigurationManager.AppSettings["YearFormat"];

        /// <summary> CSVの保存ディレクトリ </summary>
        public static string DirectoryCSV = ConfigurationManager.AppSettings["DirectoryCSV"];

        /// <summary> 給与明細Excelのテンプレートファイル名 </summary>
        public static string ExcelTemplateName = ConfigurationManager.AppSettings["ExcelTemplateName"];

        /// <summary> XMLのファイル名 </summary>
        public static string XMLName = ConfigurationManager.AppSettings["XMLName"];
       
        /// <summary> フォントファミリ </summary>
        public static string FontFamily = ConfigurationManager.AppSettings["FontFamily"];
    }
}
