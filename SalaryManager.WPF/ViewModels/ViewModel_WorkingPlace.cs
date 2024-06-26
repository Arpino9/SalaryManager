﻿namespace SalaryManager.WPF.ViewModels;

/// <summary>
/// ViewMoldel - 職場
/// </summary>
public class ViewModel_WorkingPlace : ViewModelBase<Model_WorkingPlace>
{
    public override event PropertyChangedEventHandler PropertyChanged;

    public ViewModel_WorkingPlace()
    {
        this.Model.ViewModel = this;

        this.Model.Initialize();

        this.BindEvents();
    }

    protected override void BindEvents()
    {
        // 派遣元会社
        this.DispatchingCompanyName_SelectionChanged.Subscribe(_ => this.Model.EnableWaitingButton());
        // 派遣先会社
        this.DispatchedCompanyName_SelectionChanged.Subscribe(_ => this.Model.EnableWaitingButton());
        // 就業場所
        this.WorkingPlace_TextChanged.Subscribe(_ => this.Model.SearchAddress());
        // 就業中
        this.IsWorking_Checked.Subscribe(_ => this.Model.IsWorking_Checked());
        // 就業場所の住所
        this.Address_TextChanged.Subscribe(_ => this.Model.EnableAddButton());
        // 経歴一覧
        this.WorkingPlaces_SelectionChanged.Subscribe(_ => this.Model.ListView_SelectionChanged());

        // 追加
        this.Add_Command.Subscribe(_ => this.Model.Add());
        this.Add_Command.Subscribe(_ => this.Model.Reload());

        // 更新
        this.Update_Command.Subscribe(_ => this.Model.Update());
        this.Update_Command.Subscribe(_ => this.Model.Reload());

        // 削除
        this.Delete_Command.Subscribe(_ => this.Model.Delete());
        this.Delete_Command.Subscribe(_ => this.Model.Reload());
    }

    /// <summary> Model </summary>
    protected override Model_WorkingPlace Model { get; }
        = Model_WorkingPlace.GetInstance(new WorkingPlaceSQLite());

    #region Window

    /// <summary> Window - Title </summary>
    public ReactiveProperty<string> Window_Title { get; }
        = new ReactiveProperty<string>("就業場所登録");

    /// <summary> Window - Title </summary>
    public ReactiveProperty<FontFamily> Window_FontFamily { get; set; }
        = new ReactiveProperty<FontFamily>();

    /// <summary> Window - FontSize </summary>
    public ReactiveProperty<decimal> Window_FontSize { get; set; }
        = new ReactiveProperty<decimal>();

    /// <summary> Window - Background </summary>
    public ReactiveProperty<SolidColorBrush> Window_Background { get; set; }
        = new ReactiveProperty<SolidColorBrush>();

    #endregion

    #region 就業場所一覧

    /// <summary> 就業場所一覧 - ItemSource </summary>
    public ReactiveCollection<WorkingPlaceEntity> WorkingPlaces_ItemSource { get; set; }
        = new ReactiveCollection<WorkingPlaceEntity>();

