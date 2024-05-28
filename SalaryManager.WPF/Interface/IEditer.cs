namespace SalaryManager.WPF.Interface;

/// <summary>
/// Interface - 編集用
/// </summary>
internal interface IEditer
{
    /// <summary> リストビュー - SelectionChanged </summary>
    public void ListView_SelectionChanged();

    /// <summary> 再描画 </summary>
    /// <remarks> 入力部分の再描画 </remarks>
    public void Refresh();

    /// <summary> リロード </summary>
    /// <remarks> Static Valuesの再読込 </remarks>
    public void Reload();

    /// <summary> クリア - 一覧部分 </summary>
    public void Clear_ListView();

    /// <summary> クリア - 入力部分 </summary>
    public void Clear_InputForm();

    /// <summary> 追加 </summary>
    public void Add();

    /// <summary> 更新 </summary>
    public void Update();

    /// <summary> 削除 </summary>
    public void Delete();
}
