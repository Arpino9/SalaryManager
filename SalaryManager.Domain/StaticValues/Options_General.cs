using SalaryManager.Domain.Modules.Logics;

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
            if (_settings?.FontFamily is null)
            {
                return new System.Windows.Media.FontFamily(Shared.FontFamily);
            }

            return new System.Windows.Media.FontFamily(_settings.FontFamily);
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
    }
}
