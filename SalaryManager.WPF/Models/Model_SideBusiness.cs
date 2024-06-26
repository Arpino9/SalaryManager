﻿namespace SalaryManager.WPF.Models;

/// <summary>
/// Model - 副業
/// </summary>
public class Model_SideBusiness : ModelBase<ViewModel_SideBusiness>, IParallellyEditable
{

    #region Get Instance

    private static Model_SideBusiness model = null;

    public static Model_SideBusiness GetInstance(ISideBusinessRepository repository)
    {
        if (model == null)
        {
            model = new Model_SideBusiness(repository);
        }

        return model;
    }

    #endregion

    /// <summary> Repository </summary>
    private ISideBusinessRepository _repository;

    public Model_SideBusiness(ISideBusinessRepository repository)
    {
        _repository = repository;
    }

    /// <summary> ViewModel - 副業 </summary>
    internal override ViewModel_SideBusiness ViewModel { get; set; }

    /// <summary> ViewModel - ヘッダ </summary>
    internal ViewModel_Header Header { get; set; }

    /// <summary> Model - ヘッダー </summary>
    private Model_Header Model_Header { get; set; } = Model_Header.GetInstance(new HeaderSQLite());

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
        this.ViewModel.Window_FontFamily.Value = XMLLoader.FetchFontFamily();
        this.ViewModel.Window_FontSize.Value   = XMLLoader.FetchFontSize();
        this.ViewModel.Window_Background.Value = XMLLoader.FetchBackgroundColorBrush();
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

            this.Entity          = SideBusinesses.Fetch(this.Header.Year_Text.Value, this.Header.Month_Text.Value);
            this.Entity_LastYear = SideBusinesses.Fetch(this.Header.Year_Text.Value - 1, this.Header.Month_Text.Value);
        
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
        this.ViewModel.SideBusiness_Text.Value = default(double);
        // 臨時収入
        this.ViewModel.Perquisite_Text.Value   = default(double);
        // その他
        this.ViewModel.Others_Text.Value       = default(double);
        // 備考
        this.ViewModel.Remarks_Text.Value      = default(string);
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
        this.ViewModel.SideBusiness_Text.Value = this.Entity.SideBusiness;
        // 臨時収入
        this.ViewModel.Perquisite_Text.Value   = this.Entity.Perquisite;
        // その他
        this.ViewModel.Others_Text.Value       = this.Entity.Others;
        // 備考
        this.ViewModel.Remarks_Text.Value      = this.Entity.Remarks;
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
        var entity = new SideBusinessEntity(
            this.Model_Header.ID,
            this.Model_Header.YearMonth,
            this.ViewModel.SideBusiness_Text.Value,
            this.ViewModel.Perquisite_Text.Value,
            this.ViewModel.Others_Text.Value,
            this.ViewModel.Remarks_Text.Value);

        _repository.Save(transaction, entity);
    }
}
