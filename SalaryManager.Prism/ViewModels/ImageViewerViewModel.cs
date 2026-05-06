using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SalaryManager.Prism.ViewModels;

public class ImageViewerViewModel : ViewModelBase<FileStorageModel>
{
    public ImageViewerViewModel()
    {
        this.Model.ViewModel_ImageViewer = this;
    }

    protected override void BindEvents()
    {
        throw new System.NotImplementedException();
    }

    /// <summary> Model - 添付ファイル管理 </summary>
    protected override FileStorageModel Model { get; }
        = FileStorageModel.GetInstance(new FileStorageSQLite());

    #region Window

    /// <summary> Window - Title </summary>
    public ReactiveProperty<string> Window_Title { get; }
        = new ReactiveProperty<string>("イメージビューワー");

    #endregion

    #region 画像

    /// <summary> 画像 - Height </summary>
    public double FileImage_Height
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 画像 - Width </summary>
    public double FileImage_Width
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 画像 - Image </summary>
    public ImageSource FileImage_Image
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

}
