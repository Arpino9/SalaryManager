using Message = SalaryManager.Domain.Modules.Logics.Message;

namespace SalaryManager.Prism.Models;

/// <summary>
/// Model - 勤務備考
/// </summary>
public class WorkingReferenceModel : ModelBase<WorkingReferenceViewModel>, IParallellyEditable
{

    #region Get Instance

    private static WorkingReferenceModel model = null;

    public static WorkingReferenceModel GetInstance(IWorkingReferencesRepository repository)
    {
        if (model == null)
        {
            model = new WorkingReferenceModel(repository);
        }

        return model;
    }

    #endregion

    /// <summary> Repository </summary>
    private IWorkingReferencesRepository _repository;

    public WorkingReferenceModel(IWorkingReferencesRepository repository)
    {
        _repository = repository;
    }

    /// <summary> ViewModel - 勤務備考 </summary>
    internal override WorkingReferenceViewModel ViewModel { get; set; }

    /// <summary> ViewModel - メイン画面 </summary>
    internal MainWindowViewModel MainWindow { get; set; }

    /// <summary> ViewModel - ヘッダ </summary>
    internal HeaderViewModel Header { get; set; }

    /// <summary> ViewModel - 勤務先 </summary>
    internal WorkPlaceViewModel WorkPlace { get; set; }

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
        this.ViewModel.Window_FontFamily = base.ConvertToWpfFontFamily(XMLLoader.FetchFontFamily());
        this.ViewModel.Window_FontSize   = XMLLoader.FetchFontSize();
        this.ViewModel.Window_Background = base.ConvertToBrush(XMLLoader.FetchBackgroundColorBrush());
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

            this.Entity          = WorkingReferences.Fetch(this.Header.Year_Text,     this.Header.Month_Text);
            this.Entity_LastYear = WorkingReferences.Fetch(this.Header.Year_Text - 1, this.Header.Month_Text);

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
        this.ViewModel.OvertimeTime_Text      = default(double);
        // 休出時間
        this.ViewModel.WeekendWorktime_Text   = default(double);
        // 深夜時間
        this.ViewModel.MidnightWorktime_Text  = default(double);
        // 遅刻早退欠勤H
        this.ViewModel.LateAbsentH_Text       = default(double);
        // 支給額-保険
        this.ViewModel.Insurance_Text         = default(double);
        // 標準月額千円
        this.ViewModel.Norm_Text              = default(double);
        // 扶養人数
        this.ViewModel.NumberOfDependent_Text = default(double);
        // 有給残日数
        this.ViewModel.PaidVacation_Text      = default(double);
        // 勤務時間
        this.ViewModel.WorkingHours_Text      = default(double);
        // 備考
        this.ViewModel.Remarks_Text           = default(string);
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
        this.ViewModel.OvertimeTime_Text      = this.Entity.OvertimeTime;
        // 休出時間
        this.ViewModel.WeekendWorktime_Text   = this.Entity.WeekendWorktime;
        // 深夜時間
        this.ViewModel.MidnightWorktime_Text  = this.Entity.MidnightWorktime;
        // 遅刻早退欠勤H
        this.ViewModel.LateAbsentH_Text       = this.Entity.LateAbsentH;
        // 支給額-保険
        this.ViewModel.Insurance_Text         = this.Entity.Insurance.Value;
        // 標準月額千円
        this.ViewModel.Norm_Text              = this.Entity.Norm;
        // 扶養人数
        this.ViewModel.NumberOfDependent_Text = this.Entity.NumberOfDependent;
        // 有給残日数
        this.ViewModel.PaidVacation_Text      = this.Entity.PaidVacation.Value;
        // 勤務時間
        this.ViewModel.WorkingHours_Text      = this.Entity.WorkingHours;
        // 備考
        this.ViewModel.Remarks_Text           = this.Entity.Remarks;
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
                this.MainWindow.Window_Title);

            return false;
        }

        return true;
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="yearMonth">年月</param>
    /// <param name="transaction">トランザクション</param>
    /// <remarks>
    /// SQLiteに接続し、入力項目を保存する。
    /// </remarks>
    public void Save(ITransactionRepository transaction, int id, DateOnly yearMonth)
    {
        var entity = new WorkingReferencesEntity(
            id,
            yearMonth,
            this.ViewModel.OvertimeTime_Text,
            this.ViewModel.WeekendWorktime_Text,
            this.ViewModel.MidnightWorktime_Text,
            this.ViewModel.LateAbsentH_Text,
            this.ViewModel.Insurance_Text,
            this.ViewModel.Norm_Text,
            this.ViewModel.NumberOfDependent_Text,
            this.ViewModel.PaidVacation_Text,
            this.ViewModel.WorkingHours_Text,
            this.WorkPlace.WorkPlace_Text,
            this.ViewModel.Remarks_Text);

        _repository.Save(transaction, entity);
    }
}
