using Prism.Commands;
using Prism.Mvvm;
using SalaryManager.Prism.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SalaryManager.Prism.ViewModels;

public class WorkPlaceViewModel : BindableBase
{
    public WorkPlaceViewModel()
    {
        this.Model.ViewModel = this;
        this.WorkingReference.WorkPlace = this;
        this.MainWindow.WorkPlace = this;
        this.Allowance.ViewModel_WorkPlace = this;

        this.Model.Initialize();
    }

    protected void BindEvents()
    {
        throw new NotImplementedException();
    }

    /// <summary> Model </summary>
    protected WorkPlaceModel Model { get; }
        = WorkPlaceModel.GetInstance();

    /// <summary> Model - 勤怠備考 </summary>
    public WorkingReferenceModel WorkingReference { get; set; }
        = WorkingReferenceModel.GetInstance(new WorkingReferenceSQLite());

    /// <summary> Model - メイン画面 </summary>
    public MainWindowModel MainWindow { get; set; }
        = MainWindowModel.GetInstance();

    /// <summary> Model - 手当 </summary>
    public AllowanceModel Allowance { get; set; }
        = AllowanceModel.GetInstance(new AllowanceSQLite());

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

    /// <summary> 背景色 - Background </summary>
    public SolidColorBrush Window_Background
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region 所属会社名

    /// <summary> 所属会社名 - Foreground </summary>
    public SolidColorBrush CompanyName_Foreground
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 所属会社名 - Text </summary>
    public string CompanyName_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region 勤務先

    /// <summary> 所属会社名 - Foreground </summary>
    public SolidColorBrush WorkPlace_Foreground
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 勤務先 - Text </summary>
    public string WorkPlace_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion
}
