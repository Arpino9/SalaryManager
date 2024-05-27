namespace SalaryManager.WPF.ViewModels;

/// <summary>
/// ViewModel - イメージビューアー
/// </summary>
public class ViewModel_ImageViewer : ViewModelBase<Model_FileStorage>
{
    public override event PropertyChangedEventHandler PropertyChanged;

    public ViewModel_ImageViewer()
    {
        this.Model.ViewModel_ImageViewer = this;
    }

    protected override void BindEvents()
    {
        throw new System.NotImplementedException();
    }

    /// <summary> Model - 添付ファイル管理 </summary>
    protected override Model_FileStorage Model { get; } 
        = Model_FileStorage.GetInstance(new FileStorageSQLite());

    #region Window

    /// <summary> Window - Title </summary>
    public ReactiveProperty<string> Window_Title { get; }
        = new ReactiveProperty<string>("イメージビューワー");

    #endregion

    #region 画像

    /// <summary> 画像 - Height </summary>
    public ReactiveProperty<double> FileImage_Height { get; set; }
        = new ReactiveProperty<double>();

    /// <summary> 画像 - Width </summary>
    public ReactiveProperty<double> FileImage_Width { get; set; }
        = new ReactiveProperty<double>();

    /// <summary> 画像 - Image </summary>
    public ReactiveProperty<ImageSource> FileImage_Image { get; set; }
        = new ReactiveProperty<ImageSource>();

    #endregion

}
