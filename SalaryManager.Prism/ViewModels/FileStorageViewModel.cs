namespace SalaryManager.Prism.ViewModels;

/// <summary>
/// Model - 添付ファイル
/// </summary>
public class FileStorageViewModel : ViewModelBase<FileStorageModel>, IDialogAware
{
    public FileStorageViewModel()
    {
        this.Model.ViewModel = this;

        this.Model.Initialize();

        this.BindEvents();
    }

    public event Action<IDialogResult> RequestClose;

    protected override void BindEvents()
    {
        // 添付ファイル一覧
        this.AttachedFile_SelectionChanged = new DelegateCommand(() => this.Model.ListView_SelectionChanged());

        // ファイルを開く
        this.SelectFile_Command = new DelegateCommand(() => this.Model.SelectFile());
        // フォルダを開く
        this.SelectFolder_Command = new DelegateCommand(() => this.Model.SelectFolder());

        // 画像を拡大表示する
        this.OpenImageViewer_Command = new DelegateCommand(() => this.Model.OpenImageViewer());

        // 追加
        this.Add_Command = new DelegateCommand(() => this.Model.AddAsync());
        // 更新
        this.Update_Command = new DelegateCommand(() => this.Model.UpdateAsync());
        // 削除
        this.Delete_Command = new DelegateCommand(() => this.Model.DeleteAsync());
    }

    /// <summar> タイトル </summary>
    public string Title => "添付ファイル管理";

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

    /// <summary> Model - 支給額 </summary>
    protected override FileStorageModel Model { get; }
        = FileStorageModel.GetInstance(new FileStorageSQLite());


    #region Window

    /// <summary> Window - FontFamily </summary>
    public FontFamily Window_FontFamily
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> Window - FontSize </summary>
    public decimal Window_FontSize
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> Window - Background </summary>
    public Brush Window_Background
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region 添付ファイル一覧

    /// <summary> 添付ファイル一覧 - ItemSource </summary>
    public ObservableCollection<FileStorageEntity> AttachedFile_ItemSource
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    } = new ObservableCollection<FileStorageEntity>();

    /// <summary> 添付ファイル一覧 - SelectedIndex </summary>
    public int AttachedFile_SelectedIndex
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 添付ファイル一覧 - SelectionChanged </summary>
    public DelegateCommand AttachedFile_SelectionChanged { get; private set; }

    #endregion

    #region ID

    /// <summary> ID - Text </summary>
    public int ID_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region ファイルのパス

    /// <summary> ファイルのパス - Text </summary>
    public string FilePath_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region ファイルを開く

    /// <summary> ファイルを開く - IsEnabled </summary>
    public bool SelectFile_IsEnabled
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> ファイルを開く - Command </summary>
    public DelegateCommand SelectFile_Command { get; private set; }

    #endregion

    #region フォルダを開く

    /// <summary> フォルダを開く - IsEnabled </summary>
    public bool SelectFolder_IsEnabled
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> フォルダを開く - Command </summary>
    public DelegateCommand SelectFolder_Command { get; private set; }

    #endregion

    #region 画像

    /// <summary> ファイルのパス - Image </summary>
    public ImageSource FileImage_Image
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region 画像を拡大表示する

    /// <summary> 画像を拡大表示する - IsEnabled </summary>
    public bool OpenImageViewer_IsEnabled
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 画像を拡大表示する - Command </summary>
    public DelegateCommand OpenImageViewer_Command { get; private set; }

    #endregion

    #region タイトル

    /// <summary> タイトル - IsEnabled </summary>
    public bool Title_IsEnabled
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> タイトル - Text </summary>
    public string Title_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region ファイル名

    /// <summary> ファイル名 - Text </summary>
    public string FileName_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region 備考

    /// <summary> 備考 - IsEnabled </summary>
    public bool Remarks_IsEnabled
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 備考 - Text </summary>
    public string Remarks_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region 追加日付

    /// <summary> 追加日付 </summary>
    public DateOnly CreateDate { get; set; }

    #endregion

    #region 更新日付

    /// <summary> 更新日付  </summary>
    public DateOnly UpdateDate { get; set; }

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
