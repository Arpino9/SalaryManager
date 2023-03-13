using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using SalaryManager.Domain;
using SalaryManager.Domain.Modules.Logics;
using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.Domain.StaticValues;
using SalaryManager.Domain.ValueObjects;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.ViewModels;
using SalaryManager.WPF.Window;
using WorkingReferences = SalaryManager.Domain.StaticValues.WorkingReferences;

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

        /// <summary> ViewModel - メイdン画面 </summary>
        internal ViewModel_MainWindow MainWindow { get; set; }

        /// <summary> ViewModel - 勤務先 </summary>
        internal ViewModel_WorkPlace WorkPlace { get; set; }

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
            var confirmingMessage = $"{this.Header.ViewModel.Year_Value}年{this.Header.ViewModel.Month_Value}月のCSVを読み込みますか？";
            if (!DialogMessage.ShowConfirmingMessage(confirmingMessage, this.MainWindow.Title))
            {
                // キャンセル
                return;
            }

            var employeeID = Careers.FetchEmployeeNumber(new CompanyValue(this.WorkPlace.CompanyName));

            if (string.IsNullOrEmpty(employeeID)) 
            {
                // 社員番号が登録されていない
                return;
            }

            var encode = System.Text.Encoding.GetEncoding("shift_jis");
;
            var path = $"{Shared.DirectoryCSV}\\{employeeID}-{this.Header.ViewModel.Year_Value}-{this.Header.ViewModel.Month_Value}.csv";
            
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
                    this.WorkingReference.ViewModel.PaidVacation_Value = paidVacation;
                }
            }
            catch(FileNotFoundException _)
            {
                var message = $"「{Shared.DirectoryCSV}」に{this.Header.ViewModel.Year_Value}年{this.Header.ViewModel.Month_Value}月分のCSVが\n保存されていません。読み込みを中断します。";
                DialogMessage.ShowResultMessage(message, this.MainWindow.Title);
            }
        }

        #endregion

        /// <summary>
        /// 金額比較
        /// </summary>
        /// <param name="thisYearPrice">今年の金額</param>
        /// <param name="lastYearPrice">去年の金額</param>
        /// <remarks>
        /// 引数は登録されていないとnullになる。
        /// </remarks>
        internal void ComparePrice(double? thisYearPrice, double? lastYearPrice)
        {
            if (thisYearPrice is null ||
                lastYearPrice is null) 
            { 
                return; 
            }

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
            var message = $"{this.Header.ViewModel.Year_Value}年{this.Header.ViewModel.Month_Value}月の給与明細を保存しますか？";
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
        internal void FetchDefault()
        {
            Headers.Create(new HeaderSQLite());

            if (Headers.FetchDefault() == null)
            {
                DialogMessage.ShowResultMessage("デフォルト明細が登録されていません。", this.MainWindow.Title);
                return;
            }

            // 支給額
            this.Allowance.ViewModel.Entity = Allowances.FetchDefault();
            this.Allowance.Refresh();

            // 控除額
            this.Deduction.ViewModel.Entity = Deductions.FetchDefault();
            this.Deduction.Refresh();

            // 勤務備考
            this.WorkingReference.ViewModel.Entity = WorkingReferences.FetchDefault();
            this.WorkingReference.Refresh();

            // 副業
            this.SideBusiness.ViewModel.Entity = SideBusinesses.FetchDefault();
            this.SideBusiness.Refresh();
        }

        #endregion

        #region 今月の明細を表示

        /// <summary>
        /// 今月の明細を表示
        /// </summary>
        internal void ShowCurrentPayslip()
        {
            // ヘッダ
            this.Header.ViewModel.Year_Value  = DateTime.Today.Year;
            this.Header.ViewModel.Month_Value = DateTime.Today.Month;
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
            var workbook = new Excel();

            using (var cursor = new CursorWaiting())
            {
                // Create Records
                Headers.Create(new HeaderSQLite());
                Allowances.Create(new AllowanceSQLite());
                Deductions.Create(new DeductionSQLite());
                WorkingReferences.Create(new WorkingReferenceSQLite());
                SideBusinesses.Create(new SideBusinessSQLite());

                // Write Records
                await System.Threading.Tasks.Task.WhenAll(
                    workbook.WriteAllHeader(Headers.FetchByDescending()),
                    workbook.WriteAllAllowance(Allowances.FetchByDescending()),
                    workbook.WriteAllDeduction(Deductions.FetchByDescending()),
                    workbook.WriteAllWorkingReferences(WorkingReferences.FetchByDescending()),
                    workbook.WriteAllSideBusiness(SideBusinesses.FetchByDescending()),
                    workbook.SetStyle()
                );
            }

            var directory = DirectorySelector.Select("Excel出力先のフォルダを選択してください。");

            workbook.CopyAsWorkbook(directory);
        }

        #endregion

        #region 経歴入力

        /// <summary>
        /// 経歴管理画面を開く
        /// </summary>
        internal void ShowCareerManager()
        {
            var career = new Career();
            career.Show();
        }

        #endregion

        #region オプション

        /// <summary>
        /// オプション画面を開く
        /// </summary>
        internal void ShowOption()
        {
            var career = new Option();
            career.Show();
        }

        #endregion

    }
}
