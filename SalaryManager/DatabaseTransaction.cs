using System.Collections.Generic;
using MySql.Data.MySqlClient;

/// <summary>
/// Salary Manager
/// </summary>
namespace SalaryManager
{

    /// <summary>
    /// トランザクション
    /// </summary>
    public class DatabaseTransaction
    {
        

        #region Utilities

        /// <summary>
        /// 共通ユーティリティ
        /// </summary>
        private Utils Utils
        {
            get;
            set;
        }

        #endregion

        #region Elements

        /// <summary>
        /// 支給額
        /// </summary>
        public ItemAllowance ItemAllowance
        {
            get;
            set;
        } = new ItemAllowance();

        /// <summary>
        /// 控除額
        /// </summary>
        public ItemDeduction ItemDeduction
        {
            get;
            set;
        } = new ItemDeduction();

        /// <summary>
        /// 勤務備考
        /// </summary>
        public ItemWorkingReferences ItemWorkingReferences
        {
            get;
            set;
        } = new ItemWorkingReferences();

        /// <summary>
        /// 副業
        /// </summary>
        public ItemSideBusiness ItemSideBusiness
        {
            get;
            set;
        } = new ItemSideBusiness();

        #endregion

        /// <summary>
        /// 前年との金額比較用
        /// </summary>
        /// <value>
        /// (対象月, 金額）
        /// </value>
        private static Dictionary<string, int> MapCompareToPreviousYear
        {
            get;
            set;
        } = new Dictionary<string, int>();

        /// <summary>
        /// 去年の総支給額
        /// </summary>
        public int LastTotalSalary
        {
            get;
            set;
        }


        /// <summary>
        /// 去年の差引支給額
        /// </summary>
        public int LastTotalDeductedSalary
        {
            get;
            set;
        }

        #region データの有無

        /// <summary>
        /// データの有無
        /// </summary>
        public bool Existence
        {
            get;
            set;
        } = false;

        #endregion

        /// <summary>
        /// 変更箇所の照合用
        /// </summary>
        public Dictionary<int, string> MapRegisteredData
        {
            get;
            set;
        } = new Dictionary<int, string>();

        // 接続情報
        private static readonly string Server = "localhost";      // ホスト名
        private static readonly int Port = 3305;                  // ポート番号
        private static readonly string Database = "mysql";       // データベース名
        private static readonly string Uid = "root";              // ユーザ名
        private static readonly string Pwd = "";          // パスワード

        public static bool isDefault = false;       // 新規登録時のデフォルト明細フラグ

        // 接続文字列
        private static readonly string ConnectionString = $"Server={Server}; Port={Port}; Database={Database}; Uid={Uid}; Pwd={Pwd}";

        #region 取得 - 日付の有無

        /// <summary>
        /// 指定された日付データをチェックする。
        /// </summary>
        /// <param name="connection">接続</param>
        /// <param name="targetDate">対象日付</param>
        /// <returns>日付の有無</returns>
        public bool ExistId
        (
            // 接続
            MySql.Data.MySqlClient.MySqlConnection connection,
            // 対象日付
            string targetDate
        )
        {
            var sql = string.Empty;

            sql += "SELECT COUNT(*) ";
            sql += "  FROM mylife.t_salary SALARY ";
            sql += "LEFT OUTER JOIN mylife.t_deduct DEDUCT ";
            sql += " ON SALARY.ID = DEDUCT.ID ";
            sql += "LEFT OUTER JOIN mylife.t_duty DUTY ";
            sql += " ON SALARY.ID = DUTY.ID ";
            sql += "LEFT OUTER JOIN mylife.t_side_business SIDE ";
            sql += " ON SALARY.ID = SIDE.ID ";
            sql += " WHERE SALARY.work_month = '" + targetDate + "' "
                ;

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            // コネクションをオープンします。
            cmd.Connection.Open();

            var value = System.Convert.ToInt32(cmd.ExecuteScalar());

            return (value > 0);
        }

