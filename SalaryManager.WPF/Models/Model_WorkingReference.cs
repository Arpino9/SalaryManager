using Message = SalaryManager.Domain.Modules.Logics.Message;

namespace SalaryManager.WPF.Models;

/// <summary>
/// Model - 勤務備考
/// </summary>
public class Model_WorkingReference : ModelBase<ViewModel_WorkingReference>, IParallellyEditable
{

    #region Get Instance

    private static Model_WorkingReference model = null;

    public static Model_WorkingReference GetInstance(IWorkingReferencesRepository repository)
    {
        if (model == null)
        {
            model = new Model_WorkingReference(repository);
        }

        return model;
    }

    #endregion

    /// <summary> Repository </summary>
    private IWorkingReferencesRepository _repository;

    public Model_WorkingReference(IWorkingReferencesRepository repository)
    {
        _repository = repository;
    }
    
    /// <summary> ViewModel - 勤務備考 </summary>
    internal override ViewModel_WorkingReference ViewModel { get; set; }

    /// <summary> ViewModel - メイン画面 </summary>
    internal ViewModel_MainWindow MainWindow { get; set; }

    /// <summary> ViewModel - ヘッダ </summary>
    internal ViewModel_Header Header { get; set; }

    /// <summary> ViewModel - 勤務先 </summary>
    internal ViewModel_WorkPlace WorkPlace { get; set; }

    /// <summary> Model - ヘッダー </summary>
    private Model_Header Model_Header { get; set; } = Model_Header.GetInstance(new HeaderSQLite());

    /// <summary> Entity - 勤務備考 </summary>
    public WorkingReferencesEntity Entity { get; set; }

    /// <summary> Entity - 勤務備考 (昨年度) </summary>
    public WorkingReferencesEntity Entity_LastYear { get; set; }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <remarks>
    /// 画面起動時に、項目を初期化する。
    /// </remarks>
    public void Initialize()
    {
        this.Window_Activated();

        this.Reload();

        var showDefaultPayslip = XMLLoader.FetchShowDefaultPayslip();

        if (this.Entity is null && showDefaultPayslip)
        {
            // デフォルト明細
            this.Entity = WorkingReferences.FetchDefault();
        }
    }

    public void Window_Activated()
    {
        this.ViewModel.Window_FontFamily.Value = XMLLoader.FetchFontFamily();
        this.ViewModel.Window_FontSize.Value   = XMLLoader.FetchFontSize();
        this.ViewModel.Window_Background.Value = XMLLoader.FetchBackgroundColorBrush();
    }

    /// <summary>
    /// リロード
    /// </summary>
    /// <remarks>
    /// 年月の変更時などに、該当月の項目を取得する。
    /// </remarks>
    public void Reload()
    {
        using (var cursor = new CursorWaiting())
        {
            WorkingReferences.Create(_repository);

            this.Entity          = WorkingReferences.Fetch(this.Header.Year_Text.Value,     this.Header.Month_Text.Value);
            this.Entity_LastYear = WorkingReferences.Fetch(this.Header.Year_Text.Value - 1, this.Header.Month_Text.Value);

            this.Reload_InputForm();
        }   
    }

    /// <summary>
    /// クリア
    /// </summary>
    /// <remarks>
    /// 各項目を初期化する。
    /// </remarks>
    public void Clear()
    {
        // 時間外時間
        this.ViewModel.OvertimeTime_Text.Value      = default(double);
        // 休出時間
        this.ViewModel.WeekendWorktime_Text.Value   = default(double);
        // 深夜時間
        this.ViewModel.MidnightWorktime_Text.Value  = default(double);
        // 遅刻早退欠勤H
        this.ViewModel.LateAbsentH_Text.Value       = default(double);
        // 支給額-保険
        this.ViewModel.Insurance_Text.Value         = default(double);
        // 標準月額千円
        this.ViewModel.Norm_Text.Value              = default(double);
        // 扶養人数
        this.ViewModel.NumberOfDependent_Text.Value = default(double);
        // 有給残日数
        this.ViewModel.PaidVacation_Text.Value      = default(double);
        // 勤務時間
        this.ViewModel.WorkingHours_Text.Value      = default(double);
        // 備考
        this.ViewModel.Remarks_Text.Value            = default(string);
    }

    /// <summary>
    /// 再描画
    /// </summary>
    /// <remarks>
    /// 該当月に控除額が存在すれば、各項目を再描画する。
    /// </remarks>
    public void Reload_InputForm()
    {
        if (this.Entity is null)
        {
            this.Clear();
            return;
        }

        // 時間外時間
        this.ViewModel.OvertimeTime_Text.Value      = this.Entity.OvertimeTime;
        // 休出時間
        this.ViewModel.WeekendWorktime_Text.Value   = this.Entity.WeekendWorktime;
        // 深夜時間
        this.ViewModel.MidnightWorktime_Text.Value  = this.Entity.MidnightWorktime;
        // 遅刻早退欠勤H
        this.ViewModel.LateAbsentH_Text.Value       = this.Entity.LateAbsentH;
        // 支給額-保険
        this.ViewModel.Insurance_Text.Value         = this.Entity.Insurance.Value;
        // 標準月額千円
        this.ViewModel.Norm_Text.Value              = this.Entity.Norm;
        // 扶養人数
        this.ViewModel.NumberOfDependent_Text.Value = this.Entity.NumberOfDependent;
        // 有給残日数
        this.ViewModel.PaidVacation_Text.Value      = this.Entity.PaidVacation.Value;
        // 勤務時間
        this.ViewModel.WorkingHours_Text.Value      = this.Entity.WorkingHours;
        // 備考
        this.ViewModel.Remarks_Text.Value           = this.Entity.Remarks;
    }

    /// <summary>
    /// エディットバリデーションチェック
    /// </summary>
    /// <returns>判定可否</returns>
    public bool EditValidationCheck()
    {
        if (this.Entity is null)
        {
            // 今月の明細の新規登録
            return true;
        }

        var paidVacation = this.Entity.PaidVacation;

        if (paidVacation.Value < PaidVacationDaysValue.Minimum ||
            paidVacation.Value > PaidVacationDaysValue.Maximum)
        {
            Message.ShowErrorMessage(
                $"有給休暇は{PaidVacationDaysValue.Minimum}から{PaidVacationDaysValue.Maximum}までの日数で入力して下さい。",
                this.MainWindow.Window_Title.Value);

            return false;
        }

        return true;
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="transaction">トランザクション</param>
    /// <remarks>
    /// SQLiteに接続し、入力項目を保存する。
    /// </remarks>
    public void Save(ITransactionRepository transaction)
    {
        var entity = new WorkingReferencesEntity(
            this.Model_Header.ID,
            this.Model_Header.YearMonth,
            this.ViewModel.OvertimeTime_Text.Value,
            this.ViewModel.WeekendWorktime_Text.Value,
            this.ViewModel.MidnightWorktime_Text.Value,
            this.ViewModel.LateAbsentH_Text.Value,
            this.ViewModel.Insurance_Text.Value,
            this.ViewModel.Norm_Text.Value,
            this.ViewModel.NumberOfDependent_Text.Value,
            this.ViewModel.PaidVacation_Text.Value,
            this.ViewModel.WorkingHours_Text.Value,
            this.WorkPlace.WorkPlace_Text.Value,
            this.ViewModel.Remarks_Text.Value);

        _repository.Save(transaction, entity);
    }
}
