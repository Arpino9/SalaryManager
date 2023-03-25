namespace SalaryManager.Infrastructure.XML
{
    /// <summary>
    /// XMLタグ
    /// </summary>
    /// <remarks>
    /// XML生成時に出力されるタグの一覧。適宜追加すること。
    /// </remarks>
    public sealed class XMLTag
    {
        /// <summary> Excelテンプレートのパス </summary>
        public string ExcelTemplatePath;

        /// <summary> SQLiteのパス </summary>
        public string SQLitePath;

        /// <summary> フォントファミリ </summary>
        public string FontFamily;

        /// <summary> フォントサイズ </summary>
        public decimal FontSize;

        /// <summary> 初期表示時にデフォルト明細を表示する </summary>
        public bool ShowDefaultPayslip;

        /// <summary> 背景色 (カラーコード) </summary>
        public string BackgroundColor_ColorCode;

        /// <summary> 背景色 (ARGB) </summary>
        public string BackgroundColor;
    }
}
