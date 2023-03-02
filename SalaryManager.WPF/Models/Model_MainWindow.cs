using SalaryManager.Domain;
using SalaryManager.Domain.Helpers;
using SalaryManager.Domain.Logics;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
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

        #region CSV読込


        /// <summary>
        /// CSV読み込み
        /// </summary>
        internal void ReadCSV()
        {
            var confirmingMessage = $"{this.Header.ViewModel.Year}年{this.Header.ViewModel.Month}月のCSVを読み込みますか？";
            if (!DialogMessage.ShowConfirmingMessage(confirmingMessage, this.MainWindow.Title))
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
                DialogMessage.ShowResultMessage(message, this.MainWindow.Title);
            }
        }

        #endregion

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

        #region 保存

        /// <summary>
        /// 保存
        /// </summary>
        /// <remarks>
        /// 入力された勤怠情報をDB登録する。
        /// </remarks>
        internal void Save()
        {
            var message = $"{this.Header.ViewModel.Year}年{this.Header.ViewModel.Month}月の給与明細を保存しますか？";
            if (!DialogMessage.ShowConfirmingMessage(message, this.MainWindow.Title))
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

        #endregion

        #region デフォルトから取得

        /// <summary>
        /// デフォルト明細を取得する
        /// </summary>
        internal void GetDefault()
        {
            var header = new HeaderSQLite();
            var defaultEntity = header.GetDefaultEntity();

            if (defaultEntity == null)
            {
                DialogMessage.ShowResultMessage("デフォルト明細が登録されていません。", this.MainWindow.Title);
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

        #endregion

        #region 今月の明細を表示

        /// <summary>
        /// 今月の明細を表示
        /// </summary>
        internal void ShowCurrentPayslip()
        {
            // ヘッダ
            this.Header.ViewModel.Year  = DateTime.Today.Year;
            this.Header.ViewModel.Month = DateTime.Today.Month;
            // 支給額
            this.Allowance.Initialize(DateTime.Today);
            // 控除額
            this.Deduction.Initialize(DateTime.Today);
            // 勤務備考
            this.WorkingReference.Initialize(DateTime.Today);
            // 副業
            this.SideBusiness.Initialize(DateTime.Today);
        }

        #endregion

        #region Excel出力

        /// <summary>
        /// Excel出力
        /// </summary>
        internal async void OutputExcel()
        {
            /*// 元のカーソルを保持
            Cursor preCursor = Cursor.Current;

            // カーソルを待機カーソルに変更
            Cursor.Current = Cursors.WaitCursor;*/


            var workbook = new Excel();

            // ヘッダ
            var sqlite_Header = new HeaderSQLite();
            var header = sqlite_Header.GetEntities().OrderBy(x => x.YearMonth).ToList();

            // 支給額
            var sqlite_allowance = new AllowanceSQLite();
            var allowance = sqlite_allowance.GetEntities().OrderBy(x => x.YearMonth).ToList();

            // 控除額
            var sqlite_deduction = new DeductionSQLite();
            var deduction = sqlite_deduction.GetEntities().OrderBy(x => x.YearMonth).ToList();

            // 勤務備考
            var sqlite_workingReferences = new WorkingReferenceSQLite();
            var workingReference = sqlite_workingReferences.GetEntities().OrderBy(x => x.YearMonth).ToList();

            // 副業
            var sqlite_SideBusiness = new SideBusinessSQLite();
            var sideBusiness = sqlite_SideBusiness.GetEntities().OrderBy(x => x.YearMonth).ToList();

            // Write
            await System.Threading.Tasks.Task.WhenAll(
                workbook.WriteAllHeader(header, 5),
                workbook.WriteAllAllowance(allowance, 5),
                workbook.WriteAllDeduction(deduction, 5),
                workbook.WriteAllWorkingReferences(workingReference, 5),
                workbook.WriteAllSideBusiness(sideBusiness, 5)
            );

            var selector = new DirectorySelector();
            var directory = selector.Select("Excel出力先のフォルダを選択してください。");

            workbook.CopyAsWorkbook(directory);
        }

        #endregion

    }
}
