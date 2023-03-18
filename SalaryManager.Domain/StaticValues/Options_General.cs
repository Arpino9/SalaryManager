using SalaryManager.Domain.Modules.Logics;

namespace SalaryManager.Domain.StaticValues
{
    public sealed class Options_General
    {
        private static Settings _settings;

        public static void Create()
        {
            using (var reader = new XMLReader(Shared.XMLPath, new Settings().GetType()))
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
            return _settings?.ExcelTemplatePath ?? Shared.PathOutputPayslip;
        }

        /// <summary>
        /// フォントファミリを取得
        /// </summary>
        /// <returns>フォントファミリ</returns>
        public static string FetchFontFamily()
        {
            return _settings?.FontFamily ?? "Yu Gothic UI";
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
    }
}
