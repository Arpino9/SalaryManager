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

        /// <summary> Excelの保存先 </summary>
        public static string PathExcel = ConfigurationManager.AppSettings["PathCSV"];

        /// <summary> 接続先 </summary>
        public static string ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
    }
}
