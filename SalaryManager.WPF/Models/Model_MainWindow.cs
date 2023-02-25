using SalaryManager.WPF.ViewModels;

namespace SalaryManager.WPF.Models
{
    /// <summary>
    /// Model - メイン画面
    /// </summary>
    public class Model_MainWindow
    {

        #region Get Instance

        private static Model_MainWindow model = null;

        public static Model_MainWindow GetInstance()
        {
            if (model == null)
            {
                model = new Model_MainWindow();
            }

            return model;
        }

        #endregion

        /// <summary> ViewModel - メイン画面 </summary>
        public ViewModel_MainWindow MainWindow { get; set; }

        /// <summary>
        /// 金額比較
        /// </summary>
        /// <param name="thisYearPrice">今年の金額</param>
        /// <param name="lastYearPrice">去年の金額</param>
        public void ComparePrice(double? thisYearPrice, double? lastYearPrice)
        {
            if (thisYearPrice is null ||
                lastYearPrice is null)
            {
                // 未登録
                this.MainWindow.PriceUpdown_Content = string.Empty;
                return;
            }

            if (thisYearPrice.Value == 0)
            {
                this.MainWindow.PriceUpdown_Content = string.Empty;
                return;
            }

            var price = thisYearPrice.Value - lastYearPrice.Value;
            this.MainWindow.PriceUpdown_Content = price.ToString();
        }
    }
}
