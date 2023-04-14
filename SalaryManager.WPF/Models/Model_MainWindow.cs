using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using SalaryManager.Domain;
using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.Domain.StaticValues;
using SalaryManager.Domain.ValueObjects;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.ViewModels;
using SalaryManager.WPF.Window;
using WorkingReferences = SalaryManager.Domain.StaticValues.WorkingReferences;
using SalaryManager.Domain.Exceptions;
using SalaryManager.Infrastructure.XML;
using SalaryManager.Domain.Modules.Logics;
using SalaryManager.Infrastructure.Excel;

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

        public Model_MainWindow()
        {

        }

        /// <summary> Repository - Excel書き込み </summary>
        private ExcelWriter ExcelWriter = new ExcelWriter();

        /// <summary> ViewModel - メイン画面 </summary>
        internal ViewModel_MainWindow ViewModel { get; set; }

        /// <summary> ViewModel - 勤務先 </summary>
        internal ViewModel_WorkPlace WorkPlace { get; set; }

        /// <summary> ViewModel - 月収一覧 </summary>
        internal ViewModel_AnnualChart AnnualChart { get; set; }

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

        #region 初期化

        /// <summary>
        /// 初期化
        /// </summary>
        internal void Initialize()
        {
            this.InitializeSQLite();
            this.Window_Activated();
        }

        /// <summary>
        /// SQLiteの設定ファイル初期化
        /// </summary>
        private void InitializeSQLite()
        {
            var dllName = "SQLite.Interop.dll";

            var sqlite64Directory = $"{FilePath.GetAppFolderPath()}\\x64";

            if (!Directory.Exists(sqlite64Directory))
            {
                Directory.CreateDirectory(sqlite64Directory);

                var sqlite = $"{FilePath.GetSolutionPath()}\\SQLite\\x64\\{dllName}";
                File.Copy(sqlite, $"{sqlite64Directory}\\{dllName}");
            }

            var sqlite86Directory = $"{FilePath.GetAppFolderPath()}\\x86";

            if (!Directory.Exists(sqlite86Directory))
            {
                Directory.CreateDirectory(sqlite86Directory);

                var sqlite = $"{FilePath.GetSolutionPath()}\\SQLite\\x86\\{dllName}";
                File.Copy(sqlite, $"{sqlite86Directory}\\{dllName}");
            }
        }

        /// <summary>
        /// 画面起動時の処理
        /// </summary>
        internal void Window_Activated()
        {
            this.ViewModel.FontFamily        = XMLLoader.FetchFontFamily();
            this.ViewModel.FontSize          = XMLLoader.FetchFontSize();
            this.ViewModel.Window_Background = XMLLoader.FetchBackgroundColorBrush();
        }

        #endregion

        #region CSV読込

        /// <summary>
        /// CSV読み込み
        /// </summary>
        /// <remarks>
        /// 暫定的なメソッドのため、あえてInfrastructure層には加えていない。
        /// </remarks>
        internal void ReadCSV()
        {
            var confirmingMessage = $"{this.Header.ViewModel.Year_Value}年{this.Header.ViewModel.Month_Value}月のCSVを読み込みますか？";
            if (!Message.ShowConfirmingMessage(confirmingMessage, this.ViewModel.Window_Title))
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
                    this.WorkPlace.WorkPlace = values[3];

                    // 有給残日数
                    var paidVacation = Convert.ToDouble(values[17]) + Convert.ToDouble(values[25]);
                    this.WorkingReference.ViewModel.PaidVacation_Value = paidVacation;
                }
            }
            catch(FileNotFoundException _)
            {
                var message = $"「{Shared.DirectoryCSV}」に{this.Header.ViewModel.Year_Value}年{this.Header.ViewModel.Month_Value}月分のCSVが\n保存されていません。読み込みを中断します。";
                Message.ShowResultMessage(message, this.ViewModel.Window_Title);
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
                this.ViewModel.PriceUpdown_Content = string.Empty;
                return;
            }

            if (difference > 0) 
            {
                this.ViewModel.PriceUpdown_Foreground = new SolidColorBrush(Colors.Blue);
                this.ViewModel.PriceUpdown_Content    = $"+{difference.ToString()}";
            }
            else
            {
                this.ViewModel.PriceUpdown_Foreground = new SolidColorBrush(Colors.Red);
                this.ViewModel.PriceUpdown_Content    = difference.ToString();
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
            if (!Message.ShowConfirmingMessage(message, this.ViewModel.Window_Title))
            {
                // キャンセル
                return;
            }

            if (this.WorkingReference.EditValidationCheck() == false)
            {
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

        #region DBバックアップ

        /// <summary>
        /// DBバックアップ
        /// </summary>
        internal void CreateBackup()
        {
            var filter = "Databaseファイル(*.db)|*.db|すべてのファイル(*.*)|*.*";

            var directory = DialogUtils.SelectWithName("SalaryManager.db", filter);

            if (string.IsNullOrEmpty(directory))
            {
                return;
            }

            File.Copy(XMLLoader.FetchSQLitePath(), directory);
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
                Message.ShowResultMessage("デフォルト明細が登録されていません。", this.ViewModel.Window_Title);
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
            using (var cursor = new CursorWaiting())
            {
                // Create Records
                Headers.Create(new HeaderSQLite());
                Allowances.Create(new AllowanceSQLite());
                Deductions.Create(new DeductionSQLite());
                WorkingReferences.Create(new WorkingReferenceSQLite());
                SideBusinesses.Create(new SideBusinessSQLite());

                // Write Records
                try
                {
                    await System.Threading.Tasks.Task.WhenAll(
                        this.ExcelWriter.WriteAllHeader(Headers.FetchByDescending()),
                        this.ExcelWriter.WriteAllAllowance(Allowances.FetchByDescending()),
                        this.ExcelWriter.WriteAllDeduction(Deductions.FetchByDescending()),
                        this.ExcelWriter.WriteAllWorkingReferences(WorkingReferences.FetchByDescending()),
                        this.ExcelWriter.WriteAllSideBusiness(SideBusinesses.FetchByDescending()),
                        this.ExcelWriter.SetStyle()
                    );
                }
                catch (Exception ex) 
                {
                    throw new FileWriterException("Excelへの書き込みに失敗しました。", ex);
                }
            }

            var directory = DialogUtils.SelectDirectory("Excel出力先のフォルダを選択してください。");

            this.ExcelWriter.CopyAsWorkbook(directory);
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

        #region 添付ファイル管理

        /// <summary>
        /// 添付ファイル管理画面を開く
        /// </summary>
        internal void ShowFileSotrage()
        {
            var storage = new FileStorage();
            storage.Show();
        }

        #endregion

    }
}
