using SalaryManager.Domain.Entities;
using SalaryManager.Domain.StaticValues;
using SalaryManager.Domain.ValueObjects;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.Infrastructure.XML;
using SalaryManager.WPF.ViewModels;

namespace SalaryManager.WPF.Models
{
    /// <summary>
    /// Model - 月収一覧
    /// </summary>
    public class Model_AnnualChart
    {
        #region Get Instance

        private static Model_AnnualChart model = null;

        public static Model_AnnualChart GetInstance()
        {
            if (model == null)
            {
                model = new Model_AnnualChart();
            }

            return model;
        }

        #endregion

        public Model_AnnualChart()
        {
            
        }

        /// <summary> ViewModel - ヘッダ </summary>
        internal ViewModel_Header Header { get; set; }

        /// <summary> ViewModel - 月収一覧 </summary>
        internal ViewModel_AnnualChart ViewModel { get; set; }

        /// <summary>
        /// リロード
        /// </summary>
        internal void Reload() => this.Initialize(this.Header.Year_Text.Value);

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="fetchingYear">取得する年</param>
        /// <remarks>
        /// 画面起動時に、項目を初期化する。
        /// </remarks>
        internal void Initialize(int fetchingYear)
        {
            this.Window_Activated();

            // 対象日付
            if (this.Header.Year_Text.Value.ToString().Length != 4)
            {
                return;
            }

            var yearValue = new DateValue(this.Header.Year_Text.Value, this.Header.Month_Text.Value);
            this.ViewModel.TargetDate_Content.Value = yearValue?.YearWithJapaneseCalendar;

            if (this.ViewModel.TargetDate_Content is null)
            {
                return;
            }

            this.Clear();

            AnnualCharts.Create(new AnnualChartSQLite());

            foreach (AnnualChartEntity annualChart in AnnualCharts.Fetch(fetchingYear))
            {
                switch (annualChart.YearMonth.Month)
                {
                    // 1月
                    case 1:
                        this.ViewModel.January_TotalSalary_Text.Value         = annualChart.TotalSalary;
                        this.ViewModel.January_TotalDeductedSalary_Text.Value = annualChart.TotalDeducetedSalary;
                        this.ViewModel.January_TotalSideBusiness_Text.Value   = annualChart.TotalSideBusiness;
                        break;

                    // 2月
                    case 2:
                        this.ViewModel.Feburary_TotalSalary_Text.Value         = annualChart.TotalSalary;
                        this.ViewModel.Feburary_TotalDeductedSalary_Text.Value = annualChart.TotalDeducetedSalary;
                        this.ViewModel.Feburary_TotalSideBusiness_Text.Value   = annualChart.TotalSideBusiness;
                        break;

                    // 3月
                    case 3:
                        this.ViewModel.March_TotalSalary_Text.Value         = annualChart.TotalSalary;
                        this.ViewModel.March_TotalDeductedSalary_Text.Value = annualChart.TotalDeducetedSalary;
                        this.ViewModel.March_TotalSideBusiness_Text.Value   = annualChart.TotalSideBusiness;
                        break;

                    // 4月
                    case 4:
                        this.ViewModel.April_TotalSalary_Text.Value         = annualChart.TotalSalary;
                        this.ViewModel.April_TotalDeductedSalary_Text.Value = annualChart.TotalDeducetedSalary;
                        this.ViewModel.April_TotalSideBusiness_Text.Value   = annualChart.TotalSideBusiness;
                        break;

                    // 5月
                    case 5:
                        this.ViewModel.May_TotalSalary_Text.Value         = annualChart.TotalSalary;
                        this.ViewModel.May_TotalDeductedSalary_Text.Value = annualChart.TotalDeducetedSalary;
                        this.ViewModel.May_TotalSideBusiness_Text.Value   = annualChart.TotalSideBusiness;
                        break;

                    // 6月
                    case 6:
                        this.ViewModel.June_TotalSalary_Text.Value         = annualChart.TotalSalary;
                        this.ViewModel.June_TotalDeductedSalary_Text.Value = annualChart.TotalDeducetedSalary;
                        this.ViewModel.June_TotalSideBusiness_Text.Value   = annualChart.TotalSideBusiness;
                        break;

                    // 7月
                    case 7:
                        this.ViewModel.July_TotalSalary_Text.Value         = annualChart.TotalSalary;
                        this.ViewModel.July_TotalDeductedSalary_Text.Value = annualChart.TotalDeducetedSalary;
                        this.ViewModel.July_TotalSideBusiness_Text.Value   = annualChart.TotalSideBusiness;
                        break;

                    // 8月
                    case 8:
                        this.ViewModel.August_TotalSalary_Text.Value         = annualChart.TotalSalary;
                        this.ViewModel.August_TotalDeductedSalary_Text.Value = annualChart.TotalDeducetedSalary;
                        this.ViewModel.August_TotalSideBusiness_Text.Value   = annualChart.TotalSideBusiness;
                        break;

                    // 9月
                    case 9:
                        this.ViewModel.September_TotalSalary_Text.Value         = annualChart.TotalSalary;
                        this.ViewModel.September_TotalDeductedSalary_Text.Value = annualChart.TotalDeducetedSalary;
                        this.ViewModel.September_TotalSideBusiness_Text.Value   = annualChart.TotalSideBusiness;
                        break;

                    // 10月
                    case 10:
                        this.ViewModel.October_TotalSalary_Text.Value         = annualChart.TotalSalary;
                        this.ViewModel.October_TotalDeductedSalary_Text.Value = annualChart.TotalDeducetedSalary;
                        this.ViewModel.October_TotalSideBusiness_Text.Value   = annualChart.TotalSideBusiness;
                        break;

                    // 11月
                    case 11:
                        this.ViewModel.November_TotalSalary_Text.Value         = annualChart.TotalSalary;
                        this.ViewModel.November_TotalDeductedSalary_Text.Value = annualChart.TotalDeducetedSalary;
                        this.ViewModel.November_TotalSideBusiness_Text.Value   = annualChart.TotalSideBusiness;
                        break;

                    // 12月
                    case 12:
                        this.ViewModel.December_TotalSalary_Text.Value         = annualChart.TotalSalary;
                        this.ViewModel.December_TotalDeductedSalary_Text.Value = annualChart.TotalDeducetedSalary;
                        this.ViewModel.December_TotalSideBusiness_Text.Value   = annualChart.TotalSideBusiness;
                        break;
                }
            }

            this.Recalculate();
        }

