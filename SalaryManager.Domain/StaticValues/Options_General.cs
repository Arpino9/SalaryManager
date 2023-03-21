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
            if (_settings?.BackgroundColor is null)
            {
                return SystemColors.ControlLight;
            }

            return System.Drawing.Color.FromName(_settings.BackgroundColor);
        }

        private static readonly SolidColorBrush Default = ColorUtil.ToWPFColor("255", "227", "227", "227");

        /// <summary>
        /// 背景色を取得
        /// </summary>
        /// <returns>背景色</returns>
        public static SolidColorBrush FetchBackgroundColorBrush()
        {
            if (_settings?.BackgroundColor is null)
            {
                return Default;
            }

            return ColorUtil.ToWPFColor(_settings.BackgroundColor_A, _settings.BackgroundColor_R, _settings.BackgroundColor_G, _settings.BackgroundColor_B);
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
        public string BackgroundColor;
        /// <summary> 背景色 (アルファ) </summary>
        public string BackgroundColor_A;
        /// <summary> 背景色 (赤) </summary>
        public string BackgroundColor_R;
        /// <summary> 背景色 (緑) </summary>
        public string BackgroundColor_G;
        /// <summary> 背景色 (黒) </summary>
        public string BackgroundColor_B;
    }
}
