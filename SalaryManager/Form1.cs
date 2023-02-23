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
        //private static readonly string Pwd = "ltmc4874";          // パスワード

        // 接続文字列
        //private static readonly string ConnectionString = $"Server={Server}; Port={Port}; Database={Database}; Uid={Uid}; Pwd={Pwd}";
        private static readonly string ConnectionString = $"Server={Server}; Port={Port}; Database={Database}; Uid={Uid}";

        public static bool isUpdate = false;        // 更新フラグ

        public static bool isConfirmAllow = false;  // 確認ボックス表示の要否

        #region Utilities

        /// <summary>
        /// 共通ユーティリティ
        /// </summary>
        private Utils Utils
        {
            get;
            set;
        } = new Utils();

        /// <summary>
        /// データベース
        /// </summary>
        private DatabaseTransaction DatabaseTransaction
        {
            get;
            set;
        } = new DatabaseTransaction();

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

            // DataSet - 日付
            this.DataSetPayslip.TargetDate = this.txtYear.Text + "/" + Convert.ToInt32(this.txtMonth.Text).ToString("00");

            // Clear
            this.Clear();

            // Set - DataTable
            this.SetDataTable();

            // Get - 明細
            this.GetSalary(false);

            // Get - 年収
            this.GetAnnualIncomey();
        }

        /// <summary>
        /// 共通 - Leave
        /// </summary>
        /// <param name="textBox">対象のText Box</param>
        /// <param name="tableName">テーブル名</param>
        /// <param name="columnName">列名</param>
        /// <param name="value">値</param>
        private void Common_Leave(
            // 対象のText Box
            TextBox textBox,
            // テーブル名
            DataTable tableName,
            // 列名
            string columnName,
            // 値
            object value,
            // 0 → 空白にするか
            bool convertZeroToBlank
        )
        {
            // テーブルの値比較
            if (!this.DataSetPayslip.CompareValue(
                    // テーブル名
                    tableName,
                    // 列名
                    columnName,
                    // 値
                    value
                ))
            {
                // 背景色変更
                this.Utils.ChangeBackgroundColor(
                    // 基本給
                    textBox,
                    // 変更
                    true
                );
            }
            else
            {
                // 背景色変更
                this.Utils.ChangeBackgroundColor(
                    // 基本給
                    textBox,
                    // 変更なし
                    false
                );
            }

            // 0 → 空白にするか
            if (convertZeroToBlank)
            {
                // not int ?
                if (!(value is int))
                {
                    // 無効
                    return;
                }

                // 0 → 空白
                this.Utils.ZeroToBlank(
                    // 対象のText Box
                    textBox,
                    // 値
                    (int)value
                );
            }

            if (value is int)
            {
                // Textに反映
                textBox.Text = this.Utils.FormatIntToPrice(value.ToString());

                // 再計算
                CalculateSum();
            }
        }

        #region 支給額 - 基本給

        /// <summary>
        /// 基本給 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtBasicSalary_KeyPress(
            // Sender
            object sender,
            // EventArgs
            KeyPressEventArgs e
        )
        {
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 基本給 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtBasicSalary_Leave(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 支給額 - 基本給
            this.ItemAllowance.BasicSalary =
                // 金額 → 数値
                this.Utils.FormatPriceToInt(
                    // 基本給
                    this.txtBasicSalary.Text
                );

            // 共通 - Leave
            this.Common_Leave(
                // 対象のText Box
                this.txtBasicSalary,
                // テーブル名
                this.DataSetPayslip.TableAllowance,
                // 列名
                nameof(this.ItemAllowance.BasicSalary),
                // 値
                this.ItemAllowance.BasicSalary.Value,
                // 0 → 空白にするか
                false
            );
        }

        /// <summary>
        /// 基本給 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtBasicSalary_MouseEnter(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            if (ItemAllowance.TotalDeductedSalary.Value <= 0)
            {
                return;
            }

            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                System.Convert.ToInt32(this.txtMonth.Text),
                // 取得する列名
                "SALARY.basic_salary"
            );
        }

        #endregion

        #region 支給額 - 役職手当

        /// <summary>
        /// 役職手当- KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtExecutiveAllowance_KeyPress(
            // Sender
            object sender,
            // EventArgs
            KeyPressEventArgs e
        )
        {
            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 役職手当 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtExecutiveAllowance_Leave(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 支給額 - 役職手当
            this.ItemAllowance.ExecutiveAllowance =
                // 金額 → 数値
                this.Utils.FormatPriceToInt(
                    // 役職手当
                    this.txtExecutiveAllowance.Text
                );

            // 共通 - Leave
            this.Common_Leave(
                // 対象のText Box
                this.txtExecutiveAllowance,
                // テーブル名
                this.DataSetPayslip.TableAllowance,
                // 列名
                nameof(this.ItemAllowance.ExecutiveAllowance),
                // 値
                this.ItemAllowance.ExecutiveAllowance.Value,
                // 0 → 空白にするか
                true
            );
        }

        /// <summary>
        /// 役職手当 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtExecutiveAllowance_MouseEnter(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                System.Convert.ToInt32(this.txtMonth.Text),
                // 取得する列名
                "SALARY.executive_allowance"
            );
        }

        #endregion

        #region 支給額 - 扶養手当

        /// <summary>
        /// 扶養手当 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtDependencyAllowance_KeyPress(
            // Sender
            object sender,
            // EventArgs
            KeyPressEventArgs e
        )
        {
            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 扶養手当 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtDependencyAllowance_Leave(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 支給額 - 扶養手当
            this.ItemAllowance.DependencyAllowance =
                // 金額 → 数値
                this.Utils.FormatPriceToInt(
                    // 扶養手当
                    this.txtDependencyAllowance.Text
                );

            // 共通 - Leave
            this.Common_Leave(
                // 対象のText Box
                this.txtDependencyAllowance,
                // テーブル名
                this.DataSetPayslip.TableAllowance,
                // 列名
                nameof(this.ItemAllowance.DependencyAllowance),
                // 値
                this.ItemAllowance.DependencyAllowance.Value,
                // 0 → 空白にするか
                true
            );
        }

        /// <summary>
        /// 扶養手当 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtDependencyAllowance_MouseEnter(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                System.Convert.ToInt32(this.txtMonth.Text),
                // 取得する列名
                "SALARY.dependency_allowance"
            );
        }

        #endregion

        #region 支給額 - 時間外手当

        /// <summary>
        /// 時間外手当 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtOvertimeAllowance_KeyPress(
            // Sender
            object sender,
            // EventArgs
            KeyPressEventArgs e
        )
        {
            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 時間外手当 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtOvertimeAllowance_Leave(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 支給額 - 時間外手当
            this.ItemAllowance.OvertimeAllowance =
                // 金額 → 数値
                this.Utils.FormatPriceToInt(
                    // 時間外手当
                    this.txtOvertimeAllowance.Text
                );

            // 共通 - Leave
            this.Common_Leave(
                // 対象のText Box
                this.txtOvertimeAllowance,
                // テーブル名
                this.DataSetPayslip.TableAllowance,
                // 列名
                nameof(this.ItemAllowance.OvertimeAllowance),
                // 値
                this.ItemAllowance.OvertimeAllowance.Value,
                // 0 → 空白にするか
                false
            );
        }

        #endregion

        #region 支給額 - 休日割増

        /// <summary>
        /// 休日割増 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtDaysoffIncreased_KeyPress(
            // Sender
            object sender, 
            // EventArgs
            KeyPressEventArgs e
        )
        {
            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 休日割増 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtDaysoffIncreased_Leave(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 支給額 - 休日割増
            this.ItemAllowance.DaysoffIncreased =
                // 金額 → 数値
                this.Utils.FormatPriceToInt(
                    // 休日割増
                    this.txtDaysoffIncreased.Text
                );

            // 共通 - Leave
            this.Common_Leave(
                // 対象のText Box
                this.txtDaysoffIncreased,
                // テーブル名
                this.DataSetPayslip.TableAllowance,
                // 列名
                nameof(this.ItemAllowance.DaysoffIncreased),
                // 値
                this.ItemAllowance.DaysoffIncreased.Value,
                // 0 → 空白にするか
                false
            );
        }

        /// <summary>
        /// 休日割増 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtDaysoffIncreased_MouseEnter(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                System.Convert.ToInt32(this.txtMonth.Text),
                // 取得する列名
                "SALARY.daysoff_increased"
            );
        }

        #endregion

        #region 支給額 - 深夜割増

        /// <summary>
        /// 深夜割増 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtNightworkIncreased_KeyPress(
            // Sender
            object sender,
            // EventArgs
            KeyPressEventArgs e
        )
        {
            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 深夜割増 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtNightworkIncreased_Leave(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 支給額 - 深夜割増
            this.ItemAllowance.NightworkIncreased =
                // 金額 → 数値
                this.Utils.FormatPriceToInt(
                    // 深夜割増
                    this.txtNightworkIncreased.Text
                );

            // 共通 - Leave
            this.Common_Leave(
                // 対象のText Box
                this.txtNightworkIncreased,
                // テーブル名
                this.DataSetPayslip.TableAllowance,
                // 列名
                nameof(this.ItemAllowance.NightworkIncreased),
                // 値
                this.ItemAllowance.NightworkIncreased.Value,
                // 0 → 空白にするか
                false
            );
        }

        /// <summary>
        /// 深夜割増 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtNightworkIncreased_MouseEnter(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                System.Convert.ToInt32(this.txtMonth.Text),
                // 取得する列名
                "SALARY.nightwork_increased"
            );
        }

        #endregion

        #region 支給額 - 住宅手当

        /// <summary>
        /// 住宅手当 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtHousingAllowance_KeyPress(
            // Sender
            object sender,
            // EventArgs
            KeyPressEventArgs e
        )
        {
            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 住宅手当 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtHousingAllowance_Leave(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 支給額 - 住宅手当
            this.ItemAllowance.HousingAllowance =
                // 金額 → 数値
                this.Utils.FormatPriceToInt(
                    // 住宅手当
                    this.txtHousingAllowance.Text
                );

            // 共通 - Leave
            this.Common_Leave(
                // 対象のText Box
                this.txtHousingAllowance,
                // テーブル名
                this.DataSetPayslip.TableAllowance,
                // 列名
                nameof(this.ItemAllowance.HousingAllowance),
                // 値
                this.ItemAllowance.HousingAllowance.Value,
                // 0 → 空白にするか
                true
            );
        }

        /// <summary>
        /// 住宅手当 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtHousingAllowance_MouseEnter(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                System.Convert.ToInt32(this.txtMonth.Text),
                // 取得する列名
                "SALARY.housing_allowance"
            );
        }

        #endregion

        #region 支給額 - 遅刻早退欠勤

        /// <summary>
        /// 遅刻早退欠勤 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtLateAbsent_KeyPress(
            // Sender
            object sender,
            // EventArgs
            KeyPressEventArgs e
        )
        {
            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 遅刻早退欠勤 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtLateAbsent_Leave(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 支給額 - 遅刻早退欠勤
            this.ItemAllowance.LateAbsent =
                // 金額 → 数値
                this.Utils.FormatPriceToInt(
                    // 遅刻早退欠勤
                    this.txtLateAbsent.Text
                );

            // 共通 - Leave
            this.Common_Leave(
                // 対象のText Box
                this.txtLateAbsent,
                // テーブル名
                this.DataSetPayslip.TableAllowance,
                // 列名
                nameof(this.ItemAllowance.LateAbsent),
                // 値
                this.ItemAllowance.LateAbsent.Value,
                // 0 → 空白にするか
                false
            );
        }

        /// <summary>
        /// 遅刻早退欠勤 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtLateAbsent_MouseEnter(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                System.Convert.ToInt32(this.txtMonth.Text),
                // 取得する列名
                "SALARY.late_absent"
            );
        }

        #endregion

        #region 支給額 - 交通費

        /// <summary>
        /// 交通費 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtTransportationExpenses_KeyPress(
            // Sender
            object sender,
            // EventArgs
            KeyPressEventArgs e
        )
        {
            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 交通費 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtTransportationExpenses_Leave(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 支給額 - 交通費
            this.ItemAllowance.TransportationExpenses =
                // 金額 → 数値
                this.Utils.FormatPriceToInt(
                    // 交通費
                    this.txtTransportationExpenses.Text
                );

            // 共通 - Leave
            this.Common_Leave(
                // 対象のText Box
                this.txtTransportationExpenses,
                // テーブル名
                this.DataSetPayslip.TableAllowance,
                // 列名
                nameof(this.ItemAllowance.TransportationExpenses),
                // 値
                this.ItemAllowance.TransportationExpenses.Value,
                // 0 → 空白にするか
                false
            );
        }

        /// <summary>
        /// 交通費 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtTransportationExpenses_MouseEnter(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                System.Convert.ToInt32(this.txtMonth.Text),
                // 取得する列名
                "SALARY.transportation_expenses"
            );
        }

        #endregion

        #region 支給額 - 特別手当

        /// <summary>
        /// 特別手当 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtSpecialAllowance_KeyPress(
            // Sender
            object sender,
            // EventArgs
            KeyPressEventArgs e
        )
        {
            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 特別手当 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtSpecialAllowance_Leave(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 支給額 - 特別手当
            this.ItemAllowance.SpecialAllowance =
                // 金額 → 数値
                this.Utils.FormatPriceToInt(
                    // 特別手当
                    this.txtSpecialAllowance.Text
                );

            // 共通 - Leave
            this.Common_Leave(
                // 対象のText Box
                this.txtTransportationExpenses,
                // テーブル名
                this.DataSetPayslip.TableAllowance,
                // 列名
                nameof(this.ItemAllowance.TransportationExpenses),
                // 値
                this.ItemAllowance.TransportationExpenses.Value,
                // 0 → 空白にするか
                true
            );
        }

        /// <summary>
        /// 特別手当 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtSpecialAllowance_MouseEnter(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                System.Convert.ToInt32(this.txtMonth.Text),
                // 取得する列名
                "SALARY.special_allowance"
            );
        }

        #endregion

        #region 支給額 - 予備

        /// <summary>
        /// 予備 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtSpareAllowance_KeyPress(
            // Sender
            object sender,
            // EventArgs
            KeyPressEventArgs e
        )
        {
            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 予備 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtSpareAllowance_Leave(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 支給額 - 特別手当
            this.ItemAllowance.SpecialAllowance =
                // 金額 → 数値
                this.Utils.FormatPriceToInt(
                    // 特別手当
                    this.txtSpareAllowance.Text
                );

            // 共通 - Leave
            this.Common_Leave(
                // 対象のText Box
                this.txtSpareAllowance,
                // テーブル名
                this.DataSetPayslip.TableAllowance,
                // 列名
                nameof(this.ItemAllowance.SpecialAllowance),
                // 値
                this.ItemAllowance.SpecialAllowance.Value,
                // 0 → 空白にするか
                false
            );
        }

        /// <summary>
        /// 予備 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtSpareAllowance_MouseEnter(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                System.Convert.ToInt32(this.txtMonth.Text),
                // 取得する列名
                "SALARY.spare_allowance"
            );
        }

        #endregion

        #region 支給額 - 支給額計

        /// <summary>
        /// 支給額計 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtTotalSalary_KeyPress(
            // Sender
            object sender,
            // EventArgs
            KeyPressEventArgs e
        )
        {
            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 支給額計 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        /// <remarks>差引支給額も計算する</remarks>
        private void txtTotalSalary_Leave(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 支給額 - 支給額計
            this.ItemAllowance.TotalSalary =
                // 金額 → 数値
                this.Utils.FormatPriceToInt(
                    // 特別手当
                    this.txtTotalSalary.Text
                );

            // 共通 - Leave
            this.Common_Leave(
                // 対象のText Box
                this.txtTotalSalary,
                // テーブル名
                this.DataSetPayslip.TableAllowance,
                // 列名
                nameof(this.ItemAllowance.TotalSalary),
                // 値
                this.ItemAllowance.TotalSalary.Value,
                // 0 → 空白にするか
                false
            );

            if (ItemAllowance.TotalSalary == 0)
            {
                return;
            }

            if (ItemDeduction.TotalDeduct.Value == 0)
            {
                return;
            }

            // 差引支給額計
            this.txtTotalDeductedSalary.Text = 
                (ItemAllowance.TotalSalary - ItemDeduction.TotalDeduct.Value).ToString();
        }

        /// <summary>
        /// 支給額計 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtTotalSalary_MouseEnter(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                System.Convert.ToInt32(this.txtMonth.Text),
                // 取得する列名
                "SALARY.total_salary"
            );
        }

        /// <summary>
        /// 差引支給額計 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtTotalDeductedSalary_MouseEnter(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                System.Convert.ToInt32(this.txtMonth.Text),
                // 取得する列名
                "DEDUCT.total_deducted_salary"
            );
        }

        #endregion

        #region 控除額 - 健康保険

        /// <summary>
        /// 健康保険 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtHealthInsurance_KeyPress(
            // Sender
            object sender,
            // EventArgs
            KeyPressEventArgs e
        )
        {
            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 健康保険 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtHealthInsurance_Leave(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 控除額 - 健康保険
            this.ItemDeduction.HealthInsurance =
                // 金額 → 数値
                this.Utils.FormatPriceToInt(
                    // 健康保険
                    this.txtHealthInsurance.Text
                );

            // 共通 - Leave
            this.Common_Leave(
                // 対象のText Box
                this.txtHealthInsurance,
                // テーブル名
                this.DataSetPayslip.TableDeduction,
                // 列名
                nameof(this.ItemDeduction.HealthInsurance),
                // 値
                this.ItemDeduction.HealthInsurance.Value,
                // 0 → 空白にするか
                false
            );
        }

        /// <summary>
        /// 健康保険 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtHealthInsurance_MouseEnter(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                System.Convert.ToInt32(this.txtMonth.Text),
                // 取得する列名
                "DEDUCT.health_insurance"
            );
        }

        #endregion

        #region 控除額 - 介護保険

        /// <summary>
        /// 介護保険 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtNursingInsurance_KeyPress(
            // Sender
            object sender,
            // EventArgs
            KeyPressEventArgs e
        )
        {
            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 介護保険 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtNursingInsurance_Leave(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 控除額 - 介護保険
            this.ItemDeduction.NursingInsurance =
                // 金額 → 数値
                this.Utils.FormatPriceToInt(
                    // 介護保険
                    this.txtNursingInsurance.Text
                );

            // 共通 - Leave
            this.Common_Leave(
                // 対象のText Box
                this.txtNursingInsurance,
                // テーブル名
                this.DataSetPayslip.TableDeduction,
                // 列名
                nameof(this.ItemDeduction.NursingInsurance),
                // 値
                this.ItemDeduction.NursingInsurance.Value,
                // 0 → 空白にするか
                false
            );
        }

        /// <summary>
        /// 介護保険 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtNursingInsurance_MouseEnter(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                System.Convert.ToInt32(this.txtMonth.Text),
                // 取得する列名
                "DEDUCT.nursing_insurance"
            );
        }

        #endregion

        #region 控除額 - 厚生年金

        /// <summary>
        /// 厚生年金 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtWelfareAnnuity_KeyPress(
            // Sender
            object sender,
            // EventArgs
            KeyPressEventArgs e
        )
        {
            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 厚生年金 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtWelfareAnnuity_Leave(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 控除額 - 厚生年金
            this.ItemDeduction.WelfareAnnuity =
                // 金額 → 数値
                this.Utils.FormatPriceToInt(
                    // 厚生年金
                    this.txtWelfareAnnuity.Text
                );

            // 共通 - Leave
            this.Common_Leave(
                // 対象のText Box
                this.txtWelfareAnnuity,
                // テーブル名
                this.DataSetPayslip.TableDeduction,
                // 列名
                nameof(this.ItemDeduction.WelfareAnnuity),
                // 値
                this.ItemDeduction.WelfareAnnuity.Value,
                // 0 → 空白にするか
                false
            );
        }

        /// <summary>
        /// 厚生年金 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtWelfareAnnuity_MouseEnter(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                System.Convert.ToInt32(this.txtMonth.Text),
                // 取得する列名
                "DEDUCT.welfare_annuity"
            );
        }

        #endregion

        #region 控除額 - 雇用保険

        /// <summary>
        /// 雇用保険 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtEmploymentInsurance_KeyPress(
            // Sender
            object sender,
            // EventArgs
            KeyPressEventArgs e
        )
        {
            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 雇用保険 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtEmploymentInsurance_Leave(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 控除額 - 雇用保険
            this.ItemDeduction.EmploymentInsurance =
                // 金額 → 数値
                this.Utils.FormatPriceToInt(
                    // 雇用保険
                    this.txtEmploymentInsurance.Text
                );

            // 共通 - Leave
            this.Common_Leave(
                // 対象のText Box
                this.txtWelfareAnnuity,
                // テーブル名
                this.DataSetPayslip.TableDeduction,
                // 列名
                nameof(this.ItemDeduction.WelfareAnnuity),
                // 値
                this.ItemDeduction.WelfareAnnuity.Value,
                // 0 → 空白にするか
                false
            );
        }

        /// <summary>
        /// 雇用保険 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtEmploymentInsurance_MouseEnter(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                System.Convert.ToInt32(this.txtMonth.Text),
                // 取得する列名
                "DEDUCT.employment_insurance"
            );
        }

        #endregion

        #region 控除額 - 所得税

        /// <summary>
        /// 所得税 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtIncomeTax_KeyPress(
            // Sender
            object sender,
            // EventArgs
            KeyPressEventArgs e
        )
        {
            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 所得税 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtIncomeTax_Leave(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 控除額 - 雇用保険
            this.ItemDeduction.IncomeTax =
                // 金額 → 数値
                this.Utils.FormatPriceToInt(
                    // 雇用保険
                    this.txtIncomeTax.Text
                );

            // 共通 - Leave
            this.Common_Leave(
                // 対象のText Box
                this.txtIncomeTax,
                // テーブル名
                this.DataSetPayslip.TableDeduction,
                // 列名
                nameof(this.ItemDeduction.IncomeTax),
                // 値
                this.ItemDeduction.IncomeTax.Value,
                // 0 → 空白にするか
                false
            );
        }

        /// <summary>
        /// 所得税 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtIncomeTax_MouseEnter(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                System.Convert.ToInt32(this.txtMonth.Text),
                // 取得する列名
                "DEDUCT.income_tax"
            );
        }

        #endregion

        #region 控除額 - 市町村税

        /// <summary>
        /// 市町村税 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtMunicipalTax_KeyPress(
            // Sender
            object sender,
            // EventArgs
            KeyPressEventArgs e
        )
        {
            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 市町村税 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtMunicipalTax_Leave(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 控除額 - 市町村税
            this.ItemDeduction.MunicipalTax =
                // 金額 → 数値
                this.Utils.FormatPriceToInt(
                    // 市町村税
                    this.txtMunicipalTax.Text
                );

            // 共通 - Leave
            this.Common_Leave(
                // 対象のText Box
                this.txtMunicipalTax,
                // テーブル名
                this.DataSetPayslip.TableDeduction,
                // 列名
                nameof(this.ItemDeduction.MunicipalTax),
                // 値
                this.ItemDeduction.MunicipalTax.Value,
                // 0 → 空白にするか
                false
            );
        }

        /// <summary>
        /// 市町村税 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtMunicipalTax_MouseEnter(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                System.Convert.ToInt32(this.txtMonth.Text),
                // 取得する列名
                "DEDUCT.municipal_tax"
            );
        }

        #endregion

        #region 控除額 - 互助会

        /// <summary>
        /// 互助会 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtFriendshipAssociation_KeyPress(
            // Sender
            object sender,
            // EventArgs
            KeyPressEventArgs e
        )
        {
            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 互助会 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtFriendshipAssociation_Leave(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 控除額 - 互助会
            this.ItemDeduction.FriendshipAssociation =
                // 金額 → 数値
                this.Utils.FormatPriceToInt(
                    // 互助会
                    this.txtFriendshipAssociation.Text
                );

            // 共通 - Leave
            this.Common_Leave(
                // 対象のText Box
                this.txtFriendshipAssociation,
                // テーブル名
                this.DataSetPayslip.TableDeduction,
                // 列名
                nameof(this.ItemDeduction.FriendshipAssociation),
                // 値
                this.ItemDeduction.FriendshipAssociation.Value,
                // 0 → 空白にするか
                true
            );
        }

        /// <summary>
        /// 互助会 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtFriendshipAssociation_MouseEnter(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                System.Convert.ToInt32(this.txtMonth.Text),
                // 取得する列名
                "DEDUCT.friendship_association"
            );
        }

        #endregion

        #region 控除額 - 年末調整他

        /// <summary>
        /// 年末調整他 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtYearEndTaxAdjustment_KeyPress(
            // Sender
            object sender,
            // EventArgs
            KeyPressEventArgs e
        )
        {
            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 年末調整他 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtYearEndTaxAdjustment_Leave(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 控除額 - 互助会
            this.ItemDeduction.YearEndTaxAdjustment =
                // 金額 → 数値
                this.Utils.FormatPriceToInt(
                    // 互助会
                    this.txtYearEndTaxAdjustment.Text
                );

            // 共通 - Leave
            this.Common_Leave(
                // 対象のText Box
                this.txtYearEndTaxAdjustment,
                // テーブル名
                this.DataSetPayslip.TableDeduction,
                // 列名
                nameof(this.ItemDeduction.YearEndTaxAdjustment),
                // 値
                this.ItemDeduction.YearEndTaxAdjustment.Value,
                // 0 → 空白にするか
                true
            );
        }

        /// <summary>
        /// 年末調整他 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtYearEndTaxAdjustment_MouseEnter(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                System.Convert.ToInt32(this.txtMonth.Text),
                // 取得する列名
                "DEDUCT.year_end_tax_adjustment"
            );
        }

        #endregion

        #region 控除額 - 控除額計

        /// <summary>
        /// 控除額計 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtTotalDeduct_KeyPress(
            // Sender
            object sender,
            // EventArgs
            KeyPressEventArgs e
        )
        {
            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 控除額計 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtTotalDeduct_Leave(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 控除額 - 控除額計
            this.ItemDeduction.TotalDeduct =
                // 金額 → 数値
                this.Utils.FormatPriceToInt(
                    // 控除額計
                    this.txtTotalDeduct.Text
                );

            // 共通 - Leave
            this.Common_Leave(
                // 対象のText Box
                this.txtTotalDeduct,
                // テーブル名
                this.DataSetPayslip.TableDeduction,
                // 列名
                nameof(this.ItemDeduction.TotalDeduct),
                // 値
                this.ItemDeduction.TotalDeduct.Value,
                // 0 → 空白にするか
                true
            );

            // 支給総計
            var totalSalary = this.ItemAllowance.TotalSalary.Value;

            // 差引支給額
            var totalDeduct = this.ItemDeduction.TotalDeduct.Value;

            if (totalSalary == 0)
            {
                return;
            }

            if (totalDeduct == 0)
            {
                return;
            }

            this.txtTotalDeductedSalary.Text = 
                (totalSalary - totalDeduct).ToString();

           /* // 合計支給額の自動計算
            this.CalculateSum();*/
        }

        /// <summary>
        /// 控除額計 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtTotalDeduct_MouseEnter(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                System.Convert.ToInt32(this.txtMonth.Text),
                // 取得する列名
                "DEDUCT.total_deduct"
            );
        }

        #endregion

        #region 勤務備考 - 時間外時間

        /// <summary>
        /// 時間外時間 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtOvertimeTime_KeyPress(
            // Sender
            object sender,
            // EventArgs
            KeyPressEventArgs e
        )
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
        private void txtOvertimeTime_Leave(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // is Null ?
            if (string.IsNullOrEmpty(this.txtOvertimeTime.Text))
            {
                return;
            }

            // 勤務備考 - 時間外時間
            this.ItemWorkingReferences.OvertimeTime = 
                Convert.ToDouble(
                    // 時間外時間
                    this.txtOvertimeTime.Text
                );

            // 共通 - Leave
            this.Common_Leave(
                // 対象のText Box
                this.txtOvertimeTime,
                // テーブル名
                this.DataSetPayslip.TableWorkingReferences,
                // 列名
                nameof(this.ItemWorkingReferences.OvertimeTime),
                // 値
                this.ItemWorkingReferences.OvertimeTime.Value,
                // 0 → 空白にするか
                false
            );
        }

        /// <summary>
        /// 時間外時間 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtOvertimeAllowance_MouseEnter(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                System.Convert.ToInt32(this.txtMonth.Text),
                // 取得する列名
                "SALARY.overtime_allowance"
            );
        }

        #endregion

        #region 勤務備考 - 休出時間

        /// <summary>
        /// 休出時間 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtWeekendWorktime_KeyPress(
            // Sender
            object sender,
            // EventArgs
            KeyPressEventArgs e
        )
        {
            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 休出時間 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtWeekendWorktime_Leave(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 勤務備考 - 休出時間
            this.ItemWorkingReferences.WeekendWorktime =
                // 金額 → 数値
                this.Utils.FormatPriceToInt(
                    // 休出時間
                    this.txtWeekendWorktime.Text
                );

            // 共通 - Leave
            this.Common_Leave(
                // 対象のText Box
                this.txtWeekendWorktime,
                // テーブル名
                this.DataSetPayslip.TableWorkingReferences,
                // 列名
                nameof(this.ItemWorkingReferences.WeekendWorktime),
                // 値
                this.ItemWorkingReferences.WeekendWorktime.Value,
                // 0 → 空白にするか
                true
            );
        }

        #endregion

        #region 勤務備考 - 深夜時間

        /// <summary>
        /// 深夜時間 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtMidnightWorktime_KeyPress(
            // Sender
            object sender,
            // EventArgs
            KeyPressEventArgs e
        )
        {
            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 深夜時間 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtMidnightWorktime_Leave(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 勤務備考 - 深夜時間
            this.ItemWorkingReferences.MidnightWorktime =
                // 金額 → 数値
                this.Utils.FormatPriceToInt(
                    // 深夜時間
                    this.txtMidnightWorktime.Text
                );

            // 共通 - Leave
            this.Common_Leave(
                // 対象のText Box
                this.txtMidnightWorktime,
                // テーブル名
                this.DataSetPayslip.TableWorkingReferences,
                // 列名
                nameof(this.ItemWorkingReferences.MidnightWorktime),
                // 値
                this.ItemWorkingReferences.MidnightWorktime.Value,
                // 0 → 空白にするか
                true
            );
        }

        #endregion

        #region 勤務備考 - 遅刻早退欠勤H

        /// <summary>
        /// 遅刻早退欠勤H - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtLateAbsentH_KeyPress(
            // Sender
            object sender,
            // EventArgs
            KeyPressEventArgs e
        )
        {
            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 遅刻早退欠勤H - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtLateAbsentH_Leave(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 勤務備考 - 遅刻早退欠勤H
            this.ItemWorkingReferences.LateAbsentH =
                // 金額 → 数値
                this.Utils.FormatPriceToInt(
                    // 遅刻早退欠勤H
                    this.txtLateAbsentH.Text
                );

            // 共通 - Leave
            this.Common_Leave(
                // 対象のText Box
                this.txtLateAbsentH,
                // テーブル名
                this.DataSetPayslip.TableWorkingReferences,
                // 列名
                nameof(this.ItemWorkingReferences.LateAbsentH),
                // 値
                this.ItemWorkingReferences.LateAbsentH.Value,
                // 0 → 空白にするか
                true
            );
        }

        #endregion

        #region 勤務備考 - 支給額-保険

        /// <summary>
        /// 支給額-保険 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtInsurance_KeyPress(
            // Sender
            object sender,
            // EventArgs
            KeyPressEventArgs e
        )
        {
            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 支給額-保険 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtInsurance_Leave(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 勤務備考 - 支給額-保険
            this.ItemWorkingReferences.Insurance =
                // 金額 → 数値
                this.Utils.FormatPriceToInt(
                    // 支給額-保険
                    this.txtInsurance.Text
                );

            // 共通 - Leave
            this.Common_Leave(
                // 対象のText Box
                this.txtInsurance,
                // テーブル名
                this.DataSetPayslip.TableWorkingReferences,
                // 列名
                nameof(this.ItemWorkingReferences.Insurance),
                // 値
                this.ItemWorkingReferences.Insurance.Value,
                // 0 → 空白にするか
                false
            );
        }

        /// <summary>
        /// 支給額-保険 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtInsurance_MouseEnter(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                System.Convert.ToInt32(this.txtMonth.Text),
                // 取得する列名
                "SIDEBUS.insurance"
            );
        }

        #endregion

        #region 勤務備考 - 扶養人数

        /// <summary>
        /// 扶養人数 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtNumberOfDependent_KeyPress(
            // Sender
            object sender,
            // EventArgs
            KeyPressEventArgs e
        )
        {
            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 扶養人数 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtNumberOfDependent_Leave(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 勤務備考 - 扶養人数
            this.ItemWorkingReferences.NumberOfDependent =
                // 金額 → 数値
                this.Utils.FormatPriceToInt(
                    // 扶養人数
                    this.txtNumberOfDependent.Text
                );

            // 共通 - Leave
            this.Common_Leave(
                // 対象のText Box
                this.txtNumberOfDependent,
                // テーブル名
                this.DataSetPayslip.TableWorkingReferences,
                // 列名
                nameof(this.ItemWorkingReferences.NumberOfDependent),
                // 値
                this.ItemWorkingReferences.NumberOfDependent.Value,
                // 0 → 空白にするか
                true
            );
        }

        #endregion

        #region 勤務備考 - 有給残日数

        /// <summary>
        /// 有給残日数 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtPaidVacation_KeyPress(
            // Sender
            object sender,
            // EventArgs
            KeyPressEventArgs e
        )
        {
            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 有給残日数 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtPaidVacation_Leave(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 勤務備考 - 有給残日数
            this.ItemWorkingReferences.PaidVacation =
                // 金額 → 数値
                this.Utils.FormatPriceToInt(
                    // 有給残日数
                    this.txtPaidVacation.Text
                );

            // 共通 - Leave
            this.Common_Leave(
                // 対象のText Box
                this.txtPaidVacation,
                // テーブル名
                this.DataSetPayslip.TableWorkingReferences,
                // 列名
                nameof(this.ItemWorkingReferences.PaidVacation),
                // 値
                this.ItemWorkingReferences.PaidVacation.Value,
                // 0 → 空白にするか
                false
            );
        }

        #endregion

        #region 勤務備考 - 勤務時間

        /// <summary>
        /// 勤務時間 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtWorkingHours_KeyPress(
            // Sender
            object sender,
            // EventArgs
            KeyPressEventArgs e
        )
        {
            if (e.KeyChar.ToString() == ".")
            {
                return;
            }

            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 勤務時間 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtWorkingHours_Leave(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            if (string.IsNullOrEmpty(this.txtWorkingHours.Text))
            {
                return;
            }

            // 勤務備考 - 勤務時間
            this.ItemWorkingReferences.WorkingHours = 
                Convert.ToDouble(
                    // 勤務時間
                    this.txtWorkingHours.Text
                );

            // 共通 - Leave
            this.Common_Leave(
                // 対象のText Box
                this.txtWorkingHours,
                // テーブル名
                this.DataSetPayslip.TableWorkingReferences,
                // 列名
                nameof(this.ItemWorkingReferences.WorkingHours),
                // 値
                this.ItemWorkingReferences.WorkingHours.Value,
                // 0 → 空白にするか
                false
            );
        }

        #endregion

        #region 勤務備考 - 勤務先

        /// <summary>
        /// 勤務先 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtWorkplace_Leave(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // キー入力判定
            this.ItemWorkingReferences.Workplace = this.txtWorkplace.Text;
        }

        #endregion

        #region 副業 - 副業収入

        /// <summary>
        /// 副業収入 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtSideBusiness_KeyPress(
            // Sender
            object sender,
            // EventArgs
            KeyPressEventArgs e
        )
        {
            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 副業収入 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtSideBusiness_Leave(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            if (this.txtSideBusiness.Text == string.Empty)
            {
                return;
            }

            // 副業 - 副業収入
            this.ItemSideBusiness.SideBusiness =
                // 金額 → 数値
                this.Utils.FormatPriceToInt(
                    // 副業収入
                    this.txtSideBusiness.Text
                );

            // 共通 - Leave
            this.Common_Leave(
                // 対象のText Box
                this.txtSideBusiness,
                // テーブル名
                this.DataSetPayslip.TableSIdeBusiness,
                // 列名
                nameof(this.ItemSideBusiness.SideBusiness),
                // 値
                this.ItemSideBusiness.SideBusiness.Value,
                // 0 → 空白にするか
                false
            );
        }

        /// <summary>
        /// 副業収入 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtSideBusiness_MouseEnter(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                System.Convert.ToInt32(this.txtMonth.Text),
                // 取得する列名
                "SIDEBUS.side_business"
            );
        }

        #endregion

        #region 副業 - 臨時収入

        /// <summary>
        /// 臨時収入 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtPerquisite_KeyPress(
            // Sender
            object sender,
            // EventArgs
            KeyPressEventArgs e
        )
        {
            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// 臨時収入 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtPerquisite_Leave(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 副業 - 臨時収入
            this.ItemSideBusiness.Perquisite =
                // 金額 → 数値
                this.Utils.FormatPriceToInt(
                    // 臨時収入
                    this.txtPerquisite.Text
                );

            // 共通 - Leave
            this.Common_Leave(
                // 対象のText Box
                this.txtPerquisite,
                // テーブル名
                this.DataSetPayslip.TableSIdeBusiness,
                // 列名
                nameof(this.ItemSideBusiness.SideBusiness),
                // 値
                this.ItemSideBusiness.Perquisite.Value,
                // 0 → 空白にするか
                false
            );
        }

        /// <summary>
        /// 臨時収入 - MouseEnter
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtPerquisite_MouseEnter(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                System.Convert.ToInt32(this.txtMonth.Text),
                // 取得する列名
                "SIDEBUS.perquisite"
            );
        }

        #endregion

        #region 副業 - その他

        /// <summary>
        /// その他 - KeyPress
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtOthers_KeyPress(
            // Sender
            object sender,
            // EventArgs
            KeyPressEventArgs e
        )
        {
            // キー入力判定
            this.Utils.IsInputValNumeric(e);
        }

        /// <summary>
        /// その他 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtOthers_Leave(
            // Sender
            object sender,
            // EventArgs
            EventArgs e
        )
        {
            // 副業 - その他
            this.ItemSideBusiness.Others =
                // 金額 → 数値
                this.Utils.FormatPriceToInt(
                    // その他
                    this.txtOthers.Text
                );

            // 共通 - Leave
            this.Common_Leave(
                // 対象のText Box
                this.txtOthers,
                // テーブル名
                this.DataSetPayslip.TableSIdeBusiness,
                // 列名
                nameof(this.ItemSideBusiness.Others),
                // 値
                this.ItemSideBusiness.Others.Value,
                // 0 → 空白にするか
                false
            );
        }

        /// <summary>
        /// MouseEnter - その他
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        private void txtOthers_MouseEnter(
            // Sender
            object sender,
            // Event Args
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                System.Convert.ToInt32(this.txtMonth.Text),
                // 取得する列名
                "SIDEBUS.others"
            );
        }

        #endregion

        #region 副業 - 覚書

        /// <summary>
        /// 覚書 - Leave
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void txtRemarks_Leave(
            // Sender
            object sender,
            // Event Args
            EventArgs e
        )
        {
            // 覚書
            this.ItemSideBusiness.Remarks = this.txtRemarks.Text;
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

            // Utils - 日付
            this.Utils.TargetDate = this.txtYear.Text + "/" + Convert.ToInt32(this.txtMonth.Text).ToString("00");
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
        private async void btnRegister_Click(object sender, EventArgs e)
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

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                var strTargetDate = this.GetTargetDate
                (
                    // デフォルト明細を使わない
                    false
                );

                // 日付の登録有無
                var exist = this.DatabaseTransaction.ExistId(
                    connection,
                    strTargetDate
                );

                // DBサイドに項目をセット
                this.SetItem();

                // Existence ?
                if (!exist)
                {
                    // 登録
                    await DatabaseTransactionInsert();
                }
                else
                {
                    // 更新
                    this.DatabaseTransactionUpdate();
                }
            }
        }

        /// <summary>
        /// 指定した給与明細を登録します。
        /// </summary>
        private async System.Threading.Tasks.Task DatabaseTransactionInsert()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    var command = new MySqlCommand();

                    // コネクションをオープンします。
                    await connection.OpenAsync();

                    
                    var num = this.DatabaseTransaction.GetId(
                        // 接続
                        connection
                    );

                    // 対象日付
                    var targetDate = this.GetTargetDate(false);

                    // 対象日付の取得
                    num = targetDate == "9999/99" ? 0 : num += 1;

                    // WhenAll
                    await System.Threading.Tasks.Task.WhenAll(
                        // DatabaseTransaction - 登録 - 支給額
                        this.DatabaseTransaction.Insert_SalaryAsync(
                            // 接続
                            connection,
                            // ID
                            num,
                            // 年
                            this.txtYear.Text,
                            // 日付
                            targetDate
                        ),

                        // DatabaseTransaction - 登録 - 控除額
                        this.DatabaseTransaction.Insert_DeductionAsync(
                            // 接続
                            connection,
                            // ID
                            num,
                            // 年
                            this.txtYear.Text,
                            // 日付
                            targetDate
                        ),

                        // DatabaseTransaction - 登録 - 勤務備考
                        this.DatabaseTransaction.Insert_WorkingReferencesAsync(
                            // 接続
                            connection,
                            // ID
                            num,
                            // 年
                            this.txtYear.Text,
                            // 日付
                            targetDate
                        ),

                        // DatabaseTransaction - 登録 - 副業
                        this.DatabaseTransaction.Insert_SideBusinessAsync(
                            // 接続
                            connection,
                            // ID
                            num,
                            // 年
                            this.txtYear.Text,
                            // 日付
                            targetDate
                        )
                    );


                    if (this.txtTotalDeductedSalary.Text != "0")
                    {
                        this.btnRegister.Text = "更新";
                        isUpdate = true;
                    }
                }

                // 年収フォームを更新
                this.GetAnnualIncomey();

                // 登録完了
                MessageBox.Show("登録情報を追加しました。", "追加OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// 指定した年月の給与明細を更新します。
        /// </summary>
        private void DatabaseTransactionUpdate()
        {
            // 対象日付
            var strTargetDate = this.GetTargetDate(false);

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                try
                {
                    // コネクションをオープンします。
                    connection.OpenAsync();

                    // DatabaseTransaction - Update - Salary
                    this.DatabaseTransaction.UpdateSalaryAsync(
                        // 接続
                        connection,
                        // 更新日付
                        strTargetDate
                    );

                    // DatabaseTransaction - Update - Deduction
                    this.DatabaseTransaction.Update_DeductionAsync(
                        // 接続
                        connection,
                        // 更新日付
                        strTargetDate
                    );

                    // DatabaseTransaction - Update - WorkingReferences
                    this.DatabaseTransaction.Update_WorkingReferencesAsync(
                        // 接続
                        connection,
                        // 更新日付
                        strTargetDate
                    );

                    // DatabaseTransaction - Update - SideBusiness
                    this.DatabaseTransaction.Update_SideBusinessAsync(
                        // 接続
                        connection,
                        // 更新日付
                        strTargetDate
                    );

                    // 年収フォームを更新
                    this.GetAnnualIncomey();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }

            // 登録完了
            MessageBox.Show("登録情報を更新しました。",
                            "更新OK",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

            this.SetDefaultColor();
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
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                try
                {
                    var strTargetDate = this.GetTargetDate(false);

                    var exist = this.DatabaseTransaction.ExistId(
                        connection,
                        strTargetDate
                    );

                    if (exist)
                    {
                        // 項目クリア
                        this.Reset();

                        // 前月、次月のデータがある場合、対象月のデータを取得
                        this.GetSalary(false);

                        // 年収取得
                        this.GetAnnualIncomey();

                        return;
                    }

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
                            // 項目クリア
                            this.Reset();

                            // 金額取得
                            this.GetSalary(true);

                            // 年収取得
                            this.GetAnnualIncomey();
                        }

                    }
                    else
                    {
                        // Set - 日時
                        SetDateTime(addmonths);
                    }
                }

                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        /// <summary>
        /// 支給総計 - 自動算出
        /// </summary>
        private void CalculateSum()
        {
            // 支出額計
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

            // 支出額計
            this.txtTotalSalary.Text = this.Utils.FormatIntToPrice(ItemAllowance.TotalSalary.ToString());

            // テーブルの値比較
            if (!this.DataSetPayslip.CompareValue(
                    // テーブル名
                    this.DataSetPayslip.TableAllowance,
                    // 列名
                    nameof(this.ItemAllowance.TotalSalary),
                    // 値
                    this.ItemAllowance.TotalSalary
                ))
            {
                // 背景色変更
                this.Utils.ChangeBackgroundColor(
                    // 支出額計
                    txtTotalSalary,
                    // 変更
                    true
                );
            }
            else
            {
                // 背景色変更
                this.Utils.ChangeBackgroundColor(
                    // 支出額計
                    txtTotalSalary,
                    // 変更なし
                    false
                );
            }

            // 控除額計
            this.ItemDeduction.TotalDeduct = ItemDeduction.HealthInsurance.Value
                                           + ItemDeduction.NursingInsurance.Value
                                           + ItemDeduction.WelfareAnnuity.Value
                                           + ItemDeduction.EmploymentInsurance.Value
                                           + ItemDeduction.IncomeTax.Value
                                           + ItemDeduction.MunicipalTax.Value
                                           + ItemDeduction.FriendshipAssociation.Value
                                           + ItemDeduction.YearEndTaxAdjustment.Value
                                           ;

            // 控除額計
            this.txtTotalDeduct.Text = this.Utils.FormatIntToPrice(ItemDeduction.TotalDeduct.ToString());

            // テーブルの値比較
            if (!this.DataSetPayslip.CompareValue(
                    // テーブル名
                    this.DataSetPayslip.TableDeduction,
                    // 列名
                    nameof(this.ItemDeduction.TotalDeduct),
                    // 値
                    this.ItemDeduction.TotalDeduct
                ))
            {
                // 背景色変更
                this.Utils.ChangeBackgroundColor(
                    // 控除額計
                    txtTotalDeduct,
                    // 変更
                    true
                );
            }
            else
            {
                // 背景色変更
                this.Utils.ChangeBackgroundColor(
                    // 控除額計
                    txtTotalDeduct,
                    // 変更なし
                    false
                );
            }

            // 支給総計
            var totalSalary = this.ItemAllowance.TotalSalary;


            // 控除額計
            var deduction = this.ItemDeduction.TotalDeduct;

            // 12.差引支給額
            if (totalSalary == 0 && deduction == 0)
            {
                this.txtTotalDeductedSalary.Text = "0";

                return;
            }

            if (totalSalary > 0 || deduction > 0)
            {
                this.ItemAllowance.TotalDeductedSalary = totalSalary - deduction;

                // テーブルの値比較
                if (!this.DataSetPayslip.CompareValue(
                        // テーブル名
                        this.DataSetPayslip.TableAllowance,
                        // 列名
                        nameof(this.ItemAllowance.TotalDeductedSalary),
                        // 値
                        this.ItemAllowance.TotalDeductedSalary
                    ))
                {
                    // 背景色変更
                    this.Utils.ChangeBackgroundColor(
                        // 控除額計
                        txtTotalDeductedSalary,
                        // 変更
                        true
                    );
                }
                else
                {
                    // 背景色変更
                    this.Utils.ChangeBackgroundColor(
                        // 控除額計
                        txtTotalDeductedSalary,
                        // 変更なし
                        false
                    );
                }

                this.txtTotalDeductedSalary.Text = this.Utils.FormatIntToPrice((totalSalary - deduction).ToString());
            }
        }

        #region GetTargetDate

        /// <summary>
        /// 対象日付の取得
        /// </summary>
        /// <param name="useDefault">デフォルト明細を使うか</param>
        /// <returns>対象日付</returns>
        private string GetTargetDate
        (
            // デフォルト明細を使うか
            bool useDefault
        )
        {
            // 対象日付の取得
            if (useDefault)
            {
                return "9999/99";
            }

            return this.txtYear.Text + "/" + this.txtMonth.Text;
        }

        #endregion

        #region DataSet

        /// <summary>
        /// 給与明細
        /// </summary>
        private DataSetPayslip DataSetPayslip
        {
            get;
            set;
        } = new DataSetPayslip();

        /// <summary>
        /// Set Data Table
        /// </summary>
        private void SetDataTable()
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                var results = this.DatabaseTransaction.GetAllPaySlip(connection);

                results.Read();

                this.DataSetPayslip.Results = results;

                this.DataSetPayslip.AddTables();
            }
        }

        #endregion

        /// <summary>
        /// 指定した給与明細を取得します。
        /// </summary>
        private void GetSalary
        (
            bool useDefault
        )
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                var sql = string.Empty;

                var strTargetDate = this.GetTargetDate(useDefault);

                var results = this.DatabaseTransaction.GetSalary(
                                    strTargetDate
                                );

                // SetId
                var exist = this.DatabaseTransaction.ExistId(
                    // 接続
                    connection,
                    // 対象日付
                    strTargetDate
                );


                if (!exist)
                {
                    // 0件の場合
                    this.Reset();

                    return;
                }

                // 01.基本給
                this.ItemAllowance.BasicSalary = this.Utils.FormatPriceToInt(results[0].ToString());
                this.txtBasicSalary.Text = this.Utils.FormatIntToPrice(this.ItemAllowance.BasicSalary.Value.ToString());

                // 02.役職手当
                this.ItemAllowance.ExecutiveAllowance = this.Utils.FormatPriceToInt(results[1].ToString());

                if (this.ItemAllowance.ExecutiveAllowance.ToString() == "0")
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

                // DBサイドに項目をセット
                this.SetItem();

                if (ItemSideBusiness.Remarks.Equals("0"))
                {
                    this.txtRemarks.Text = string.Empty;
                }
                else
                {
                    this.txtRemarks.Text = ItemSideBusiness.Remarks;
                }

                if (this.txtTotalDeductedSalary.Text != "0")
                {
                    this.btnRegister.Text = "更新";
                    isUpdate = true;
                }
            }
        }

        /// <summary>
        /// 明細項目のセット
        /// </summary>
        private void SetItem()
        {
            // 支給額
            this.DatabaseTransaction.ItemAllowance = this.ItemAllowance;

            // 控除額
            this.DatabaseTransaction.ItemDeduction = this.ItemDeduction;

            // 副業
            this.DatabaseTransaction.ItemSideBusiness = this.ItemSideBusiness;

            // 勤務備考
            this.DatabaseTransaction.ItemWorkingReferences = this.ItemWorkingReferences;
        }

        #region Delegate

        /// <summary>
        /// Delegate - MonthCheck
        /// </summary>
        /// <param name="month">判定月</param>
        /// <returns></returns>
        delegate TextBox MonthCheck(
            // 判定月
            int month
        );

        /// <summary>
        /// 判定値から支給総計のText Boxを導出する
        /// </summary>
        /// <param name="value">判定値</param>
        /// <returns></returns>
        private TextBox FindTextBoxTotal(
            // 判定値
            int value
        )
        {
            switch (value)
            {
                // 1月
                case 1:
                    // 支給総計 - 1月
                    return this.txtJanuary_Total;
                // 2月
                case 2:
                    // 支給総計 - 2月
                    return this.txtFeburary_Total;
                // 3月
                case 3:
                    // 支給総計 - 3月
                    return this.txtMarch_Total;
                // 4月
                case 4:
                    // 支給総計 - 4月
                    return this.txtApril_Total;
                // 5月
                case 5:
                    // 支給総計 - 5月
                    return this.txtMay_Total;
                // 6月
                case 6:
                    // 支給総計 - 6月
                    return this.txtJune_Total;
                // 7月
                case 7:
                    // 支給総計 - 7月
                    return this.txtJuly_Total;
                // 8月
                case 8:
                    // 支給総計 - 8月
                    return this.txtAugust_Total;
                // 9月
                case 9:
                    // 支給総計 - 9月
                    return this.txtSeptember_Total;
                // 10月
                case 10:
                    // 支給総計 - 10月
                    return this.txtOctober_Total;
                // 11月
                case 11:
                    // 支給総計 - 11月
                    return this.txtNovember_Total;
                // 12月
                case 12:
                    // 支給総計 - 12月
                    return this.txtDecember_Total;

                default:
                    // 支給総計 - 1月
                    return this.txtJanuary_Total;
            }
        }

        /// <summary>
        /// 判定値から差引支給額のText Boxを導出する
        /// </summary>
        /// <param name="value">判定値</param>
        /// <returns></returns>
        private TextBox FindTextBoxTotalDeducted(
            int value
        )
        {
            switch (value)
            {
                // 1月
                case 1:
                    // 差引支給額 - 1月
                    return this.txtJanuary_TotalDeducted;
                // 2月
                case 2:
                    // 差引支給額 - 2月
                    return this.txtFeburary_TotalDeducted;
                // 3月
                case 3:
                    // 差引支給額 - 3月
                    return this.txtMarch_TotalDeducted;
                // 4月
                case 4:
                    // 差引支給額 - 4月
                    return this.txtApril_TotalDeducted;
                // 5月
                case 5:
                    // 差引支給額 - 5月
                    return this.txtMay_TotalDeducted;
                // 6月
                case 6:
                    // 差引支給額 - 6月
                    return this.txtJune_TotalDeducted;
                // 7月
                case 7:
                    // 差引支給額 - 7月
                    return this.txtJuly_TotalDeducted;
                // 8月
                case 8:
                    // 差引支給額 - 8月
                    return this.txtAugust_TotalDeducted;
                // 9月
                case 9:
                    // 差引支給額 - 9月
                    return this.txtSeptember_TotalDeducted;
                // 10月
                case 10:
                    // 差引支給額 - 10月
                    return this.txtOctober_TotalDeducted;
                // 11月
                case 11:
                    // 差引支給額 - 11月
                    return this.txtNovember_TotalDeducted;
                // 12月
                case 12:
                    // 差引支給額 - 12月
                    return this.txtDecember_TotalDeducted;

                default:
                    // 差引支給額 - 1月
                    return this.txtJanuary_TotalDeducted;
            }
        }

        #endregion

        /// <summary>
        /// 月収チェック
        /// </summary>
        /// <param name="reader">MySql DataReader</param>
        /// <param name="month">判定月</param>
        /// <param name="monthCheckTotal">支給総計</param>
        /// <param name="monthCheckTotalDeduced">差引支給額</param>
        private void CheckSalaryPerMonth(
            MySqlDataReader reader,
            int month,
            MonthCheck monthCheckTotal,
            MonthCheck monthCheckTotalDeduced
        )
        {
            // 対象年
            string strTargetYear = this.txtYear.Text;

            // 対象月
            string strMonth = month.ToString("00");

            // 支給総計
            var textBoxTotal = monthCheckTotal(month);

            // 差引支給額
            var textBoxTotalDeducted = monthCheckTotalDeduced(month);

            // データが存在するか
            var existDeduction = this.DataSetPayslip.HasDataInTable(
                            // テーブル名
                            this.DataSetPayslip.TableDeduction,
                            // 対象日付
                            strTargetYear + "/" + strMonth
                        );

            if (existDeduction)
            {
                // 総計
                var totalSalary
                    = this.DataSetPayslip.GetDataInTable(
                            // テーブル名
                            this.DataSetPayslip.TableAllowance,
                            // 対象日付
                            strTargetYear + "/" + strMonth,
                            // 列名
                            nameof(this.ItemAllowance.TotalSalary)
                        );

                // 差引額
                var totalDeductedSalary
                    = this.DataSetPayslip.GetDataInTable(
                            // テーブル名
                            this.DataSetPayslip.TableAllowance,
                            // 対象日付
                            strTargetYear + "/" + strMonth,
                            // 列名
                            nameof(this.ItemAllowance.TotalDeductedSalary)
                        );

                // 差引額 = 0 ?
                if (Convert.ToInt32(totalDeductedSalary) == 0)
                {
                    textBoxTotal.Text = "0";
                    textBoxTotalDeducted.Text = "0";

                    return;
                }

                textBoxTotal.Text = this.Utils.FormatIntToPrice(totalSalary.ToString());
                textBoxTotalDeducted.Text = this.Utils.FormatIntToPrice(totalDeductedSalary.ToString());

                return;
            }

            // No Value
            textBoxTotal.Text = string.Empty;
            textBoxTotalDeducted.Text = string.Empty;
        }

        /// <summary>
        /// 各月の月収と年収を取得します。
        /// </summary>
        private void GetAnnualIncomey()
        {
            // コネクションオブジェクトとコマンドオブジェクトを生成します。
            using (var connection = new MySqlConnection(ConnectionString))
            {
                using (var command = new MySqlCommand())
                {
                    int sumTotal = 0;
                    int sumTotalDeducted = 0;

                    // 取得 - 年収
                    var reader = this.DatabaseTransaction.GetAnnualIncome(
                        // 対象年
                        this.txtYear.Text
                    );

                    // 件数0は除外
                    if (!reader.HasRows)
                    {
                        return;
                    }

                    while (reader.Read())
                    {
                        string work_month = (string)reader["work_month"];
                        int total_salary = (int)reader["total_salary"];
                        int total_deducted_salary = (int)reader["total_deducted_salary"];

                        // 合計額
                        sumTotal = sumTotal + total_salary;
                        sumTotalDeducted = sumTotalDeducted + total_deducted_salary;
                    }

                    // Month 1 to 12
                    for (var month = 1; month <= 12; month++)
                    {
                        // 月収チェック
                        this.CheckSalaryPerMonth(
                            // MySql DataReader
                            reader,
                            // 判定月
                            month,
                            // 支給総計
                            this.FindTextBoxTotal,
                            // 差引支給額
                            this.FindTextBoxTotalDeducted
                        );
                    }

                    // 合計
                    this.txtSum_Total.Text = this.Utils.FormatIntToPrice(sumTotal.ToString());
                    this.txtSum_TotalDeducted.Text = this.Utils.FormatIntToPrice(sumTotalDeducted.ToString());

                    // 年号取得
                    this.GetEra();
                }
            }
        }

        /// <summary>
        /// 年号取得
        /// </summary>
        private void GetEra()
        {
            // CultureInfo
            var ci = new System.Globalization.CultureInfo("ja-JP", false);

            // Calendar
            ci.DateTimeFormat.Calendar = new System.Globalization.JapaneseCalendar();

            // 西暦
            var date = new DateTime(Convert.ToInt32(this.txtYear.Text), Convert.ToInt32(this.txtMonth.Text), 1);
            
            // (年号)gg年
            var era = date.ToString("gy", ci);

            // Text
            this.lblTargetYear.Text = this.txtYear.Text + "年 (" + era + "年)";
        }

        /// <summary>
        /// Clear - 入力項目
        /// </summary>
        private void Clear()
        {
            // Clear - Text Box
            this.ClearTextBox();

            // Set - Default Color
            this.SetDefaultColor();

            // Set - Payslip
            this.SeTables();
        }

        /// <summary>
        /// Set Tables
        /// </summary>
        private void SeTables()
        {
            // 支給額
            this.DataSetPayslip.SetTableAllowance();

            // 控除額
            this.DataSetPayslip.SetTableDeduction();

            // 勤務備考
            this.DataSetPayslip.SetTableWorkingReferences();

            // 副業
            this.DataSetPayslip.SetTableSideBusiness();
        }

        #region Clear Text Box

        /// <summary>
        /// Clear - Text Box
        /// </summary>
        private void ClearTextBox()
        {
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
            this.txtNursingInsurance.Text = string.Empty;
            this.txtWelfareAnnuity.Text = "0";
            this.txtEmploymentInsurance.Text = "0";
            this.txtIncomeTax.Text = "0";
            this.txtMunicipalTax.Text = string.Empty;
            this.txtFriendshipAssociation.Text = "0";
            this.txtYearEndTaxAdjustment.Text = string.Empty;
            this.txtTotalDeduct.Text = "0";
            this.txtTotalDeductedSalary.Text = string.Empty;

            // 勤務備考
            this.txtOvertimeTime.Text = string.Empty;
            this.txtWeekendWorktime.Text = string.Empty;
            this.txtMidnightWorktime.Text = string.Empty;
            this.txtLateAbsent.Text = string.Empty;
            this.txtInsurance.Text = "0";
            this.txtNumberOfDependent.Text = string.Empty;
            this.txtPaidVacation.Text = "0";
            this.txtWorkingHours.Text = "0";
            this.txtWorkplace.Text = string.Empty;

            // 備考など
            this.txtSideBusiness.Text = "0";
            this.txtPerquisite.Text = "0";
            this.txtOthers.Text = "0";
            this.txtRemarks.Text = string.Empty;
        }

        #endregion

        #region Set Default Color

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

        #endregion

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

            if (confirmClose == DialogResult.Yes)
            {
                Close();
            }
        }

        /// <summary>
        /// 新規登録時のデフォルト明細を設定する
        /// </summary>
        private async void btnSetDefault_Click(object sender, EventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                var strTargetDate = 
                    // 対象日付の取得
                    this.GetTargetDate(
                        // デフォルト明細を使う
                        true
                    );

                var exist = 
                    // IDチェック
                    this.DatabaseTransaction.ExistId(
                        // 接続
                        connection,
                        // 対象日付
                        strTargetDate
                    );

                // not Exist ?
                if (!exist)
                {
                    // 登録
                    await this.DatabaseTransactionInsert();
                }
                else
                {
                    // 更新
                    this.DatabaseTransactionUpdate();
                }
            }
        }

        /// <summary>
        /// 前年と金額比較する
        /// </summary>
        /// <param name="targetColumnName">対象列名</param>
        private void CompareWithPreviousYear(string targetColumnName, string strTargetDate, string strTargetPreviousDate)
        {
            // 支給額
            try
            {
                // 前年と金額比較
                this.DatabaseTransaction.CompareWithPreviousMonth(
                    // 対象列名
                    targetColumnName,
                    // 対象日付
                    strTargetDate,
                    // 前年日付
                    strTargetPreviousDate,
                    // 金額
                    ref targetMoney,
                    // 前年の金額
                    ref targetPreviousMoney
                );

                if (targetMoney > targetPreviousMoney)
                {
                    // 去年よりも増↑
                    this.lblStatus.ForeColor = Color.Red;
                    this.lblStatus.Text = "前年比： \\" + 
                        // 数値から金額へ
                        this.Utils.FormatIntToPrice(
                            // 金額
                            (targetMoney - targetPreviousMoney).ToString()
                        ) + " UP";

                    return;
                }

                if (targetMoney < targetPreviousMoney)
                {
                    // 去年よりも減↓
                    this.lblStatus.ForeColor = Color.Blue;
                    this.lblStatus.Text = "前年比： \\" +
                        // 数値から金額へ
                        this.Utils.FormatIntToPrice(
                            // 金額
                            (targetPreviousMoney - targetMoney).ToString()
                        ) + " DOWN";

                    return;
                }

                // 同額
                this.lblStatus.ForeColor = Color.Green;
                this.lblStatus.Text = "前年比： \\0";
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

                // DataSet - 日付
                this.DataSetPayslip.TargetDate = this.txtYear.Text + "/" + Convert.ToInt32(this.txtMonth.Text).ToString("00");

                try
                {
                    // 年収取得
                    this.GetAnnualIncomey();

                    // 給与明細の取得
                    this.GetSalary(false);
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

        #region MouseEnter

        /// <summary>
        /// 共通 - MouseEnter
        /// </summary>
        /// <param name="month">対象月</param>
        /// <param name="ColumnName">取得する列名</param>
        private void Common_MouseEnter(
            // 対象月
            int month,
            // 取得する列名
            string ColumnName
        )
        {
            // 対象月
            string strMonth = month.ToString("00");
            var strTargetDate = this.txtYear.Text + "/" + strMonth;

            // 前年
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString() + "/" + strMonth;
            
            // 金額比較
            CompareWithPreviousYear(
                // 列名
                ColumnName,
                // 対象月
                strTargetDate,
                // 前年
                strTargetPreviousDate
            );
        }

        /// <summary>
        /// MouseEnter - 額面 - 1月
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        private void txtJanuary_Total_MouseEnter(
            // Sender
            object sender,
            // Event Args
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                1,
                // 取得する列名
                "SALARY.total_salary"
            );
        }

        /// <summary>
        /// MouseEnter - 額面 - 2月
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        private void txtFeburary_Total_MouseEnter(
            // Sender
            object sender,
            // Event Args
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                2,
                // 取得する列名
                "SALARY.total_salary"
            );
        }

        /// <summary>
        /// MouseEnter - 額面 - 3月
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        private void txtMarch_Total_MouseEnter(
            // Sender
            object sender,
            // Event Args
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                3,
                // 取得する列名
                "SALARY.total_salary"
            );
        }

        /// <summary>
        /// MouseEnter - 額面 - 4月
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        private void txtApril_Total_MouseEnter(
            // Sender
            object sender,
            // Event Args
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                4,
                // 取得する列名
                "SALARY.total_salary"
            );
        }

        /// <summary>
        /// MouseEnter - 額面 - 5月
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        private void txtMay_Total_MouseEnter(
            // Sender
            object sender,
            // Event Args
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                5,
                // 取得する列名
                "SALARY.total_salary"
            );
        }

        /// <summary>
        ///  MouseEnter - 額面 - 6月
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        private void txtJune_Total_MouseEnter(
            // Sender
            object sender,
            // Event Args
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                6,
                // 取得する列名
                "SALARY.total_salary"
            );
        }

        /// <summary>
        /// MouseEnter - 額面 - 7月
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        private void txtJuly_Total_MouseEnter(
            // Sender
            object sender,
            // Event Args
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                7,
                // 取得する列名
                "SALARY.total_salary"
            );
        }

        /// <summary>
        /// MouseEnter - 額面 - 8月
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        private void txtAugust_Total_MouseEnter(
            // Sender
            object sender,
            // Event Args
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                8,
                // 取得する列名
                "SALARY.total_salary"
            );
        }

        /// <summary>
        /// MouseEnter - 額面 - 9月
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        private void txtSeptember_Total_MouseEnter(
            // Sender
            object sender,
            // Event Args
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                9,
                // 取得する列名
                "SALARY.total_salary"
            );
        }

        /// <summary>
        /// MouseEnter - 額面 - 10月
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        private void txtOctober_Total_MouseEnter(
            // Sender
            object sender,
            // Event Args
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                10,
                // 取得する列名
                "SALARY.total_salary"
            );
        }

        /// <summary>
        /// MouseEnter - 額面 - 11月
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        private void txtNovember_Total_MouseEnter(
            // Sender
            object sender,
            // Event Args
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                11,
                // 取得する列名
                "SALARY.total_salary"
            );
        }

        /// <summary>
        /// MouseEnter - 額面 - 12月
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        private void txtDecember_Total_MouseEnter(
            // Sender
            object sender,
            // Event Args
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                12,
                // 取得する列名
                "SALARY.total_salary"
            );
        }

        /// <summary>
        /// MouseEnter - 手取 - 1月
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        private void txtJanuary_TotalDeducted_MouseEnter(
            // Sender
            object sender,
            // Event Args
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                1,
                // 取得する列名
                "SALARY.total_deducted_salary"
            );
        }

        // 手取(2月)
        private void txtFeburary_TotalDeducted_MouseEnter(
            // Sender
            object sender,
            // Event Args
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                2,
                // 取得する列名
                "SALARY.total_deducted_salary"
            );
        }

        // 手取(3月)
        private void txtMarch_TotalDeducted_MouseEnter(
            // Sender
            object sender,
            // Event Args
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                3,
                // 取得する列名
                "SALARY.total_deducted_salary"
            );
        }

        // 手取(4月)
        private void txtApril_TotalDeducted_MouseEnter(
            // Sender
            object sender,
            // Event Args
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                4,
                // 取得する列名
                "SALARY.total_deducted_salary"
            );
        }

        // 手取(5月)
        private void txtMay_TotalDeducted_MouseEnter(
            // Sender
            object sender,
            // Event Args
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                5,
                // 取得する列名
                "SALARY.total_deducted_salary"
            );
        }

        // 手取(6月)
        private void txtJune_TotalDeducted_MouseEnter(
            // Sender
            object sender,
            // Event Args
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                6,
                // 取得する列名
                "SALARY.total_deducted_salary"
            );
        }

        // 手取(7月)
        private void txtJuly_TotalDeducted_MouseEnter(
            // Sender
            object sender,
            // Event Args
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                7,
                // 取得する列名
                "SALARY.total_deducted_salary"
            );
        }

        // 手取(8月)
        private void txtAugust_TotalDeducted_MouseEnter(
            // Sender
            object sender,
            // Event Args
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                8,
                // 取得する列名
                "SALARY.total_deducted_salary"
            );
        }

        // 手取(9月)
        private void txtSeptember_TotalDeducted_MouseEnter(
            // Sender
            object sender,
            // Event Args
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                9,
                // 取得する列名
                "SALARY.total_deducted_salary"
            );
        }

        // 手取(10月)
        private void txtOctober_TotalDeducted_MouseEnter(
            // Sender
            object sender,
            // Event Args
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                10,
                // 取得する列名
                "SALARY.total_deducted_salary"
            );
        }

        // 手取(11月)
        private void txtNovember_TotalDeducted_MouseEnter(
            // Sender
            object sender,
            // Event Args
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                11,
                // 取得する列名
                "SALARY.total_deducted_salary"
            );
        }

        // 手取(12月)
        private void txtDecember_TotalDeducted_MouseEnter(
            // Sender
            object sender,
            // Event Args
            EventArgs e
        )
        {
            // 共通 - MouseEnter
            this.Common_MouseEnter(
                // 対象月
                12,
                // 取得する列名
                "SALARY.total_deducted_salary"
            );
        }

        #endregion

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
            GetLastSalary("total_salary");
        }

        private void txtSum_TotalDeducted_MouseEnter(object sender, EventArgs e)
        {
            GetLastSalary("total_deducted_salary");
        }

        /// <summary>
        /// 去年の月収と比較する
        /// </summary>
        /// <param name="displaySort">比較対象のカラム</param>
        private void GetLastSalary(
            // 比較対象のカラム
            string displaySort
        )
        {
            // 昨年
            var strTargetPreviousDate = (Convert.ToInt32(this.txtYear.Text) - 1).ToString();

            try
            {
                // Data Reader
                this.DatabaseTransaction.CompareWithPreviousYear(
                    // 比較対象の日付
                    strTargetPreviousDate
                );

                // 月収比較
                this.CompareMoneyDiff(
                    // 
                    displaySort
                );
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// 月収比較
        /// </summary>
        /// <param name="reader">DataReader</param>
        /// <param name="displaySort">比較対象のカラム</param>
        private void CompareMoneyDiff(
            // 比較対象のカラム
            string displaySort
        )
        {
            if (displaySort == "total_salary")
            {
                int moneyDiffTotal = this.Utils.FormatPriceToInt(txtSum_Total.Text);
                int lastTotalSalary = this.DatabaseTransaction.LastTotalSalary;

                // 年収 greater than 去年の年収
                if (moneyDiffTotal > lastTotalSalary)
                {
                    // 去年よりも増↑
                    this.lblStatus.ForeColor = Color.Red;
                    this.lblStatus.Text = "前年比： \\" + this.Utils.FormatIntToPrice((moneyDiffTotal - lastTotalSalary).ToString()) + " UP";

                    return;
                }

                // 年収 less than 去年の年収
                if (targetMoney < targetPreviousMoney)
                {
                    // 去年よりも減↓
                    this.lblStatus.ForeColor = Color.Blue;
                    this.lblStatus.Text = "前年比： \\" + this.Utils.FormatIntToPrice((lastTotalSalary - moneyDiffTotal).ToString()) + " DOWN";

                    return;
                }

                // 去年と同額
                this.lblStatus.ForeColor = Color.Green;
                this.lblStatus.Text = "前年比： \\0";
            }
            else
            {
                int moneyDiffDeductTotal = this.Utils.FormatPriceToInt(txtSum_TotalDeducted.Text);
                int lastTotalDeductedSalary = this.DatabaseTransaction.LastTotalDeductedSalary;

                // 手取 greater than 去年の年収
                if (moneyDiffDeductTotal > lastTotalDeductedSalary)
                {
                    // 去年よりも増↑
                    this.lblStatus.ForeColor = Color.Red;
                    this.lblStatus.Text = "前年比： \\" + this.Utils.FormatIntToPrice((moneyDiffDeductTotal - lastTotalDeductedSalary).ToString()) + " UP";

                    return;
                }

                // 手取 less than 去年の年収
                if (moneyDiffDeductTotal < lastTotalDeductedSalary)
                {
                    // 去年よりも減↓
                    this.lblStatus.ForeColor = Color.Blue;
                    this.lblStatus.Text = "前年比： \\" + this.Utils.FormatIntToPrice((lastTotalDeductedSalary - moneyDiffDeductTotal).ToString()) + " DOWN";

                    return;
                }

                // 去年と同額
                this.lblStatus.ForeColor = Color.Green;
                this.lblStatus.Text = "前年比： \\0";
            }
        }

        #region Excel出力

        /// <summary>
        /// Excel出力
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Argument</param>
        private void button1_Click(
            // Sender
            object sender, 
            // Event Argument
            EventArgs e
        )
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

        /// <summary>
        /// Excelに出力
        /// </summary>
        /// <remarks>
        /// Excelに出力する
        /// </remarks>
        private void ExcelOutput()
        {
            // 元のカーソルを保持
            Cursor preCursor = Cursor.Current;

            // カーソルを待機カーソルに変更
            Cursor.Current = Cursors.WaitCursor;

            this.lblStatus.Text = "出力中です。。。";

            //Excelオブジェクトの初期化
            Excel.Application ExcelApp = null;
            Excel.Workbooks wbs = null;

            //Excelシートのインスタンスを作る
            ExcelApp = new Excel.Application();

            wbs = ExcelApp.Workbooks;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    var result = this.DatabaseTransaction.GetAllPaySlip(connection);
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

            #endregion

        }
    }
}