        /// <summary>
        /// 画面起動時の処理
        /// </summary>
        internal void Window_Activated()
        {
            if (this.ViewModel is null) return;

            this.ViewModel.Window_Background.Value = XMLLoader.FetchBackgroundColorBrush();
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
            this.ViewModel.TotalSalary_Sum_Text.Value = this.ViewModel.January_TotalSalary_Text.Value
                                                      + this.ViewModel.Feburary_TotalSalary_Text.Value
                                                      + this.ViewModel.March_TotalSalary_Text.Value
                                                      + this.ViewModel.April_TotalSalary_Text.Value
                                                      + this.ViewModel.May_TotalSalary_Text.Value
                                                      + this.ViewModel.June_TotalSalary_Text.Value
                                                      + this.ViewModel.July_TotalSalary_Text.Value
                                                      + this.ViewModel.August_TotalSalary_Text.Value
                                                      + this.ViewModel.September_TotalSalary_Text.Value
                                                      + this.ViewModel.October_TotalSalary_Text.Value
                                                      + this.ViewModel.November_TotalSalary_Text.Value
                                                      + this.ViewModel.December_TotalSalary_Text.Value;

            // 差引支給額
            this.ViewModel.TotalDeductedSalary_Sum_Text.Value = this.ViewModel.January_TotalDeductedSalary_Text.Value
                                                              + this.ViewModel.Feburary_TotalDeductedSalary_Text.Value
                                                              + this.ViewModel.March_TotalDeductedSalary_Text.Value
                                                              + this.ViewModel.April_TotalDeductedSalary_Text.Value
                                                              + this.ViewModel.May_TotalDeductedSalary_Text.Value
                                                              + this.ViewModel.June_TotalDeductedSalary_Text.Value
                                                              + this.ViewModel.July_TotalDeductedSalary_Text.Value
                                                              + this.ViewModel.August_TotalDeductedSalary_Text.Value
                                                              + this.ViewModel.September_TotalDeductedSalary_Text.Value
                                                              + this.ViewModel.October_TotalDeductedSalary_Text.Value
                                                              + this.ViewModel.November_TotalDeductedSalary_Text.Value
                                                              + this.ViewModel.December_TotalDeductedSalary_Text.Value;

            // 副業
            this.ViewModel.TotalSideBusiness_Sum_Text.Value = this.ViewModel.January_TotalSideBusiness_Text.Value
                                                            + this.ViewModel.Feburary_TotalSideBusiness_Text.Value
                                                            + this.ViewModel.March_TotalSideBusiness_Text.Value
                                                            + this.ViewModel.April_TotalSideBusiness_Text.Value
                                                            + this.ViewModel.May_TotalSideBusiness_Text.Value
                                                            + this.ViewModel.June_TotalSideBusiness_Text.Value
                                                            + this.ViewModel.July_TotalSideBusiness_Text.Value
                                                            + this.ViewModel.August_TotalSideBusiness_Text.Value
                                                            + this.ViewModel.September_TotalSideBusiness_Text.Value
                                                            + this.ViewModel.October_TotalSideBusiness_Text.Value
                                                            + this.ViewModel.November_TotalSideBusiness_Text.Value
                                                            + this.ViewModel.December_TotalSideBusiness_Text.Value;
        }

