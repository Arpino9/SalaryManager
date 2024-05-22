using TEST;
using Message = SalaryManager.Domain.Modules.Logics.Message;

namespace SalaryManager.WPF.Models;

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
    internal async void Initialize()
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
        this.ViewModel.Window_FontFamily.Value = XMLLoader.FetchFontFamily();
        this.ViewModel.Window_FontSize.Value   = XMLLoader.FetchFontSize();
        this.ViewModel.Window_Background.Value = XMLLoader.FetchBackgroundColorBrush();
    }

    #endregion

    #region メニュー - 編集

    /// <summary>
    /// 会社マスタを開く
    /// </summary>
    internal void EditCompany()
    {
        var company = new Company();
        company.Show();
    }

    /// <summary>
    /// 経歴マスタを開く
    /// </summary>
    internal void EditCareer()
    {
        var career = new Career();
        career.Show();
    }

    /// <summary>
    /// 就業時間マスタを開く
    /// </summary>
    internal void EditWorkingPlace()
    {
        var workingPlace = new Window.WorkingPlace();
        workingPlace.Show();
    }

    /// <summary>
    /// 在宅マスタを開く
    /// </summary>
    internal void EditHome()
    {
        var home = new Home();
        home.Show();
    }

    /// <summary>
    /// 祝日マスタを開く
    /// </summary>
    internal void EditHoliday()
    {
        var holiday = new Holiday();
        holiday.Show();
    }

    /// <summary>
    /// 添付ファイル管理画面を開く
    /// </summary>
    internal void EditFileSotrage()
    {
        var storage = new FileStorage();
        storage.Show();
    }

    /// <summary>
    /// オプション画面を開く
    /// </summary>
    internal void EditOption()
    {
        var career = new Option();
        career.Show();
    }

    #endregion

    #region メニュー - 読込

    /// <summary>
    /// 勤怠表を開く
    /// </summary>
    internal void ReadWorkSchedule()
    {
        var workSchedule = new WorkSchedule();
        workSchedule.Show();
    }

    /// <summary>
    /// デフォルト明細を取得する
    /// </summary>
    internal void ReadDefaultPayslip()
    {
        Headers.Create(new HeaderSQLite());

        if (Headers.FetchDefault() == null)
        {
            Message.ShowResultMessage("デフォルト明細が登録されていません。", this.ViewModel.Window_Title.Value);
            return;
        }

        // 支給額
        this.Allowance.Entity = Allowances.FetchDefault();
        this.Allowance.Refresh();

        // 控除額
        this.Deduction.Entity = Deductions.FetchDefault();
        this.Deduction.Refresh();

        // 勤務備考
        this.WorkingReference.Entity = WorkingReferences.FetchDefault();
        this.WorkingReference.Refresh();

        // 副業
        this.SideBusiness.Entity = SideBusinesses.FetchDefault();
        this.SideBusiness.Refresh();

        // 勤務先、勤務場所
        this.WorkPlace.Model.Entity = WorkingReferences.FetchDefault();
        this.WorkPlace.Model.Refresh();
    }

    /// <summary>
    /// CSV読み込み
    /// </summary>
    /// <remarks>
    /// 暫定的なメソッドのため、あえてInfrastructure層には加えていない。
    /// </remarks>
    internal void ReadCSV()
    {
        var confirmingMessage = $"{this.Header.ViewModel.Year_Text.Value}年{this.Header.ViewModel.Month_Text.Value}月のCSVを読み込みますか？";
        if (!Message.ShowConfirmingMessage(confirmingMessage, this.ViewModel.Window_Title.Value))
        {
            // キャンセル
            return;
        }

        var employeeID = Careers.FetchEmployeeNumber(new CompanyNameValue(this.WorkPlace.CompanyName_Text.Value));

        if (string.IsNullOrEmpty(employeeID))
        {
            // 社員番号が登録されていない
            return;
        }

        var encode = System.Text.Encoding.GetEncoding("shift_jis");
        ;
        var path = $"{Shared.DirectoryCSV}\\{employeeID}-{this.Header.ViewModel.Year_Text.Value}-{this.Header.ViewModel.Month_Text.Value}.csv";

        try
        {
            var reader = new StreamReader(path, encode);
            {
                string line = reader.ReadLine();
                string[] values = line.Split(',');

                List<string> lists = new List<string>();
                lists.AddRange(values);

                // 勤務先
                this.WorkPlace.WorkPlace_Text.Value = values[3];

                // 有給残日数
                var paidVacation = Convert.ToDouble(values[17]) + Convert.ToDouble(values[25]);
                this.WorkingReference.ViewModel.PaidVacation_Text.Value = paidVacation;
            }
        }
        catch (FileNotFoundException)
        {
            var message = $"「{Shared.DirectoryCSV}」に{this.Header.ViewModel.Year_Text.Value}年{this.Header.ViewModel.Month_Text.Value}月分のCSVが\n保存されていません。読み込みを中断します。";
            Message.ShowResultMessage(message, this.ViewModel.Window_Title.Value);
        }
    }

    #endregion

    #region メニュー - 表示

    /// <summary>
    /// 今月の明細を表示
    /// </summary>
    internal void ShowCurrentPayslip()
    {
        // ヘッダ
        this.Header.ViewModel.Year_Text.Value  = DateTime.Today.Year;
        this.Header.ViewModel.Month_Text.Value = DateTime.Today.Month;
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

    #region メニュー - 出力

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

    /// <summary>
    /// スプレッドシート出力
    /// </summary>
    internal void OutputSpreadSheet()
    {
        if (string.IsNullOrEmpty(XMLLoader.FetchPrivateKeyPath_SpreadSheet()))
        {
            Message.ShowErrorMessage("認証ファイルのパスを指定してください。", "エラー");
            return;
        }

        if (string.IsNullOrEmpty(XMLLoader.FetchSheetId()))
        {
            Message.ShowErrorMessage("スプレッドシートのシートIDを指定してください。", "エラー");
            return;
        }

        // Create Records
        Headers.Create(new HeaderSQLite());
        Allowances.Create(new AllowanceSQLite());
        Deductions.Create(new DeductionSQLite());
        WorkingReferences.Create(new WorkingReferenceSQLite());
        SideBusinesses.Create(new SideBusinessSQLite());

        try
        {
            var writer = new SpreadSheetWriter(Headers.FetchByDescending(),
                                               Allowances.FetchByDescending(),
                                               Deductions.FetchByDescending(),
                                               WorkingReferences.FetchByDescending(),
                                               SideBusinesses.FetchByDescending());

            writer.WritePayslips();
        }
        catch (Exception ex)
        {
            throw new FileWriterException("スプレッドシートへの書き込みに失敗しました。", ex);
        }
    }

    #endregion

    #region メニュー - 保存

    /// <summary>
    /// 保存
    /// </summary>
    /// <remarks>
    /// 入力された勤怠情報をDB登録する。
    /// </remarks>
    internal void SavePayslip()
    {
        var message = $"{this.Header.ViewModel.Year_Text.Value}年{this.Header.ViewModel.Month_Text.Value}月の給与明細を保存しますか？";
        if (!Message.ShowConfirmingMessage(message, this.ViewModel.Window_Title.Value))
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


    /// <summary>
    /// DBバックアップ
    /// </summary>
    internal void SaveDBBackup()
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

    #region 金額比較

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
            this.ViewModel.PriceUpdown_Content.Value = string.Empty;
            return;
        }

        if (difference > 0)
        {
            this.ViewModel.PriceUpdown_Foreground.Value = new SolidColorBrush(Colors.Blue);
            this.ViewModel.PriceUpdown_Content.Value = $"+{difference.ToString()}";
        }
        else
        {
            this.ViewModel.PriceUpdown_Foreground.Value = new SolidColorBrush(Colors.Red);
            this.ViewModel.PriceUpdown_Content.Value = difference.ToString();
        }
    }

    #endregion

}
