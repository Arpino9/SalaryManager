using SalaryManager.Domain.Entities;

namespace SalaryManager.Infrastructure.SpreadSheet
{
    /// <summary>
    /// スプレッドシートのカラム定義
    /// </summary>
    public sealed class SpreadSheetDefinition
    {
        /// <summary>
        /// シート名
        /// </summary>
        public struct SheetName
        {
            /// <summary> 概要 </summary>
            public static string Outline = "収支推移";

            /// <summary> 明細 </summary>
            public static string Detail = "給与明細";
        }

        /// <summary> 秘密鍵 </summary>
        /// <remarks> 対象のブックと紐付ける </remarks>
        internal static readonly string PrivateKey = @"C:\Users\OKAJIMA\source\repos\SalaryManager\SalaryManager.Infrastructure\SpreadSheet\cogent-jetty-298301-aa9962858f21.json";

        /// <summary> ID </summary>
        internal static readonly string SheetId = "1nvm_16P7d5qjujxg5vqn2xy3N6ye5AE4v0wa4ZWaXPQ";

        /// <summary> 範囲 (収支推移) </summary>
        internal static readonly string OutlineRange = $"{SpreadSheetDefinition.SheetName.Outline}!B:G";

        /// <summary> 範囲 (給与明細) </summary>
        internal static readonly string DetailRange = $"{SpreadSheetDefinition.SheetName.Detail}!B:AQ";

        /// <summary> Entity - ヘッダ </summary>
        internal HeaderEntity HeaderEntity { get; set; }

        /// <summary> Entity - 支給額 </summary>
        internal AllowanceValueEntity AllowanceValueEntity { get; set; }

        /// <summary> Entity - 控除額 </summary>
        internal DeductionEntity DeductionEntity { get; set; }

        /// <summary> Entity - 勤務備考 </summary>
        internal WorkingReferencesEntity WorkingReferencesEntity { get; set; }

        /// <summary> Entity - 副業 </summary>
        internal SideBusinessEntity SideBusinessEntity { get; set; }
    }
}
