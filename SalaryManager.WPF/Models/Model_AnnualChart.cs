using System.Linq;
using SalaryManager.Domain.Entities;
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
        internal void Reload() => this.Initialize(this.Header.Year);

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="fetchingYear">取得する年</param>
        internal void Initialize(int fetchingYear)
        {
            // 対象日付
            this.ViewModel.TargetDate = new YearValue(this.Header.Year, this.Header.Month).YearWithJapaneseCalendar;

            var chart = new AnnualChartSQLite();

            var annualCharts = chart.GetEntities()
                                    .Where(x => x.YearMonth.Year == fetchingYear)
                                    .ToList().AsReadOnly();

            this.Clear();

            foreach (AnnualChartEntity annualChart in annualCharts)
            {
                switch (annualChart.YearMonth.Month)
                {
                    // 1月
                    case 1:
                        this.ViewModel.TotalSalary_January         = annualChart.TotalSalary;
                        this.ViewModel.TotalDeductedSalary_January = annualChart.TotalDeducetedSalary;
                        break;

                    // 2月
                    case 2:
                        this.ViewModel.TotalSalary_Feburary         = annualChart.TotalSalary;
                        this.ViewModel.TotalDeductedSalary_Feburary = annualChart.TotalDeducetedSalary;
                        break;

                    // 3月
                    case 3:
                        this.ViewModel.TotalSalary_March         = annualChart.TotalSalary;
                        this.ViewModel.TotalDeductedSalary_March = annualChart.TotalDeducetedSalary;
                        break;

                    // 4月
                    case 4:
                        this.ViewModel.TotalSalary_April         = annualChart.TotalSalary;
                        this.ViewModel.TotalDeductedSalary_April = annualChart.TotalDeducetedSalary;
                        break;

                    // 5月
                    case 5:
                        this.ViewModel.TotalSalary_May         = annualChart.TotalSalary;
                        this.ViewModel.TotalDeductedSalary_May = annualChart.TotalDeducetedSalary;
                        break;

                    // 6月
                    case 6:
                        this.ViewModel.TotalSalary_June         = annualChart.TotalSalary;
                        this.ViewModel.TotalDeductedSalary_June = annualChart.TotalDeducetedSalary;
                        break;

                    // 7月
                    case 7:
                        this.ViewModel.TotalSalary_July         = annualChart.TotalSalary;
                        this.ViewModel.TotalDeductedSalary_July = annualChart.TotalDeducetedSalary;
                        break;

                    // 8月
                    case 8:
                        this.ViewModel.TotalSalary_August         = annualChart.TotalSalary;
                        this.ViewModel.TotalDeductedSalary_August = annualChart.TotalDeducetedSalary;
                        break;

                    // 9月
                    case 9:
                        this.ViewModel.TotalSalary_September         = annualChart.TotalSalary;
                        this.ViewModel.TotalDeductedSalary_September = annualChart.TotalDeducetedSalary;
                        break;

                    // 10月
                    case 10:
                        this.ViewModel.TotalSalary_October         = annualChart.TotalSalary;
                        this.ViewModel.TotalDeductedSalary_October = annualChart.TotalDeducetedSalary;
                        break;

                    // 11月
                    case 11:
                        this.ViewModel.TotalSalary_November         = annualChart.TotalSalary;
                        this.ViewModel.TotalDeductedSalary_November = annualChart.TotalDeducetedSalary;
                        break;

                    // 12月
                    case 12:
                        this.ViewModel.TotalSalary_December         = annualChart.TotalSalary;
                        this.ViewModel.TotalDeductedSalary_December = annualChart.TotalDeducetedSalary;
                        break;
                }
            }
        }

        /// <summary>
        /// Clear
        /// </summary>
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
        }
    }
}