    /// <summary> 就業場所一覧 - SelectedIndex </summary>
    public ReactiveProperty<int> WorkingPlaces_SelectedIndex { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 就業場所一覧 - SelectionChanged </summary>
    public ReactiveCommand WorkingPlaces_SelectionChanged { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 会社名

    /// <summary> 就業場所 - ItemSource </summary>
    public ReactiveCollection<string> CompanyName_ItemSource { get; set; }
        = new ReactiveCollection<string>();

    /// <summary> 派遣元会社名 - Text </summary>
    public ReactiveProperty<string> DispatchingCompanyName_Text { get; set; }
        = new ReactiveProperty<string>();

    /// <summary> 派遣元会社名 - SelectionChanged </summary>
    public ReactiveCommand DispatchingCompanyName_SelectionChanged { get; private set; }
        = new ReactiveCommand();

    /// <summary> 派遣先会社名 - Text </summary>
    public ReactiveProperty<string> DispatchedCompanyName_Text { get; set; }
        = new ReactiveProperty<string>();

    /// <summary> 派遣先会社名 - SelectionChanged </summary>
    public ReactiveCommand DispatchedCompanyName_SelectionChanged { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 就業期間

    /// <summary> 就業期間 - 開始 - SelectedDate </summary>
    public ReactiveProperty<DateTime> WorkingStart_SelectedDate { get; set; }
        = new ReactiveProperty<DateTime>();

    /// <summary> 就業期間 - 終了 - SelectedDate </summary>
    public ReactiveProperty<DateTime> WorkingEnd_SelectedDate { get; set; }
        = new ReactiveProperty<DateTime>();

    #endregion

    #region 待機チェックボックス

    /// <summary> 待機 - IsChecked </summary>
    public ReactiveProperty<bool> IsWaiting_IsChacked { get; set; }
        = new ReactiveProperty<bool>();

    /// <summary> 待機 - Visibility </summary>
    public ReactiveProperty<Visibility> IsWaiting_Visibility { get; set; }
        = new ReactiveProperty<Visibility>();

    #endregion

    #region 就業中

    /// <summary> 就業中 - IsChecked </summary>
    public ReactiveProperty<bool> IsWorking_IsChacked { get; set; }
        = new ReactiveProperty<bool>();

    /// <summary> 派遣先会社名 - SelectionChanged </summary>
    public ReactiveCommand IsWorking_Checked { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 就業場所

    /// <summary> 就業場所 - ItemSource </summary>
    public ReactiveCollection<string> WorkingPlace_ItemSource { get; set; }
        = new ReactiveCollection<string>();

    /// <summary> 就業場所 - Text </summary>
    public ReactiveProperty<string> WorkingPlace_Name_Text { get; set; }
        = new ReactiveProperty<string>();

    /// <summary> 就業場所 - TextChanged </summary>
    public ReactiveCommand WorkingPlace_TextChanged { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 住所

    /// <summary> 住所 - Text </summary>
    public ReactiveProperty<string> WorkingPlace_Address_Text { get; set; }
        = new ReactiveProperty<string>();

    /// <summary> 住所 - TextChanged </summary>
    public ReactiveCommand Address_TextChanged { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 労働

    /// <summary> 労働開始 - 時 - Text </summary>
    public ReactiveProperty<int> WorkingTime_Start_Hour_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 労働開始 -  分 - Text </summary>
    public ReactiveProperty<int> WorkingTime_Start_Minute_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 労働終了 - 時 - Text </summary>
    public ReactiveProperty<int> WorkingTime_End_Hour_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 労働終了 - 分 - Text </summary>
    public ReactiveProperty<int> WorkingTime_End_Minute_Text { get; set; }
        = new ReactiveProperty<int>();

    #endregion

    #region 昼休憩

    /// <summary> 昼休憩開始 - 時 - Text </summary>
    public ReactiveProperty<int> LunchTime_Start_Hour_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 昼休憩開始 - 分 - Text </summary>
    public ReactiveProperty<int> LunchTime_Start_Minute_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 昼休憩終了 - 時 - Text </summary>
    public ReactiveProperty<int> LunchTime_End_Hour_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 昼休憩終了 - 分 - Text </summary>
    public ReactiveProperty<int> LunchTime_End_Minute_Text { get; set; }
        = new ReactiveProperty<int>();

    #endregion

    #region 休憩

    /// <summary> 休憩開始 - 時 - Text </summary>
    public ReactiveProperty<int> BreakTime_Start_Hour_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 休憩開始 - 分 - Text </summary>
    public ReactiveProperty<int> BreakTime_Start_Minute_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 休憩終了 - 時 - Text </summary>
    public ReactiveProperty<int> BreakTime_End_Hour_Text { get; set; }
        = new ReactiveProperty<int>();

    /// <summary> 休憩終了 - 分 - Text </summary>
    public ReactiveProperty<int> BreakTime_End_Minute_Text { get; set; }
        = new ReactiveProperty<int>();

    #endregion

    #region 備考

    /// <summary> 備考 - Text </summary>
    public ReactiveProperty<string> Remarks_Text { get; set; }
        = new ReactiveProperty<string>();

    #endregion

    #region 追加

    /// <summary> 追加 - IsEnabled </summary>
    public ReactiveProperty<bool> Add_IsEnabled { get; set; }
        = new ReactiveProperty<bool>();

    /// <summary> 追加 - Command </summary>
    public ReactiveCommand Add_Command { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 更新

    /// <summary> 更新 - IsEnabled </summary>
    public ReactiveProperty<bool> Update_IsEnabled { get; set; }
        = new ReactiveProperty<bool>();

    /// <summary> 更新 - Command </summary>
    public ReactiveCommand Update_Command { get; private set; }
        = new ReactiveCommand();

    #endregion

    #region 削除

    /// <summary> 削除 - IsEnabled </summary>
    public ReactiveProperty<bool> Delete_IsEnabled { get; set; }
        = new ReactiveProperty<bool>();

    /// <summary> 削除 - Command </summary>
    public ReactiveCommand Delete_Command { get; private set; }
        = new ReactiveCommand();

    #endregion

}
