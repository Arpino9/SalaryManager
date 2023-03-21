using SalaryManager.Domain.Entities;
using SalaryManager.Domain.StaticValues;
using SalaryManager.Domain.ValueObjects;
using SalaryManager.Infrastructure.SQLite;
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

        /// <summary> ViewModel - ヘッダ </summary>
        internal ViewModel_Header Header { get; set; }

        /// <summary> ViewModel - 月収一覧 </summary>
        internal ViewModel_AnnualChart ViewModel { get; set; }

        /// <summary>
        /// リロード
        /// </summary>
        internal void Reload() => this.Initialize(this.Header.Year_Value);

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="fetchingYear">取得する年</param>
        /// <remarks>
        /// 画面起動時に、項目を初期化する。
        /// </remarks>
        internal void Initialize(int fetchingYear)
        {
            Options_General.Create();
            this.ViewModel.Window_Background = Options_General.FetchBackgroundColorBrush();

            // 対象日付
            if (this.Header.Year_Value.ToString().Length != 4)
            {
                return;
            }

            var yearValue = new YearValue(this.Header.Year_Value, this.Header.Month_Value);
            this.ViewModel.TargetDate = yearValue?.YearWithJapaneseCalendar;

            if (this.ViewModel.TargetDate is null)
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
                        this.ViewModel.TotalSalary_January         = annualChart.TotalSalary;
                        this.ViewModel.TotalDeductedSalary_January = annualChart.TotalDeducetedSalary;
                        this.ViewModel.TotalSideBusiness_January   = annualChart.TotalSideBusiness;
                        break;

                    // 2月
                    case 2:
                        this.ViewModel.TotalSalary_Feburary         = annualChart.TotalSalary;
                        this.ViewModel.TotalDeductedSalary_Feburary = annualChart.TotalDeducetedSalary;
                        this.ViewModel.TotalSideBusiness_Feburary   = annualChart.TotalSideBusiness;
                        break;

                    // 3月
                    case 3:
                        this.ViewModel.TotalSalary_March         = annualChart.TotalSalary;
                        this.ViewModel.TotalDeductedSalary_March = annualChart.TotalDeducetedSalary;
                        this.ViewModel.TotalSideBusiness_March   = annualChart.TotalSideBusiness;
                        break;

                    // 4月
                    case 4:
                        this.ViewModel.TotalSalary_April         = annualChart.TotalSalary;
                        this.ViewModel.TotalDeductedSalary_April = annualChart.TotalDeducetedSalary;
                        this.ViewModel.TotalSideBusiness_April   = annualChart.TotalSideBusiness;
                        break;

                    // 5月
                    case 5:
                        this.ViewModel.TotalSalary_May         = annualChart.TotalSalary;
                        this.ViewModel.TotalDeductedSalary_May = annualChart.TotalDeducetedSalary;
                        this.ViewModel.TotalSideBusiness_May   = annualChart.TotalSideBusiness;
                        break;

                    // 6月
                    case 6:
                        this.ViewModel.TotalSalary_June         = annualChart.TotalSalary;
                        this.ViewModel.TotalDeductedSalary_June = annualChart.TotalDeducetedSalary;
                        this.ViewModel.TotalSideBusiness_June   = annualChart.TotalSideBusiness;
                        break;

                    // 7月
                    case 7:
                        this.ViewModel.TotalSalary_July         = annualChart.TotalSalary;
                        this.ViewModel.TotalDeductedSalary_July = annualChart.TotalDeducetedSalary;
                        this.ViewModel.TotalSideBusiness_July   = annualChart.TotalSideBusiness;
                        break;

                    // 8月
                    case 8:
                        this.ViewModel.TotalSalary_August         = annualChart.TotalSalary;
                        this.ViewModel.TotalDeductedSalary_August = annualChart.TotalDeducetedSalary;
                        this.ViewModel.TotalSideBusiness_August   = annualChart.TotalSideBusiness;
                        break;

                    // 9月
                    case 9:
                        this.ViewModel.TotalSalary_September         = annualChart.TotalSalary;
                        this.ViewModel.TotalDeductedSalary_September = annualChart.TotalDeducetedSalary;
                        this.ViewModel.TotalSideBusiness_September   = annualChart.TotalSideBusiness;
                        break;

                    // 10月
                    case 10:
                        this.ViewModel.TotalSalary_October         = annualChart.TotalSalary;
                        this.ViewModel.TotalDeductedSalary_October = annualChart.TotalDeducetedSalary;
                        this.ViewModel.TotalSideBusiness_October   = annualChart.TotalSideBusiness;
                        break;

                    // 11月
                    case 11:
                        this.ViewModel.TotalSalary_November         = annualChart.TotalSalary;
                        this.ViewModel.TotalDeductedSalary_November = annualChart.TotalDeducetedSalary;
                        this.ViewModel.TotalSideBusiness_November   = annualChart.TotalSideBusiness;
                        break;

                    // 12月
                    case 12:
                        this.ViewModel.TotalSalary_December         = annualChart.TotalSalary;
                        this.ViewModel.TotalDeductedSalary_December = annualChart.TotalDeducetedSalary;
                        this.ViewModel.TotalSideBusiness_December   = annualChart.TotalSideBusiness;
                        break;
                }
            }

            this.Recalculate();
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
            this.ViewModel.TotalSalary_Sum = this.ViewModel.TotalSalary_January
                                           + this.ViewModel.TotalSalary_Feburary
                                           + this.ViewModel.TotalSalary_March
                                           + this.ViewModel.TotalSalary_April
                                           + this.ViewModel.TotalSalary_May
                                           + this.ViewModel.TotalSalary_June
                                           + this.ViewModel.TotalSalary_July
                                           + this.ViewModel.TotalSalary_August
                                           + this.ViewModel.TotalSalary_September
                                           + this.ViewModel.TotalSalary_October
                                           + this.ViewModel.TotalSalary_November
                                           + this.ViewModel.TotalSalary_December;

            // 差引支給額
            this.ViewModel.TotalDeductedSalary_Sum = this.ViewModel.TotalDeductedSalary_January
                                                   + this.ViewModel.TotalDeductedSalary_Feburary
                                                   + this.ViewModel.TotalDeductedSalary_March
                                                   + this.ViewModel.TotalDeductedSalary_April
                                                   + this.ViewModel.TotalDeductedSalary_May
                                                   + this.ViewModel.TotalDeductedSalary_June
                                                   + this.ViewModel.TotalDeductedSalary_July
                                                   + this.ViewModel.TotalDeductedSalary_August
                                                   + this.ViewModel.TotalDeductedSalary_September
                                                   + this.ViewModel.TotalDeductedSalary_October
                                                   + this.ViewModel.TotalDeductedSalary_November
                                                   + this.ViewModel.TotalDeductedSalary_December;

            // 副業
            this.ViewModel.TotalSideBusiness_Sum = this.ViewModel.TotalSideBusiness_January
                                                 + this.ViewModel.TotalSideBusiness_Feburary
                                                 + this.ViewModel.TotalSideBusiness_March
                                                 + this.ViewModel.TotalSideBusiness_April
                                                 + this.ViewModel.TotalSideBusiness_May
                                                 + this.ViewModel.TotalSideBusiness_June
                                                 + this.ViewModel.TotalSideBusiness_July
                                                 + this.ViewModel.TotalSideBusiness_August
                                                 + this.ViewModel.TotalSideBusiness_September
                                                 + this.ViewModel.TotalSideBusiness_October
                                                 + this.ViewModel.TotalSideBusiness_November
                                                 + this.ViewModel.TotalSideBusiness_December;
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
            this.ViewModel.TotalSalary_January   = default(int);
            this.ViewModel.TotalSalary_Feburary  = default(int);
            this.ViewModel.TotalSalary_March     = default(int);
            this.ViewModel.TotalSalary_April     = default(int);
            this.ViewModel.TotalSalary_May       = default(int);
            this.ViewModel.TotalSalary_June      = default(int);
            this.ViewModel.TotalSalary_July      = default(int);
            this.ViewModel.TotalSalary_August    = default(int);
            this.ViewModel.TotalSalary_September = default(int);
            this.ViewModel.TotalSalary_October   = default(int);
            this.ViewModel.TotalSalary_November  = default(int);
            this.ViewModel.TotalSalary_December  = default(int);

            // 差引支給額
            this.ViewModel.TotalDeductedSalary_January   = default(int);
            this.ViewModel.TotalDeductedSalary_Feburary  = default(int);
            this.ViewModel.TotalDeductedSalary_March     = default(int);
            this.ViewModel.TotalDeductedSalary_April     = default(int);
            this.ViewModel.TotalDeductedSalary_May       = default(int);
            this.ViewModel.TotalDeductedSalary_June      = default(int);
            this.ViewModel.TotalDeductedSalary_July      = default(int);
            this.ViewModel.TotalDeductedSalary_August    = default(int);
            this.ViewModel.TotalDeductedSalary_September = default(int);
            this.ViewModel.TotalDeductedSalary_October   = default(int);
            this.ViewModel.TotalDeductedSalary_November  = default(int);
            this.ViewModel.TotalDeductedSalary_December  = default(int);

            // 副業
            this.ViewModel.TotalSideBusiness_January   = default(int);
            this.ViewModel.TotalSideBusiness_Feburary  = default(int);
            this.ViewModel.TotalSideBusiness_March     = default(int);
            this.ViewModel.TotalSideBusiness_April     = default(int);
            this.ViewModel.TotalSideBusiness_May       = default(int);
            this.ViewModel.TotalSideBusiness_June      = default(int);
            this.ViewModel.TotalSideBusiness_July      = default(int);
            this.ViewModel.TotalSideBusiness_August    = default(int);
            this.ViewModel.TotalSideBusiness_September = default(int);
            this.ViewModel.TotalSideBusiness_October   = default(int);
            this.ViewModel.TotalSideBusiness_November  = default(int);
            this.ViewModel.TotalSideBusiness_December  = default(int);
        }
    }
}