        #endregion

        #region 取得 - 給与明細

        /// <summary>
        /// 指定した給与明細を取得します。
        /// </summary>
        public MySqlDataReader GetSalary(
            string strTargetDate
        )
        {
            MySql.Data.MySqlClient.MySqlConnection connection = new MySql.Data.MySqlClient.MySqlConnection(ConnectionString);

            var sql = string.Empty;

            // コネクションをオープンします。
            connection.Open();

            var command = new MySql.Data.MySqlClient.MySqlCommand();

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

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            MySqlDataReader results = cmd.ExecuteReader();

            results.Read();

            return results;
        }

        #endregion

        #region 取得 - 月収と年収

        /// <summary>
        /// 各月の月収と年収を取得します。
        /// </summary>
        /// <param name="strTargetYear">対象年</param>
        public MySqlDataReader GetAnnualIncome(
            // 対象年
            string strTargetYear
        )
        {
            // コネクションオブジェクトとコマンドオブジェクトを生成します。
            MySqlConnection connection = new MySqlConnection(ConnectionString);

            // データ検索SQLを実行します。
            var command = new MySqlCommand();

            // コネクションをオープンします。
            connection.Open();

            // データ検索SQLを実行します。
            command.Connection = connection;

            string sql = string.Empty;

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

            return command.ExecuteReader();
        }

        #endregion

        #region 取得 - 日付の有無

        /// <summary>
        /// 指定された日付データをチェックする。
        /// </summary>
        /// <param name="connection">接続</param>
        /// <returns>日付の有無</returns>
        public int GetId
        (
            // 接続
            MySql.Data.MySqlClient.MySqlConnection connection
        )
        {
            var sql = string.Empty;

            sql += "SELECT COUNT(ID) ";
            sql += " FROM mylife.t_salary ";

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            var value = cmd.ExecuteScalar();

            return System.Convert.ToInt32(value);
        }

        #endregion

        #region 取得 - 前年比較

