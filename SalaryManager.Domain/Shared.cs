using System.Configuration;

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
        /// <summary> Excelの保存先 </summary>
        public static string PathOutputPayslip = ConfigurationManager.AppSettings["PathOutputPayslip"];
       
        /// <summary> XMLの保存先 </summary>
        public static string XMLPath = ConfigurationManager.AppSettings["XMLPath"];
    }
}
