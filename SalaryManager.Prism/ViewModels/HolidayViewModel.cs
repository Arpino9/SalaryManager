namespace SalaryManager.Prism.ViewModels;

/// <summary>
/// ViewModel - 祝日
/// </summary>
public class HolidayViewModel : ViewModelBase<HolidayModel>, IDialogAware
{
    public HolidayViewModel()
    {
        this.Model.ViewModel = this;

        this.BindEvents();
        this.Model.Initialize();
    }

    public event Action<IDialogResult> RequestClose;

    protected override void BindEvents()
    {
        // 日付
        this.Date_SelectedDateChanged = new DelegateCommand(() => this.Model.EnableControlButton());
        // 祝日名
        this.Name_TextChanged = new DelegateCommand(() => this.Model.EnableControlButton());
        // 備考
        this.Remarks_TextChanged = new DelegateCommand(() => this.Model.EnableControlButton());
        // 会社休日
        this.CompanyHoliday_Checked = new DelegateCommand(() => this.Model.EnableCompanyNameComboBox());
        // 祝日一覧
        this.Holidays_SelectionChanged = new DelegateCommand(() => this.Model.ListView_SelectionChanged());

        // 追加
        this.Add_Command = new DelegateCommand(() => {
            this.Model.AddAsync();
            this.Model.Reload();
        });

        // 更新
        this.Update_Command = new DelegateCommand(() => {
            this.Model.UpdateAsync();
            this.Model.Reload();
        });

        // 削除
        this.Delete_Command = new DelegateCommand(() => {
            this.Model.DeleteAsync();
            this.Model.Reload();
        });
    }

    /// <summary> タイトル </summary>
    public string Title => "祝日マスタ";

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
    protected override HolidayModel Model { get; } = new HolidayModel();

    #region 祝日一覧

    /// <summary> 祝日一覧 - ItemSource </summary>
    public ObservableCollection<HolidayEntity> Holidays_ItemSource
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    } = new ObservableCollection<HolidayEntity>();

    /// <summary> 祝日一覧 - SelectedIndex </summary>
    public int Holidays_SelectedIndex
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 祝日一覧 - SelectionChanged </summary>
    public DelegateCommand Holidays_SelectionChanged { get; private set; }

    #endregion

    #region 日付

    /// <summary> 日付 - SelectedDate </summary>
    public DateTime Date_SelectedDate
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 日付 - TextChanged </summary>
    public DelegateCommand Date_SelectedDateChanged { get; private set; }

    #endregion

    #region 祝日名

    /// <summary> 祝日名 - IsEnabled </summary>
    public bool Name_IsEnabled
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 祝日名 - Text </summary>
    public string Name_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 祝日名 - TextChanged </summary>
    public DelegateCommand Name_TextChanged { get; private set; }

    #endregion

    #region 会社休日設定

    /// <summary> 会社休日 - IsChecked </summary>
    public bool CompanyHoliday_IsChecked
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 会社休日 - Checked </summary>
    public DelegateCommand CompanyHoliday_Checked { get; private set; }

    /// <summary> 会社名 - IsEnabled </summary>
    public bool CompanyName_IsEnabled
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 会社名 - ItemSource </summary>
    public ObservableCollection<CompanyEntity> CompanyName_ItemSource
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    } = new ObservableCollection<CompanyEntity>();

    /// <summary> 会社名 - SelectedIndex </summary>
    public int CompanyName_SelectedIndex
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 会社名 - Text </summary>
    public string CompanyName_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region 備考

    /// <summary> 備考 - Text </summary>
    public string Remarks_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 備考 - TextChanged </summary>
    public DelegateCommand Remarks_TextChanged { get; private set; }

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
