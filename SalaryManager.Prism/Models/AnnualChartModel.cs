using SalaryManager.Prism.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalaryManager.Prism.Models;

/// <summary>
/// Model - 月収一覧
/// </summary>
public class AnnualChartModel : ModelBase<AnnualChartViewModel>, IViewable
{
    #region Get Instance

    private static AnnualChartModel model = null;

    public static AnnualChartModel GetInstance()
    {
        if (model == null)
        {
            model = new AnnualChartModel();
        }

        return model;
    }

    #endregion

    public AnnualChartModel()
    {

    }

    /// <summary> ViewModel - ヘッダ </summary>
    internal HeaderViewModel Header { get; set; }

    /// <summary> ViewModel - 月収一覧 </summary>
    internal override AnnualChartViewModel ViewModel { get; set; }

    /// <summary>
    /// Initialize
    /// </summary>
    /// <remarks>
    /// 画面起動時に、項目を初期化する。
    /// </remarks>
    public void Initialize()
    {
        this.Window_Activated();

        if (this.Header.Year_Text.ToString().Length != 4)
        {
            // 対象日付が不正
            return;
        }

        var yearValue = new DateValue(this.Header.Year_Text, this.Header.Month_Text);
        this.ViewModel.TargetDate_Content = yearValue?.YearWithJapaneseCalendar;

        if (this.ViewModel.TargetDate_Content is null)
        {
            return;
        }

        this.Clear();

        this.Reload();

        this.Recalculate();
    }

    /// <summary>
    /// 画面起動時の処理
    /// </summary>
    public void Window_Activated()
    {
        if (this.ViewModel is null) return;

        this.ViewModel.Window_Background = base.ConvertToBrush(XMLLoader.FetchBackgroundColorBrush());
    }

    /// <summary>
    /// リロード
    /// </summary>
    public void Reload()
    {
        AnnualCharts.Create(new AnnualChartSQLite());

        var entities = AnnualCharts.Fetch(this.Header.Year_Text);

        if (!entities.Any())
        {
            return;
        }

        foreach (var entity in entities)
        {
            var salary = this.RecalcMontlyIncome(entity);

            switch (entity.YearMonth.Month)
            {
                // 1月
                case 1:
                    this.ViewModel.January_TotalSalary_Text         = salary.Total + GetTransportation(1);
                    this.ViewModel.January_TotalDeductedSalary_Text = salary.TotalDeduced;
                    this.ViewModel.January_TotalSideBusiness_Text   = entity.TotalSideBusiness;
                    break;

                // 2月
                case 2:
                    this.ViewModel.Feburary_TotalSalary_Text         = salary.Total + GetTransportation(2);
                    this.ViewModel.Feburary_TotalDeductedSalary_Text = salary.TotalDeduced;
                    this.ViewModel.Feburary_TotalSideBusiness_Text   = entity.TotalSideBusiness;
                    break;

                // 3月
                case 3:
                    this.ViewModel.March_TotalSalary_Text         = salary.Total + GetTransportation(3);
                    this.ViewModel.March_TotalDeductedSalary_Text = salary.TotalDeduced;
                    this.ViewModel.March_TotalSideBusiness_Text   = entity.TotalSideBusiness;
                    break;

                // 4月
                case 4:
                    this.ViewModel.April_TotalSalary_Text         = salary.Total + GetTransportation(4);
                    this.ViewModel.April_TotalDeductedSalary_Text = salary.TotalDeduced;
                    this.ViewModel.April_TotalSideBusiness_Text   = entity.TotalSideBusiness;
                    break;

                // 5月
                case 5:
                    this.ViewModel.May_TotalSalary_Text         = salary.Total + GetTransportation(5);
                    this.ViewModel.May_TotalDeductedSalary_Text = salary.TotalDeduced;
                    this.ViewModel.May_TotalSideBusiness_Text   = entity.TotalSideBusiness;
                    break;

                // 6月
                case 6:
                    this.ViewModel.June_TotalSalary_Text         = salary.Total + GetTransportation(6);
                    this.ViewModel.June_TotalDeductedSalary_Text = salary.TotalDeduced;
                    this.ViewModel.June_TotalSideBusiness_Text   = entity.TotalSideBusiness;
                    break;

                // 7月
                case 7:
                    this.ViewModel.July_TotalSalary_Text         = salary.Total + GetTransportation(7);
                    this.ViewModel.July_TotalDeductedSalary_Text = salary.TotalDeduced;
                    this.ViewModel.July_TotalSideBusiness_Text   = entity.TotalSideBusiness;
                    break;

                // 8月
                case 8:
                    this.ViewModel.August_TotalSalary_Text         = salary.Total + GetTransportation(8);
                    this.ViewModel.August_TotalDeductedSalary_Text = salary.TotalDeduced;
                    this.ViewModel.August_TotalSideBusiness_Text   = entity.TotalSideBusiness;
                    break;

                // 9月
                case 9:
                    this.ViewModel.September_TotalSalary_Text         = salary.Total + GetTransportation(9);
                    this.ViewModel.September_TotalDeductedSalary_Text = salary.TotalDeduced;
                    this.ViewModel.September_TotalSideBusiness_Text   = entity.TotalSideBusiness;
                    break;

                // 10月
                case 10:
                    this.ViewModel.October_TotalSalary_Text         = salary.Total + GetTransportation(10);
                    this.ViewModel.October_TotalDeductedSalary_Text = salary.TotalDeduced;
                    this.ViewModel.October_TotalSideBusiness_Text   = entity.TotalSideBusiness;
                    break;

                // 11月
                case 11:
                    this.ViewModel.November_TotalSalary_Text         = salary.Total + GetTransportation(11);
                    this.ViewModel.November_TotalDeductedSalary_Text = salary.TotalDeduced;
                    this.ViewModel.November_TotalSideBusiness_Text   = entity.TotalSideBusiness;
                    break;

                // 12月
                case 12:
                    this.ViewModel.December_TotalSalary_Text         = salary.Total + GetTransportation(12);
                    this.ViewModel.December_TotalDeductedSalary_Text = salary.TotalDeduced;
                    this.ViewModel.December_TotalSideBusiness_Text   = entity.TotalSideBusiness;
                    break;
            }

            int GetTransportation(int month)
            {
                var entity = Allowances.Fetch(this.Header.Year_Text, month);
                return entity != null ? (int)entity.TransportationExpenses.Value : 0;
            }
        }
    }

