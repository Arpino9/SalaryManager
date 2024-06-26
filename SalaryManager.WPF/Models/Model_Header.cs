﻿using Message = SalaryManager.Domain.Modules.Logics.Message;

namespace SalaryManager.WPF.Models;

/// <summary>
/// Model - ヘッダー
/// </summary>
public sealed class Model_Header : ModelBase<ViewModel_Header>, IViewable
{
    #region Get Instance

    private static Model_Header model = null;
    
    public static Model_Header GetInstance(IHeaderRepository repository)
    {
        if (model == null)
        {
            model = new Model_Header(repository);
        }

        return model;
    }

    #endregion

    /// <summary> Repository </summary>
    private IHeaderRepository _repository;

    public Model_Header(IHeaderRepository repository)
    {
        _repository = repository;
    }

    /// <summary> ViewModel - ヘッダー </summary>
    internal override ViewModel_Header ViewModel { get; set; }

    /// <summary> ViewModel - メイン画面 </summary>
    internal ViewModel_MainWindow MainWindow { get; set; }

    /// <summary> ID </summary>
    public int ID { get; internal set; }

    /// <summary> 年月 </summary>
    public DateTime YearMonth { get; set; } = DateTime.Today;

    /// <summary> デフォルトか </summary>
    public bool IsDefault { get; set; }

    /// <summary> 作成日 </summary>
    public DateTime CreateDate { get; set; }

    /// <summary> 更新日 </summary>
    public DateTime UpdateDate { get; set; }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <remarks>
    /// 画面起動時に、項目を初期化する。
    /// </remarks>
    public void Initialize()
    {
        this.Window_Activated();

        this.ViewModel.Year_Text.Value  = DateTime.Today.Year;
        this.ViewModel.Month_Text.Value = DateTime.Today.Month;

        this.Reload();
    }

    /// <summary>
    /// 画面起動時の処理
    /// </summary>
    public void Window_Activated()
    {
        this.ViewModel.Window_Background.Value = XMLLoader.FetchBackgroundColorBrush();
    }

    /// <summary>
    /// ←ボタン
    /// </summary>
    internal void Return()
    {
        var yearMonth = this.YearMonth.AddMonths(-1);

        this.ViewModel.Year_Text.Value  = yearMonth.Year;
        this.ViewModel.Month_Text.Value = yearMonth.Month;

        this.Reload();
    }

    /// <summary>
    /// →ボタン
    /// </summary>
    internal void Proceed()
    {
        var yearMonth = this.YearMonth.AddMonths(1);

        this.ViewModel.Year_Text.Value  = yearMonth.Year;
        this.ViewModel.Month_Text.Value = yearMonth.Month;

        this.Reload();
    }

    /// <summary>
    /// Reload
    /// </summary>
    /// <remarks>
    /// ヘッダ情報が存在すれば、ヘッダ情報を更新する。
    /// ここで「Refresh()」を呼び出すと再帰呼出になるので注意！
    /// </remarks>
    public void Reload()
    {
        Headers.Create(_repository);

        var entities = Headers.FetchByAscending();

        var entity = entities.Where(x => x.YearMonth.Year  == this.ViewModel.Year_Text.Value
                                      && x.YearMonth.Month == this.ViewModel.Month_Text.Value)
                             .FirstOrDefault();

        if (entity is null)
        {
            // 新規追加
            if (entities.Any())
            {
                this.ID = entities.Max(x => x.ID) + 1;
            }
            else
            {
                this.Clear();
            }
        }
        else
        {
            // 更新
            this.ID         = entity.ID;
            this.CreateDate = entity.CreateDate;
            this.UpdateDate = entity.UpdateDate;
        }

        this.YearMonth = new DateTime(this.ViewModel.Year_Text.Value, this.ViewModel.Month_Text.Value, 1);
    }

    /// <summary>
    /// クリア
    /// </summary>
    /// <remarks>
    /// 各項目を初期化する。
    /// </remarks>
    public void Clear()
    {
        // 年
        this.ViewModel.Year_Text.Value  = this.YearMonth.Year;

        // 月
        this.ViewModel.Month_Text.Value = this.YearMonth.Month;

        // 作成日
        this.CreateDate  = DateTime.Today;

        // 更新日
        this.UpdateDate  = DateTime.Today;
    }

    /// <summary>
    /// 保存
    /// </summary>
    public void SaveDefaultPayslip()
    {
        var entity = new HeaderEntity(this.ID, this.YearMonth, this.IsDefault, 
                                      this.CreateDate, this.UpdateDate);

        _repository.Save(entity);
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="transaction">トランザクション</param>
    public void Save(ITransactionRepository transaction)
    {
        var entity = new HeaderEntity(this.ID, this.YearMonth, this.IsDefault,
                                      this.CreateDate, this.UpdateDate);

        _repository.Save(transaction, entity);
    }

    /// <summary>
    /// デフォルトに設定
    /// </summary>
    internal void SetDefaultPayslip()
    {
        var confirmingMessage = $"{this.ViewModel.Year_Text.Value}年{this.ViewModel.Month_Text.Value}月の給与明細をデフォルト明細として設定しますか？";
        if (!Message.ShowConfirmingMessage(confirmingMessage, this.MainWindow.Window_Title.Value))
        {
            // キャンセル
            return;
        }

        // 前回のデフォルト設定を解除する
        var defaultEntity = _repository.FetchDefault();

        if (defaultEntity != null)
        {
            defaultEntity.IsDefault = false;
            _repository.Save(defaultEntity);
        }

        // 今回のデフォルト設定を登録する
        this.IsDefault = true;

        this.SaveDefaultPayslip();
    }

    /// <summary>
    /// IsValid - 年
    /// </summary>
    internal void IsValid_Year()
    {
        var value = this.ViewModel.Year_Text.Value;
        if (value.ToString().Length != 4)
        {
            return;
        }

        this.ViewModel.Year_Text.Value = value;
    }

    /// <summary>
    /// IsValid - 月
    /// </summary>
    internal void IsValid_Month()
    {
        var value = this.ViewModel.Month_Text.Value;

        if (value < 1)
        {
            this.ViewModel.Month_Text.Value = 1;
        }
        else if (value > 12)
        {
            this.ViewModel.Month_Text.Value = 12;
        }
        else
        {
            this.ViewModel.Month_Text.Value = value;
        }
    }
}
