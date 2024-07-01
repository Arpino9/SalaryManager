using WorkingPlace = SalaryManager.Domain.StaticValues.WorkingPlace;

namespace SalaryManager.WPF.Models;

/// <summary>
/// Model - 勤務場所
/// </summary>
public sealed class Model_WorkPlace : ModelBase<ViewModel_WorkPlace>, IParallellyEditable
{

    #region Get Instance

    private static Model_WorkPlace model = null;

    public static Model_WorkPlace GetInstance()
    {
        if (model == null)
        {
            model = new Model_WorkPlace();
        }

        return model;
    }

    #endregion

    public Model_WorkPlace()
    {

    }

    /// <summary> ViewModel - 勤務先 </summary>
    internal override ViewModel_WorkPlace ViewModel { get; set; }

    /// <summary> ViewModel - ヘッダ </summary>
    internal ViewModel_Header Header { get; set; }

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

    /// <summary>
    /// 画面起動時の処理
    /// </summary>
    public void Window_Activated()
    {
        this.ViewModel.Window_Background.Value = XMLLoader.FetchBackgroundColorBrush();
    }

    /// <summary>
    /// 再描画
    /// </summary>
    /// <remarks>
    /// 勤務先、所属会社名を再描画する。
    /// </remarks>
    public void Reload_InputForm()
    {
        WorkingPlace.Create(new WorkingPlaceSQLite());

        if (this.Entity is null)
        {
            this.Clear();
            return;
        }

        // 勤務先
        this.ViewModel.WorkPlace_Text.Value = this.Entity.WorkPlace;

        // 所属会社名
        var workingPlace = WorkingPlace.FetchByDate(new DateTime(this.Header.Year_Text.Value, this.Header.Month_Text.Value, 1));

        if (workingPlace.Any()) 
        {
            this.ViewModel.CompanyName_Text.Value = workingPlace.First().DispatchingCompany.Text;
            this.ViewModel.WorkPlace_Text.Value   = workingPlace.First().DispatchedCompany.Text;
        }
        else
        {
            this.ViewModel.CompanyName_Text.Value = CompanyNameValue.Undefined.DisplayValue;
            this.ViewModel.WorkPlace_Text.Value   = CompanyNameValue.Undefined.DisplayValue;
        }

        this.ViewModel.CompanyName_Foreground.Value = new SolidColorBrush(Colors.Black);
        this.ViewModel.WorkPlace_Foreground.Value   = new SolidColorBrush(Colors.Black);
    }

    /// <summary>
    /// リロード
    /// </summary>
    /// <remarks>
    /// 年月の変更時などに、該当月の項目を取得する。
    /// </remarks>
    public void Reload()
    {
        if (this.ViewModel is null)
        {
            return;
        }

        using (var cursor = new CursorWaiting())
        {
            WorkingReferences.Create(new WorkingReferenceSQLite());

            this.Entity          = WorkingReferences.Fetch(this.Header.Year_Text.Value, this.Header.Month_Text.Value);
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
        // 所属会社名
        var workingPlace = WorkingPlace.FetchByDate(new DateTime(this.Header.Year_Text.Value, this.Header.Month_Text.Value, 1));

        if (workingPlace.Any())
        {
            // 所属会社名
            this.ViewModel.CompanyName_Text.Value       = workingPlace.First().DispatchingCompany.Text;
            this.ViewModel.CompanyName_Foreground.Value = new SolidColorBrush(Colors.Black);
            // 勤務先
            this.ViewModel.WorkPlace_Text.Value       = workingPlace.First().WorkingPlace_Name.Text;
            this.ViewModel.WorkPlace_Foreground.Value = new SolidColorBrush(Colors.Black);
        }
        else
        {
            // 所属会社名
            this.ViewModel.CompanyName_Text.Value       = CompanyNameValue.Undefined.DisplayValue;
            this.ViewModel.CompanyName_Foreground.Value = new SolidColorBrush(Colors.Gray);
            // 勤務先
            this.ViewModel.WorkPlace_Text.Value       = CompanyNameValue.Undefined.DisplayValue;
            this.ViewModel.WorkPlace_Foreground.Value = new SolidColorBrush(Colors.Gray);
        }
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="transaction">トランザクション</param>
    /// <see cref="Model_WorkingReference"/>
    /// <exception cref="NotImplementedException">未実装例外</exception>
    [Obsolete("保存先は勤怠備考テーブルなので実装していない。")]
    public void Save(ITransactionRepository transaction)
    {
        throw new NotImplementedException();
    }
}
