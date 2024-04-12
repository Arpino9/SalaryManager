using Newtonsoft.Json;

namespace SalaryManager.Infrastructure.JSON
{
    /// <summary>
    /// JSON属性
    /// </summary>
    /// <remarks>
    /// 原則、オプションの区分ごとに設定すること。
    /// </remarks>
    public record class JSONProperty_Settings
    {
        /// <summary> 全般 </summary>
        [JsonProperty("general")]
        public General General { get; set; }

        /// <summary> SpreadSheet </summary>
        [JsonProperty("spreadSheet")]
        public SpreadSheet SpreadSheet { get; set; }

        /// <summary> Excel </summary>
        [JsonProperty("excel")]
        public Excel Excel { get; set; }

        /// <summary> SQLite </summary>
        [JsonProperty("sqlite")]
        public SQLite SQLite { get; set; }

        /// <summary> Googleカレンダー </summary>
        [JsonProperty("googleCalendar")]
        public GoogleCalendar GoogleCalendar { get; set; }

        /// <summary> PDF </summary>
        [JsonProperty("pdf")]
        public PDF PDF { get; set; }
    }

    /// <summary>
    /// JSON属性 - 全般
    /// </summary>
    public record class General
    {
        /// <summary> フォントファミリ </summary>
        [JsonProperty("fontFamily")]
        public string FontFamily { get; set; }

        /// <summary> フォントサイズ </summary>
        [JsonProperty("fontSize")]
        public decimal FontSize { get; set; }

        /// <summary> 初期明細の要否 </summary>
        [JsonProperty("showDefaultPayslip")]
        public bool ShowDefaultPayslip { get; set; }

        /// <summary> 背景色(RGB) </summary>
        [JsonProperty("backgroundColor_ColorCode")]
        public string BackgroundColor_ColorCode { get; set; }

        /// <summary> 背景色 </summary>
        [JsonProperty("backgroundColor")]
        public string BackgroundColor { get; set; }

        /// <summary> 画像の保存方法 </summary>
        [JsonProperty("howToSaveImage")]
        public string HowToSaveImage { get; set; }

        /// <summary> 画像の保存先パス </summary>
        [JsonProperty("imageFolderPath")]
        public string ImageFolderPath { get; set; }
    }

    /// <summary>
    /// JSON属性 - Googleカレンダー
    /// </summary>
    public record class GoogleCalendar
    {
        /// <summary> 秘密鍵のパス </summary>
        [JsonProperty("privateKeyPath")]
        public string PrivateKeyPath { get; set; }

        /// <summary> ID </summary>
        [JsonProperty("id")]
        public string ID { get; set; }
    }

    /// <summary>
    /// JSON属性 - スプレッドシート
    /// </summary>
    public record class SpreadSheet
    {
        /// <summary> 秘密鍵のパス </summary>
        [JsonProperty("privateKeyPath")]
        public string PrivateKeyPath { get; set; }

        /// <summary> ID </summary>
        [JsonProperty("id")]
        public string ID { get; set; }
    }

    /// <summary>
    /// JSON属性 - Excel
    /// </summary>
    public record class Excel
    {
        /// <summary> テンプレートのパス </summary>
        [JsonProperty("templatePath")]
        public string TemplatePath { get; set; }
    }

    /// <summary>
    /// JSON属性 - SQLite
    /// </summary>
    public record class SQLite
    {
        /// <summary> パス </summary>
        [JsonProperty("path")]
        public string Path { get; set; }
    }

    /// <summary>
    /// JSON属性 - PDF
    /// </summary>
    public record class PDF
    {
        /// <summary> パスワード </summary>
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
