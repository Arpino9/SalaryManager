using Message = SalaryManager.Domain.Modules.Logics.Message;

namespace SalaryManager.Prism.Models;

/// <summary>
/// Model - メイン画面
/// </summary>
public sealed class MainWindowModel : ModelBase<MainWindowViewModel>
{

    #region Get Instance

    private static MainWindowModel model = null;

    public static MainWindowModel GetInstance()
    {
        if (model == null)
        {
            model = new MainWindowModel();
        }

        return model;
    }

    #endregion

    public MainWindowModel()
    {

    }

    /// <summary> ViewModel - メイン画面 </summary>
    internal override MainWindowViewModel ViewModel { get; set; }

    /// <summary> Repository - Excel書き込み </summary>
    private ExcelWriter ExcelWriter = new ExcelWriter();

    /// <summary> ViewModel - 勤務先 </summary>
    internal WorkPlaceViewModel WorkPlace { get; set; }

    /// <summary> ViewModel - 月収一覧 </summary>
    internal AnnualChartViewModel AnnualChart { get; set; }

    /// <summary> Model - ヘッダー </summary>
    internal HeaderModel Header { get; set; }

    /// <summary> Model - 支給額 </summary>
    public AllowanceModel Allowance { get; set; }

    /// <summary> Model - 控除額 </summary>
    internal DeductionModel Deduction { get; set; }

    /// <summary> Model - 勤務備考 </summary>
    internal WorkingReferenceModel WorkingReference { get; set; }

    /// <summary> Model - 副業 </summary>
    internal SideBusinessModel SideBusiness { get; set; }

    /// <summary> Model - ヘッダー </summary>
    private WorkPlaceModel Model_WorkPlace { get; set; } = WorkPlaceModel.GetInstance();

    #region 初期化

    /// <summary>
    /// 初期化
    /// </summary>
    internal async void Initialize()
    {
        Task.Run(() => Invoker.ExecuteCommands());
        this.InitializeSQLite();
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

    #endregion

    #region メニュー - 読込

    /// <summary>
    /// デフォルト明細を取得する
    /// </summary>
    internal void ReadDefaultPayslip()
    {
        Headers.Create(new HeaderSQLite());

        if (Headers.FetchDefault() == null)
        {
            Message.ShowResultMessage("デフォルト明細が登録されていません。", this.ViewModel.Window_Title);
            return;
        }

        // 支給額
        this.Allowance.Entity = Allowances.FetchDefault();
        this.Allowance.Reload_InputForm();

        // 控除額
        this.Deduction.Entity = Deductions.FetchDefault();
        this.Deduction.Reload_InputForm();

        // 勤務備考
        this.WorkingReference.Entity = WorkingReferences.FetchDefault();
        this.WorkingReference.Reload_InputForm();

        // 副業
        this.SideBusiness.Entity = SideBusinesses.FetchDefault();
        this.SideBusiness.Reload_InputForm();

        // 勤務先、勤務場所
        this.Model_WorkPlace.Entity = WorkingReferences.FetchDefault();
        this.Model_WorkPlace.Reload_InputForm();
    }

    /// <summary>
    /// CSV読み込み
    /// </summary>
    /// <remarks>
    /// 暫定的なメソッドのため、あえてInfrastructure層には加えていない。
    /// </remarks>
    internal void ReadCSV()
    {
        var confirmingMessage = $"{this.Header.ViewModel.Year_Text}年{this.Header.ViewModel.Month_Text}月のCSVを読み込みますか？";
        if (!Message.ShowConfirmingMessage(confirmingMessage, this.ViewModel.Window_Title))
        {
            // キャンセル
            return;
        }

        var employeeID = Careers.FetchEmployeeNumber(new CompanyNameValue(this.WorkPlace.CompanyName_Text));

        if (string.IsNullOrEmpty(employeeID))
        {
            // 社員番号が登録されていない
            return;
        }

        var encode = System.Text.Encoding.GetEncoding("shift_jis");
        ;
        var path = $"{Shared.DirectoryCSV}\\{employeeID}-{this.Header.ViewModel.Year_Text}-{this.Header.ViewModel.Month_Text}.csv";

        try
        {
            var reader = new StreamReader(path, encode);
            {
                string line = reader.ReadLine();
                string[] values = line.Split(',');

                List<string> lists = new List<string>();
                lists.AddRange(values);

                // 勤務先
                this.WorkPlace.WorkPlace_Text = values[3];

                // 有給残日数
                var paidVacation = Convert.ToDouble(values[17]) + Convert.ToDouble(values[25]);
                this.WorkingReference.ViewModel.PaidVacation_Text = paidVacation;
            }
        }
        catch (FileNotFoundException)
        {
            var message = $"「{Shared.DirectoryCSV}」に{this.Header.ViewModel.Year_Text}年{this.Header.ViewModel.Month_Text}月分のCSVが\n保存されていません。読み込みを中断します。";
            Message.ShowResultMessage(message, this.ViewModel.Window_Title);
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
        this.Header.ViewModel.Year_Text  = DateTime.Today.Year;
        this.Header.ViewModel.Month_Text = DateTime.Today.Month;
        // 支給額
        this.Allowance.Initialize();
        // 控除額
        this.Deduction.Initialize();
        // 勤務備考
        this.WorkingReference.Initialize();
        // 副業
        this.SideBusiness.Initialize();
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
        var message = $"{this.Header.ViewModel.Year_Text}年{this.Header.ViewModel.Month_Text}月の給与明細を保存しますか？";
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
            this.Allowance.Save(transaction, this.Header.ID, this.Header.YearMonth);
            // 控除額
            this.Deduction.Save(transaction, this.Header.ID, this.Header.YearMonth);
            // 勤務備考
            this.WorkingReference.Save(transaction, this.Header.ID, this.Header.YearMonth);
            // 副業
            this.SideBusiness.Save(transaction, this.Header.ID, this.Header.YearMonth);

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
    public void ComparePrice(double? thisYearPrice, double? lastYearPrice)
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

        this.ViewModel.PriceUpdown_Foreground = (difference > 0) ? new SolidColorBrush(Colors.Blue) : new SolidColorBrush(Colors.Red);
        this.ViewModel.PriceUpdown_Content    = difference.ToString();
    }

    #endregion

}
