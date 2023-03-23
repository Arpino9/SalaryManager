using System.Windows.Media;

namespace SalaryManager.Domain.Modules.Helpers
{
    public static  class ColorUtils
    {
        //TODO: メソッド名は要検討、
        //MEMO: Labelにバインディングするため、値オブジェクトは不採用
        /// <summary>
        /// WPFの前景色、背景色に変換
        /// </summary>
        /// <param name="color"></param>
        /// <returns>色定義</returns>
        public static SolidColorBrush ToWPFColor(System.Drawing.Color color)
        {
            var brush = new SolidColorBrush();
            brush.Color = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);

            return brush;
        }

        public static SolidColorBrush ToWPFColor(byte colorAlpha, byte colorRed, byte colorGreen, byte colorBlue)
        {
            var brush = new SolidColorBrush();

            brush.Color = System.Windows.Media.Color.FromArgb(colorAlpha, colorRed, colorGreen, colorGreen);

            return brush;
        }

        public static SolidColorBrush ToWPFColor(string colorAlpha, string colorRed, string colorGreen, string colorBlue)
        {
            return ToWPFColor(byte.Parse(colorAlpha), byte.Parse(colorRed), byte.Parse(colorGreen), byte.Parse(colorBlue));
        }
    }
}
