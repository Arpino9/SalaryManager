using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.ViewModels;
using System.Windows.Media;

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
        internal ViewModel_MainWindow MainWindow { get; set; }

        /// <summary> Model - ヘッダー </summary>
        internal Model_Header Header { get; set; }

        /// <summary> Model - 支給額 </summary>
        internal Model_Allowance Allowance { get; set; }

        /// <summary> Model - 控除額 </summary>
        internal Model_Deduction Deduction { get; set; }

        /// <summary> Model - 勤務備考 </summary>
        internal Model_WorkingReference WorkingReference { get; set; }

        /// <summary> Model - 副業 </summary>
        internal Model_SideBusiness SideBusiness { get; set; }

        /// <summary>
        /// 金額比較
        /// </summary>
        /// <param name="thisYearPrice">今年の金額</param>
        /// <param name="lastYearPrice">去年の金額</param>
        internal void ComparePrice(double thisYearPrice, double lastYearPrice)
        {
            var difference = thisYearPrice - lastYearPrice;

            if (thisYearPrice == 0 || 
                difference == 0)
            {
                // 変更なし
                this.MainWindow.PriceUpdown_Content = string.Empty;
                return;
            }

            if (difference > 0) 
            {
                this.MainWindow.PriceUpdown_Foreground = new SolidColorBrush(Colors.Blue);
                this.MainWindow.PriceUpdown_Content    = $"+{difference.ToString()}";
            }
            else
            {
                this.MainWindow.PriceUpdown_Foreground = new SolidColorBrush(Colors.Red);
                this.MainWindow.PriceUpdown_Content    = difference.ToString();
            }
        }

        /// <summary>
        /// 登録
        /// </summary>
        /// <remarks>
        /// 入力された勤怠情報をDB登録する。
        /// </remarks>
        internal void Register()
        {
            using (var transaction = new SQLiteTransaction())
            {
                // ヘッダー
                this.Header.Register(transaction);
                // 支給額
                this.Allowance.Register(transaction);
                // 控除額
                this.Deduction.Register(transaction);
                // 勤務備考
                this.WorkingReference.Register(transaction);
                // 副業
                this.SideBusiness.Register(transaction);

                transaction.Commit();
            }
        }
    }
}