        /// <summary>
        /// Clear
        /// </summary>
        /// <remarks>
        /// 各項目を初期化する。
        /// </remarks>
        private void Clear()
        {
            // 支給額計
            this.ViewModel.January_TotalSalary_Text.Value   = default(int);
            this.ViewModel.Feburary_TotalSalary_Text.Value  = default(int);
            this.ViewModel.March_TotalSalary_Text.Value     = default(int);
            this.ViewModel.April_TotalSalary_Text.Value     = default(int);
            this.ViewModel.May_TotalSalary_Text.Value       = default(int);
            this.ViewModel.June_TotalSalary_Text.Value      = default(int);
            this.ViewModel.July_TotalSalary_Text.Value      = default(int);
            this.ViewModel.August_TotalSalary_Text.Value    = default(int);
            this.ViewModel.September_TotalSalary_Text.Value = default(int);
            this.ViewModel.October_TotalSalary_Text.Value   = default(int);
            this.ViewModel.November_TotalSalary_Text.Value  = default(int);
            this.ViewModel.December_TotalSalary_Text.Value  = default(int);

            // 差引支給額
            this.ViewModel.January_TotalDeductedSalary_Text.Value   = default(int);
            this.ViewModel.Feburary_TotalDeductedSalary_Text.Value  = default(int);
            this.ViewModel.March_TotalDeductedSalary_Text.Value     = default(int);
            this.ViewModel.April_TotalDeductedSalary_Text.Value     = default(int);
            this.ViewModel.May_TotalDeductedSalary_Text.Value       = default(int);
            this.ViewModel.June_TotalDeductedSalary_Text.Value      = default(int);
            this.ViewModel.July_TotalDeductedSalary_Text.Value      = default(int);
            this.ViewModel.August_TotalDeductedSalary_Text.Value    = default(int);
            this.ViewModel.September_TotalDeductedSalary_Text.Value = default(int);
            this.ViewModel.October_TotalDeductedSalary_Text.Value   = default(int);
            this.ViewModel.November_TotalDeductedSalary_Text.Value = default(int);
            this.ViewModel.December_TotalDeductedSalary_Text.Value  = default(int);

            // 副業
            this.ViewModel.January_TotalSideBusiness_Text.Value   = default(int);
            this.ViewModel.Feburary_TotalSideBusiness_Text.Value  = default(int);
            this.ViewModel.March_TotalSideBusiness_Text.Value     = default(int);
            this.ViewModel.April_TotalSideBusiness_Text.Value     = default(int);
            this.ViewModel.May_TotalSideBusiness_Text.Value       = default(int);
            this.ViewModel.June_TotalSideBusiness_Text.Value      = default(int);
            this.ViewModel.July_TotalSideBusiness_Text.Value      = default(int);
            this.ViewModel.August_TotalSideBusiness_Text.Value    = default(int);
            this.ViewModel.September_TotalSalary_Text.Value       = default(int);
            this.ViewModel.October_TotalSideBusiness_Text.Value   = default(int);
            this.ViewModel.November_TotalSideBusiness_Text.Value = default(int);
            this.ViewModel.December_TotalSideBusiness_Text.Value  = default(int);
        }
    }
}
