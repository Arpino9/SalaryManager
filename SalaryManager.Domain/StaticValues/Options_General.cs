using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.Domain.Modules.Logics;
using System.Drawing;
using System.Windows.Media;

namespace SalaryManager.Domain.StaticValues
{
    public sealed class Options_General
    {
        private static Settings _settings;

        /// <summary>
        /// 値の生成
        /// </summary>
        public static void Create()
        {
            using (var reader = new XMLReader(FilePath.GetXMLDefaultPath(), new Settings().GetType()))
            {
                _settings = reader.Deserialize() as Settings;
            }
        }

        /// <summary>
        /// SQLiteのパスを取得
        /// </summary>
        /// <returns>Excelテンプレートのパス</returns>
        public static string FetchSQLitePath()
        {
            return _settings?.SQLitePath ?? FilePath.GetSQLiteDefaultPath(); ;
        }

        /// <summary>
        /// Excelテンプレートのパスを取得
        /// </summary>
        /// <returns>Excelテンプレートのパス</returns>
        public static string FetchExcelTemplatePath()
        {
            return _settings?.ExcelTemplatePath ?? FilePath.GetExcelTempleteDefaultPath();
        }

        /// <summary>
        /// フォントファミリを取得
        /// </summary>
        /// <returns>フォントファミリ</returns>
        public static string FetchFontFamilyText()
        {
            return _settings?.FontFamily ?? Shared.FontFamily;
        }

        /// <summary>
        /// フォントファミリを取得
        /// </summary>
        /// <returns>フォントファミリ</returns>
        public static System.Windows.Media.FontFamily FetchFontFamily()
        {
            var fontFamily = _settings?.FontFamily ?? Shared.FontFamily;

            return new System.Windows.Media.FontFamily(fontFamily);
        }

        /// <summary>
        /// 背景色を取得
        /// </summary>
        /// <returns>背景色</returns>
        public static System.Drawing.Color FetchBackgroundColor()
        {
            if (_settings?.BackgroundColor_ColorCode is null)
            {
                return SystemColors.ControlLight;
            }

            return System.Drawing.Color.FromName(_settings.BackgroundColor_ColorCode);
        }

        /// <summary> 背景色 (初期値) </summary>
        private static readonly SolidColorBrush Default = ColorUtils.ToWPFColor("255", "227", "227", "227");

        /// <summary>
        /// 背景色を取得
        /// </summary>
        /// <returns>背景色</returns>
        public static SolidColorBrush FetchBackgroundColorBrush()
        {
            if (_settings?.BackgroundColor_ColorCode is null)
            {
                return Default;
            }

            var color = StringUtils.Separate(_settings.BackgroundColor);
            return ColorUtils.ToWPFColor(color[0], color[1], color[2], color[3]);
        }

        /// <summary>
        /// 「初期表示時にデフォルト明細を表示する」のチェック有無を取得する
        /// </summary>
        /// <returns></returns>
        public static bool FetchShowDefaultPayslip() 
        {
            return _settings?.ShowDefaultPayslip ?? false;
        }
    }

    /// <summary>
    /// 設定
    /// </summary>
    public class Settings
    {
        /// <summary> Excelテンプレートのパス </summary>
        public string ExcelTemplatePath;

        /// <summary> SQLiteのパス </summary>
        public string SQLitePath;

        /// <summary> フォントファミリ </summary>
        public string FontFamily;

        /// <summary> 初期表示時にデフォルト明細を表示する </summary>
        public bool ShowDefaultPayslip;

        /// <summary> 背景色 (カラーコード) </summary>
        public string BackgroundColor_ColorCode;
        
        /// <summary> 背景色 (ARGB) </summary>
        public string BackgroundColor;
    }
}