        /// <summary>
        /// 前年と金額比較する
        /// </summary>
        /// <param name="targetColumnName">対象列名</param>
        /// <param name="strTargetDate">対象日付</param>
        /// <param name="strTargetPreviousDate">前年日付</param>
        /// <param name="targetMoney">金額</param>
        /// <param name="targetPreviousMoney">前年の金額</param>
        public void CompareWithPreviousMonth(
            string targetColumnName,
            string strTargetDate,
            string strTargetPreviousDate,
            ref int targetMoney,
            ref int targetPreviousMoney
        )
        {
            // マップ初期化
            MapCompareToPreviousYear.Clear();

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

                        string sql = string.Empty;

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

                                MapCompareToPreviousYear.Add(work_month, money);
                            }
                        }
                    }
                }

                targetMoney = MapCompareToPreviousYear[strTargetDate];
                targetPreviousMoney = MapCompareToPreviousYear[strTargetPreviousDate];
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// 去年の年収と比較する
        /// </summary>
        /// <param name="strTargetPreviousDate">比較対象のカラム</param>
        public void CompareWithPreviousYear(
            // 比較対象の日付
            string strTargetPreviousDate
        )
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

                    string sql = string.Empty;

                    sql += "SELECT SALARY.year , ";
                    sql += " SUM(SALARY.total_salary) AS total_salary , ";
                    sql += " SUM(SALARY.total_deducted_salary) AS total_deducted_salary ";
                    sql += "FROM mylife.t_salary SALARY ";
                    sql += "WHERE SALARY.year = '" + strTargetPreviousDate + "' ";
                    sql += "AND SALARY.id <> '0' ";
                    sql += "GROUP BY year ";

                    command.CommandText = sql;

                    var reader = command.ExecuteReader();

                    // 件数0は除外
                    if (!reader.HasRows)
                    {
                        return;
                    }

                    reader.Read();

                    this.LastTotalSalary = System.Convert.ToInt32(reader["total_salary"]);
                    this.LastTotalDeductedSalary = System.Convert.ToInt32(reader["total_deducted_salary"]);
                }
            }
        }

        #endregion

        #region 登録 - 支給額

        /// <summary>
        /// 登録 - 支給額
        /// </summary>
        /// <param name="connection">接続</param>
        /// <param name="id">ID</param>
        /// <param name="year">年</param>
        /// <param name="strTargetDate">対象日付</param>
        /// <returns>登録済の行数</returns>
        public System.Threading.Tasks.Task<int> Insert_SalaryAsync(
            // 接続
            MySqlConnection connection,
            // ID
            int id,
            // 年
            string year,
            // 対象日付
            string strTargetDate
        )
        {
            // 支給額
            var allowance = this.ItemAllowance;

            // SQL - Empty
            var sql = string.Empty;

            sql += "INSERT mylife.t_salary VALUES (";
            sql += " @id ,";
            sql += " @year ,";
            sql += " @work_month ,";
            sql += " @basic_salary ,";
            sql += " @executive_allowance ,";
            sql += " @dependency_allowance ,";
            sql += " @overtime_allowance ,";
            sql += " @daysoff_increased ,";
            sql += " @nightwork_increased ,";
            sql += " @housing_allowance ,";
            sql += " @late_absent ,";
            sql += " @transportation_expenses ,";
            sql += " @special_allowance ,";
            sql += " @spare_allowance ,";
            sql += " @total_salary ,";
            sql += " @total_deducted_salary ,";
            sql += " @create_date ,";
            sql += " @update_date";
            sql += " )";
            ;

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.Clear();

            // ID
            cmd.Parameters.Add(new MySqlParameter("id", id));

            // 年
            cmd.Parameters.Add(new MySqlParameter("year", year));

            // 対象日付
            cmd.Parameters.Add(new MySqlParameter("work_month", strTargetDate));

            // 基本給
            cmd.Parameters.Add(new MySqlParameter("basic_salary", allowance.BasicSalary.Value));

            // 役職手当
            cmd.Parameters.Add(new MySqlParameter("executive_allowance", allowance.ExecutiveAllowance.Value));

            // 扶養手当
            cmd.Parameters.Add(new MySqlParameter("dependency_allowance", allowance.DependencyAllowance.Value));

            // 時間外手当
            cmd.Parameters.Add(new MySqlParameter("overtime_allowance", allowance.OvertimeAllowance.Value));

            // 休日割増
            cmd.Parameters.Add(new MySqlParameter("daysoff_increased", allowance.DaysoffIncreased.Value));

            // 深夜割増
            cmd.Parameters.Add(new MySqlParameter("nightwork_increased", allowance.NightworkIncreased.Value));

            // 住宅手当
            cmd.Parameters.Add(new MySqlParameter("housing_allowance", allowance.HousingAllowance.Value));

            // 遅刻早退欠勤
            cmd.Parameters.Add(new MySqlParameter("late_absent", allowance.LateAbsent.Value));

            // 交通費
            cmd.Parameters.Add(new MySqlParameter("transportation_expenses", allowance.TransportationExpenses.Value));

            // 特別手当
            cmd.Parameters.Add(new MySqlParameter("special_allowance", allowance.SpecialAllowance.Value));

            // 予備
            cmd.Parameters.Add(new MySqlParameter("spare_allowance", allowance.SpareAllowance.Value));

            // 支出総計
            cmd.Parameters.Add(new MySqlParameter("total_salary", allowance.TotalSalary.Value));

            // 支出総計
            cmd.Parameters.Add(new MySqlParameter("total_deducted_salary", allowance.TotalDeductedSalary.Value));

            // 更新日
            cmd.Parameters.Add(new MySqlParameter("create_date", System.DateTime.Now));

            // 更新日
            cmd.Parameters.Add(new MySqlParameter("update_date", System.DateTime.Now));

            // Execute
            return cmd.ExecuteNonQueryAsync();
        }

        #endregion

        #region 登録 - 控除額

        /// <summary>
        /// 登録 - 控除額
        /// </summary>
        /// <param name="connection">接続</param>
        /// <param name="strTargetDate">対象日付</param>
        /// <remarks>レコードを更新するためのSQL文を作成</remarks>
        /// <returns>更新済の行数</returns>
        public System.Threading.Tasks.Task<int> Insert_DeductionAsync(
            // 接続
            MySqlConnection connection,
            // ID
            int id,
            // 年
            string year,
            // 対象日付
            string strTargetDate
        )
        {
            // 控除額
            var deduction = this.ItemDeduction;

            // SQL - Empty
            var sql = string.Empty;

            sql += "INSERT mylife.t_deduct VALUES (";
            sql += " @id ,";
            sql += " @work_month ,";
            sql += " @health_insurance ,";
            sql += " @nursing_insurance ,";
            sql += " @welfare_annuity ,";
            sql += " @employment_insurance ,";
            sql += " @income_tax ,";
            sql += " @municipal_tax ,";
            sql += " @friendship_association ,";
            sql += " @year_end_tax_adjustment ,";
            sql += " @total_deduct ,";
            sql += " @create_date ,";
            sql += " @update_date";
            sql += " )";

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.Clear();

            // ID
            cmd.Parameters.Add(new MySqlParameter("id", id));

            // 対象日付
            cmd.Parameters.Add(new MySqlParameter("work_month", strTargetDate));

            // 健康保険
            cmd.Parameters.Add(new MySqlParameter("health_insurance", deduction.HealthInsurance.Value));

            // 介護保険
            cmd.Parameters.Add(new MySqlParameter("nursing_insurance", deduction.NursingInsurance.Value));

            // 厚生年金
            cmd.Parameters.Add(new MySqlParameter("welfare_annuity", deduction.WelfareAnnuity.Value));

            // 雇用保険
            cmd.Parameters.Add(new MySqlParameter("employment_insurance", deduction.EmploymentInsurance.Value));

            // 所得税
            cmd.Parameters.Add(new MySqlParameter("income_tax", deduction.IncomeTax.Value));

            // 市町村税
            cmd.Parameters.Add(new MySqlParameter("municipal_tax", deduction.MunicipalTax.Value));

            // 互助会
            cmd.Parameters.Add(new MySqlParameter("friendship_association", deduction.FriendshipAssociation.Value));

            // 年末調整他
            cmd.Parameters.Add(new MySqlParameter("year_end_tax_adjustment", deduction.YearEndTaxAdjustment.Value));

            // 控除額計
            cmd.Parameters.Add(new MySqlParameter("total_deduct", deduction.TotalDeduct.Value));

            // 更新日
            cmd.Parameters.Add(new MySqlParameter("create_date", System.DateTime.Now));

            // 更新日
            cmd.Parameters.Add(new MySqlParameter("update_date", System.DateTime.Now));

            // Execute
            return cmd.ExecuteNonQueryAsync();
        }

        #endregion

        #region 登録 - 勤怠備考

        /// <summary>
        /// 登録 - 勤怠備考
        /// </summary>
        /// <param name="connection">接続</param>
        /// <param name="strTargetDate">対象日付</param>
        /// <remarks>レコードを更新するためのSQL文を作成</remarks>
        /// <returns>更新済の行数</returns>
        public System.Threading.Tasks.Task<int> Insert_WorkingReferencesAsync(
            // 接続
            MySqlConnection connection,
            // ID
            int id,
            // 年
            string year,
            // 対象日付
            string strTargetDate
        )
        {
            // 勤怠備考
            var workingReferences = this.ItemWorkingReferences;

            // SQL - Empty
            var sql = string.Empty;

            sql += "INSERT mylife.t_duty VALUES (";
            sql += " @id ,";
            sql += " @work_month ,";
            sql += " @overtime_time ,";
            sql += " @weekend_worktime ,";
            sql += " @midnight_worktime ,";
            sql += " @late_absent ,";
            sql += " @insurance ,";
            sql += " @number_of_dependent ,";
            sql += " @paid_vacation ,";
            sql += " @working_hours ,";
            sql += " @create_date ,";
            sql += " @update_date ,";
            sql += " @workplace";
            sql += " )";
            ;

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            // Clear
            cmd.Parameters.Clear();

            // ID
            cmd.Parameters.Add(new MySqlParameter("id", id));

            // 対象日付
            cmd.Parameters.Add(new MySqlParameter("work_month", strTargetDate));

            // 時間外労働
            cmd.Parameters.Add(new MySqlParameter("overtime_time", workingReferences.OvertimeTime.Value));

            // 休出時間
            cmd.Parameters.Add(new MySqlParameter("weekend_worktime", workingReferences.WeekendWorktime.Value));

            // 深夜時間
            cmd.Parameters.Add(new MySqlParameter("midnight_worktime", workingReferences.MidnightWorktime.Value));

            // 早退遅刻欠勤H
            cmd.Parameters.Add(new MySqlParameter("late_absent", workingReferences.LateAbsentH.Value));

            // 保険
            cmd.Parameters.Add(new MySqlParameter("insurance", workingReferences.Insurance.Value));

            // 扶養人数
            cmd.Parameters.Add(new MySqlParameter("number_of_dependent", workingReferences.NumberOfDependent.Value));

            // 有休残日数
            cmd.Parameters.Add(new MySqlParameter("paid_vacation", workingReferences.PaidVacation.Value));

            // 勤務時間
            cmd.Parameters.Add(new MySqlParameter("working_hours", workingReferences.WorkingHours.Value));

            // 更新日
            cmd.Parameters.Add(new MySqlParameter("create_date", System.DateTime.Now));

            // 更新日
            cmd.Parameters.Add(new MySqlParameter("update_date", System.DateTime.Now));

            // 勤務先
            cmd.Parameters.Add(new MySqlParameter("workplace", workingReferences.Workplace));

            // Execute
            return cmd.ExecuteNonQueryAsync();
        }

        #endregion

        #region 登録 - 副業

        /// <summary>
        /// 登録 - 副業
        /// </summary>
        /// <param name="connection">接続</param>
        /// <param name="strTargetDate">対象日付</param>
        /// <remarks>レコードを更新するためのSQL文を作成</remarks>
        /// <returns>更新済の行数</returns>
        public System.Threading.Tasks.Task<int> Insert_SideBusinessAsync
        (
            // 接続
            MySqlConnection connection,
            // ID
            int id,
            // 年
            string year,
            // 対象日付
            string strTargetDate
        )
        {
            // 副業
            var sideBusiness = this.ItemSideBusiness;

            // SQL - Empty
            var sql = string.Empty;

            sql += "INSERT mylife.t_side_business VALUES (";
            sql += " @id ,";
            sql += " @work_month ,";
            sql += " @side_business ,";
            sql += " @perquisite ,";
            sql += " @others ,";
            sql += " @remarks ,";
            sql += " @create_date ,";
            sql += " @update_date";
            sql += " )";

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            // Clear
            cmd.Parameters.Clear();

            // ID
            cmd.Parameters.Add(new MySqlParameter("id", id));

            // 対象日付
            cmd.Parameters.Add(new MySqlParameter("work_month", strTargetDate));

            // 時間外労働
            cmd.Parameters.Add(new MySqlParameter("side_business", sideBusiness.SideBusiness.Value));

            // 副業収入
            cmd.Parameters.Add(new MySqlParameter("perquisite", sideBusiness.Perquisite.Value));

            // その他
            cmd.Parameters.Add(new MySqlParameter("others", sideBusiness.Others.Value));

            // 備考
            cmd.Parameters.Add(new MySqlParameter("remarks", sideBusiness.Remarks));

            // 更新日
            cmd.Parameters.Add(new MySqlParameter("update_date", System.DateTime.Now));

            // 更新日
            cmd.Parameters.Add(new MySqlParameter("create_date", System.DateTime.Now));

            // Execute
            return cmd.ExecuteNonQueryAsync();
        }

        #endregion

        #region 更新 - 支給額

        /// <summary>
        /// 更新 - 支給額
        /// </summary>
        /// <param name="connection">接続</param>
        /// <param name="strTargetDate">対象日付</param>
        /// <remarks>更新済の行数</remarks>
        public System.Threading.Tasks.Task<int> UpdateSalaryAsync
        (
            // 接続
            MySqlConnection connection,
            // 対象日付
            string strTargetDate
        )
        {
            // 支給額
            var allowance = this.ItemAllowance;

            // SQL - Empty
            var sql = string.Empty;

            sql += "UPDATE mylife.t_salary SET ";
            sql += " basic_salary = @basic_salary ,";
            sql += " executive_allowance = @executive_allowance ,";
            sql += " dependency_allowance = @dependency_allowance ,";
            sql += " overtime_allowance = @overtime_allowance ,";
            sql += " daysoff_increased = @daysoff_increased ,";
            sql += " nightwork_increased = @nightwork_increased ,";
            sql += " housing_allowance = @housing_allowance ,";
            sql += " late_absent = @late_absent ,";
            sql += " transportation_expenses = @transportation_expenses ,";
            sql += " special_allowance = @special_allowance ,";
            sql += " spare_allowance = @spare_allowance ,";
            sql += " total_salary = @total_salary ,";
            sql += " total_deducted_salary = @total_deducted_salary ,";
            sql += " update_date = @update_date";
            sql += " where work_month = @work_month";

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.Clear();

            // 基本給
            cmd.Parameters.Add(new MySqlParameter("basic_salary", allowance.BasicSalary.Value));

            // 役職手当
            cmd.Parameters.Add(new MySqlParameter("executive_allowance", allowance.ExecutiveAllowance.Value));

            // 扶養手当
            cmd.Parameters.Add(new MySqlParameter("dependency_allowance", allowance.DependencyAllowance.Value));

            // 時間外手当
            cmd.Parameters.Add(new MySqlParameter("overtime_allowance", allowance.OvertimeAllowance.Value));

            // 休日割増
            cmd.Parameters.Add(new MySqlParameter("daysoff_increased", allowance.DaysoffIncreased.Value));

            // 深夜割増
            cmd.Parameters.Add(new MySqlParameter("nightwork_increased", allowance.NightworkIncreased.Value));

            // 住宅手当
            cmd.Parameters.Add(new MySqlParameter("housing_allowance", allowance.HousingAllowance.Value));

            // 遅刻早退欠勤
            cmd.Parameters.Add(new MySqlParameter("late_absent", allowance.LateAbsent.Value));

            // 交通費
            cmd.Parameters.Add(new MySqlParameter("transportation_expenses", allowance.TransportationExpenses.Value));

            // 特別手当
            cmd.Parameters.Add(new MySqlParameter("special_allowance", allowance.SpecialAllowance.Value));

            // 予備
            cmd.Parameters.Add(new MySqlParameter("spare_allowance", allowance.SpareAllowance.Value));

            // 支出総計
            cmd.Parameters.Add(new MySqlParameter("total_salary", allowance.TotalSalary.Value));

            // 支出総計
            cmd.Parameters.Add(new MySqlParameter("total_deducted_salary", allowance.TotalDeductedSalary.Value));

            // 更新日
            cmd.Parameters.Add(new MySqlParameter("update_date", System.DateTime.Now));

            // 対象日付
            cmd.Parameters.Add(new MySqlParameter("work_month", strTargetDate));

            // Execute
            return cmd.ExecuteNonQueryAsync();
        }

        #endregion

        #region 更新 - 控除額

        /// <summary>
        /// 更新 - 控除額
        /// </summary>
        /// <param name="connection">接続</param>
        /// <param name="strTargetDate">対象日付</param>
        /// <remarks>レコードを更新するためのSQL文を作成</remarks>
        /// <returns>更新済の行数</returns>
        public System.Threading.Tasks.Task<int> Update_DeductionAsync
        (
            // 接続
            MySqlConnection connection,
            // 対象日付
            string strTargetDate
        )
        {
            // 控除額
            var deduction = this.ItemDeduction;

            // SQL - Empty
            var sql = string.Empty;

            sql += "UPDATE mylife.t_deduct SET ";
            sql += " health_insurance = @health_insurance ,";
            sql += " nursing_insurance = @nursing_insurance ,";
            sql += " welfare_annuity = @welfare_annuity ,";
            sql += " employment_insurance = @employment_insurance ,";
            sql += " income_tax = @income_tax ,";
            sql += " municipal_tax = @municipal_tax ,";
            sql += " friendship_association = @friendship_association ,";
            sql += " year_end_tax_adjustment = @year_end_tax_adjustment ,";
            sql += " total_deduct = @total_deduct ,";
            sql += " update_date = @update_date";
            sql += " where work_month = @work_month";
            ;

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.Clear();

            // 健康保険
            cmd.Parameters.Add(new MySqlParameter("health_insurance", deduction.HealthInsurance.Value));

            // 介護保険
            cmd.Parameters.Add(new MySqlParameter("nursing_insurance", deduction.NursingInsurance.Value));

            // 厚生年金
            cmd.Parameters.Add(new MySqlParameter("welfare_annuity", deduction.WelfareAnnuity.Value));

            // 雇用保険
            cmd.Parameters.Add(new MySqlParameter("employment_insurance", deduction.EmploymentInsurance.Value));

            // 所得税
            cmd.Parameters.Add(new MySqlParameter("income_tax", deduction.IncomeTax.Value));

            // 市町村税
            cmd.Parameters.Add(new MySqlParameter("municipal_tax", deduction.MunicipalTax.Value));

            // 互助会
            cmd.Parameters.Add(new MySqlParameter("friendship_association", deduction.FriendshipAssociation.Value));

            // 年末調整他
            cmd.Parameters.Add(new MySqlParameter("year_end_tax_adjustment", deduction.YearEndTaxAdjustment.Value));

            // 控除額計
            cmd.Parameters.Add(new MySqlParameter("total_deduct", deduction.TotalDeduct.Value));

            // 更新日
            cmd.Parameters.Add(new MySqlParameter("update_date", System.DateTime.Now));

            // 対象日付
            cmd.Parameters.Add(new MySqlParameter("work_month", strTargetDate));

            // Execute
            return cmd.ExecuteNonQueryAsync();
        }

        #endregion

        #region 更新 - 勤怠備考

        /// <summary>
        /// 更新 - 勤怠備考
        /// </summary>
        /// <param name="connection">接続</param>
        /// <param name="strTargetDate">対象日付</param>
        /// <remarks>レコードを更新するためのSQL文を作成</remarks>
        /// <returns>更新済の行数</returns>
        public System.Threading.Tasks.Task<int> Update_WorkingReferencesAsync
        (
            // 接続
            MySqlConnection connection,
            // 対象日付
            string strTargetDate
        )
        {
            // 勤怠備考
            var workingReferences = this.ItemWorkingReferences;

            // SQL - Empty
            var sql = string.Empty;

            sql += "UPDATE mylife.t_duty SET ";
            sql += " overtime_time = @overtime_time ,";
            sql += " weekend_worktime = @weekend_worktime ,";
            sql += " midnight_worktime = @midnight_worktime ,";
            sql += " late_absent = @late_absent ,";
            sql += " insurance = @insurance ,";
            sql += " number_of_dependent = @number_of_dependent ,";
            sql += " paid_vacation = @paid_vacation ,";
            sql += " working_hours = @working_hours ,";
            sql += " workplace = @workplace ,";
            sql += " update_date = @update_date";
            sql += " where work_month = @work_month";
            ;

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            // Clear
            cmd.Parameters.Clear();

            // 時間外労働
            cmd.Parameters.Add(new MySqlParameter("overtime_time", workingReferences.OvertimeTime.Value));

            // 休出時間
            cmd.Parameters.Add(new MySqlParameter("weekend_worktime", workingReferences.WeekendWorktime.Value));

            // 深夜時間
            cmd.Parameters.Add(new MySqlParameter("midnight_worktime", workingReferences.MidnightWorktime.Value));

            // 早退遅刻欠勤H
            cmd.Parameters.Add(new MySqlParameter("late_absent", workingReferences.LateAbsentH.Value));

            // 保険
            cmd.Parameters.Add(new MySqlParameter("insurance", workingReferences.Insurance.Value));

            // 扶養人数
            cmd.Parameters.Add(new MySqlParameter("number_of_dependent", workingReferences.NumberOfDependent.Value));

            // 有休残日数
            cmd.Parameters.Add(new MySqlParameter("paid_vacation", workingReferences.PaidVacation.Value));

            // 勤務時間
            cmd.Parameters.Add(new MySqlParameter("working_hours", workingReferences.WorkingHours.Value));

            // 勤務先
            cmd.Parameters.Add(new MySqlParameter("workplace", workingReferences.Workplace));

            // 更新日
            cmd.Parameters.Add(new MySqlParameter("update_date", System.DateTime.Now));

            // 対象日付
            cmd.Parameters.Add(new MySqlParameter("work_month", strTargetDate));

            // Execute
            return cmd.ExecuteNonQueryAsync();
        }

        #endregion

        #region 更新 - 副業

        /// <summary>
        /// 更新 - 副業
        /// </summary>
        /// <param name="connection">接続</param>
        /// <param name="strTargetDate">対象日付</param>
        /// <remarks>レコードを更新するためのSQL文を作成</remarks>
        /// <returns>更新済の行数</returns>
        public System.Threading.Tasks.Task<int> Update_SideBusinessAsync
        (
            // 接続
            MySqlConnection connection,
            // 対象日付
            string strTargetDate
        )
        {
            // 副業
            var sideBusiness = this.ItemSideBusiness;

            // SQL - Empty
            var sql = string.Empty;

            sql += "UPDATE mylife.t_side_business SET ";
            sql += " side_business = @side_business ,";
            sql += " perquisite = @perquisite ,";
            sql += " others = @others ,";
            sql += " update_date = @update_date";
            sql += " where work_month = @work_month";
            ;

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            // Clear
            cmd.Parameters.Clear();

            // 時間外労働
            cmd.Parameters.Add(new MySqlParameter("side_business", sideBusiness.SideBusiness.Value));

            // 副業収入
            cmd.Parameters.Add(new MySqlParameter("perquisite", sideBusiness.Perquisite.Value));

            // その他
            cmd.Parameters.Add(new MySqlParameter("others", sideBusiness.Others.Value));

            // 更新日
            cmd.Parameters.Add(new MySqlParameter("update_date", System.DateTime.Now));

            // 対象日付
            cmd.Parameters.Add(new MySqlParameter("work_month", strTargetDate));

            // Execute
            return cmd.ExecuteNonQueryAsync();
        }

        #endregion

        #region 出力 - Excel

        /// <summary>
        /// 全給与明細の取得
        /// </summary>
        public MySqlDataReader GetAllPaySlip(
            // Connection
            MySqlConnection connection
        )
        {
            using (var command = new MySqlCommand())
            {
                // コネクションをオープンします。
                connection.Open();

                // データ検索SQLを実行します。
                command.Connection = connection;

                MySqlCommand selectCommand = new MySqlCommand(" SELECT  " +
                                                                "     SALARY.work_month " +                   // 対象日付
                                                                "     , SALARY.basic_salary " +                 // 01.基本給
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
                                                                "     SALARY.ID = SIDE.ID " +
                                                                " ORDER BY 1", connection);
                return selectCommand.ExecuteReader();

            }
        }

        #endregion
    }
}
