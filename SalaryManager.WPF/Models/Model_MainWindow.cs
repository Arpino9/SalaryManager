﻿using System.Transactions;
using SalaryManager.Infrastructure.SQLite;
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

        /// <summary> Model - ヘッダー </summary>
        public Model_Header Header { get; set; }

        /// <summary> Model - 支給額 </summary>
        public Model_Allowance Allowance { get; set; }

        /// <summary> Model - 控除額 </summary>
        public Model_Deduction Deduction { get; set; }

        /// <summary> Model - 勤務備考 </summary>
        public Model_WorkingReference WorkingReference { get; set; }

        /// <summary> Model - 副業 </summary>
        public Model_SideBusiness SideBusiness { get; set; }

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

        /// <summary>
        /// 登録
        /// </summary>
        /// <remarks>
        /// 入力された勤怠情報をDB登録する。
        /// </remarks>
        public void Register()
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