    /// <summary> 昨年度の月収 - 1月 </summary>
    internal int PreviousTotalSalary_Jan => this.GetPreviousTotalSalary(1).Total;

    /// <summary> 昨年度の月収 - 2月 </summary>
    internal int PreviousTotalSalary_Feb => this.GetPreviousTotalSalary(2).Total;

    /// <summary> 昨年度の月収 - 3月 </summary>
    internal int PreviousTotalSalary_Mar => this.GetPreviousTotalSalary(3).Total;

    /// <summary> 昨年度の月収 - 4月 </summary>
    internal int PreviousTotalSalary_Apr => this.GetPreviousTotalSalary(4).Total;

    /// <summary> 昨年度の月収 - 5月 </summary>
    internal int PreviousTotalSalary_May => this.GetPreviousTotalSalary(5).Total;

    /// <summary> 昨年度の月収 - 6月 </summary>
    internal int PreviousTotalSalary_Jun => this.GetPreviousTotalSalary(6).Total;

    /// <summary> 昨年度の月収 - 7月 </summary>
    internal int PreviousTotalSalary_Jul => this.GetPreviousTotalSalary(7).Total;

    /// <summary> 昨年度の月収 - 8月 </summary>
    internal int PreviousTotalSalary_Aug => this.GetPreviousTotalSalary(8).Total;

    /// <summary> 昨年度の月収 - 9月 </summary>
    internal int PreviousTotalSalary_Sep => this.GetPreviousTotalSalary(9).Total;

    /// <summary> 昨年度の月収 - 10月 </summary>
    internal int PreviousTotalSalary_Oct => this.GetPreviousTotalSalary(10).Total;

    /// <summary> 昨年度の月収 - 11月 </summary>
    internal int PreviousTotalSalary_Nov => this.GetPreviousTotalSalary(11).Total;

    /// <summary> 昨年度の月収 - 12月 </summary>
    internal int PreviousTotalSalary_Dec => this.GetPreviousTotalSalary(12).Total;

    /// <summary>昨年度の月収 - 合計 </summary>
    internal int PreviousTotalSalary_Sum
        => this.PreviousTotalSalary_Jan
         + this.PreviousTotalSalary_Feb
         + this.PreviousTotalSalary_Mar
         + this.PreviousTotalSalary_Apr
         + this.PreviousTotalSalary_May
         + this.PreviousTotalSalary_Jun
         + this.PreviousTotalSalary_Jul
         + this.PreviousTotalSalary_Aug
         + this.PreviousTotalSalary_Sep
         + this.PreviousTotalSalary_Oct
         + this.PreviousTotalSalary_Nov
         + this.PreviousTotalSalary_Dec;

