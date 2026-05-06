using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SalaryManager.Prism.ViewModels;

public class SpreadSheetOptionViewModel : ViewModelBase<OptionModel>
{
    public SpreadSheetOptionViewModel()
    {
        this.Model.SpreadSheetOption = this;

        this.Model.Initialize_SpreadSheet();

        this.BindEvents();
    }

    protected override void BindEvents()
    {
        this.SelectPrivateKey_Command = new DelegateCommand(() => this.Model.SelectPrivateKeyPath_SpreadSheet());
    }

    /// <summary> Model - オプション </summary>
    protected override OptionModel Model { get; } = OptionModel.GetInstance();

    #region 認証ファイルの保存先パス

    /// <summary> 認証ファイルの保存先パス - Text </summary>
    public string SelectPrivateKey_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    /// <summary> 認証ファイルの保存先パス - Command </summary>
    public DelegateCommand SelectPrivateKey_Command { get; private set; }

    #endregion

    #region シートID

    /// <summary> シートID - Text </summary>
    public string SheetId_Text
    {
        get { return field; }
        set { SetProperty(ref field, value); }
    }

    #endregion

}
