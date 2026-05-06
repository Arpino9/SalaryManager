using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using SalaryManager.Prism.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SalaryManager.Prism.ViewModels;

public class CompanyViewModel : BindableBase, IDialogAware
{
    public CompanyViewModel()
    {
        this.Model.ViewModel = this;

        this.BindEvents();

        this.Model.Initialize();
    }

    public event Action<IDialogResult> RequestClose;

    protected void BindEvents()
    {
        // 会社名
        this.CompanyName_TextChanged = new DelegateCommand(() => this.Model.EnableAddButton());
        // 住所
        this.Address_Google_TextChanged = new DelegateCommand(() => this.Model.EnableAddButton());
        // 業種
        this.BusinessCategory_Large_SelectionChanged = new DelegateCommand(() => this.Model.BusinessCategory_Large_SelectionChanged());

        // 追加
        this.Add_Command = new DelegateCommand(() => this.Add_Command_Execute());

        // 更新
        this.Update_Command = new DelegateCommand(() => this.Update_Command_Execute());

        // 削除
        this.Delete_Command = new DelegateCommand(() => this.Delete_Command_Execute());

        // 会社一覧
        this.Companies_SelectionChanged = new DelegateCommand(() => this.Model.ListView_SelectionChanged());
    }

    /// <summary> Model - 会社 </summary>
    protected CompanyModel Model { get; } = CompanyModel.GetInstance(new CompanySQLite());

    /// <summary> タイトル </summary>
    public string Title => "会社マスタ";

    public bool CanCloseDialog()
    {
        return true;
    }

    public void OnDialogClosed()
    {
        //throw new NotImplementedException();
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        //throw new NotImplementedException();
    }

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

    #region 会社一覧

    /// <summary> 会社一覧 - ItemSource </summary>
    public ObservableCollection<CompanyEntity> Companies_ItemSource
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    } = new ObservableCollection<CompanyEntity>();

    /// <summary> 会社一覧 - SelectedIndex </summary>
    public int Companies_SelectedIndex
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 会社一覧 - SelectionChanged </summary>
    public DelegateCommand Companies_SelectionChanged { get; private set; }

    #endregion

    #region 業種 (大区分)

    /// <summary>  業種 (大区分) - ItemsSource </summary>
    /// <remarks> 固定なので、値オブジェクトのリストを流用。 </remarks>
    public ObservableCollection<BusinessCategoryValue> BusinessCategory_Large_ItemsSource
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    } = new ObservableCollection<BusinessCategoryValue>();

    /// <summary> 業種 (大区分) - SelectedItem </summary>
    public string BusinessCategory_Large_SelectedItem
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 業種 (大区分) - Text </summary>
    public string BusinessCategory_Large_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 業種 (大区分) - SelectionChanged </summary>
    public DelegateCommand BusinessCategory_Large_SelectionChanged { get; private set; }

    #endregion

    #region 業種 (中区分)

    /// <summary> 業種 (中区分) - ItemSource </summary>
    public ObservableCollection<string> BusinessCategory_Middle_ItemSource
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    } = new ObservableCollection<string>();

    /// <summary> 業種 (中区分) - Text </summary>
    public string BusinessCategory_Middle_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region 会社名

    /// <summary> 会社名 - Text </summary>
    public string CompanyName_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 会社名 - TextChanged </summary>
    public DelegateCommand CompanyName_TextChanged { get; private set; }

    #endregion

    #region 郵便番号

    /// <summary> 郵便番号 - Text </summary>
    public string PostCode_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

    #region 住所

    /// <summary> 住所 - Text </summary>
    public string Address_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 住所(Googleカレンダー登録用) - Text </summary>
    public string Address_Google_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 住所(Googleカレンダー登録用) - TextChanged </summary>
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

    /// <summary> 追加 - Command - Execute </summary>
    private void Add_Command_Execute()
    {
        this.Model.Add();
        this.Model.AddtionalUpdate();
        this.Model.Reload();
    }

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

    /// <summary> 更新 - Command - Execute </summary>
    private void Update_Command_Execute()
    {
        this.Model.Update();
        this.Model.AddtionalUpdate();
        this.Model.Reload();
    }

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

    /// <summary> 削除 - Command - Execute </summary>
    private void Delete_Command_Execute()
    {
        this.Model.Delete();
        this.Model.Reload();
    }

    #endregion

}