    /// <summary>
    /// 昨年度の月収と差引額を取得する
    /// </summary>
    /// <param name="month">月</param>
    /// <returns>(月収, 手取り)</returns>
    private (int Total, int Deduced) GetPreviousTotalSalary(int month)
    {
        var entities = AnnualCharts.Fetch(this.Header.Year_Text - 1);

        var entity = entities.Where(x => x.YearMonth.Month == month).FirstOrDefault();

        return this.RecalcMontlyIncome(entity);
    }

    /// <summary> 昨年度の控除額 - 1月 </summary>
    internal int PreviousTotalDeducedSalary_Jan => this.GetPreviousTotalDeducedSalary(1);

    /// <summary> 昨年度の控除額 - 2月 </summary>
    internal int PreviousTotalDeducedSalary_Feb => this.GetPreviousTotalDeducedSalary(2);

    /// <summary> 昨年度の控除額 - 3月 </summary>
    internal int PreviousTotalDeducedSalary_Mar => this.GetPreviousTotalDeducedSalary(3);

    /// <summary> 昨年度の控除額 - 4月 </summary>
    internal int PreviousTotalDeducedSalary_Apr => this.GetPreviousTotalDeducedSalary(4);

    /// <summary> 昨年度の控除額 - 5月 </summary>
    internal int PreviousTotalDeducedSalary_May => this.GetPreviousTotalDeducedSalary(5);

    /// <summary> 昨年度の控除額 - 6月 </summary>
    internal int PreviousTotalDeducedSalary_Jun => this.GetPreviousTotalDeducedSalary(6);

    /// <summary> 昨年度の控除額 - 7月 </summary>
    internal int PreviousTotalDeducedSalary_Jul => this.GetPreviousTotalDeducedSalary(7);

    /// <summary> 昨年度の控除額 - 8月 </summary>
    internal int PreviousTotalDeducedSalary_Aug => this.GetPreviousTotalDeducedSalary(8);

    /// <summary> 昨年度の控除額 - 9月 </summary>
    internal int PreviousTotalDeducedSalary_Sep => this.GetPreviousTotalDeducedSalary(9);

    /// <summary> 昨年度の控除額 - 10月 </summary>
    internal int PreviousTotalDeducedSalary_Oct => this.GetPreviousTotalDeducedSalary(10);

    /// <summary> 昨年度の控除額 - 11月 </summary>
    internal int PreviousTotalDeducedSalary_Nov => this.GetPreviousTotalDeducedSalary(11);

    /// <summary> 昨年度の控除額 - 12月 </summary>
    internal int PreviousTotalDeducedSalary_Dec => this.GetPreviousTotalDeducedSalary(12);

    /// <summary>昨年度の控除額 - 合計 </summary>
    internal int PreviousTotalDeducedSalary_Sum
        => this.PreviousTotalDeducedSalary_Jan
         + this.PreviousTotalDeducedSalary_Feb
         + this.PreviousTotalDeducedSalary_Mar
         + this.PreviousTotalDeducedSalary_Apr
         + this.PreviousTotalDeducedSalary_May
         + this.PreviousTotalDeducedSalary_Jun
         + this.PreviousTotalDeducedSalary_Jul
         + this.PreviousTotalDeducedSalary_Aug
         + this.PreviousTotalDeducedSalary_Sep
         + this.PreviousTotalDeducedSalary_Oct
         + this.PreviousTotalDeducedSalary_Nov
         + this.PreviousTotalDeducedSalary_Dec;

    /// <summary>
    /// 昨年度の差引額を取得する
    /// </summary>
    /// <param name="month">月</param>
    /// <returns>差引額</returns>
    private int GetPreviousTotalDeducedSalary(int month)
    {
        var entities = AnnualCharts.Fetch(this.Header.Year_Text - 1);

        var entity = entities.Where(x => x.YearMonth.Month == month).FirstOrDefault();

        return entity.TotalDeducetedSalary;
    }

    /// <summary> 昨年度の副業額 - 1月 </summary>
    internal int PreviousSideBusiness_Jan => this.GetPreviousSideBusiness(1);

    /// <summary> 昨年度の副業額 - 2月 </summary>
    internal int PreviousSideBusiness_Feb => this.GetPreviousSideBusiness(2);

    /// <summary> 昨年度の副業額 - 3月 </summary>
    internal int PreviousSideBusiness_Mar => this.GetPreviousSideBusiness(3);

