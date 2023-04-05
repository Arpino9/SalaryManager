using System.Drawing;
using System.Windows.Media;
using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.Domain.Modules.Logics;
using SalaryManager.Domain;
using DocumentFormat.OpenXml.Bibliography;

namespace SalaryManager.Infrastructure.XML
{
    /// <summary>
    /// XMLローダー
    /// </summary>
    /// <remarks>
    /// staticValuesと挙動は同じだが、using絡みで他XMLクラスに依存するため分離。
    /// 呼び出しが面倒(コンストラクタ部分にあたるDeserialize()で逐一usingする必要がある)なので、
    /// あえてインターフェースを介さないことにした。
    /// </remarks>
    public static class XMLLoader
    {
        static XMLLoader()
        {
            XMLLoader.Deserialize();
        } 

        private static XMLTag _tag;

        /// <summary>
        /// デシリアライズ
        /// </summary>
        public static void Deserialize()
        {
            using (var reader = new XMLReader(FilePath.GetXMLDefaultPath(), new XMLTag().GetType()))
            {
                _tag = reader.Deserialize() as XMLTag;
            }
        }

        /// <summary>
        /// SQLiteのパスを取得
        /// </summary>
        /// <returns>Excelテンプレートのパス</returns>
        public static string FetchSQLitePath()
        {
            return _tag?.SQLitePath ?? FilePath.GetSQLiteDefaultPath(); ;
        }

        /// <summary>
        /// Excelテンプレートのパスを取得
        /// </summary>
        /// <returns>Excelテンプレートのパス</returns>
        public static string FetchExcelTemplatePath()
        {
            return _tag?.ExcelTemplatePath ?? FilePath.GetExcelTempleteDefaultPath();
        }

        /// <summary>
        /// フォントファミリを取得
        /// </summary>
        /// <returns>フォントファミリ</returns>
        public static string FetchFontFamilyText()
        {
            return _tag?.FontFamily ?? Shared.FontFamily;
        }

        /// <summary>
        /// フォントファミリを取得
        /// </summary>
        /// <returns>フォントファミリ</returns>
        public static System.Windows.Media.FontFamily FetchFontFamily()
        {
            var fontFamily = _tag?.FontFamily ?? Shared.FontFamily;

            return new System.Windows.Media.FontFamily(fontFamily);
        }

        /// <summary>
        /// 背景色を取得
        /// </summary>
        /// <returns>背景色</returns>
        public static System.Drawing.Color FetchBackgroundColor()
        {
            if (_tag?.BackgroundColor_ColorCode is null)
            {
                return SystemColors.ControlLight;
            }

            var color = StringUtils.Separate(_tag.BackgroundColor);

            if (color.Count < 2)
            {
                return System.Drawing.Color.FromArgb(int.Parse(color[0]), 0, 0, 0);
            }
            else if (color.Count < 3)
            {
                return System.Drawing.Color.FromArgb(int.Parse(color[0]), int.Parse(color[1]), 0, 0);
            }
            else if (color.Count < 4)
            {
                return System.Drawing.Color.FromArgb(int.Parse(color[0]), int.Parse(color[1]), int.Parse(color[2]), 0);
            }

            return System.Drawing.Color.FromArgb(int.Parse(color[0]), int.Parse(color[1]), int.Parse(color[2]), int.Parse(color[3]));
        }

        /// <summary>
        /// フォントサイズを取得
        /// </summary>
        /// <returns>フォントサイズ</returns>
        public static decimal FetchFontSize()
        {
            return _tag?.FontSize ?? decimal.Parse(Shared.FontSize);
        }

        /// <summary> 背景色 (初期値) </summary>
        private static readonly SolidColorBrush Default = ColorUtils.ToWPFColor("255", "227", "227", "227");

        /// <summary>
        /// 背景色を取得
        /// </summary>
        /// <returns>背景色</returns>
        public static SolidColorBrush FetchBackgroundColorBrush()
        {
            if (_tag?.BackgroundColor_ColorCode is null)
            {
                return Default;
            }

            var color = StringUtils.Separate(_tag.BackgroundColor);

            if (color.Count < 2)
            {
                return ColorUtils.ToWPFColor(color[0], "0", "0", "0");
            }
            else if (color.Count < 3)
            {
                return ColorUtils.ToWPFColor(color[0], color[1], "0", "0");
            }
            else if (color.Count < 4)
            {
                return ColorUtils.ToWPFColor(color[0], color[1], color[2], "0");
            }

            return ColorUtils.ToWPFColor(color[0], color[1], color[2], color[3]);
        }

        /// <summary>
        /// 「初期表示時にデフォルト明細を表示する」のチェック有無を取得する
        /// </summary>
        /// <returns></returns>
        public static bool FetchShowDefaultPayslip()
        {
            return _tag?.ShowDefaultPayslip ?? bool.Parse(Shared.ShowDefaultPayslip);
        }

        /// <summary>
        /// 画像の保存方法を取得する
        /// </summary>
        /// <returns></returns>
        public static string FetchHowToSaveImage() 
        {
            return _tag?.HowToSaveImage;
        }
    }
}
