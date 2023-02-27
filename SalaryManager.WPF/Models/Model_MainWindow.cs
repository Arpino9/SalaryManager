using SalaryManager.Domain;
using SalaryManager.Domain.Logics;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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
        /// CSV読み込み
        /// </summary>
        internal void ReadCSV()
        {
            var confirmingMessage = $"{this.Header.ViewModel.Year}年{this.Header.ViewModel.Month}月のCSVを読み込みますか？";
            if (!DialogMessageUtils.ShowConfirmingMessage(confirmingMessage, this.MainWindow.Title))
            {
                // キャンセル
                return;
            }

            var encode = System.Text.Encoding.GetEncoding("shift_jis");

            var path = $"{Shared.DirectoryCSV}\\{Shared.EmployeeID}-{this.Header.ViewModel.Year}-{this.Header.ViewModel.Month}.csv";
            
            try
            {
                var reader = new StreamReader(path, encode);
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(',');

                    List<string> lists = new List<string>();
                    lists.AddRange(values);

                    // 勤務先
                    this.WorkingReference.WorkPlace.WorkPlace = values[3];

                    // 有給残日数
                    var paidVacation = Convert.ToDouble(values[17]) + Convert.ToDouble(values[25]);
                    this.WorkingReference.ViewModel.PaidVacation = paidVacation;
                }
            }
            catch(FileNotFoundException _)
            {
                var message = $"「{Shared.DirectoryCSV}」に{this.Header.ViewModel.Year}年{this.Header.ViewModel.Month}月分のCSVが\n保存されていません。読み込みを中断します。";
                DialogMessageUtils.ShowResultMessage(message, this.MainWindow.Title);
            }
        }

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
        /// 保存
        /// </summary>
        /// <remarks>
        /// 入力された勤怠情報をDB登録する。
        /// </remarks>
        internal void Save()
        {
            var message = $"{this.Header.ViewModel.Year}年{this.Header.ViewModel.Month}月の給与明細を保存しますか？";
            if (!DialogMessageUtils.ShowConfirmingMessage(message, this.MainWindow.Title))
            {
                // キャンセル
                return;
            }

            using (var transaction = new SQLiteTransaction())
            {
                // ヘッダー
                this.Header.Save(transaction);
                // 支給額
                this.Allowance.Save(transaction);
                // 控除額
                this.Deduction.Save(transaction);
                // 勤務備考
                this.WorkingReference.Save(transaction);
                // 副業
                this.SideBusiness.Save(transaction);

                transaction.Commit();
            }
        }

        /// <summary>
        /// デフォルト明細を取得する
        /// </summary>
        internal void GetDefault()
        {
            var header = new HeaderSQLite();
            var defaultEntity = header.GetDefaultEntity();

            if (defaultEntity == null)
            {
                DialogMessageUtils.ShowResultMessage("デフォルト明細が登録されていません。", this.MainWindow.Title);
                return;
            }

            // 支給額
            this.Allowance.Initialize(defaultEntity.YearMonth);
            // 控除額
            this.Deduction.Initialize(defaultEntity.YearMonth);
            // 勤務備考
            this.WorkingReference.Initialize(defaultEntity.YearMonth);
            // 副業
            this.SideBusiness.Initialize(defaultEntity.YearMonth);
        }
    }
}
