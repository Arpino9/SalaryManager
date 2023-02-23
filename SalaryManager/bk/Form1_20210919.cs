using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace SalaryManager
{
    public partial class SalaryManager : Form
    {
        // 接続情報
        private static readonly string Server = "localhost";      // ホスト名
        private static readonly int Port = 3305;                  // ポート番号
        private static readonly string Database = "mysql";       // データベース名
        private static readonly string Uid = "root";              // ユーザ名
        private static readonly string Pwd = "ltmc4874";          // パスワード

        // 接続文字列
        private static readonly string ConnectionString = $"Server={Server}; Port={Port}; Database={Database}; Uid={Uid}; Pwd={Pwd}";

        public static Boolean isRegister = false;      // 登録フラグ
        public static Boolean isUpdate = false;        // 更新フラグ
        public static Boolean isDefault = false;       // 新規登録時のデフォルト明細フラグ

        public static Boolean isConfirmAllow = false;  // 確認ボックス表示の要否

        //　変更箇所の照合用
        public static Dictionary<int, string> mapRegisteredData = new Dictionary<int, string>();

        //　月別の支給総計額 取得用
        public static Dictionary<string, int> mapTotalSalary = new Dictionary<string, int>();

        //　月別の差引支給額 取得用
        public static Dictionary<string, int> mapTotalDeductedSalary = new Dictionary<string, int>();

        //　前年との金額比較用
        public static Dictionary<string, int> mapCompareToPreviousYear = new Dictionary<string, int>();

        #region Utilities

        /// <summary>
        /// 共通ユーティリティ
        /// </summary>
        private Utils Utils
        {
            get;
            set;
        } = new Utils();

        #endregion

        #region Keys

        /// <summary>
        /// 対象年月
        /// </summary>
        public System.DateTime TargetDate
        {
            get;
            set;
        } = new System.DateTime();

        /// <summary>
        /// 年
        /// </summary>
        public int TargetYear
        {
            get;
            set;
        }

        /// <summary>
        /// 月
        /// </summary>
        public int TargetMonth
        {
            get;
            set;
        }

        #endregion

        #region Elements

        /// <summary>
        /// 支給額
        /// </summary>
        private ItemAllowance ItemAllowance
        {
            get;
            set;
        } = new ItemAllowance();

        /// <summary>
        /// 控除額
        /// </summary>
        private ItemDeduction ItemDeduction
        {
            get;
            set;
        } = new ItemDeduction();

        /// <summary>
        /// 勤務備考
        /// </summary>
        private ItemWorkingReferences ItemWorkingReferences
        {
            get;
            set;
        } = new ItemWorkingReferences();

        /// <summary>
        /// 副業
        /// </summary>
        private ItemSideBusiness ItemSideBusiness
        {
            get;
            set;
        } = new ItemSideBusiness();

        #endregion

        public int sumTotal;                          // 年収算出用
        public int sumTotalDeducted;                  // 年間手取算出用

        public int targetMoney = 0;                   // 対象の金額
        public int targetPreviousMoney = 0;           // 対象の前年金額

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public SalaryManager()
        {
            InitializeComponent();
        }

        #endregion

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void Form1_Load(object sender, EventArgs e)
        {
            // TargetDate = Today
            this.TargetDate = DateTime.Today;

            // Year - Text
            this.txtYear.Text = this.TargetDate.ToString("yyyy");

            // Month - Text
            this.txtMonth.Text = this.TargetDate.ToString("MM");

            ClearAll();

            GetSalary();
            GetAnnualIncomey();

            this.chkConfirm.Checked = isConfirmAllow;
        }

        #region 支給額 - 基本給

        /// <summary>
        /// 基本給 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtBasicSalary_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 基本給 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtBasicSalary_Leave(object sender, EventArgs e)
        {
            ItemAllowance.BasicSalary = this.Utils.FormatPriceToInt(this.txtBasicSalary.Text);

            // 値を更新して、変更があれば色をつける
            if (this.Utils.UpdateIntPrice(this.txtBasicSalary, ItemAllowance.BasicSalary.Value, 1))
            {
                // 再計算
                CalculateSum();
            }
        }

        /// <summary>
        /// 基本給 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtBasicSalary_MouseEnter(object sender, EventArgs e)
        {
            if (ItemAllowance.TotalDeductedSalary.Value <= 0)
            {
                return;
            }

            var strTargetDate = this.txtYear.Text + "/" + this.txtMonth.Text;
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/" + this.txtMonth.Text;
            CompareWithPreviousYear("SALARY.basic_salary", strTargetDate, strTargetPreviousDate);
        }

        #endregion

        #region 支給額 - 役職手当

        /// <summary>
        /// 役職手当- KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtExecutiveAllowance_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 役職手当 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtExecutiveAllowance_Leave(object sender, EventArgs e)
        {
            ItemAllowance.ExecutiveAllowance = Utils.FormatPriceToInt(this.txtExecutiveAllowance.Text);

            // 値を更新して、変更があれば色をつける
            if (!this.Utils.UpdateIntPrice(this.txtExecutiveAllowance, ItemAllowance.ExecutiveAllowance.Value, 2))
            {
                return;
            }

            // 1以上の値なら支給総計を再計算する
            if (!this.Utils.ConvertZeroToBlank(this.txtExecutiveAllowance, ItemAllowance.ExecutiveAllowance.Value))
            {
                return;
            }

            // 再計算
            CalculateSum();
        }

        /// <summary>
        /// 基本給 - MouseEnter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtExecutiveAllowance_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/" + this.txtMonth.Text;
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/" + this.txtMonth.Text;
            CompareWithPreviousYear("SALARY.executive_allowance", strTargetDate, strTargetPreviousDate);
        }

        #endregion

        #region 支給額 - 役職手当

        /// <summary>
        /// 扶養手当 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtDependencyAllowance_KeyPress(object sender, KeyPressEventArgs e)
        {
            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 扶養手当 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtDependencyAllowance_Leave(object sender, EventArgs e)
        {
            this.ItemAllowance.DependencyAllowance = this.Utils.FormatPriceToInt(this.txtDependencyAllowance.Text);

            // 値を更新して、変更があれば色をつける
            if (!this.Utils.UpdateIntPrice(this.txtDependencyAllowance, this.ItemAllowance.DependencyAllowance.Value, 3))
            {
                return;
            }

            // 1以上の値なら支給総計を再計算する
            if (!this.Utils.ConvertZeroToBlank(this.txtDependencyAllowance, this.ItemAllowance.DependencyAllowance.Value))
            {
                return;
            }

            CalculateSum();
        }

        /// <summary>
        /// 扶養手当 - MouseEnter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDependencyAllowance_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/" + this.txtMonth.Text;
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/" + this.txtMonth.Text;
            CompareWithPreviousYear("SALARY.dependency_allowance", strTargetDate, strTargetPreviousDate);
        }

        #endregion

        #region 支給額 - 時間外手当

        /// <summary>
        /// 時間外手当 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtOvertimeAllowance_KeyPress(object sender, KeyPressEventArgs e)
        {
            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 時間外手当 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtOvertimeAllowance_Leave(object sender, EventArgs e)
        {
            // 時間外手当
            var overtimeAllowance = ItemAllowance.DependencyAllowance;

            overtimeAllowance = this.Utils.FormatPriceToInt(this.txtOvertimeAllowance.Text);

            // 値を更新して、変更があれば色をつける
            if (!this.Utils.UpdateIntPrice(this.txtOvertimeAllowance, overtimeAllowance.Value, 4))
            {
                return;
            }

            CalculateSum();
        }

        #endregion

        #region 支給額 - 休日割増

        /// <summary>
        /// 休日割増 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtDaysoffIncreased_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 休日割増 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtDaysoffIncreased_Leave(object sender, EventArgs e)
        {
            // 休日割増
            var daysoffIncreased = ItemAllowance.DaysoffIncreased;

            daysoffIncreased = this.Utils.FormatPriceToInt(this.txtDaysoffIncreased.Text);

            // 値を更新して、変更があれば色をつける
            if (!this.Utils.UpdateIntPrice(this.txtDaysoffIncreased, daysoffIncreased.Value, 5))
            {
                return;
            }

            CalculateSum();
        }

        /// <summary>
        /// 休日割増 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtDaysoffIncreased_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/" + this.txtMonth.Text;
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/" + this.txtMonth.Text;
            CompareWithPreviousYear("SALARY.daysoff_increased", strTargetDate, strTargetPreviousDate);
        }

        #endregion

        #region 支給額 - 深夜割増

        /// <summary>
        /// 深夜割増 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtNightworkIncreased_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 深夜割増 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtNightworkIncreased_Leave(object sender, EventArgs e)
        {
            // 深夜割増
            var nightworkIncreased = ItemAllowance.NightworkIncreased;

            nightworkIncreased = this.Utils.FormatPriceToInt(this.txtNightworkIncreased.Text);

            // 値を更新して、変更があれば色をつける
            if (!this.Utils.UpdateIntPrice(this.txtNightworkIncreased, nightworkIncreased.Value, 6))
            {
                return;
            }

            CalculateSum();
        }

        /// <summary>
        /// 深夜割増 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtNightworkIncreased_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/" + this.txtMonth.Text;
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/" + this.txtMonth.Text;
            CompareWithPreviousYear("SALARY.nightwork_increased", strTargetDate, strTargetPreviousDate);
        }

        #endregion

        #region 支給額 - 深夜割増

        /// <summary>
        /// 住宅手当 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtHousingAllowance_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 住宅手当 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtHousingAllowance_Leave(object sender, EventArgs e)
        {
            // 住宅手当
            var housingAllowance = ItemAllowance.HousingAllowance;

            housingAllowance = this.Utils.FormatPriceToInt(this.txtHousingAllowance.Text);

            // 値を更新して、変更があれば色をつける
            if (!this.Utils.UpdateIntPrice(this.txtHousingAllowance, housingAllowance.Value, 7))
            {
                return;
            }

            // 1以上の値なら支給総計を再計算する
            if (!this.Utils.ConvertZeroToBlank(this.txtHousingAllowance, housingAllowance.Value))
            {
                return;
            }

            // 再計算
            CalculateSum();
        }

        /// <summary>
        /// 住宅手当 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtHousingAllowance_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/" + this.txtMonth.Text;
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/" + this.txtMonth.Text;
            CompareWithPreviousYear("SALARY.housing_allowance", strTargetDate, strTargetPreviousDate);
        }

        #endregion

        #region 支給額 - 遅刻早退欠勤

        /// <summary>
        /// 遅刻早退欠勤 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtLateAbsent_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 遅刻早退欠勤 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtLateAbsent_Leave(object sender, EventArgs e)
        {
            // 遅刻早退欠勤
            var lateAbsent = ItemAllowance.LateAbsent;

            lateAbsent = this.Utils.FormatPriceToInt(this.txtLateAbsent.Text);

            // 値を更新して、変更があれば色をつける
            if (!this.Utils.UpdateIntPrice(this.txtLateAbsent, lateAbsent.Value, 8))
            {
                return;
            }

            CalculateSum();
        }

        /// <summary>
        /// 遅刻早退欠勤 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtLateAbsent_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/" + this.txtMonth.Text;
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/" + this.txtMonth.Text;
            CompareWithPreviousYear("SALARY.late_absent", strTargetDate, strTargetPreviousDate);
        }

        #endregion

        #region 支給額 - 交通費

        /// <summary>
        /// 交通費 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtTransportationExpenses_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 交通費 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtTransportationExpenses_Leave(object sender, EventArgs e)
        {
            // 交通費
            var transportationExpenses = ItemAllowance.TransportationExpenses;

            transportationExpenses = this.Utils.FormatPriceToInt(this.txtTransportationExpenses.Text);

            // 値を更新して、変更があれば色をつける
            if (!this.Utils.UpdateIntPrice(this.txtTransportationExpenses, transportationExpenses.Value, 9))
            {
                return;
            }

            CalculateSum();
        }

        /// <summary>
        /// 交通費 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtTransportationExpenses_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/" + this.txtMonth.Text;
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/" + this.txtMonth.Text;
            CompareWithPreviousYear("SALARY.transportation_expenses", strTargetDate, strTargetPreviousDate);
        }

        #endregion

        #region 支給額 - 特別手当

        /// <summary>
        /// 特別手当 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtSpecialAllowance_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 特別手当 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtSpecialAllowance_Leave(object sender, EventArgs e)
        {
            // 特別手当
            var specialAllowance = ItemAllowance.SpecialAllowance;

            specialAllowance = this.Utils.FormatPriceToInt(this.txtSpecialAllowance.Text);

            // 値を更新して、変更があれば色をつける
            if (!this.Utils.UpdateIntPrice(this.txtSpecialAllowance, specialAllowance.Value, 10))
            {
                return;
            }

            // 1以上の値なら支給総計を再計算する
            if (this.Utils.ConvertZeroToBlank(this.txtSpecialAllowance, specialAllowance.Value))
            {
                return;
            }

            CalculateSum();
        }

        /// <summary>
        /// 特別手当 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtSpecialAllowance_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/" + this.txtMonth.Text;
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/" + this.txtMonth.Text;
            CompareWithPreviousYear("SALARY.special_allowance", strTargetDate, strTargetPreviousDate);
        }

        #endregion

        #region 支給額 - 予備

        /// <summary>
        /// 予備 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtSpareAllowance_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 予備 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtSpareAllowance_Leave(object sender, EventArgs e)
        {
            // 特別手当
            var specialAllowance = ItemAllowance.SpecialAllowance;

            specialAllowance = this.Utils.FormatPriceToInt(this.txtSpareAllowance.Text);

            // 値を更新して、変更があれば色をつける
            if (!this.Utils.UpdateIntPrice(this.txtSpareAllowance, specialAllowance.Value, 11))
            {
                return;
            }

            CalculateSum();
        }

        /// <summary>
        /// 予備 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtSpareAllowance_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/" + this.txtMonth.Text;
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/" + this.txtMonth.Text;
            CompareWithPreviousYear("SALARY.spare_allowance", strTargetDate, strTargetPreviousDate);
        }

        #endregion

        #region 支給額 - 支給額計

        /// <summary>
        /// 支給額計 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtTotalSalary_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 支給額計 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        /// <remarks>差引支給額も計算する</remarks>
        private void txtTotalSalary_Leave(object sender, EventArgs e)
        {
            // 特別手当
            var totalSalary = ItemAllowance.TotalSalary;

            totalSalary = this.Utils.FormatPriceToInt(this.txtTotalSalary.Text);

            // 値を更新して、変更があれば色をつける
            if (!this.Utils.UpdateIntPrice(this.txtTotalSalary, totalSalary.Value, 12))
            {
                return;
            }

            CalculateSum();

            if (totalSalary == 0)
            {
                return;
            }

            if (ItemDeduction.TotalDeduct.Value == 0)
            {
                return;
            }

            // 差引支給額計
            this.txtTotalDeductedSalary.Text = (totalSalary - ItemDeduction.TotalDeduct.Value).ToString();
        }

        /// <summary>
        /// 支給額計 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtTotalSalary_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/" + this.txtMonth.Text;
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/" + this.txtMonth.Text;
            CompareWithPreviousYear("SALARY.total_salary", strTargetDate, strTargetPreviousDate);
        }

        /// <summary>
        /// 差引支給額計 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtTotalDeductedSalary_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/" + this.txtMonth.Text;
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/" + this.txtMonth.Text;
            CompareWithPreviousYear("SALARY.total_deducted_salary", strTargetDate, strTargetPreviousDate);
        }

        #endregion

        #region 控除額 - 健康保険

        /// <summary>
        /// 健康保険 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtHealthInsurance_KeyPress(object sender, KeyPressEventArgs e)
        {
            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 健康保険 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtHealthInsurance_Leave(object sender, EventArgs e)
        {
            // 健康保険
            var healthInsurance = ItemDeduction.HealthInsurance;

            healthInsurance = this.Utils.FormatPriceToInt(this.txtHealthInsurance.Text);

            // 値を更新して、変更があれば色をつける
            if (!this.Utils.UpdateIntPrice(this.txtHealthInsurance, healthInsurance.Value, 14))
            {
                return;
            }

            CalculateSum();
        }

        /// <summary>
        /// 健康保険 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtHealthInsurance_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/" + this.txtMonth.Text;
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/" + this.txtMonth.Text;
            CompareWithPreviousYear("DEDUCT.health_insurance", strTargetDate, strTargetPreviousDate);
        }

        #endregion

        #region 控除額 - 介護保険

        /// <summary>
        /// 介護保険 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtNursingInsurance_KeyPress(object sender, KeyPressEventArgs e)
        {
            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 介護保険 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtNursingInsurance_Leave(object sender, EventArgs e)
        {
            // 介護保険
            var nursingInsurance = ItemDeduction.NursingInsurance;

            nursingInsurance = this.Utils.FormatPriceToInt(this.txtNursingInsurance.Text);

            // 値を更新して、変更があれば色をつける
            if (!this.Utils.UpdateIntPrice(this.txtNursingInsurance, nursingInsurance.Value, 15))
            {
                return;
            }

            // 1以上の値なら支給総計を再計算する
            if (!this.Utils.ConvertZeroToBlank(this.txtNursingInsurance, ItemDeduction.NursingInsurance.Value))
            {
                return;
            }

            CalculateSum();
        }

        /// <summary>
        /// 介護保険 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtNursingInsurance_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/" + this.txtMonth.Text;
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/" + this.txtMonth.Text;
            CompareWithPreviousYear("DEDUCT.nursing_insurance", strTargetDate, strTargetPreviousDate);
        }

        #endregion

        #region 控除額 - 厚生年金

        /// <summary>
        /// 厚生年金 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtWelfareAnnuity_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 厚生年金 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtWelfareAnnuity_Leave(object sender, EventArgs e)
        {
            // 厚生年金
            var welfareAnnuity = ItemDeduction.WelfareAnnuity;

            welfareAnnuity = this.Utils.FormatPriceToInt(this.txtWelfareAnnuity.Text);

            // 値を更新して、変更があれば色をつける
            if (!this.Utils.UpdateIntPrice(this.txtWelfareAnnuity, welfareAnnuity.Value, 16))
            {
                return;
            }

            CalculateSum();
        }

        /// <summary>
        /// 厚生年金 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtWelfareAnnuity_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/" + this.txtMonth.Text;
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/" + this.txtMonth.Text;
            CompareWithPreviousYear("DEDUCT.welfare_annuity", strTargetDate, strTargetPreviousDate);
        }

        #endregion

        #region 控除額 - 雇用保険

        /// <summary>
        /// 雇用保険 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtEmploymentInsurance_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 雇用保険 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtEmploymentInsurance_Leave(object sender, EventArgs e)
        {
            // 雇用保険
            var employmentInsurance = ItemDeduction.EmploymentInsurance;

            employmentInsurance = this.Utils.FormatPriceToInt(this.txtEmploymentInsurance.Text);

            // 値を更新して、変更があれば色をつける
            if (!this.Utils.UpdateIntPrice(this.txtEmploymentInsurance, ItemDeduction.EmploymentInsurance.Value, 17))
            {
                return;
            }

            CalculateSum();
        }

        /// <summary>
        /// 雇用保険 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtEmploymentInsurance_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/" + this.txtMonth.Text;
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/" + this.txtMonth.Text;
            CompareWithPreviousYear("DEDUCT.employment_insurance", strTargetDate, strTargetPreviousDate);
        }

        #endregion

        #region 控除額 - 所得税

        /// <summary>
        /// 所得税 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtIncomeTax_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 所得税 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtIncomeTax_Leave(object sender, EventArgs e)
        {
            // 雇用保険
            var employmentInsurance = ItemDeduction.EmploymentInsurance;

            employmentInsurance = this.Utils.FormatPriceToInt(this.txtIncomeTax.Text);

            // 値を更新して、変更があれば色をつける
            if (!this.Utils.UpdateIntPrice(this.txtIncomeTax, ItemDeduction.IncomeTax.Value, 18))
            {
                return;
            }

            // 合計支給額の自動計算
            CalculateSum();
        }

        /// <summary>
        /// 所得税 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtIncomeTax_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/" + this.txtMonth.Text;
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/" + this.txtMonth.Text;
            CompareWithPreviousYear("DEDUCT.income_tax", strTargetDate, strTargetPreviousDate);
        }

        #endregion

        #region 控除額 - 市町村税

        /// <summary>
        /// 市町村税 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtMunicipalTax_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 市町村税 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtMunicipalTax_Leave(object sender, EventArgs e)
        {
            // 市町村税
            var municipalTax = ItemDeduction.MunicipalTax;

            municipalTax = this.Utils.FormatPriceToInt(this.txtMunicipalTax.Text);

            // 値を更新して、変更があれば色をつける
            if (!this.Utils.UpdateIntPrice(this.txtMunicipalTax, municipalTax.Value, 19))
            {
                return;
            }

            // 1以上の値なら支給総計を再計算する
            if (this.Utils.ConvertZeroToBlank(this.txtMunicipalTax, municipalTax.Value))
            {
                return;
            }

            CalculateSum();
        }

        /// <summary>
        /// 市町村税 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtMunicipalTax_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/" + this.txtMonth.Text;
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/" + this.txtMonth.Text;
            CompareWithPreviousYear("DEDUCT.municipal_tax", strTargetDate, strTargetPreviousDate);
        }

        #endregion

        #region 控除額 - 互助会

        /// <summary>
        /// 互助会 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtFriendshipAssociation_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 互助会 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtFriendshipAssociation_Leave(object sender, EventArgs e)
        {
            // 互助会
            ItemDeduction.FriendshipAssociation = this.Utils.FormatPriceToInt(this.txtFriendshipAssociation.Text);

            // 値を更新して、変更があれば色をつける
            if (!this.Utils.UpdateIntPrice(this.txtFriendshipAssociation, ItemDeduction.FriendshipAssociation.Value, 20))
            {
                return;
            }

            CalculateSum();
        }

        /// <summary>
        /// 互助会 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtFriendshipAssociation_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/" + this.txtMonth.Text;
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/" + this.txtMonth.Text;
            CompareWithPreviousYear("DEDUCT.friendship_association", strTargetDate, strTargetPreviousDate);
        }

        #endregion

        #region 控除額 - 年末調整他

        /// <summary>
        /// 年末調整他 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtYearEndTaxAdjustment_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 年末調整他 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtYearEndTaxAdjustment_Leave(object sender, EventArgs e)
        {
            // 互助会
            var yearEndTaxAdjustment = ItemDeduction.YearEndTaxAdjustment;

            yearEndTaxAdjustment = this.Utils.FormatPriceToInt(this.txtYearEndTaxAdjustment.Text);

            // 値を更新して、変更があれば色をつける
            if (!this.Utils.UpdateIntPrice(this.txtYearEndTaxAdjustment, yearEndTaxAdjustment.Value, 21) == true)
            {
                return;
            }

            // 1以上の値なら支給総計を再計算する
            if (this.Utils.ConvertZeroToBlank(this.txtYearEndTaxAdjustment, yearEndTaxAdjustment.Value))
            {
                return;
            }

            CalculateSum();
        }

        /// <summary>
        /// 年末調整他 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtYearEndTaxAdjustment_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/" + this.txtMonth.Text;
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/" + this.txtMonth.Text;
            CompareWithPreviousYear("DEDUCT.year_end_tax_adjustment", strTargetDate, strTargetPreviousDate);
        }

        #endregion

        #region 控除額 - 控除額計

        /// <summary>
        /// 控除額計 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtTotalDeduct_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 控除額計 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtTotalDeduct_Leave(object sender, EventArgs e)
        {
            // 互助会
            var totalDeduct = ItemDeduction.TotalDeduct;

            totalDeduct = this.Utils.FormatPriceToInt(this.txtTotalDeduct.Text);

            // 値を更新して、変更があれば色をつける
            if (!this.Utils.UpdateIntPrice(this.txtTotalDeduct, totalDeduct.Value, 22))
            {
                return;
            }

            CalculateSum();

            // 差引支給額
            if (ItemAllowance.TotalSalary.Value == 0)
            {
                return;
            }

            if (totalDeduct.Value == 0)
            {
                return;
            }

            this.txtTotalDeductedSalary.Text = (ItemAllowance.TotalSalary.Value - totalDeduct.Value).ToString();

            // 合計支給額の自動計算
            CalculateSum();
        }

        #endregion

        #region 勤務備考 - 時間外時間

        /// <summary>
        /// 時間外時間 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtOvertimeTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            // KeyChar has . ?
            if (e.KeyChar.ToString() == ".")
            {
                return;
            }

            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 時間外時間 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtOvertimeTime_Leave(object sender, EventArgs e)
        {
            // is Double ?
            if (!double.TryParse(this.txtOvertimeTime.Text, out double overTime))
            {
                return;
            }

            // OverTime
            ItemWorkingReferences.OvertimeTime = overTime;

            // 値を更新して、変更があれば色をつける
            if (!this.Utils.UpdateDoublePrice(this.txtOvertimeTime, overTime, 23))
            {
                return;
            }
        }

        /// <summary>
        /// 時間外時間 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtOvertimeAllowance_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/" + this.txtMonth.Text;
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/" + this.txtMonth.Text;
            CompareWithPreviousYear("SALARY.overtime_allowance", strTargetDate, strTargetPreviousDate);
        }

        #endregion

        #region 勤務備考 - 時間外時間

        /// <summary>
        /// 休出時間 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtWeekendWorktime_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 休出時間 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtWeekendWorktime_Leave(object sender, EventArgs e)
        {
            ItemWorkingReferences.WeekendWorktime = this.Utils.FormatPriceToInt(this.txtWeekendWorktime.Text);

            var weekendWorkTime = ItemWorkingReferences.WeekendWorktime.Value;

            // 値を更新して、変更があれば色をつける
            if (!this.Utils.UpdateDoublePrice(this.txtWeekendWorktime, weekendWorkTime, 24))
            {
                return;
            }

            // 1以上の値なら支給総計を再計算する
            if (this.Utils.ConvertZeroToBlank(this.txtWeekendWorktime, weekendWorkTime))
            {
                return;
            }

            CalculateSum();
        }

        #endregion

        #region 勤務備考 - 深夜時間

        /// <summary>
        /// 深夜時間 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtMidnightWorktime_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 深夜時間 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtMidnightWorktime_Leave(object sender, EventArgs e)
        {
            ItemWorkingReferences.MidnightWorktime = this.Utils.FormatPriceToInt(this.txtMidnightWorktime.Text);

            var midnightWorktime = ItemWorkingReferences.MidnightWorktime.Value;

            // 値を更新して、変更があれば色をつける
            if (!this.Utils.UpdateDoublePrice(this.txtMidnightWorktime, midnightWorktime, 25))
            {
                return;
            }

            // 1以上の値なら支給総計を再計算する
            if (!this.Utils.ConvertZeroToBlank(this.txtMidnightWorktime, midnightWorktime))
            {
                return;
            }

            CalculateSum();
        }

        #endregion

        #region 勤務備考 - 遅刻早退欠勤H

        /// <summary>
        /// 遅刻早退欠勤H - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtLateAbsentH_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 遅刻早退欠勤H - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtLateAbsentH_Leave(object sender, EventArgs e)
        {
            ItemWorkingReferences.LateAbsentH = this.Utils.FormatPriceToInt(this.txtLateAbsentH.Text);

            var lateAbsentH = ItemWorkingReferences.LateAbsentH.Value;

            // 値を更新して、変更があれば色をつける
            if (!this.Utils.UpdateDoublePrice(this.txtLateAbsentH, lateAbsentH, 26))
            {
                return;
            }

            // 1以上の値なら支給総計を再計算する
            if (this.Utils.ConvertZeroToBlank(this.txtLateAbsentH, lateAbsentH))
            {
                return;
            }

            CalculateSum();
        }

        #endregion

        #region 勤務備考 - 支給額-保険

        /// <summary>
        /// 支給額-保険 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtInsurance_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 支給額-保険 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtInsurance_Leave(object sender, EventArgs e)
        {
            ItemWorkingReferences.Insurance = this.Utils.FormatPriceToInt(this.txtInsurance.Text);

            // 値を更新して、変更があれば色をつける
            if (!this.Utils.UpdateIntPrice(this.txtInsurance, ItemWorkingReferences.Insurance.Value, 27))
            {
                return;
            }

            CalculateSum();
        }

        /// <summary>
        /// 支給額-保険 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtInsurance_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/" + this.txtMonth.Text;
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/" + this.txtMonth.Text;
            CompareWithPreviousYear("DUTY.insurance", strTargetDate, strTargetPreviousDate);
        }

        #endregion

        #region 勤務備考 - 扶養人数

        /// <summary>
        /// 扶養人数 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtNumberOfDependent_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 扶養人数 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtNumberOfDependent_Leave(object sender, EventArgs e)
        {
            ItemWorkingReferences.NumberOfDependent = this.Utils.FormatPriceToInt(this.txtNumberOfDependent.Text);

            var numberOfDependent = ItemWorkingReferences.NumberOfDependent.Value;

            // 値を更新して、変更があれば色をつける
            if (!this.Utils.UpdateDoublePrice(this.txtNumberOfDependent, numberOfDependent, 28))
            {
                return;
            }

            // 1以上の値なら支給総計を再計算する
            if (!this.Utils.ConvertZeroToBlank(this.txtNumberOfDependent, numberOfDependent))
            {
                return;
            }

            CalculateSum();
        }

        #endregion

        #region 勤務備考 - 有給残日数

        /// <summary>
        /// 有給残日数 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtPaidVacation_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 有給残日数 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtPaidVacation_Leave(object sender, EventArgs e)
        {
            ItemWorkingReferences.PaidVacation = this.Utils.FormatPriceToInt(this.txtPaidVacation.Text);

            // 値を更新して、変更があれば色をつける
            if (!this.Utils.UpdateIntPrice(this.txtPaidVacation, ItemWorkingReferences.PaidVacation.Value, 29))
            {
                return;
            }

            CalculateSum();
        }

        #endregion

        #region 勤務備考 - 勤務時間

        /// <summary>
        /// 勤務時間 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtWorkingHours_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString() == ".")
            {
                return;
            }

            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 勤務時間 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtWorkingHours_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtWorkingHours.Text))
            {
                return;
            }

            ItemWorkingReferences.WorkingHours = Convert.ToDouble(this.txtWorkingHours.Text);

            // 値を更新して、変更があれば色をつける
            if (!this.Utils.UpdateDoublePrice(this.txtWorkingHours, ItemWorkingReferences.WorkingHours.Value, 30))
            {
                return;
            }

            CalculateSum();
        }

        #endregion

        #region 勤務備考 - 勤務先

        /// <summary>
        /// 勤務先 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtWorkplace_Leave(object sender, EventArgs e)
        {
            ItemWorkingReferences.Workplace = this.txtWorkplace.Text;
        }

        #endregion

        #region 副業 - 副業収入

        /// <summary>
        /// 副業収入 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtSideBusiness_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 副業収入 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtSideBusiness_Leave(object sender, EventArgs e)
        {
            if (this.txtSideBusiness.Text == string.Empty)
            {
                return;
            }

            ItemSideBusiness.SideBusiness = this.Utils.FormatPriceToInt(this.txtSideBusiness.Text);

            // 値を更新して、変更があれば色をつける
            if (!this.Utils.UpdateIntPrice(this.txtSideBusiness, ItemSideBusiness.SideBusiness.Value, 32))
            {
                return;
            }

            CalculateSum();
        }

        #endregion

        #region 副業 - 臨時収入

        /// <summary>
        /// 臨時収入 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtPerquisite_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 臨時収入 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtPerquisite_Leave(object sender, EventArgs e)
        {
            ItemSideBusiness.SideBusiness = this.Utils.FormatPriceToInt(this.txtPerquisite.Text);

            // 値を更新して、変更があれば色をつける
            if (!this.Utils.UpdateIntPrice(this.txtPerquisite, ItemSideBusiness.SideBusiness.Value, 33))
            {
                return;
            }

            CalculateSum();
        }

        #endregion

        #region 副業 - その他

        /// <summary>
        /// その他 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtOthers_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// その他 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtOthers_Leave(object sender, EventArgs e)
        {
            ItemSideBusiness.Others = this.Utils.FormatPriceToInt(this.txtOthers.Text);

            // 値を更新して、変更があれば色をつける
            if (!this.Utils.UpdateIntPrice(this.txtOthers, ItemSideBusiness.Others.Value, 34))
            {
                return;
            }

            CalculateSum();
        }

        #endregion

        #region 副業 - 覚書

        /// <summary>
        /// 覚書 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtRemarks_Leave(object sender, EventArgs e)
        {
            ItemSideBusiness.Remarks = this.txtRemarks.Text;
        }

        #endregion

        #region SetDateTime

        /// <summary>
        /// Set - 日時
        /// </summary>
        /// <param name="addmonths">進める / 戻す月数</param>
        private void SetDateTime
        (
            // 進める / 戻す月数
            int addmonths
        )
        {
            // 対象年月
            this.TargetDate = new DateTime(Convert.ToInt32(this.txtYear.Text), Convert.ToInt32(this.txtMonth.Text), 1);
            this.TargetDate = this.TargetDate.AddMonths(addmonths);

            // 対象年
            this.TargetYear = Convert.ToInt32(this.TargetDate.ToString("yyyy"));
            this.txtYear.Text = this.TargetYear.ToString();

            // 対象月
            this.TargetMonth = Convert.ToInt32(this.TargetDate.ToString("MM"));
            this.txtMonth.Text = String.Format("{0:00}", this.TargetMonth);
        }

        #endregion

        #region 前月の値取得

        /// <summary>
        /// 前月の値を取得します
        /// </summary>
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            using (var waiting = new MouseCursorChangeWaiting())
            {
                // Set - 日付
                this.SetDateTime(-1);

                if (!isConfirmAllow)
                {
                    // データがあるか検索
                    SearchPreviousOrNextData(1);

                    return;
                }

                DialogResult confirm = MessageBox.Show("登録もしくは更新がされていない状態です。表示中の明細を閉じてもいいですか？",
                                                    "確認",
                                                    MessageBoxButtons.YesNo,
                                                    MessageBoxIcon.Information,
                                                    MessageBoxDefaultButton.Button2);

                // confirm is Yes ?
                if (confirm != DialogResult.Yes)
                {
                    return;
                }

                // データがあるか検索
                SearchPreviousOrNextData(1);
            }
        }

        #endregion

        #region 次月の値取得

        /// <summary>
        /// 次月の値を取得します
        /// </summary>
        private void btnNext_Click(object sender, EventArgs e)
        {
            using (var waiting = new MouseCursorChangeWaiting())
            {
                // Set - 日付
                this.SetDateTime(1);

                if (!isConfirmAllow)
                {
                    // データがあるか検索
                    SearchPreviousOrNextData(-1);

                    return;
                }

                DialogResult confirm = MessageBox.Show("登録もしくは更新がされていない状態です。表示中の明細を閉じてもいいですか？",
                                                    "確認",
                                                    MessageBoxButtons.YesNo,
                                                    MessageBoxIcon.Information,
                                                    MessageBoxDefaultButton.Button2);
                if (confirm == DialogResult.Yes)
                {
                    // データがあるか検索
                    SearchPreviousOrNextData(-1);
                }
            }
        }

        #endregion

        /// <summary>
        /// 登録判定用
        /// </summary>
        private void btnRegister_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("登録しますか？",
                                                  "確認",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Information,
                                                  MessageBoxDefaultButton.Button2);

            // result is not Yes ?
            if (result != DialogResult.Yes)
            {
                return;
            }

            // isUpdate ?
            if (!isUpdate)
            {
                // 登録
                Register();
            }
            else
            {
                // 更新
                this.Update(
                    this.txtYear.Text,
                    this.txtMonth.Text
                );
            }
        }

        /// <summary>
        /// 指定した給与明細を登録します。
        /// </summary>
        private void Register()
        {
            try
            {
                // コネクションオブジェクトとコマンドオブジェクトを生成します。
                var connection = new MySqlConnection(ConnectionString);
                var command = new MySqlCommand();
                int num;
                string targetDate;

                {
                    // コネクションをオープンします。
                    connection.Open();

                    // バージョン情報取得SQLを実行します。
                    command.Connection = connection;

                    MySqlDataAdapter selectCommand = new MySqlDataAdapter(" SELECT  " +
                                                                          "     ID " +
                                                                          " FROM " +
                                                                          "     mylife.t_salary ", connection);
                    DataTable dt = new DataTable();
                    selectCommand.Fill(dt);
                    num = dt.Rows.Count;

                    // 登録時のIDを設定
                    num = num + 1;

                    // 対象日付の取得
                    if (isDefault == false)
                    {
                        targetDate = this.txtYear.Text + "/" + this.txtMonth.Text;
                    }
                    else
                    {
                        num = 0;
                        targetDate = "9999/99";
                    }


                    // 本日日付
                    DateTime today = DateTime.Now;

                    // 支給額
                    MySqlDataAdapter insertCommand_Salary = new MySqlDataAdapter("insert into mylife.t_salary values (" +
                    "                        " + num +                                        // 01.ID
                    "                     , '" + this.txtYear.Text + "'" +                    // 02.年
                    "                     , '" + targetDate + "'" +                           // 03.対象年月
                    "                     , '" + ItemAllowance.BasicSalary.Value + "'" +                // 04.基本給
                    "                     , '" + ItemAllowance.ExecutiveAllowance.Value + "'" +         // 05.役職手当
                    "                     , '" + ItemAllowance.DependencyAllowance.Value + "'" +        // 06.扶養手当
                    "                     , '" + ItemAllowance.OvertimeAllowance.Value + "'" +          // 07.時間外手当
                    "                     , '" + ItemAllowance.DaysoffIncreased.Value + "'" +           // 08.休日割増
                    "                     , '" + ItemAllowance.NightworkIncreased.Value + "'" +         // 09.深夜割増
                    "                     , '" + ItemAllowance.HousingAllowance.Value + "'" +           // 10.住宅手当
                    "                     , '" + ItemAllowance.LateAbsent.Value + "'" +                 // 11.遅刻早退欠勤
                    "                     , '" + ItemAllowance.TransportationExpenses.Value + "'" +     // 12.交通費
                    "                     , '" + ItemAllowance.SpecialAllowance.Value + "'" +           // 13.特別手当
                    "                     , '" + ItemAllowance.SpareAllowance.Value + "'" +             // 14.予備
                    "                     , '" + ItemAllowance.TotalSalary.Value + "'" +                // 15.支給総計
                    "                     , '" + ItemAllowance.TotalDeductedSalary.Value + "'" +        // 16.差引支給額
                    "                     , '" + today + "'" +                                // 17.作成日
                    "                     , '" + today + "'" +                                // 18.更新日
                    "                     )", connection);

                    insertCommand_Salary.Fill(dt);

                    // 控除額
                    MySqlDataAdapter insertCommand_Deduct = new MySqlDataAdapter("insert into mylife.t_deduct values (" +
                        "                        " + num +                                        // 01.ID
                        "                     , '" + targetDate + "'" +                           // 02.対象年月
                        "                     , '" + ItemDeduction.HealthInsurance.Value + "'" +            // 03.健康保険
                        "                     , '" + ItemDeduction.NursingInsurance.Value + "'" +           // 04.介護保険
                        "                     , '" + ItemDeduction.WelfareAnnuity.Value + "'" +             // 05.厚生年金
                        "                     , '" + ItemDeduction.EmploymentInsurance.Value + "'" +        // 06.雇用保険
                        "                     , '" + ItemDeduction.IncomeTax.Value + "'" +                  // 07.所得税
                        "                     , '" + ItemDeduction.MunicipalTax.Value + "'" +               // 08.市町村税
                        "                     , '" + ItemDeduction.FriendshipAssociation.Value + "'" +      // 09.互助会
                        "                     , '" + ItemDeduction.YearEndTaxAdjustment.Value + "'" +       // 10.年末調整他
                        "                     , '" + ItemDeduction.TotalDeduct.Value + "'" +                // 11.控除額計
                        "                     , '" + today + "'" +                                // 12.作成日
                        "                     , '" + today + "'" +                                // 13.更新日
                        "                     )", connection);

                    insertCommand_Deduct.Fill(dt);

                    // 勤務備考
                    MySqlDataAdapter insertCommand_Duty = new MySqlDataAdapter("insert into mylife.t_duty values (" +
                        "                        " + num +                                        // 01.ID
                        "                     , '" + targetDate + "'" +                           // 02.対象年月
                        "                     , '" + ItemWorkingReferences.OvertimeTime.Value + "'" +                 // 03.時間外時間
                        "                     , '" + ItemWorkingReferences.WeekendWorktime.Value + "'" +              // 04.休出時間
                        "                     , '" + ItemWorkingReferences.MidnightWorktime.Value + "'" +             // 05.深夜時間
                        "                     , '" + ItemWorkingReferences.LateAbsentH.Value + "'" +                  // 06.遅刻早退欠勤H
                        "                     , '" + ItemWorkingReferences.Insurance.Value + "'" +                    // 07.支給額-保健
                        "                     , '" + ItemWorkingReferences.NumberOfDependent.Value + "'" +            // 08.扶養人数
                        "                     , '" + ItemWorkingReferences.PaidVacation.Value + "'" +                 // 09.有給残日数
                        "                     , '" + ItemWorkingReferences.WorkingHours.Value + "'" +                 // 10.勤務時間
                        "                     , '" + ItemWorkingReferences.Workplace + "'" +                    // 11.勤務先
                        "                     , '" + today + "'" +                                // 12.作成日
                        "                     , '" + today + "'" +                                // 13.更新日
                        "                     )", connection);

                    insertCommand_Duty.Fill(dt);

                    // 副業など
                    MySqlDataAdapter insertCommand_Side = new MySqlDataAdapter("insert into mylife.t_side_business values (" +
                        "                        " + num +                                        // 01.ID
                        "                     , '" + targetDate + "'" +                           // 02.対象年月
                        "                     , '" + ItemSideBusiness.SideBusiness.Value + "'" +                 // 03.副業
                        "                     , '" + ItemSideBusiness.Perquisite.Value + "'" +                   // 04.臨時収入
                        "                     , '" + ItemSideBusiness.Others.Value + "'" +                       // 05.その他
                        "                     , '" + ItemSideBusiness.Remarks + "'" +                      // 06.覚書
                        "                     , '" + today + "'" +                                // 07.作成日
                        "                     , '" + today + "'" +                                // 08.更新日
                        "                     )", connection);

                    insertCommand_Side.Fill(dt);

                    if (this.txtTotalDeductedSalary.Text != "0")
                    {
                        this.btnRegister.Text = "更新";
                        isUpdate = true;
                        isRegister = true;
                    }

                    // 年収フォームを更新
                    GetAnnualIncomey();

                    // 登録完了
                    MessageBox.Show("登録情報を追加しました。", "追加OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        /// <summary>
        /// 指定した年月の給与明細を更新します。
        /// </summary>
        /// <param name="year">更新年</param>
        /// <param name="month">更新月</param>
        private void Update
        (
            // 更新年
            string year,
            // 更新月
            string month
        )
        {
            try
            {
                var strTargetDate = string.Empty;

                // コネクションオブジェクトとコマンドオブジェクトを生成します。
                MySqlConnection connection = new MySqlConnection(ConnectionString);

                // コネクションをオープンします。
                connection.Open();

                if (!isDefault)
                {
                    strTargetDate = year + "/" + month;
                }
                else
                {
                    strTargetDate = "9999/99";
                }

                DataTable dt = new DataTable();

                // 本日日付
                DateTime today = DateTime.Now;

                // 支給額
                //レコードを更新するためのSQL文を作成

                MySqlCommand updateCommand_Salary = new MySqlCommand("update mylife.t_salary set " +
                    "                        basic_salary = '" + ItemAllowance.BasicSalary.Value + "'" +                          // 01.基本給
                    "                      , executive_allowance = '" + ItemAllowance.ExecutiveAllowance.Value + "'" +            // 02.役職手当
                    "                      , dependency_allowance = '" + ItemAllowance.DependencyAllowance.Value + "'" +          // 03.扶養手当
                    "                      , overtime_allowance = '" + ItemAllowance.OvertimeAllowance.Value + "'" +              // 04.時間外手当
                    "                      , daysoff_increased = '" + ItemAllowance.DaysoffIncreased.Value + "'" +                // 05.休日割増
                    "                      , nightwork_increased = '" + ItemAllowance.NightworkIncreased.Value + "'" +            // 06.深夜割増
                    "                      , housing_allowance = '" + ItemAllowance.HousingAllowance.Value + "'" +                // 07.住宅手当
                    "                      , late_absent = '" + ItemAllowance.LateAbsent.Value + "'" +                            // 08.遅刻早退欠勤
                    "                      , transportation_expenses = '" + ItemAllowance.TransportationExpenses.Value + "'" +    // 09.交通費
                    "                      , special_allowance = '" + ItemAllowance.SpecialAllowance.Value + "'" +                // 10.特別手当
                    "                      , spare_allowance = '" + ItemAllowance.SpareAllowance.Value + "'" +                    // 11.予備
                    "                      , total_salary = '" + ItemAllowance.TotalSalary.Value + "'" +                          // 12.支給総計
                    "                      , total_deducted_salary = '" + ItemAllowance.TotalDeductedSalary.Value + "'" +         // 13.差引支給額
                    "                      , update_date = '" + today + "'" +                                           // 14.更新日
                    "                     where work_month = '" + strTargetDate + "'", connection);
                updateCommand_Salary.ExecuteNonQuery();


                // 控除額
                MySqlCommand updateCommand_Deduct = new MySqlCommand("update mylife.t_deduct set" +
                    "                        health_insurance = '" + ItemDeduction.HealthInsurance.Value + "'" +                  // 01.健康保険
                    "                      , nursing_insurance = '" + ItemDeduction.NursingInsurance.Value + "'" +                // 02.介護保険
                    "                      , welfare_annuity = '" + ItemDeduction.WelfareAnnuity.Value + "'" +                    // 03.厚生年金
                    "                      , employment_insurance = '" + ItemDeduction.EmploymentInsurance.Value + "'" +          // 04.雇用保険
                    "                      , income_tax = '" + ItemDeduction.IncomeTax.Value + "'" +                              // 05.所得税
                    "                      , municipal_tax = '" + ItemDeduction.MunicipalTax.Value + "'" +                        // 06.市町村税
                    "                      , friendship_association = '" + ItemDeduction.FriendshipAssociation.Value + "'" +      // 07.互助会
                    "                      , year_end_tax_adjustment = '" + ItemDeduction.YearEndTaxAdjustment.Value + "'" +      // 08.年末調整他
                    "                      , total_deduct = '" + ItemDeduction.TotalDeduct.Value + "'" +                          // 09.控除額計
                    "                      , update_date = '" + today + "'" +                                           // 10.更新日
                    "                     where work_month = '" + strTargetDate + "'", connection);

                updateCommand_Deduct.ExecuteNonQuery();


                // 勤務備考
                MySqlCommand updateCommand_Duty = new MySqlCommand("update mylife.t_duty set" +
                    "                        overtime_time = '" + ItemWorkingReferences.OvertimeTime.Value + "'" +                        // 01.時間外時間
                    "                      , weekend_worktime = '" + ItemWorkingReferences.WeekendWorktime.Value + "'" +                  // 02.休出時間
                    "                      , midnight_worktime = '" + ItemWorkingReferences.MidnightWorktime.Value + "'" +                // 03.深夜時間
                    "                      , late_absent = '" + ItemWorkingReferences.LateAbsentH.Value + "'" +                           // 04.遅刻早退欠勤H
                    "                      , insurance = '" + ItemWorkingReferences.Insurance.Value + "'" +                               // 05.支給額-保険
                    "                      , number_of_dependent = '" + ItemWorkingReferences.NumberOfDependent.Value + "'" +             // 06.扶養人数
                    "                      , paid_vacation = '" + ItemWorkingReferences.PaidVacation.Value + "'" +                        // 07.有給残日数
                    "                      , working_hours = '" + ItemWorkingReferences.WorkingHours.Value + "'" +                        // 08.勤務時間
                    "                      , workplace = '" + ItemWorkingReferences.Workplace + "'" +                               // 09.勤務先
                    "                      , update_date = '" + today + "'" +                                         // 10.更新日
                    "                     where work_month = '" + strTargetDate + "'", connection);

                updateCommand_Duty.ExecuteNonQuery();

                // 副業など
                MySqlCommand updateCommand_Side = new MySqlCommand("update mylife.t_side_business set" +
                    "                        side_business = '" + ItemSideBusiness.SideBusiness.Value + "'" +                        // 01.副業
                    "                       , perquisite   = '" + ItemSideBusiness.Perquisite.Value + "'" +                          // 02.臨時収入
                    "                       , others       = '" + ItemSideBusiness.Others.Value + "'" +                              // 03.その他
                    "                       , remarks      = '" + ItemSideBusiness.Remarks + "'" +                             // 04.覚書
                    "                       , update_date = '" + today + "'" +                                        // 05.更新日
                    "                     where work_month = '" + strTargetDate + "'", connection);

                updateCommand_Side.ExecuteNonQuery();

                // 年収フォームを更新
                GetAnnualIncomey();

                // 登録完了
                MessageBox.Show("登録情報を更新しました。", "更新OK", MessageBoxButtons.OK, MessageBoxIcon.Information);

                SetDefaultColor();

                isRegister = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// 指定した年月の給与明細を検索します。
        /// </summary>
        /// <param name="addmonths">進める / 戻す月数</param>
        private void SearchPreviousOrNextData
        (
            // 進める / 戻す月数
            int addmonths
        )
        {
            try
            {
                int num = SetId();

                if (num > 0)
                {
                    // 項目クリア
                    this.Reset();

                    // 前月、次月のデータがある場合、対象月のデータを取得
                    GetSalary();

                    // 年収取得
                    GetAnnualIncomey();
                }
                else
                {
                    // 前月、次月のデータがない場合
                    DialogResult result = MessageBox.Show("対象年月のデータがありません。新規登録しますか？",
                                              "確認",
                                              MessageBoxButtons.YesNo,
                                              MessageBoxIcon.Information,
                                              MessageBoxDefaultButton.Button2);

                    if (result == DialogResult.Yes)
                    {
                        // 表示項目のリセット
                        this.Reset();

                        // ボタンを変更
                        this.btnRegister.Text = "登録";
                        isUpdate = false;

                        // 既存明細の活用
                        DialogResult useDefault = MessageBox.Show("デフォルト明細を使用しますか？",
                                                  "確認",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Information,
                                                  MessageBoxDefaultButton.Button2);


                        if (useDefault == DialogResult.Yes)
                        {
                            // デフォルト時
                            isDefault = true;

                            // 項目クリア
                            this.Reset();

                            // 金額取得
                            GetSalary();

                            // 年収取得
                            GetAnnualIncomey();

                            isDefault = false;
                        }

                    }
                    else
                    {
                        // Set - 日時
                        SetDateTime(addmonths);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        /// <summary>
        /// 指定された日付データをチェックする。
        /// </summary>
        public int SetId()
        {
            // コネクションオブジェクトとコマンドオブジェクトを生成します。
            var connection = new MySqlConnection(ConnectionString);
            var command = new MySqlCommand();

            var strTargetDate = string.Empty;

            // 対象日付の取得
            if (isDefault == false)
            {
                strTargetDate = this.txtYear.Text + "/" + this.txtMonth.Text;
            }
            else
            {
                strTargetDate = "9999/99";
            }


            // コネクションをオープンします。
            connection.Open();

            // バージョン情報取得SQLを実行します。
            command.Connection = connection;

            MySqlDataAdapter selectCommand = new MySqlDataAdapter(" SELECT  " +
                                                                  "     SALARY.id " +
                                                                  " FROM " +
                                                                  "     mylife.t_salary SALARY " +
                                                                  " LEFT OUTER JOIN " +
                                                                  "     mylife.t_deduct DEDUCT " +
                                                                  " ON " +
                                                                  "     SALARY.ID = DEDUCT.ID " +
                                                                  " LEFT OUTER JOIN " +
                                                                  "     mylife.t_duty DUTY " +
                                                                  " ON " +
                                                                  "     SALARY.ID = DUTY.ID " +
                                                                  " LEFT OUTER JOIN " +
                                                                  "     mylife.t_side_business SIDE " +
                                                                  " ON " +
                                                                  "     SALARY.ID = SIDE.ID " +
                                                                  " WHERE " +
                                                                  "    SALARY.work_month = '" + strTargetDate + "' ", connection);
            DataTable dt = new DataTable();
            selectCommand.Fill(dt);
            int num = dt.Rows.Count;

            return num;
        }

        /// <summary>
        /// 支給総計 - 自動算出
        /// </summary>
        private void CalculateSum()
        {
            // 12.支出額計
            this.ItemAllowance.TotalSalary = this.ItemAllowance.BasicSalary.Value
                                           + this.ItemAllowance.ExecutiveAllowance.Value
                                           + this.ItemAllowance.DependencyAllowance.Value
                                           + this.ItemAllowance.OvertimeAllowance.Value
                                           + this.ItemAllowance.DaysoffIncreased.Value
                                           + this.ItemAllowance.NightworkIncreased.Value
                                           + this.ItemAllowance.HousingAllowance.Value
                                           + this.ItemAllowance.LateAbsent.Value
                                           + this.ItemAllowance.TransportationExpenses.Value
                                           + this.ItemAllowance.SpecialAllowance.Value
                                           + this.ItemAllowance.SpareAllowance.Value
                                           + this.ItemSideBusiness.SideBusiness.Value
                                           + this.ItemSideBusiness.Perquisite.Value
                                           + this.ItemSideBusiness.Others.Value
                                           ;

            if (isUpdate == true)
            {
                if (this.Utils.ComparePrice(12, this.ItemAllowance.TotalSalary.ToString()))
                {
                    this.txtTotalSalary.BackColor = Color.Pink;
                }
                else
                {
                    this.txtTotalSalary.BackColor = Color.FromKnownColor(KnownColor.Window);
                }
            }

            // 22.控除額計
            this.ItemDeduction.TotalDeduct = ItemDeduction.HealthInsurance.Value
                                            + ItemDeduction.NursingInsurance.Value
                                            + ItemDeduction.WelfareAnnuity.Value
                                            + ItemDeduction.EmploymentInsurance.Value
                                            + ItemDeduction.IncomeTax.Value
                                            + ItemDeduction.MunicipalTax.Value
                                            + ItemDeduction.FriendshipAssociation.Value
                                            + ItemDeduction.YearEndTaxAdjustment.Value
                                            ;

            this.txtTotalSalary.Text = this.Utils.FormatIntToPrice(ItemAllowance.TotalSalary.ToString());
            this.txtTotalDeduct.Text = this.Utils.FormatIntToPrice(ItemDeduction.TotalDeduct.ToString());

            if (isUpdate == true)
            {
                if (this.Utils.ComparePrice(22, ItemDeduction.TotalDeduct.ToString()))
                {
                    this.txtTotalDeduct.BackColor = Color.Pink;
                }
                else
                {
                    this.txtTotalDeduct.BackColor = Color.FromKnownColor(KnownColor.Window);
                }
            }

            // 12.差引支給額
            if (this.ItemAllowance.TotalSalary > 0 || this.ItemDeduction.TotalDeduct > 0)
            {
                this.ItemAllowance.TotalDeductedSalary = this.ItemAllowance.TotalSalary - this.ItemDeduction.TotalDeduct;

                if (this.Utils.ComparePrice(13, (this.ItemAllowance.TotalSalary.Value - this.ItemDeduction.TotalDeduct.Value).ToString()))
                {
                    this.txtTotalDeductedSalary.BackColor = Color.Pink;
                }
                else
                {
                    this.txtTotalDeductedSalary.BackColor = Color.FromKnownColor(KnownColor.Window);
                }

                this.txtTotalDeductedSalary.Text = this.Utils.FormatIntToPrice((ItemAllowance.TotalSalary
                                                 - this.ItemDeduction.TotalDeduct).ToString());
            }
        }


        /// <summary>
        /// 指定した給与明細を取得します。
        /// </summary>
        private void GetSalary()
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                

                var sql = string.Empty;
                var strTargetDate = string.Empty;

                // 対象日付の取得
                if (isDefault)
                {
                    strTargetDate = "9999/99";
                }
                else
                {
                    strTargetDate = this.txtYear.Text + "/" + this.txtMonth.Text;
                }

                // コネクションをオープンします。
                connection.Open();

                var command = new MySqlCommand();

                // バージョン情報取得SQLを実行します。
                command.Connection = connection;

                sql = "SELECT ";
                sql += " SALARY.basic_salary, ";
                sql += " SALARY.executive_allowance, ";
                sql += " SALARY.dependency_allowance, ";
                sql += " SALARY.overtime_allowance, ";
                sql += " SALARY.daysoff_increased, ";
                sql += " SALARY.nightwork_increased, ";
                sql += " SALARY.housing_allowance, ";
                sql += " SALARY.late_absent, ";
                sql += " SALARY.transportation_expenses, ";
                sql += " SALARY.special_allowance, ";
                sql += " SALARY.spare_allowance, ";
                sql += " SALARY.total_salary, ";
                sql += " SALARY.total_deducted_salary, ";
                sql += " DEDUCT.health_insurance, ";
                sql += " DEDUCT.nursing_insurance, ";
                sql += " DEDUCT.welfare_annuity, ";
                sql += " DEDUCT.employment_insurance, ";
                sql += " DEDUCT.income_tax, ";
                sql += " DEDUCT.municipal_tax, ";
                sql += " DEDUCT.friendship_association, ";
                sql += " DEDUCT.year_end_tax_adjustment, ";
                sql += " DEDUCT.total_deduct, ";
                sql += " DUTY.overtime_time, ";
                sql += " DUTY.weekend_worktime, ";
                sql += " DUTY.midnight_worktime, ";
                sql += " DUTY.late_absent, ";
                sql += " DUTY.insurance, ";
                sql += " DUTY.number_of_dependent, ";
                sql += " DUTY.paid_vacation, ";
                sql += " DUTY.working_hours, ";
                sql += " DUTY.workplace, ";
                sql += " SIDE.side_business, ";
                sql += " SIDE.perquisite, ";
                sql += " SIDE.others, ";
                sql += " SIDE.remarks ";
                sql += "FROM ";
                sql += " mylife.t_salary SALARY ";
                sql += "INNER JOIN ";
                sql += " mylife.t_deduct DEDUCT ";
                sql += "ON ";
                sql += " SALARY.ID = DEDUCT.ID ";
                sql += "INNER JOIN ";
                sql += " mylife.t_duty DUTY ";
                sql += "ON ";
                sql += " SALARY.ID = DUTY.ID ";
                sql += "INNER JOIN ";
                sql += " mylife.t_side_business SIDE ";
                sql += "ON ";
                sql += " SALARY.ID = SIDE.ID ";
                sql += "WHERE ";
                sql += " SALARY.work_month = '" + strTargetDate + "' ";

                MySqlCommand selectCommand = new MySqlCommand(sql, connection);

                MySqlDataReader results = selectCommand.ExecuteReader();

                results.Read();

                int id = SetId();

                if (id == 0)
                {
                    // 0件の場合
                    this.Reset();

                    return;
                }

                // リストに代入
                mapRegisteredData.Clear();

                for (int i = 0; i < results.FieldCount; i++)
                {
                    mapRegisteredData.Add(i + 1, results[i].ToString());
                }

                // 01.基本給
                ItemAllowance.BasicSalary = this.Utils.FormatPriceToInt(results[0].ToString());
                this.txtBasicSalary.Text = this.Utils.FormatIntToPrice(ItemAllowance.BasicSalary.Value.ToString());

                // 02.役職手当
                ItemAllowance.ExecutiveAllowance = this.Utils.FormatPriceToInt(results[1].ToString());

                if (ItemAllowance.ExecutiveAllowance.ToString() == "0")
                {
                    this.txtExecutiveAllowance.Text = string.Empty;
                }
                else
                {
                    this.txtExecutiveAllowance.Text = this.Utils.FormatIntToPrice(ItemAllowance.ExecutiveAllowance.ToString());
                }

                // 03.扶養手当
                ItemAllowance.DependencyAllowance = this.Utils.FormatPriceToInt(results[2].ToString());

                if (ItemAllowance.DependencyAllowance.ToString() == "0")
                {
                    this.txtDependencyAllowance.Text = string.Empty;
                }
                else
                {
                    this.txtDependencyAllowance.Text = this.Utils.FormatIntToPrice(ItemAllowance.DependencyAllowance.ToString());
                }

                // 04.時間外手当
                ItemAllowance.OvertimeAllowance = this.Utils.FormatPriceToInt(results[3].ToString());
                this.txtOvertimeAllowance.Text = this.Utils.FormatIntToPrice(ItemAllowance.OvertimeAllowance.ToString());

                // 05.休日割増
                ItemAllowance.DaysoffIncreased = this.Utils.FormatPriceToInt(results[4].ToString());
                this.txtDaysoffIncreased.Text = this.Utils.FormatIntToPrice(ItemAllowance.DaysoffIncreased.ToString());

                // 06.深夜割増
                ItemAllowance.NightworkIncreased = this.Utils.FormatPriceToInt(results[5].ToString());
                this.txtNightworkIncreased.Text = this.Utils.FormatIntToPrice(ItemAllowance.NightworkIncreased.ToString());

                // 07.住宅手当
                ItemAllowance.HousingAllowance = this.Utils.FormatPriceToInt(results[6].ToString());

                if (ItemAllowance.HousingAllowance.ToString() == "0")
                {
                    this.txtHousingAllowance.Text = string.Empty;
                }
                else
                {
                    this.txtHousingAllowance.Text = this.Utils.FormatIntToPrice(ItemAllowance.HousingAllowance.ToString());
                }

                // 08.遅刻早退欠勤
                ItemAllowance.LateAbsent = this.Utils.FormatPriceToInt(results[7].ToString());

                if (ItemAllowance.LateAbsent != 0)
                {
                    this.txtLateAbsent.Text = this.Utils.FormatIntToPrice(ItemAllowance.LateAbsent.ToString());
                }
                else
                {
                    this.txtLateAbsent.Text = "0";
                }

                // 09.交通費
                ItemAllowance.TransportationExpenses = this.Utils.FormatPriceToInt(results[8].ToString());

                if (ItemAllowance.TransportationExpenses != 0)
                {
                    this.txtTransportationExpenses.Text = this.Utils.FormatIntToPrice(ItemAllowance.TransportationExpenses.ToString());
                }
                else
                {
                    this.txtTransportationExpenses.Text = "0";
                }

                // 10.特別手当
                ItemAllowance.SpecialAllowance = this.Utils.FormatPriceToInt(results[9].ToString());

                if (ItemAllowance.SpecialAllowance.ToString() == "0")
                {
                    this.txtSpecialAllowance.Text = string.Empty;
                }
                else
                {
                    this.txtSpecialAllowance.Text = this.Utils.FormatIntToPrice(ItemAllowance.SpecialAllowance.ToString());
                }

                // 11.予備
                ItemAllowance.SpareAllowance = this.Utils.FormatPriceToInt(results[10].ToString());
                this.txtSpareAllowance.Text = this.Utils.FormatIntToPrice(ItemAllowance.SpareAllowance.ToString());

                // 12.支給総計
                ItemAllowance.TotalSalary = this.Utils.FormatPriceToInt(results[11].ToString());
                this.txtTotalSalary.Text = this.Utils.FormatIntToPrice(ItemAllowance.TotalSalary.ToString());

                // 13.差引支給額
                ItemAllowance.TotalDeductedSalary = this.Utils.FormatPriceToInt(results[12].ToString());
                this.txtTotalDeductedSalary.Text = this.Utils.FormatIntToPrice(ItemAllowance.TotalDeductedSalary.ToString());

                // 14.健康保険
                ItemDeduction.HealthInsurance = this.Utils.FormatPriceToInt(results[13].ToString());
                this.txtHealthInsurance.Text = this.Utils.FormatIntToPrice(ItemDeduction.HealthInsurance.ToString());

                // 15.介護保険
                ItemDeduction.NursingInsurance = this.Utils.FormatPriceToInt(results[14].ToString());

                if (ItemDeduction.NursingInsurance.ToString() == "0")
                {
                    this.txtNursingInsurance.Text = string.Empty;
                }
                else
                {
                    this.txtNursingInsurance.Text = this.Utils.FormatIntToPrice(ItemDeduction.NursingInsurance.ToString());
                }

                // 16.厚生年金
                ItemDeduction.WelfareAnnuity = this.Utils.FormatPriceToInt(results[15].ToString());
                this.txtWelfareAnnuity.Text = this.Utils.FormatIntToPrice(ItemDeduction.WelfareAnnuity.ToString());

                // 17.雇用保険
                ItemDeduction.EmploymentInsurance = this.Utils.FormatPriceToInt(results[16].ToString());
                this.txtEmploymentInsurance.Text = this.Utils.FormatIntToPrice(ItemDeduction.EmploymentInsurance.ToString());

                // 18.所得税
                ItemDeduction.IncomeTax = this.Utils.FormatPriceToInt(results[17].ToString());
                this.txtIncomeTax.Text = this.Utils.FormatIntToPrice(ItemDeduction.IncomeTax.ToString());

                // 19.市町村税
                ItemDeduction.MunicipalTax = this.Utils.FormatPriceToInt(results[18].ToString());

                if (ItemDeduction.MunicipalTax.ToString() == "0")
                {
                    this.txtMunicipalTax.Text = string.Empty;
                }
                else
                {
                    this.txtMunicipalTax.Text = this.Utils.FormatIntToPrice(ItemDeduction.MunicipalTax.ToString());
                }

                // 20.互助会
                ItemDeduction.FriendshipAssociation = this.Utils.FormatPriceToInt(results[19].ToString());
                this.txtFriendshipAssociation.Text = this.Utils.FormatIntToPrice(ItemDeduction.FriendshipAssociation.ToString());

                // 21.年末調整他
                ItemDeduction.YearEndTaxAdjustment = this.Utils.FormatPriceToInt(results[20].ToString());

                if (ItemDeduction.YearEndTaxAdjustment.ToString() == "0")
                {
                    this.txtYearEndTaxAdjustment.Text = string.Empty;
                }
                else
                {
                    this.txtYearEndTaxAdjustment.Text = this.Utils.FormatIntToPrice(ItemDeduction.YearEndTaxAdjustment.ToString());
                }

                // 22.控除額計
                ItemDeduction.TotalDeduct = this.Utils.FormatPriceToInt(results[21].ToString());
                this.txtTotalDeduct.Text = this.Utils.FormatIntToPrice(ItemDeduction.TotalDeduct.ToString());

                // 23.時間外時間
                ItemWorkingReferences.OvertimeTime = Convert.ToDouble(results[22]);

                if (ItemWorkingReferences.OvertimeTime.ToString() == "0")
                {
                    this.txtOvertimeTime.Text = string.Empty;
                }
                else
                {
                    this.txtOvertimeTime.Text = this.Utils.FormatIntToPrice(ItemWorkingReferences.OvertimeTime.ToString());
                }

                // 24.休出時間
                ItemWorkingReferences.WeekendWorktime = this.Utils.FormatPriceToInt(results[23].ToString());

                if (ItemWorkingReferences.WeekendWorktime.ToString() == "0")
                {
                    this.txtWeekendWorktime.Text = string.Empty;
                }
                else
                {
                    this.txtWeekendWorktime.Text = this.Utils.FormatIntToPrice(ItemWorkingReferences.WeekendWorktime.ToString());
                }

                // 25.深夜時間
                ItemWorkingReferences.MidnightWorktime = this.Utils.FormatPriceToInt(results[24].ToString());

                if (ItemWorkingReferences.MidnightWorktime.ToString() == "0")
                {
                    this.txtMidnightWorktime.Text = string.Empty;
                }
                else
                {
                    this.txtMidnightWorktime.Text = this.Utils.FormatIntToPrice(ItemWorkingReferences.MidnightWorktime.ToString());
                }

                // 26.遅刻早退欠勤H
                ItemWorkingReferences.LateAbsentH = this.Utils.FormatPriceToInt(results[25].ToString());

                if (ItemWorkingReferences.LateAbsentH.ToString() == "0")
                {
                    this.txtLateAbsentH.Text = string.Empty;
                }
                else
                {
                    this.txtLateAbsentH.Text = this.Utils.FormatIntToPrice(ItemWorkingReferences.LateAbsentH.ToString());
                }

                // 27.支給額-保健
                ItemWorkingReferences.Insurance = this.Utils.FormatPriceToInt(results[26].ToString());
                this.txtInsurance.Text = this.Utils.FormatIntToPrice(ItemWorkingReferences.Insurance.ToString());

                // 28.扶養人数
                ItemWorkingReferences.NumberOfDependent = this.Utils.FormatPriceToInt(results[27].ToString());

                if (ItemWorkingReferences.NumberOfDependent.ToString() == "0")
                {
                    this.txtNumberOfDependent.Text = string.Empty;
                }
                else
                {
                    this.txtNumberOfDependent.Text = this.Utils.FormatIntToPrice(ItemWorkingReferences.NumberOfDependent.ToString());
                }

                // 29.有給残日数
                ItemWorkingReferences.PaidVacation = this.Utils.FormatPriceToInt(results[28].ToString());
                this.txtPaidVacation.Text = this.Utils.FormatIntToPrice(ItemWorkingReferences.PaidVacation.ToString());

                // 30.勤務時間
                ItemWorkingReferences.WorkingHours = Convert.ToDouble(results[29]);
                this.txtWorkingHours.Text = this.Utils.FormatIntToPrice(ItemWorkingReferences.WorkingHours.ToString());

                // 31.勤務先
                ItemWorkingReferences.Workplace = results[30].ToString();
                this.txtWorkplace.Text = ItemWorkingReferences.Workplace;

                // 32.副業
                ItemSideBusiness.SideBusiness = this.Utils.FormatPriceToInt(results[31].ToString());
                this.txtSideBusiness.Text = this.Utils.FormatIntToPrice(ItemSideBusiness.SideBusiness.ToString());

                // 33.臨時収入
                ItemSideBusiness.Perquisite = this.Utils.FormatPriceToInt(results[32].ToString());
                this.txtPerquisite.Text = this.Utils.FormatIntToPrice(ItemSideBusiness.Perquisite.ToString());

                // 34.その他
                ItemSideBusiness.Others = this.Utils.FormatPriceToInt(results[33].ToString());
                this.txtOthers.Text = this.Utils.FormatIntToPrice(ItemSideBusiness.Others.ToString());

                // 35.覚書
                ItemSideBusiness.Remarks = results[34].ToString();

                if (ItemSideBusiness.Remarks.Equals("0"))
                {
                    this.txtRemarks.Text = string.Empty;
                }
                else
                {
                    this.txtRemarks.Text = ItemSideBusiness.Remarks;
                }

                if (isDefault == false && this.txtTotalDeductedSalary.Text != "0")
                {
                    this.btnRegister.Text = "更新";
                    isUpdate = true;
                }
            }
        }


        /// <summary>
        /// 各月の月収と年収を取得します。
        /// </summary>
        private void GetAnnualIncomey()
        {
            string sql;
            string strTargetYear = this.txtYear.Text;
            int totalSalary;
            int totalDeductedSalary;

            // コネクションオブジェクトとコマンドオブジェクトを生成します。
            using (var connection = new MySqlConnection(ConnectionString))
            {
                using (var command = new MySqlCommand())
                {
                    // マップ初期化
                    mapTotalSalary.Clear();
                    mapTotalDeductedSalary.Clear();

                    sumTotal = 0;
                    sumTotalDeducted = 0;

                    // コネクションをオープンします。
                    connection.Open();

                    // データ検索SQLを実行します。
                    command.Connection = connection;

                    sql = "";
                    sql += "SELECT DISTINCT";
                    sql += "   SALARY.work_month";
                    sql += "   , SALARY.total_salary";
                    sql += "   , SALARY.total_deducted_salary";
                    sql += " FROM ";
                    sql += "     mylife.t_salary SALARY ";
                    sql += " LEFT OUTER JOIN ";
                    sql += "     mylife.t_deduct DEDUCT ";
                    sql += " ON ";
                    sql += "     SALARY.ID = DEDUCT.ID ";
                    sql += " LEFT OUTER JOIN ";
                    sql += "     mylife.t_duty DUTY ";
                    sql += " ON ";
                    sql += "     SALARY.ID = DUTY.ID ";
                    sql += " WHERE ";
                    sql += "     SALARY.work_month <> '9999/99' ";
                    sql += " AND ";
                    sql += "     SALARY.work_month LIKE '" + strTargetYear + "%' ";
                    sql += " ORDER BY 1 ";

                    command.CommandText = sql;
                    var reader = command.ExecuteReader();

                    // 件数0は除外
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string work_month = (string)reader["work_month"];
                            int total_salary = (int)reader["total_salary"];
                            int total_deducted_salary = (int)reader["total_deducted_salary"];

                            // 合計額
                            sumTotal = sumTotal + total_salary;
                            sumTotalDeducted = sumTotalDeducted + total_deducted_salary;

                            mapTotalSalary.Add(work_month, total_salary);
                            mapTotalDeductedSalary.Add(work_month, total_deducted_salary);
                        }
                    }

                    // 1月
                    if (mapTotalDeductedSalary.ContainsKey(strTargetYear + "/01"))
                    {

                        // 値が存在すればテキストボックスに詰めていく
                        totalSalary = mapTotalSalary[strTargetYear + "/01"];
                        totalDeductedSalary = mapTotalDeductedSalary[strTargetYear + "/01"];

                        if (totalDeductedSalary == 0)
                        {
                            this.txtJanuary_Total.Text = "0";
                            this.txtJanuary_TotalDeducted.Text = "0";
                        }
                        else
                        {
                            this.txtJanuary_Total.Text = this.Utils.FormatIntToPrice(totalSalary.ToString());
                            this.txtJanuary_TotalDeducted.Text = this.Utils.FormatIntToPrice(totalDeductedSalary.ToString());
                        }
                    }
                    else
                    {
                        this.txtJanuary_Total.Text = "";
                        this.txtJanuary_TotalDeducted.Text = "";
                    }

                    // 2月
                    if (mapTotalDeductedSalary.ContainsKey(strTargetYear + "/02"))
                    {

                        // 値が存在すればテキストボックスに詰めていく
                        totalSalary = mapTotalSalary[strTargetYear + "/02"];
                        totalDeductedSalary = mapTotalDeductedSalary[strTargetYear + "/02"];

                        if (totalDeductedSalary == 0)
                        {
                            this.txtFeburary_Total.Text = "0";
                            this.txtFeburary_TotalDeducted.Text = "0";
                        }
                        else
                        {
                            this.txtFeburary_Total.Text = this.Utils.FormatIntToPrice(totalSalary.ToString());
                            this.txtFeburary_TotalDeducted.Text = this.Utils.FormatIntToPrice(totalDeductedSalary.ToString());
                        }
                    }
                    else
                    {
                        this.txtFeburary_Total.Text = "";
                        this.txtFeburary_TotalDeducted.Text = "";
                    }

                    // 3月
                    if (mapTotalDeductedSalary.ContainsKey(strTargetYear + "/03"))
                    {

                        // 値が存在すればテキストボックスに詰めていく
                        totalSalary = mapTotalSalary[strTargetYear + "/03"];
                        totalDeductedSalary = mapTotalDeductedSalary[strTargetYear + "/03"];

                        if (totalDeductedSalary == 0)
                        {
                            this.txtMarch_Total.Text = "0";
                            this.txtMarch_TotalDeducted.Text = "0";
                        }
                        else
                        {
                            this.txtMarch_Total.Text = this.Utils.FormatIntToPrice(totalSalary.ToString());
                            this.txtMarch_TotalDeducted.Text = this.Utils.FormatIntToPrice(totalDeductedSalary.ToString());
                        }
                    }
                    else
                    {
                        this.txtMarch_Total.Text = "";
                        this.txtMarch_TotalDeducted.Text = "";
                    }

                    // 4月
                    if (mapTotalDeductedSalary.ContainsKey(strTargetYear + "/04"))
                    {

                        // 値が存在すればテキストボックスに詰めていく
                        totalSalary = mapTotalSalary[strTargetYear + "/04"];
                        totalDeductedSalary = mapTotalDeductedSalary[strTargetYear + "/04"];

                        if (totalDeductedSalary == 0)
                        {
                            this.txtApril_Total.Text = "0";
                            this.txtApril_TotalDeducted.Text = "0";
                        }
                        else
                        {
                            this.txtApril_Total.Text = this.Utils.FormatIntToPrice(totalSalary.ToString());
                            this.txtApril_TotalDeducted.Text = this.Utils.FormatIntToPrice(totalDeductedSalary.ToString());
                        }
                    }
                    else
                    {
                        this.txtApril_Total.Text = "";
                        this.txtApril_TotalDeducted.Text = "";
                    }

                    // 5月
                    if (mapTotalDeductedSalary.ContainsKey(strTargetYear + "/05"))
                    {

                        // 値が存在すればテキストボックスに詰めていく
                        totalSalary = mapTotalSalary[strTargetYear + "/05"];
                        totalDeductedSalary = mapTotalDeductedSalary[strTargetYear + "/05"];

                        if (totalDeductedSalary == 0)
                        {
                            this.txtMay_Total.Text = "0";
                            this.txtMay_TotalDeducted.Text = "0";
                        }
                        else
                        {
                            this.txtMay_Total.Text = this.Utils.FormatIntToPrice(totalSalary.ToString());
                            this.txtMay_TotalDeducted.Text = this.Utils.FormatIntToPrice(totalDeductedSalary.ToString());
                        }
                    }
                    else
                    {
                        this.txtMay_Total.Text = "";
                        this.txtMay_TotalDeducted.Text = "";
                    }

                    // 6月
                    if (mapTotalDeductedSalary.ContainsKey(strTargetYear + "/06"))
                    {
                        // 値が存在すればテキストボックスに詰めていく
                        totalSalary = mapTotalSalary[strTargetYear + "/06"];
                        totalDeductedSalary = mapTotalDeductedSalary[strTargetYear + "/06"];

                        if (totalDeductedSalary == 0)
                        {
                            this.txtJune_Total.Text = "0";
                            this.txtJune_TotalDeducted.Text = "0";
                        }
                        else
                        {
                            this.txtJune_Total.Text = this.Utils.FormatIntToPrice(totalSalary.ToString());
                            this.txtJune_TotalDeducted.Text = this.Utils.FormatIntToPrice(totalDeductedSalary.ToString());
                        }
                    }
                    else
                    {
                        this.txtJune_Total.Text = "";
                        this.txtJune_TotalDeducted.Text = "";
                    }

                    // 7月
                    if (mapTotalDeductedSalary.ContainsKey(strTargetYear + "/07"))
                    {

                        // 値が存在すればテキストボックスに詰めていく
                        totalSalary = mapTotalSalary[strTargetYear + "/07"];
                        totalDeductedSalary = mapTotalDeductedSalary[strTargetYear + "/07"];

                        if (totalDeductedSalary == 0)
                        {
                            this.txtJuly_Total.Text = "0";
                            this.txtJuly_TotalDeducted.Text = "0";
                        }
                        else
                        {
                            this.txtJuly_Total.Text = this.Utils.FormatIntToPrice(totalSalary.ToString());
                            this.txtJuly_TotalDeducted.Text = this.Utils.FormatIntToPrice(totalDeductedSalary.ToString());
                        }
                    }
                    else
                    {
                        this.txtJuly_Total.Text = "";
                        this.txtJuly_TotalDeducted.Text = "";
                    }

                    // 8月
                    if (mapTotalDeductedSalary.ContainsKey(strTargetYear + "/08"))
                    {

                        // 値が存在すればテキストボックスに詰めていく
                        totalSalary = mapTotalSalary[strTargetYear + "/08"];
                        totalDeductedSalary = mapTotalDeductedSalary[strTargetYear + "/08"];

                        if (totalDeductedSalary == 0)
                        {
                            this.txtAugust_Total.Text = "0";
                            this.txtAugust_TotalDeducted.Text = "0";
                        }
                        else
                        {
                            this.txtAugust_Total.Text = this.Utils.FormatIntToPrice(totalSalary.ToString());
                            this.txtAugust_TotalDeducted.Text = this.Utils.FormatIntToPrice(totalDeductedSalary.ToString());
                        }
                    }
                    else
                    {
                        this.txtAugust_Total.Text = "";
                        this.txtAugust_TotalDeducted.Text = "";
                    }

                    // 9月
                    if (mapTotalDeductedSalary.ContainsKey(strTargetYear + "/09"))
                    {

                        // 値が存在すればテキストボックスに詰めていく
                        totalSalary = mapTotalSalary[strTargetYear + "/09"];
                        totalDeductedSalary = mapTotalDeductedSalary[strTargetYear + "/09"];

                        if (totalDeductedSalary == 0)
                        {
                            this.txtSeptember_Total.Text = "0";
                            this.txtSeptember_TotalDeducted.Text = "0";
                        }
                        else
                        {
                            this.txtSeptember_Total.Text = this.Utils.FormatIntToPrice(totalSalary.ToString());
                            this.txtSeptember_TotalDeducted.Text = this.Utils.FormatIntToPrice(totalDeductedSalary.ToString());
                        }
                    }
                    else
                    {
                        this.txtSeptember_Total.Text = "";
                        this.txtSeptember_TotalDeducted.Text = "";
                    }

                    // 10月
                    if (mapTotalDeductedSalary.ContainsKey(strTargetYear + "/10"))
                    {

                        // 値が存在すればテキストボックスに詰めていく
                        totalSalary = mapTotalSalary[strTargetYear + "/10"];
                        totalDeductedSalary = mapTotalDeductedSalary[strTargetYear + "/10"];

                        if (totalDeductedSalary == 0)
                        {
                            this.txtOctober_Total.Text = "0";
                            this.txtOctober_TotalDeducted.Text = "0";
                        }
                        else
                        {
                            this.txtOctober_Total.Text = this.Utils.FormatIntToPrice(totalSalary.ToString());
                            this.txtOctober_TotalDeducted.Text = this.Utils.FormatIntToPrice(totalDeductedSalary.ToString());
                        }
                    }
                    else
                    {
                        this.txtOctober_Total.Text = "";
                        this.txtOctober_TotalDeducted.Text = "";
                    }

                    // 11月
                    if (mapTotalDeductedSalary.ContainsKey(strTargetYear + "/11"))
                    {

                        // 値が存在すればテキストボックスに詰めていく
                        totalSalary = mapTotalSalary[strTargetYear + "/11"];
                        totalDeductedSalary = mapTotalDeductedSalary[strTargetYear + "/11"];

                        if (totalDeductedSalary == 0)
                        {
                            this.txtNovember_Total.Text = "0";
                            this.txtNovember_TotalDeducted.Text = "0";
                        }
                        else
                        {
                            this.txtNovember_Total.Text = this.Utils.FormatIntToPrice(totalSalary.ToString());
                            this.txtNovember_TotalDeducted.Text = this.Utils.FormatIntToPrice(totalDeductedSalary.ToString());
                        }
                    }
                    else
                    {
                        this.txtNovember_Total.Text = "";
                        this.txtNovember_TotalDeducted.Text = "";
                    }

                    // 12月
                    if (mapTotalDeductedSalary.ContainsKey(strTargetYear + "/12"))
                    {

                        // 値が存在すればテキストボックスに詰めていく
                        totalSalary = mapTotalSalary[strTargetYear + "/12"];
                        totalDeductedSalary = mapTotalDeductedSalary[strTargetYear + "/12"];

                        if (totalDeductedSalary == 0)
                        {
                            this.txtDecember_Total.Text = "0";
                            this.txtDecember_TotalDeducted.Text = "0";
                        }
                        else
                        {
                            this.txtDecember_Total.Text = this.Utils.FormatIntToPrice(totalSalary.ToString());
                            this.txtDecember_TotalDeducted.Text = this.Utils.FormatIntToPrice(totalDeductedSalary.ToString());
                        }
                    }
                    else
                    {
                        this.txtDecember_Total.Text = "";
                        this.txtDecember_TotalDeducted.Text = "";
                    }

                    // 合計
                    this.txtSum_Total.Text = this.Utils.FormatIntToPrice(sumTotal.ToString());
                    this.txtSum_TotalDeducted.Text = this.Utils.FormatIntToPrice(sumTotalDeducted.ToString());

                    // 年号取得
                    System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("ja-JP", false);
                    ci.DateTimeFormat.Calendar = new System.Globalization.JapaneseCalendar();
                    DateTime date = new DateTime(Convert.ToInt32(this.txtYear.Text), Convert.ToInt32(this.txtMonth.Text), 1);
                    var datestr = date.ToString("gy", ci);

                    this.lblTargetYear.Text = this.txtYear.Text + "年 (" + datestr + "年)";
                }
            }
        }

        /// <summary>
        /// Clear - 入力項目
        /// </summary>
        private void ClearAll()
        {
            // Clear - 合計金額
            mapTotalSalary.Clear();

            // Clear - 差引合計
            mapTotalDeductedSalary.Clear();

            // 支給額
            this.txtBasicSalary.Text = "0";
            this.txtExecutiveAllowance.Text = string.Empty;
            this.txtDependencyAllowance.Text = string.Empty;
            this.txtOvertimeAllowance.Text = "0";
            this.txtDaysoffIncreased.Text = "0";
            this.txtNightworkIncreased.Text = "0";
            this.txtHousingAllowance.Text = string.Empty;
            this.txtLateAbsent.Text = "0";
            this.txtTransportationExpenses.Text = "0";
            this.txtSpecialAllowance.Text = string.Empty;
            this.txtSpareAllowance.Text = "0";
            this.txtTotalSalary.Text = "0";

            // 控除額
            this.txtHealthInsurance.Text = "0";
            this.txtNursingInsurance.Text = "";
            this.txtWelfareAnnuity.Text = "0";
            this.txtEmploymentInsurance.Text = "0";
            this.txtIncomeTax.Text = "0";
            this.txtMunicipalTax.Text = "";
            this.txtFriendshipAssociation.Text = "0";
            this.txtYearEndTaxAdjustment.Text = "";
            this.txtTotalDeduct.Text = "0";
            this.txtTotalDeductedSalary.Text = "";

            // 勤務備考
            this.txtOvertimeTime.Text = "";
            this.txtWeekendWorktime.Text = "";
            this.txtMidnightWorktime.Text = "";
            this.txtLateAbsent.Text = "";
            this.txtInsurance.Text = "0";
            this.txtNumberOfDependent.Text = "";
            this.txtPaidVacation.Text = "0";
            this.txtWorkingHours.Text = "0";
            this.txtWorkplace.Text = "";

            // 備考など
            this.txtSideBusiness.Text = "0";
            this.txtPerquisite.Text = "0";
            this.txtOthers.Text = "0";
            this.txtRemarks.Text = "";

            SetDefaultColor();
        }

        /// <summary>
        /// Set Default Color
        /// </summary>
        private void SetDefaultColor()
        {
            var knownColor = Color.FromKnownColor(KnownColor.Window);

            // 支給額
            this.txtBasicSalary.BackColor = knownColor;
            this.txtExecutiveAllowance.BackColor = knownColor;
            this.txtDependencyAllowance.BackColor = knownColor;
            this.txtOvertimeAllowance.BackColor = knownColor;
            this.txtDaysoffIncreased.BackColor = knownColor;
            this.txtNightworkIncreased.BackColor = knownColor;
            this.txtHousingAllowance.BackColor = knownColor;
            this.txtLateAbsent.BackColor = knownColor;
            this.txtTransportationExpenses.BackColor = knownColor;
            this.txtSpecialAllowance.BackColor = knownColor;
            this.txtSpareAllowance.BackColor = knownColor;
            this.txtTotalSalary.BackColor = knownColor;
            this.txtTotalDeductedSalary.BackColor = knownColor;

            // 控除額
            this.txtHealthInsurance.BackColor = knownColor;
            this.txtNursingInsurance.BackColor = knownColor;
            this.txtWelfareAnnuity.BackColor = knownColor;
            this.txtEmploymentInsurance.BackColor = knownColor;
            this.txtIncomeTax.BackColor = knownColor;
            this.txtMunicipalTax.BackColor = knownColor;
            this.txtFriendshipAssociation.BackColor = knownColor;
            this.txtYearEndTaxAdjustment.BackColor = knownColor;
            this.txtTotalDeduct.BackColor = knownColor;

            // 勤務備考
            this.txtOvertimeTime.BackColor = knownColor;
            this.txtWeekendWorktime.BackColor = knownColor;
            this.txtMidnightWorktime.BackColor = knownColor;
            this.txtLateAbsent.BackColor = knownColor;
            this.txtInsurance.BackColor = knownColor;
            this.txtNumberOfDependent.BackColor = knownColor;
            this.txtPaidVacation.BackColor = knownColor;
            this.txtWorkingHours.BackColor = knownColor;
            this.txtWorkplace.BackColor = knownColor;

            // 副業など
            this.txtSideBusiness.BackColor = knownColor;
            this.txtPerquisite.BackColor = knownColor;
            this.txtOthers.BackColor = knownColor;
            this.txtRemarks.BackColor = knownColor;
        }

        /// <summary>
        /// 画面を閉じます。
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult confirmClose = MessageBox.Show("終了しますか？",
                                                "確認",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Information,
                                                MessageBoxDefaultButton.Button2);

            if (confirmClose != DialogResult.Yes)
            {
                return;
            }

            if (isConfirmAllow && !isRegister)
            {
                DialogResult confirmClose2 = MessageBox.Show("登録もしくは更新がされていない状態です。本当に終了しますか？",
                                            "確認",
                                            MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Information,
                                            MessageBoxDefaultButton.Button2);
                if (confirmClose2 == DialogResult.Yes)
                {
                    Close();
                }
            }
            else
            {
                Close();
            }
        }

        /// <summary>
        /// 新規登録時のデフォルト明細を設定する
        /// </summary>
        private void btnSetDefault_Click(object sender, EventArgs e)
        {
            isDefault = true;
            int num = SetId();

            if (num == 0)
            {
                Register();
            }
            else
            {
                // 更新
                this.Update(
                    // 対象年 - Text
                    this.txtYear.Text,
                    // 対象月 - Text
                    this.txtMonth.Text
                );
            }

            isDefault = false;
        }

        private void chkConfirm_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkConfirm.Checked)
            {
                isConfirmAllow = true;

                return;
            }

            isConfirmAllow = false;
        }

        /// <summary>
        /// 前年と金額比較する
        /// </summary>
        /// <param name="targetColumnName">対象列名</param>
        private void CompareWithPreviousYear(string targetColumnName, string strTargetDate, string strTargetPreviousDate)
        {
            string sql;

            // マップ初期化
            mapCompareToPreviousYear.Clear();

            if (ItemAllowance.TotalDeductedSalary.Value <= 0)
            {
                return;
            }

            // 支給額
            try
            {
                // コネクションオブジェクトとコマンドオブジェクトを生成します。
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    using (var command = new MySqlCommand())
                    {
                        // コネクションをオープンします。
                        connection.Open();

                        // データ検索SQLを実行します。
                        command.Connection = connection;


                        sql = "";
                        sql += "SELECT DISTINCT";
                        sql += "   SALARY.work_month";                         // 01.対象日付
                        sql += "   , " + targetColumnName + " AS targetcol";   // 02.対象月(今年と去年)の金額
                        sql += " FROM ";
                        sql += "     mylife.t_salary SALARY ";
                        sql += " LEFT OUTER JOIN ";
                        sql += "     mylife.t_deduct DEDUCT ";
                        sql += " ON ";
                        sql += "     SALARY.ID = DEDUCT.ID ";
                        sql += " LEFT OUTER JOIN ";
                        sql += "     mylife.t_duty DUTY ";
                        sql += " ON ";
                        sql += "     SALARY.ID = DUTY.ID ";
                        sql += " LEFT OUTER JOIN ";
                        sql += "     mylife.t_side_business SIDEBUS ";
                        sql += " ON ";
                        sql += "     SALARY.ID = SIDEBUS.ID ";
                        sql += " WHERE ";
                        sql += "    SALARY.work_month = '" + strTargetDate + "' ";
                        sql += " OR ";
                        sql += "    SALARY.work_month = '" + strTargetPreviousDate + "' ";

                        command.CommandText = sql;
                        var reader = command.ExecuteReader();

                        // 件数0は除外
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string work_month = (string)reader["work_month"];
                                int money = (int)reader["targetcol"];

                                mapCompareToPreviousYear.Add(work_month, money);
                            }
                        }
                    }

                    targetMoney = mapCompareToPreviousYear[strTargetDate];
                    targetPreviousMoney = mapCompareToPreviousYear[strTargetPreviousDate];

                    if (targetMoney > targetPreviousMoney)
                    {
                        // 去年よりも増↑
                        this.lblStatus.ForeColor = Color.Red;
                        this.lblStatus.Text = "前年比： \\" + this.Utils.FormatIntToPrice((targetMoney - targetPreviousMoney).ToString()) + " UP";
                    }
                    else if (targetMoney < targetPreviousMoney)
                    {
                        // 去年よりも減↓
                        this.lblStatus.ForeColor = Color.Blue;
                        this.lblStatus.Text = "前年比： \\" + this.Utils.FormatIntToPrice((targetPreviousMoney - targetMoney).ToString()) + " DOWN";
                    }
                    else
                    {
                        // 去年と同額
                        this.lblStatus.ForeColor = Color.Green;
                        this.lblStatus.Text = "前年比： \\0";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// 年 - Leave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtYear_Leave(object sender, EventArgs e)
        {
            this.Reload();
        }

        /// <summary>
        /// 月 - Leave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMonth_Leave(object sender, EventArgs e)
        {
            this.Reload();
        }

        /// <summary>
        /// Reload
        /// </summary>
        private void Reload()
        {
            using (var waiting = new MouseCursorChangeWaiting())
            {
                if (this.TargetDate.ToString() == this.txtYear.Text + "/" + this.txtMonth.Text)
                {
                    return;
                }

                // Reset
                this.Reset();

                try
                {
                    // 年収取得
                    GetAnnualIncomey();

                    // 給与明細の取得
                    GetSalary();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// 項目リセット
        /// </summary>
        public void Reset()
        {
            // 色のリセット
            SetDefaultColor();

            // 支給額 - Reset
            ItemAllowance.Reset();

            // 控除額 - Reset
            ItemDeduction.Reset();

            // 勤務備考 - Reset
            ItemWorkingReferences.Reset();

            // 副業 - Reset
            ItemSideBusiness.Reset();
        }

        private void tabPage1_MouseEnter(object sender, EventArgs e)
        {
            this.lblStatus.ForeColor = Color.Black;
            this.lblStatus.Text = "テキストボックスにカーソルを合わせると、前年と金額比較できます(対象日付が両方とも登録されている金額入力欄のみ有効)。";
        }

        private void txtTotalDeduct_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/" + this.txtMonth.Text;
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/" + this.txtMonth.Text;
            CompareWithPreviousYear("DEDUCT.total_deduct", strTargetDate, strTargetPreviousDate);
        }

        

        private void txtSideBusiness_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/" + this.txtMonth.Text;
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/" + this.txtMonth.Text;
            CompareWithPreviousYear("SIDEBUS.side_business", strTargetDate, strTargetPreviousDate);
        }

        /// <summary>
        /// MouseEnter - 臨時収入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPerquisite_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/" + this.txtMonth.Text;
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/" + this.txtMonth.Text;
            CompareWithPreviousYear("SIDEBUS.perquisite", strTargetDate, strTargetPreviousDate);
        }

        /// <summary>
        /// MouseEnter - その他
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOthers_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/" + this.txtMonth.Text;
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/" + this.txtMonth.Text;
            CompareWithPreviousYear("SIDEBUS.others", strTargetDate, strTargetPreviousDate);
        }

        /// <summary>
        /// MouseEnter - 額面 - 1月
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtJanuary_Total_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/01";
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/01";
            CompareWithPreviousYear("SALARY.total_salary", strTargetDate, strTargetPreviousDate);
        }

        /// <summary>
        /// MouseEnter - 額面 - 2月
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFeburary_Total_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/02";
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/02";
            CompareWithPreviousYear("SALARY.total_salary", strTargetDate, strTargetPreviousDate);
        }

        /// <summary>
        /// MouseEnter - 額面 - 3月
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMarch_Total_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/03";
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/03";
            CompareWithPreviousYear("SALARY.total_salary", strTargetDate, strTargetPreviousDate);
        }

        /// <summary>
        /// MouseEnter - 額面 - 4月
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtApril_Total_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/04";
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/04";
            CompareWithPreviousYear("SALARY.total_salary", strTargetDate, strTargetPreviousDate);
        }

        /// <summary>
        /// MouseEnter - 額面 - 5月
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMay_Total_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/05";
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/05";
            CompareWithPreviousYear("SALARY.total_salary", strTargetDate, strTargetPreviousDate);
        }

        /// <summary>
        ///  MouseEnter - 額面 - 6月
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtJune_Total_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/06";
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/06";
            CompareWithPreviousYear("SALARY.total_salary", strTargetDate, strTargetPreviousDate);
        }

        // 額面(7月)
        private void txtJuly_Total_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/07";
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/07";
            CompareWithPreviousYear("SALARY.total_salary", strTargetDate, strTargetPreviousDate);
        }

        // 額面(8月)
        private void txtAugust_Total_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/08";
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/08";
            CompareWithPreviousYear("SALARY.total_salary", strTargetDate, strTargetPreviousDate);
        }

        // 額面(9月)
        private void txtSeptember_Total_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/09";
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/09";
            CompareWithPreviousYear("SALARY.total_salary", strTargetDate, strTargetPreviousDate);
        }

        // 額面(10月)
        private void txtOctober_Total_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/10";
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/10";
            CompareWithPreviousYear("SALARY.total_salary", strTargetDate, strTargetPreviousDate);
        }

        // 額面(11月)
        private void txtNovember_Total_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/11";
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/11";
            CompareWithPreviousYear("SALARY.total_salary", strTargetDate, strTargetPreviousDate);
        }

        // 額面(12月)
        private void txtDecember_Total_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/12";
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/12";
            CompareWithPreviousYear("SALARY.total_salary", strTargetDate, strTargetPreviousDate);
        }

        // 手取(1月)
        private void txtJanuary_TotalDeducted_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/01";
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/01";
            CompareWithPreviousYear("SALARY.total_deducted_salary", strTargetDate, strTargetPreviousDate);
        }

        // 手取(2月)
        private void txtFeburary_TotalDeducted_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/02";
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/02";
            CompareWithPreviousYear("SALARY.total_deducted_salary", strTargetDate, strTargetPreviousDate);
        }

        // 手取(3月)
        private void txtMarch_TotalDeducted_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/03";
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/03";
            CompareWithPreviousYear("SALARY.total_deducted_salary", strTargetDate, strTargetPreviousDate);
        }

        // 手取(4月)
        private void txtApril_TotalDeducted_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/04";
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/04";
            CompareWithPreviousYear("SALARY.total_deducted_salary", strTargetDate, strTargetPreviousDate);
        }

        // 手取(5月)
        private void txtMay_TotalDeducted_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/05";
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/05";
            CompareWithPreviousYear("SALARY.total_deducted_salary", strTargetDate, strTargetPreviousDate);
        }

        // 手取(6月)
        private void txtJune_TotalDeducted_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/06";
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/06";
            CompareWithPreviousYear("SALARY.total_deducted_salary", strTargetDate, strTargetPreviousDate);
        }

        // 手取(7月)
        private void txtJuly_TotalDeducted_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/07";
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/07";
            CompareWithPreviousYear("SALARY.total_deducted_salary", strTargetDate, strTargetPreviousDate);
        }

        // 手取(8月)
        private void txtAugust_TotalDeducted_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/08";
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/08";
            CompareWithPreviousYear("SALARY.total_deducted_salary", strTargetDate, strTargetPreviousDate);
        }

        // 手取(9月)
        private void txtSeptember_TotalDeducted_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/09";
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/09";
            CompareWithPreviousYear("SALARY.total_deducted_salary", strTargetDate, strTargetPreviousDate);
        }

        // 手取(10月)
        private void txtOctober_TotalDeducted_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/10";
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/10";
            CompareWithPreviousYear("SALARY.total_deducted_salary", strTargetDate, strTargetPreviousDate);
        }

        // 手取(11月)
        private void txtNovember_TotalDeducted_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/11";
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/11";
            CompareWithPreviousYear("SALARY.total_deducted_salary", strTargetDate, strTargetPreviousDate);
        }

        // 手取(12月)
        private void txtDecember_TotalDeducted_MouseEnter(object sender, EventArgs e)
        {
            var strTargetDate = this.txtYear.Text + "/12";
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/12";
            CompareWithPreviousYear("SALARY.total_deducted_salary", strTargetDate, strTargetPreviousDate);
        }

        private void txtYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Utils.IsInputValNumeric(e);
        }

        private void txtMonth_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Utils.IsInputValNumeric(e);
        }

        private void txtSum_Total_MouseEnter(object sender, EventArgs e)
        {
            SumPreviousTotal("total_salary");
        }

        private void txtSum_TotalDeducted_MouseEnter(object sender, EventArgs e)
        {
            SumPreviousTotal("total_deducted_salary");
        }

        private void SumPreviousTotal(string displaySort)
        {
            // 支給額
            string SelectSql;
            this.TargetYear = (Convert.ToInt32(this.txtYear.Text) - 1);
            this.TargetMonth = 1;

            try
            {
                // コネクションオブジェクトとコマンドオブジェクトを生成します。
                using (var connection = new MySqlConnection(ConnectionString))
                using (var command = new MySqlCommand())
                {
                    // 昨年
                    var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString();

                    // マップ初期化
                    mapTotalSalary.Clear();
                    mapTotalDeductedSalary.Clear();

                    sumTotal = 0;
                    sumTotalDeducted = 0;

                    // コネクションをオープンします。
                    connection.Open();

                    // データ検索SQLを実行します。
                    command.Connection = connection;

                    SelectSql = "";
                    SelectSql = SelectSql + " SELECT ";
                    SelectSql = SelectSql + "   SALARY.year";
                    SelectSql = SelectSql + "   , SALARY.total_salary";
                    SelectSql = SelectSql + "   , SALARY.total_deducted_salary";
                    SelectSql = SelectSql + " FROM ";
                    SelectSql = SelectSql + "     mylife.t_salary SALARY ";
                    SelectSql = SelectSql + " LEFT OUTER JOIN ";
                    SelectSql = SelectSql + "     mylife.t_deduct DEDUCT ";
                    SelectSql = SelectSql + " ON ";
                    SelectSql = SelectSql + "     SALARY.ID = DEDUCT.ID ";
                    SelectSql = SelectSql + " LEFT OUTER JOIN ";
                    SelectSql = SelectSql + "     mylife.t_duty DUTY ";
                    SelectSql = SelectSql + " ON ";
                    SelectSql = SelectSql + "     SALARY.ID = DUTY.ID ";
                    SelectSql = SelectSql + " WHERE ";
                    SelectSql = SelectSql + "     SALARY.year = '" + strTargetPreviousDate + "' ";
                    SelectSql = SelectSql + " GROUP BY year ";

                    command.CommandText = SelectSql;

                    var reader = command.ExecuteReader();

                    // 件数0は除外
                    if (reader.HasRows)
                    {
                        reader.Read();

                        int lastTotalSalary = (int)reader["total_salary"];
                        int lastTotalDeductedSalary = (int)reader["total_deducted_salary"];

                        int moneyDiffTotal = this.Utils.FormatPriceToInt(txtSum_Total.Text);
                        int moneyDiffDeductTotal = this.Utils.FormatPriceToInt(txtTotalDeductedSalary.Text);

                        if (displaySort == "total_salary")
                        {
                            // 年収
                            if (moneyDiffTotal > lastTotalSalary)
                            {
                                // 去年よりも増↑
                                this.lblStatus.ForeColor = Color.Red;
                                this.lblStatus.Text = "前年比： \\" + this.Utils.FormatIntToPrice((moneyDiffTotal - lastTotalSalary).ToString()) + " UP";
                            }
                            else if (targetMoney < targetPreviousMoney)
                            {
                                // 去年よりも減↓
                                this.lblStatus.ForeColor = Color.Blue;
                                this.lblStatus.Text = "前年比： \\" + this.Utils.FormatIntToPrice((lastTotalSalary - moneyDiffTotal).ToString()) + " DOWN";
                            }
                            else
                            {
                                // 去年と同額
                                this.lblStatus.ForeColor = Color.Green;
                                this.lblStatus.Text = "前年比： \\0";
                            }
                        }
                        else
                        {
                            // 手取
                            if (moneyDiffDeductTotal > lastTotalDeductedSalary)
                            {
                                // 去年よりも増↑
                                this.lblStatus.ForeColor = Color.Red;
                                this.lblStatus.Text = "前年比： \\" + this.Utils.FormatIntToPrice((moneyDiffDeductTotal - lastTotalDeductedSalary).ToString()) + " UP";
                            }
                            else if (moneyDiffDeductTotal < lastTotalDeductedSalary)
                            {
                                // 去年よりも減↓
                                this.lblStatus.ForeColor = Color.Blue;
                                this.lblStatus.Text = "前年比： \\" + this.Utils.FormatIntToPrice((lastTotalDeductedSalary - moneyDiffDeductTotal).ToString()) + " DOWN";
                            }
                            else
                            {
                                // 去年と同額
                                this.lblStatus.ForeColor = Color.Green;
                                this.lblStatus.Text = "前年比： \\0";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            {
                DialogResult confirm = MessageBox.Show("Excel出力を行いますか？",
                                                    "確認",
                                                    MessageBoxButtons.YesNo,
                                                    MessageBoxIcon.Information,
                                                    MessageBoxDefaultButton.Button2);
                if (confirm == DialogResult.Yes)
                {
                    // データがあるか検索
                    ExcelOutput();
                }
            }
        }

        private void ExcelOutput()
        {
            string SelectSql;

            // 元のカーソルを保持
            Cursor preCursor = Cursor.Current;

            // カーソルを待機カーソルに変更
            Cursor.Current = Cursors.WaitCursor;

            this.lblStatus.Text = "出力中です。。。";

            //Excelオブジェクトの初期化
            Excel.Application ExcelApp = null;
            Excel.Workbooks wbs = null;
            Excel.Workbook wb = null;
            Excel.Sheets shs = null;
            Excel.Worksheet ws = null;

            //Excelシートのインスタンスを作る
            ExcelApp = new Excel.Application();

            wbs = ExcelApp.Workbooks;

            try
            {
                // コネクションオブジェクトとコマンドオブジェクトを生成します。
                using (var connection = new MySqlConnection(ConnectionString))
                using (var command = new MySqlCommand())
                {
                    // コネクションをオープンします。
                    connection.Open();

                    // データ検索SQLを実行します。
                    command.Connection = connection;

                    MySqlCommand selectCommand = new MySqlCommand(" SELECT  " +
                                                                    "     SALARY.basic_salary " +                 // 01.基本給
                                                                    "     , SALARY.executive_allowance " +        // 02.役職手当
                                                                    "     , SALARY.dependency_allowance " +       // 03.扶養手当
                                                                    "     , SALARY.overtime_allowance " +         // 04.時間外手当
                                                                    "     , SALARY.daysoff_increased " +          // 05.休日割増
                                                                    "     , SALARY.nightwork_increased " +        // 06.深夜割増
                                                                    "     , SALARY.housing_allowance " +          // 07.住宅手当
                                                                    "     , SALARY.late_absent " +                // 08.遅刻早退欠勤
                                                                    "     , SALARY.transportation_expenses " +    // 09.交通費
                                                                    "     , SALARY.special_allowance " +          // 10.特別手当
                                                                    "     , SALARY.spare_allowance " +            // 11.予備
                                                                    "     , SALARY.total_salary " +               // 12.支給総計
                                                                    "     , SALARY.total_deducted_salary " +      // 13.差引支給額
                                                                    "     , DEDUCT.health_insurance " +           // 14.健康保険
                                                                    "     , DEDUCT.nursing_insurance " +          // 15.介護保険
                                                                    "     , DEDUCT.welfare_annuity " +            // 16.厚生年金
                                                                    "     , DEDUCT.employment_insurance " +       // 17.雇用保険
                                                                    "     , DEDUCT.income_tax " +                 // 18.所得税
                                                                    "     , DEDUCT.municipal_tax" +               // 19.市町村税
                                                                    "     , DEDUCT.friendship_association " +     // 20.互助会
                                                                    "     , DEDUCT.year_end_tax_adjustment " +    // 21.年末調整他
                                                                    "     , DEDUCT.total_deduct " +               // 22.控除額計
                                                                    "     , DUTY.overtime_time " +                // 23.時間外時間
                                                                    "     , DUTY.weekend_worktime " +             // 24.休出時間
                                                                    "     , DUTY.midnight_worktime " +            // 25.深夜時間
                                                                    "     , DUTY.late_absent " +                  // 26.遅刻早退欠勤H
                                                                    "     , DUTY.insurance " +                    // 27.支給額-保健
                                                                    "     , DUTY.number_of_dependent " +          // 28.扶養人数
                                                                    "     , DUTY.paid_vacation " +                // 29.有給残日数
                                                                    "     , DUTY.working_hours " +                // 30.勤務時間
                                                                    "     , DUTY.workplace " +                    // 31.勤務先
                                                                    "     , SIDE.side_business " +                // 32.副業
                                                                    "     , SIDE.perquisite " +                   // 33.臨時収入
                                                                    "     , SIDE.others " +                       // 34.その他
                                                                    "     , SIDE.remarks " +                      // 35.覚書
                                                                    " FROM " +
                                                                    "     mylife.t_salary SALARY " +
                                                                    " INNER JOIN " +
                                                                    "     mylife.t_deduct DEDUCT " +
                                                                    " ON " +
                                                                    "     SALARY.ID = DEDUCT.ID " +
                                                                    " INNER JOIN " +
                                                                    "     mylife.t_duty DUTY " +
                                                                    " ON " +
                                                                    "     SALARY.ID = DUTY.ID " +
                                                                    " INNER JOIN " +
                                                                    "     mylife.t_side_business SIDE " +
                                                                    " ON " +
                                                                    "     SALARY.ID = SIDE.ID ", connection);
                    MySqlDataReader results = selectCommand.ExecuteReader();
                    results.Read();

                    int a = results.RecordsAffected;

                }

            }
            finally
            {
                // カーソルを元に戻す
                Cursor.Current = preCursor;
                this.lblStatus.Text = "出力完了！";

                // 閉じる
                //wb.Close();
                ExcelApp.Quit();
            }
        }
    }
}