    /// <summary> 昨年度の副業額 - 4月 </summary>
    internal int PreviousSideBusiness_Apr => this.GetPreviousSideBusiness(4);

    /// <summary> 昨年度の副業額 - 5月 </summary>
    internal int PreviousSideBusiness_May => this.GetPreviousSideBusiness(5);

    /// <summary> 昨年度の副業額 - 6月 </summary>
    internal int PreviousSideBusiness_Jun => this.GetPreviousSideBusiness(6);

    /// <summary> 昨年度の副業額 - 7月 </summary>
    internal int PreviousSideBusiness_Jul => this.GetPreviousSideBusiness(7);

    /// <summary> 昨年度の副業額 - 8月 </summary>
    internal int PreviousSideBusiness_Aug => this.GetPreviousSideBusiness(8);

    /// <summary> 昨年度の副業額 - 9月 </summary>
    internal int PreviousSideBusiness_Sep => this.GetPreviousSideBusiness(9);

    /// <summary> 昨年度の副業額 - 10月 </summary>
    internal int PreviousSideBusiness_Oct => this.GetPreviousSideBusiness(10);

    /// <summary> 昨年度の副業額 - 11月 </summary>
    internal int PreviousSideBusiness_Nov => this.GetPreviousSideBusiness(11);

    /// <summary> 昨年度の副業額 - 12月 </summary>
    internal int PreviousSideBusiness_Dec => this.GetPreviousSideBusiness(12);

    /// <summary>昨年度の副業額 - 合計 </summary>
    internal int PreviousSideBusiness_Sum
        => this.PreviousSideBusiness_Jan
         + this.PreviousSideBusiness_Feb
         + this.PreviousSideBusiness_Mar
         + this.PreviousSideBusiness_Apr
         + this.PreviousSideBusiness_May
         + this.PreviousSideBusiness_Jun
         + this.PreviousSideBusiness_Jul
         + this.PreviousSideBusiness_Aug
         + this.PreviousSideBusiness_Sep
         + this.PreviousSideBusiness_Oct
         + this.PreviousSideBusiness_Nov
         + this.PreviousSideBusiness_Dec;

    /// <summary>
    /// 昨年度の副業額を取得する
    /// </summary>
    /// <param name="month">月</param>
    /// <returns>副業額</returns>
    private int GetPreviousSideBusiness(int month)
    {
        var entities = AnnualCharts.Fetch(this.Header.Year_Text - 1);

        var entity = entities.Where(x => x.YearMonth.Month == month).FirstOrDefault();

        return entity.TotalSideBusiness;
    }

    /// <summary>
    /// 月収計算
    /// </summary>
    /// <param name="annualChart">月収一覧</param>
    /// <returns>(月収, 手取り)</returns>
    /// <remarks>
    /// 交通費は非課税なので、月収には含めない
    /// </remarks>
    private (int Total, int TotalDeduced) RecalcMontlyIncome(AnnualChartEntity annualChart)
    {
        if (annualChart is null)
        {
            return (0, 0);
        }

        var entity = Allowances.Fetch(this.Header.Year_Text, annualChart.YearMonth.Month);

        if (entity is null)
        {
            // 未記載
            return (0, 0);
        }

        var salary = annualChart.TotalSalary - entity.TransportationExpenses.Value;
        var deduced = annualChart.TotalDeducetedSalary - entity.TransportationExpenses.Value;

        return ((int)salary, (int)deduced);
    }



