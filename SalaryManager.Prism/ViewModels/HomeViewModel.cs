namespace SalaryManager.Prism.ViewModels;

/// <summary>
/// ViewModel - 自宅
/// </summary>
public class HomeViewModel : ViewModelBase<HomeModel>, IDialogAware
{
    public HomeViewModel()
    {
        this.Model.ViewModel = this;

        this.Model.Initialize();

        this.BindEvents();
    }

    public event Action<IDialogResult> RequestClose;

    protected override void BindEvents()
    {
        // 自宅一覧
        this.Homes_SelectionChanged = new DelegateCommand(() => this.Model.ListView_SelectionChanged());

        // 在住中
        this.IsLiving_Checked = new DelegateCommand(() => this.Model.IsLiving_Checked());

        // 住所
        this.Address_TextChanged        = new DelegateCommand(() => this.Model.EnableAddButton());
        this.Address_Google_TextChanged = new DelegateCommand(() => this.Model.EnableAddButton());

        this.Add_Command = new DelegateCommand(() => 
        {
            this.Model.AddAsync();
            this.Model.Reload();
        });

        this.Update_Command = new DelegateCommand(() =>
        {
            this.Model.UpdateAsync();
            this.Model.Reload();
        });
        
        this.Delete_Command = new DelegateCommand(() =>
        {
            this.Model.DeleteAsync();
            this.Model.Reload();
        });
    }

    /// <summary> タイトル </summary>
    public string Title => "自宅マスタ";

    public bool CanCloseDialog()
    {
        return true;
    }

    public void OnDialogClosed()
    {
        
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        
    }

    /// <summary> Model - 自宅 </summary>
    protected override HomeModel Model { get; } = HomeModel.GetInstance(new HomeSQLite());

    #region 自宅一覧

    /// <summary> 自宅一覧 - ItemSource </summary>
    public ObservableCollection<HomeEntity> Homes_ItemSource
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    } = new ObservableCollection<HomeEntity>();

    /// <summary> 自宅一覧 - SelectionChanged </summary>
    public DelegateCommand Homes_SelectionChanged { get; private set; }

    /// <summary> 自宅 - SelectedIndex </summary>
    public int Homes_SelectedIndex
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region 名称

    /// <summary> 名称 - Text </summary>
    public string DisplayName_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region 郵便番号

    /// <summary> 郵便番号 - Text </summary>
    public string PostCode_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region 在住期間

    /// <summary> 在住期間 - 開始日 - Text </summary>
    public DateOnly LivingStart_SelectedDate
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 在住期間 - 終了日 - Text </summary>
    public DateOnly LivingEnd_SelectedDate
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region 在住中

    /// <summary> 在住中 - IsChecked </summary>
    public bool IsLiving_IsChecked
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 在住中 - Checked </summary>
    public DelegateCommand IsLiving_Checked { get; private set; }

    #endregion

    #region 住所

    /// <summary> 住所 - Text </summary>
    public string Address_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 住所 (Google) - TextChanged </summary>
    public DelegateCommand Address_TextChanged { get; private set; }

    /// <summary> 住所 (Google) - Text </summary>
    public string Address_Google_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 住所 (Google) - TextChanged </summary>
    public DelegateCommand Address_Google_TextChanged { get; private set; }

    #endregion

    #region 備考

    /// <summary> 備考 - Text </summary>
    public string Remarks_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region 追加

    /// <summary> 追加 - IsEnabled </summary>
    public bool Add_IsEnabled
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 追加 - Command </summary>
    public DelegateCommand Add_Command { get; private set; }

    #endregion

    #region 更新

    /// <summary> 更新 - IsEnabled </summary>
    public bool Update_IsEnabled
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 更新 - Command </summary>
    public DelegateCommand Update_Command { get; private set; }

    #endregion

    #region 削除

    /// <summary> 削除 - IsEnabled </summary>
    public bool Delete_IsEnabled
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 削除 - Command </summary>
    public DelegateCommand Delete_Command { get; private set; }

    #endregion

}
