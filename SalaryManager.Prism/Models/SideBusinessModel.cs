namespace SalaryManager.Prism.Models;

/// <summary>
/// Model - 副業
/// </summary>
public class SideBusinessModel : ModelBase<SideBusinessViewModel>, IParallellyEditable
{
    #region Get Instance

    private static SideBusinessModel model = null;

    public static SideBusinessModel GetInstance(ISideBusinessRepository repository)
    {
        if (model == null)
        {
            model = new SideBusinessModel(repository);
        }

        return model;
    }

    #endregion

    /// <summary> Repository </summary>
    private ISideBusinessRepository _repository;

    public SideBusinessModel(ISideBusinessRepository repository)
    {
        _repository = repository;
    }

    internal override SideBusinessViewModel ViewModel { get; set; }

    /// <summary> ViewModel - ヘッダ </summary>
    internal HeaderViewModel Header { get; set; }

    /// <summary> Model - ヘッダー </summary>
    private HeaderModel Model_Header { get; set; } = HeaderModel.GetInstance(new HeaderSQLite());

    /// <summary> Entity - 勤務備考 </summary>
    public SideBusinessEntity Entity { get; set; }

    /// <summary> Entity - 勤務備考 (昨年度) </summary>
    public SideBusinessEntity Entity_LastYear { get; set; }

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
            this.Entity = SideBusinesses.FetchDefault();
        }
    }

    public void Window_Activated()
    {
        this.ViewModel.Window_FontFamily = base.ConvertToWpfFontFamily(XMLLoader.FetchFontFamily());
        this.ViewModel.Window_FontSize = XMLLoader.FetchFontSize();
        this.ViewModel.Window_Background = base.ConvertToBrush(XMLLoader.FetchBackgroundColorBrush());
    }

    /// <summary>
    /// リロード
    /// </summary>
    /// <remarks>
    /// 該当月に副業額が存在すれば、各項目を再描画する。
    /// </remarks>
    public void Reload()
    {
        using (var cursor = new CursorWaiting())
        {
            SideBusinesses.Create(_repository);

            this.Entity = SideBusinesses.Fetch(this.Header.Year_Text, this.Header.Month_Text);
            this.Entity_LastYear = SideBusinesses.Fetch(this.Header.Year_Text - 1, this.Header.Month_Text);

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
        // 副業
        this.ViewModel.SideBusiness_Text = default(double);
        // 臨時収入
        this.ViewModel.Perquisite_Text = default(double);
        // その他
        this.ViewModel.Others_Text = default(double);
        // 備考
        this.ViewModel.Remarks_Text = default(string);
    }

    /// <summary>
    /// 再描画
    /// </summary>
    /// <remarks>
    /// 該当月に副業額が存在すれば、各項目を再描画する。
    /// </remarks>
    public void Reload_InputForm()
    {
        if (this.Entity is null)
        {
            this.Clear();
            return;
        }

        // 副業
        this.ViewModel.SideBusiness_Text = this.Entity.SideBusiness;
        // 臨時収入
        this.ViewModel.Perquisite_Text = this.Entity.Perquisite;
        // その他
        this.ViewModel.Others_Text = this.Entity.Others;
        // 備考
        this.ViewModel.Remarks_Text = this.Entity.Remarks;
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="yearMonh">年月</param>
    /// <param name="transaction">トランザクション</param>
    /// <remarks>
    /// SQLiteに接続し、入力項目を保存する。
    /// </remarks>
    public void Save(ITransactionRepository transaction, int id, DateOnly yearMonh)
    {
        var entity = new SideBusinessEntity(
            id,
            yearMonh,
            this.ViewModel.SideBusiness_Text,
            this.ViewModel.Perquisite_Text,
            this.ViewModel.Others_Text,
            this.ViewModel.Remarks_Text);

        _repository.Save(transaction, entity);
    }
}