    /// <summary>
    /// 再計算
    /// </summary>
    /// <remarks>
    /// 給与明細の保存後に再計算する。
    /// </remarks>
    internal void Recalculate()
    {
        // 支給額計
        this.ViewModel.TotalSalary_Sum_Text = this.ViewModel.January_TotalSalary_Text
                                            + this.ViewModel.Feburary_TotalSalary_Text
                                            + this.ViewModel.March_TotalSalary_Text
                                            + this.ViewModel.April_TotalSalary_Text
                                            + this.ViewModel.May_TotalSalary_Text
                                            + this.ViewModel.June_TotalSalary_Text
                                            + this.ViewModel.July_TotalSalary_Text
                                            + this.ViewModel.August_TotalSalary_Text
                                            + this.ViewModel.September_TotalSalary_Text
                                            + this.ViewModel.October_TotalSalary_Text
                                            + this.ViewModel.November_TotalSalary_Text
                                            + this.ViewModel.December_TotalSalary_Text;

        // 差引支給額
        this.ViewModel.TotalDeductedSalary_Sum_Text = this.ViewModel.January_TotalDeductedSalary_Text
                                                    + this.ViewModel.Feburary_TotalDeductedSalary_Text
                                                    + this.ViewModel.March_TotalDeductedSalary_Text
                                                    + this.ViewModel.April_TotalDeductedSalary_Text
                                                    + this.ViewModel.May_TotalDeductedSalary_Text
                                                    + this.ViewModel.June_TotalDeductedSalary_Text
                                                    + this.ViewModel.July_TotalDeductedSalary_Text
                                                    + this.ViewModel.August_TotalDeductedSalary_Text
                                                    + this.ViewModel.September_TotalDeductedSalary_Text
                                                    + this.ViewModel.October_TotalDeductedSalary_Text
                                                    + this.ViewModel.November_TotalDeductedSalary_Text
                                                    + this.ViewModel.December_TotalDeductedSalary_Text;

        // 副業
        this.ViewModel.TotalSideBusiness_Sum_Text = this.ViewModel.January_TotalSideBusiness_Text
                                                  + this.ViewModel.Feburary_TotalSideBusiness_Text
                                                  + this.ViewModel.March_TotalSideBusiness_Text
                                                  + this.ViewModel.April_TotalSideBusiness_Text
                                                  + this.ViewModel.May_TotalSideBusiness_Text
                                                  + this.ViewModel.June_TotalSideBusiness_Text
                                                  + this.ViewModel.July_TotalSideBusiness_Text
                                                  + this.ViewModel.August_TotalSideBusiness_Text
                                                  + this.ViewModel.September_TotalSideBusiness_Text
                                                  + this.ViewModel.October_TotalSideBusiness_Text
                                                  + this.ViewModel.November_TotalSideBusiness_Text
                                                  + this.ViewModel.December_TotalSideBusiness_Text;
    }

    /// <summary>
    /// Clear
    /// </summary>
    /// <remarks>
    /// 各項目を初期化する。
    /// </remarks>
    public void Clear()
    {
        // 支給額計
        this.ViewModel.January_TotalSalary_Text = default(int);
        this.ViewModel.Feburary_TotalSalary_Text = default(int);
        this.ViewModel.March_TotalSalary_Text = default(int);
        this.ViewModel.April_TotalSalary_Text = default(int);
        this.ViewModel.May_TotalSalary_Text = default(int);
        this.ViewModel.June_TotalSalary_Text = default(int);
        this.ViewModel.July_TotalSalary_Text = default(int);
        this.ViewModel.August_TotalSalary_Text = default(int);
        this.ViewModel.September_TotalSalary_Text = default(int);
        this.ViewModel.October_TotalSalary_Text = default(int);
        this.ViewModel.November_TotalSalary_Text = default(int);
        this.ViewModel.December_TotalSalary_Text = default(int);

        // 差引支給額
        this.ViewModel.January_TotalDeductedSalary_Text = default(int);
        this.ViewModel.Feburary_TotalDeductedSalary_Text = default(int);
        this.ViewModel.March_TotalDeductedSalary_Text = default(int);
        this.ViewModel.April_TotalDeductedSalary_Text = default(int);
        this.ViewModel.May_TotalDeductedSalary_Text = default(int);
        this.ViewModel.June_TotalDeductedSalary_Text = default(int);
        this.ViewModel.July_TotalDeductedSalary_Text = default(int);
        this.ViewModel.August_TotalDeductedSalary_Text = default(int);
        this.ViewModel.September_TotalDeductedSalary_Text = default(int);
        this.ViewModel.October_TotalDeductedSalary_Text = default(int);
        this.ViewModel.November_TotalDeductedSalary_Text = default(int);
        this.ViewModel.December_TotalDeductedSalary_Text = default(int);

        // 副業
        this.ViewModel.January_TotalSideBusiness_Text = default(int);
        this.ViewModel.Feburary_TotalSideBusiness_Text = default(int);
        this.ViewModel.March_TotalSideBusiness_Text = default(int);
        this.ViewModel.April_TotalSideBusiness_Text = default(int);
        this.ViewModel.May_TotalSideBusiness_Text = default(int);
        this.ViewModel.June_TotalSideBusiness_Text = default(int);
        this.ViewModel.July_TotalSideBusiness_Text = default(int);
        this.ViewModel.August_TotalSideBusiness_Text = default(int);
        this.ViewModel.September_TotalSideBusiness_Text = default(int);
        this.ViewModel.October_TotalSideBusiness_Text = default(int);
        this.ViewModel.November_TotalSideBusiness_Text = default(int);
        this.ViewModel.December_TotalSideBusiness_Text = default(int);
    }
}
