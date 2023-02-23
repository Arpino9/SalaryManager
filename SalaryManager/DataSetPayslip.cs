/// <summary>
/// Salary Manager
/// </summary>
namespace SalaryManager
{
    /// <summary>
    /// 給与明細
    /// </summary>
    public class DataSetPayslip
    {
        /// <summary>
        /// 対象日付
        /// </summary>
        public string TargetDate
        {
            get;
            set;
        }

        #region Key

        /// <summary>
        /// データセット
        /// </summary>
        public System.Data.DataSet DataSet
        {
            get;
            set;
        } = new System.Data.DataSet();

        /// <summary>
        /// Results
        /// </summary>
        public MySql.Data.MySqlClient.MySqlDataReader Results
        {
            get;
            set;
        }

        #endregion

        #region Element

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

        #region Table

        /// <summary>
        /// テーブル - 支給額
        /// </summary>
        public System.Data.DataTable TableAllowance
        {
            get;
            set;
        } = new System.Data.DataTable(nameof(TableAllowance));

        /// <summary>
        /// テーブル - 控除額
        /// </summary>
        public System.Data.DataTable TableDeduction
        {
            get;
            set;
        } = new System.Data.DataTable(nameof(TableDeduction));

        /// <summary>
        /// テーブル - 勤務備考
        /// </summary>
        public System.Data.DataTable TableWorkingReferences
        {
            get;
            set;
        } = new System.Data.DataTable(nameof(TableWorkingReferences));

        /// <summary>
        /// テーブル - 副業
        /// </summary>
        public System.Data.DataTable TableSIdeBusiness
        {
            get;
            set;
        } = new System.Data.DataTable(nameof(TableSIdeBusiness));

        #endregion

        #region Set - 支給額

        /// <summary>
        /// Set Table Allowance
        /// </summary>
        /// <remarks>
        /// 支給額テーブルに列を設定する
        /// </remarks>
        public void SetTableAllowance()
        {
            // 支給額
            var allowance = this.ItemAllowance;

            // 対象年月
            this.TableAllowance.Columns.Add(
                // 列名
                "WorkMonth",
                // 型名
                typeof(string)
            );

            // 基本給
            this.TableAllowance.Columns.Add(
                // 列名
                nameof(allowance.BasicSalary),
                // 型名
                allowance.BasicSalary.GetType()
            );

            // 役職手当
            this.TableAllowance.Columns.Add(
                // 列名
                nameof(allowance.ExecutiveAllowance),
                // 型名
                allowance.ExecutiveAllowance.GetType()
            );

            // 扶養手当
            this.TableAllowance.Columns.Add(
                // 列名
                nameof(allowance.DependencyAllowance),
                // 型名
                allowance.DependencyAllowance.GetType()
            );

            // 時間外手当
            this.TableAllowance.Columns.Add(
                // 列名
                nameof(allowance.OvertimeAllowance),
                // 型名
                allowance.OvertimeAllowance.GetType()
            );

            // 休日割増
            this.TableAllowance.Columns.Add(
                // 列名
                nameof(allowance.DaysoffIncreased),
                // 型名
                allowance.DaysoffIncreased.GetType()
            );

            // 深夜割増
            this.TableAllowance.Columns.Add(
                // 列名
                nameof(allowance.NightworkIncreased),
                // 型名
                allowance.NightworkIncreased.GetType()
            );

            // 住宅手当
            this.TableAllowance.Columns.Add(
                // 列名
                nameof(allowance.HousingAllowance),
                // 型名
                allowance.HousingAllowance.GetType()
            );

            // 遅刻早退欠勤
            this.TableAllowance.Columns.Add(
                // 列名
                nameof(allowance.LateAbsent),
                // 型名
                allowance.LateAbsent.GetType()
            );

            // 交通費
            this.TableAllowance.Columns.Add(
                // 列名
                nameof(allowance.TransportationExpenses),
                // 型名
                allowance.TransportationExpenses.GetType()
            );

            // 特別手当
            this.TableAllowance.Columns.Add(
                // 列名
                nameof(allowance.SpecialAllowance),
                // 型名
                allowance.SpecialAllowance.GetType()
            );

            // 予備
            this.TableAllowance.Columns.Add(
                // 列名
                nameof(allowance.SpareAllowance),
                // 型名
                allowance.SpareAllowance.GetType()
            );

            // 支給総計
            this.TableAllowance.Columns.Add(
                // 列名
                nameof(allowance.TotalSalary),
                // 型名
                allowance.TotalSalary.GetType()
            );

            // 差引支給額
            this.TableAllowance.Columns.Add(
                // 列名
                nameof(allowance.TotalDeductedSalary),
                // 型名
                allowance.TotalDeductedSalary.GetType()
            );
        }

        #endregion

        #region Set - 控除額

        /// <summary>
        /// Set Payslip
        /// </summary>
        /// <remarks>
        /// 控除額テーブルに列を設定する
        /// </remarks>
        public void SetTableDeduction()
        {
            // 控除額
            var deduction = this.ItemDeduction;

            // 対象年月
            this.TableDeduction.Columns.Add(
                // 列名
                "WorkMonth",
                // 型名
                typeof(string)
            );

            // 健康保険
            this.TableDeduction.Columns.Add(
                // 列名
                nameof(deduction.HealthInsurance),
                // 型名
                deduction.HealthInsurance.GetType()
            );

            // 介護保険
            this.TableDeduction.Columns.Add(
                // 列名
                nameof(deduction.NursingInsurance),
                // 型名
                deduction.NursingInsurance.GetType()
            );

            // 厚生年金
            this.TableDeduction.Columns.Add(
                // 列名
                nameof(deduction.WelfareAnnuity),
                // 型名
                deduction.WelfareAnnuity.GetType()
            );

            // 雇用保険
            this.TableDeduction.Columns.Add(
                // 列名
                nameof(deduction.EmploymentInsurance),
                // 型名
                deduction.EmploymentInsurance.GetType()
            );

            // 所得税
            this.TableDeduction.Columns.Add(
                // 列名
                nameof(deduction.IncomeTax),
                // 型名
                deduction.IncomeTax.GetType()
            );

            // 市町村税
            this.TableDeduction.Columns.Add(
                // 列名
                nameof(deduction.MunicipalTax),
                // 型名
                deduction.MunicipalTax.GetType()
            );

            // 互助会
            this.TableDeduction.Columns.Add(
                // 列名
                nameof(deduction.FriendshipAssociation),
                // 型名
                deduction.FriendshipAssociation.GetType()
            );

            // 年末調整他
            this.TableDeduction.Columns.Add(
                // 列名
                nameof(deduction.YearEndTaxAdjustment),
                // 型名
                deduction.YearEndTaxAdjustment.GetType()
            );

            // 控除額計
            this.TableDeduction.Columns.Add(
                // 列名
                nameof(deduction.TotalDeduct),
                // 型名
                deduction.TotalDeduct.GetType()
            );
        }

        #endregion

        #region Set - 勤務備考

        /// <summary>
        /// Set Payslip
        /// </summary>
        /// <remarks>
        /// データテーブルに列を設定する
        /// </remarks>
        public void SetTableWorkingReferences()
        {
            // 勤務備考
            var workingReferences = this.ItemWorkingReferences;

            // 対象年月
            this.TableWorkingReferences.Columns.Add(
                // 列名
                "WorkMonth", 
                // 型名
                typeof(string)
            );

            // 時間外時間
            this.TableWorkingReferences.Columns.Add(
                // 列名
                nameof(workingReferences.OvertimeTime),
                // 型名
                workingReferences.OvertimeTime.GetType()
            );

            // 休出時間
            this.TableWorkingReferences.Columns.Add(
                // 列名
                nameof(workingReferences.WeekendWorktime),
                // 型名
                workingReferences.WeekendWorktime.GetType()
            );

            // 深夜時間
            this.TableWorkingReferences.Columns.Add(
                // 列名
                nameof(workingReferences.MidnightWorktime),
                // 型名
                workingReferences.MidnightWorktime.GetType()
            );

            // 遅刻早退欠勤H
            this.TableWorkingReferences.Columns.Add(
                // 列名
                nameof(workingReferences.LateAbsentH),
                // 型名
                workingReferences.LateAbsentH.GetType()
            );

            // 支給額-保険
            this.TableWorkingReferences.Columns.Add(
                // 列名
                nameof(workingReferences.Insurance),
                // 型名
                workingReferences.Insurance.GetType()
            );

            // 扶養人数
            this.TableWorkingReferences.Columns.Add(
                // 列名
                nameof(workingReferences.NumberOfDependent),
                // 型名
                workingReferences.NumberOfDependent.GetType()
            );

            // 有給残日数
            this.TableWorkingReferences.Columns.Add(
                // 列名
                nameof(workingReferences.PaidVacation),
                // 型名
                workingReferences.PaidVacation.GetType()
            );

            // 勤務時間
            this.TableWorkingReferences.Columns.Add(
                // 列名
                nameof(workingReferences.WorkingHours),
                // 型名
                workingReferences.WorkingHours.GetType()
            );

            // 勤務先
            this.TableWorkingReferences.Columns.Add(
                nameof(workingReferences.Workplace), 
                typeof(string)
            );
        }

        #endregion

        #region Set - 副業

        /// <summary>
        /// Set Payslip
        /// </summary>
        /// <remarks>
        /// データテーブルに列を設定する
        /// </remarks>
        public void SetTableSideBusiness()
        {
            // 副業
            var sideBusiness = this.ItemSideBusiness;

            // 対象年月
            this.TableSIdeBusiness.Columns.Add(
                // 列名
                "WorkMonth", 
                // 型名
                typeof(string)
            );

            // 副業収入
            this.TableSIdeBusiness.Columns.Add(
                // 列名
                nameof(sideBusiness.SideBusiness),
                // 型名
                sideBusiness.SideBusiness.GetType()
            );

            // 臨時収入
            this.TableSIdeBusiness.Columns.Add(
                // 列名
                nameof(sideBusiness.Perquisite),
                // 型名
                sideBusiness.Perquisite.GetType()
            );

            // その他
            this.TableSIdeBusiness.Columns.Add(
                // 列名
                nameof(sideBusiness.Others),
                // 型名
                sideBusiness.Others.GetType()
            );

            // 備考
            this.TableSIdeBusiness.Columns.Add(
                // 列名
                nameof(sideBusiness.Remarks),
                // 型名
                typeof(string)
            );
        }

        #endregion

        #region Add

        /// <summary>
        /// Set Tables
        /// </summary>
        public void AddTables()
        {
            while (this.Results.Read())
            {
                // Add - 支給額
                this.AddTableAllowance();

                // Add - 支給額
                this.AddTableDeduction();

                // Add - 勤務備考
                this.AddTableWorkingReferences();

                // Add - 副業
                this.AddTableSideBusiness();
            }

            // Add - Table - Allowance
            this.DataSet.Tables.Add(this.TableAllowance);

            // Add - Table - Deduction
            this.DataSet.Tables.Add(this.TableDeduction);

            // Add - Table - WorkingReferences
            this.DataSet.Tables.Add(this.TableWorkingReferences);

            // Add - Table - SIdeBusiness
            this.DataSet.Tables.Add(this.TableSIdeBusiness);
        }

        #endregion

        #region Add - 支給額

        /// <summary>
        /// Add 支給額
        /// </summary>
        /// <param name="dataRow">行</param>
        private void AddTableAllowance()
        {
            // Table - Name
            this.TableAllowance.TableName = "Allowance";

            // Row
            System.Data.DataRow dataRow = this.TableAllowance.NewRow();

            // Row - 対象日付
            dataRow["WorkMonth"] = this.Results["work_month"].ToString();

            // Row - 基本給
            dataRow[nameof(this.ItemAllowance.BasicSalary)] = this.Results["basic_salary"].ToString();

            // Row - 役職手当
            dataRow[nameof(this.ItemAllowance.ExecutiveAllowance)] = this.Results["executive_allowance"].ToString();

            // Row - 扶養手当
            dataRow[nameof(this.ItemAllowance.DependencyAllowance)] = this.Results["dependency_allowance"].ToString();

            // Row - 時間外手当
            dataRow[nameof(this.ItemAllowance.OvertimeAllowance)] = this.Results["overtime_allowance"].ToString();

            // Row - 休日割増
            dataRow[nameof(this.ItemAllowance.DaysoffIncreased)] = this.Results["daysoff_increased"].ToString();

            // Row - 深夜割増
            dataRow[nameof(this.ItemAllowance.NightworkIncreased)] = this.Results["nightwork_increased"].ToString();

            // Row - 住宅手当
            dataRow[nameof(this.ItemAllowance.HousingAllowance)] = this.Results["housing_allowance"].ToString();

            // Row - 遅刻早退欠勤
            dataRow[nameof(this.ItemAllowance.LateAbsent)] = this.Results["late_absent"].ToString();

            // Row - 交通費
            dataRow[nameof(this.ItemAllowance.TransportationExpenses)] = this.Results["transportation_expenses"].ToString();

            // Row - 特別手当
            dataRow[nameof(this.ItemAllowance.SpecialAllowance)] = this.Results["special_allowance"].ToString();

            // Row - 予備
            dataRow[nameof(this.ItemAllowance.SpareAllowance)] = this.Results["spare_allowance"].ToString();

            // Row - 支給総計
            dataRow[nameof(this.ItemAllowance.TotalSalary)] = this.Results["total_salary"].ToString();

            // Row - 支給総計
            dataRow[nameof(this.ItemAllowance.TotalDeductedSalary)] = this.Results["total_deducted_salary"].ToString();

            // Add - Row
            this.TableAllowance.Rows.Add(dataRow);
        }

        #endregion

        #region Add - 控除額

        /// <summary>
        /// Add 控除額
        /// </summary>
        /// <param name="dataRow">行</param>
        private void AddTableDeduction()
        {
            // Table - Name
            this.TableDeduction.TableName = "Deduction";

            // Row
            System.Data.DataRow dataRow = this.TableDeduction.NewRow();

            // Row - 対象日付
            dataRow["WorkMonth"] = this.Results["work_month"].ToString();

            // Row - 健康保険
            dataRow[nameof(this.ItemDeduction.HealthInsurance)] = this.Results["health_insurance"].ToString();

            // Row - 介護保険
            dataRow[nameof(this.ItemDeduction.NursingInsurance)] = this.Results["nursing_insurance"].ToString();

            // Row - 厚生年金
            dataRow[nameof(this.ItemDeduction.WelfareAnnuity)] = this.Results["welfare_annuity"].ToString();

            // Row - 雇用保険
            dataRow[nameof(this.ItemDeduction.EmploymentInsurance)] = this.Results["employment_insurance"].ToString();

            // Row - 所得税
            dataRow[nameof(this.ItemDeduction.IncomeTax)] = this.Results["income_tax"].ToString();

            // Row - 市町村税
            dataRow[nameof(this.ItemDeduction.MunicipalTax)] = this.Results["municipal_tax"].ToString();

            // Row - 互助会
            dataRow[nameof(this.ItemDeduction.FriendshipAssociation)] = this.Results["friendship_association"].ToString();

            // Row - 年末調整他
            dataRow[nameof(this.ItemDeduction.YearEndTaxAdjustment)] = this.Results["year_end_tax_adjustment"].ToString();

            // Row - 控除額計
            dataRow[nameof(this.ItemDeduction.TotalDeduct)] = this.Results["total_deduct"].ToString();

            // Add - Row
            this.TableDeduction.Rows.Add(dataRow);
        }

        #endregion

        #region Add - 勤務備考

        /// <summary>
        /// Add 勤務備考
        /// </summary>
        /// <param name="dataRow">行</param>
        private void AddTableWorkingReferences()
        {
            // Table - Name
            this.TableWorkingReferences.TableName = "WorkingReferences";

            // Row
            System.Data.DataRow dataRow = this.TableWorkingReferences.NewRow();

            // Row - 対象日付
            dataRow["WorkMonth"] = this.Results["work_month"].ToString();

            // Row - 時間外労働
            dataRow[nameof(this.ItemWorkingReferences.OvertimeTime)] = this.Results["overtime_time"].ToString();

            // Row - 休出時間
            dataRow[nameof(this.ItemWorkingReferences.WeekendWorktime)] = this.Results["weekend_worktime"].ToString();

            // Row - 深夜時間
            dataRow[nameof(this.ItemWorkingReferences.MidnightWorktime)] = this.Results["midnight_worktime"].ToString();

            // Row - 遅刻早退欠勤H
            dataRow[nameof(this.ItemWorkingReferences.LateAbsentH)] = this.Results["late_absent"].ToString();

            // Row - 支給額 - 保険
            dataRow[nameof(this.ItemWorkingReferences.Insurance)] = this.Results["insurance"].ToString();

            // Row - 不要人数
            dataRow[nameof(this.ItemWorkingReferences.NumberOfDependent)] = this.Results["number_of_dependent"].ToString();

            // Row - 有休残日数
            dataRow[nameof(this.ItemWorkingReferences.PaidVacation)] = this.Results["paid_vacation"].ToString();

            // Row - 勤務時間
            dataRow[nameof(this.ItemWorkingReferences.WorkingHours)] = this.Results["working_hours"].ToString();

            // Row - 勤務先
            dataRow[nameof(this.ItemWorkingReferences.Workplace)] = this.Results["workplace"].ToString();

            // Add - Row
            this.TableWorkingReferences.Rows.Add(dataRow);
        }

        #endregion

        #region Add - 副業

        /// <summary>
        /// Add 副業
        /// </summary>
        /// <param name="dataRow">行</param>
        private void AddTableSideBusiness()
        {
            // Table - Name
            this.TableSIdeBusiness.TableName = "SIdeBusiness";

            // Row
            System.Data.DataRow dataRow = this.TableSIdeBusiness.NewRow();

            // Row - 対象日付
            dataRow["WorkMonth"] = this.Results["work_month"].ToString();

            // Row - 副業収入
            dataRow[nameof(this.ItemSideBusiness.SideBusiness)] = this.Results["side_business"].ToString();

            // Row - 臨時収入
            dataRow[nameof(this.ItemSideBusiness.Perquisite)] = this.Results["perquisite"].ToString();

            // Row - その他
            dataRow[nameof(this.ItemSideBusiness.Others)] = this.Results["others"].ToString();

            // Row - 勤務先
            dataRow[nameof(this.ItemSideBusiness.Remarks)] = this.Results["remarks"].ToString();

            // Add - Row
            this.TableSIdeBusiness.Rows.Add(dataRow);
        }

        #endregion

        #region テーブルの値比較

        /// <summary>
        /// テーブルの値比較
        /// </summary>
        /// <param name="tableName">対象のテーブル名</param>
        /// <param name="columnName">対象の列名</param>
        /// <param name="value">比較する値</param>
        /// <returns></returns>
        public bool CompareValue(
            // 対象のテーブル名
            System.Data.DataTable tableName,
            // 対象の列名
            string columnName,
            // 比較する値
            object value
        )
        {
            // Rows
            var rows = tableName.Select("WorkMonth" + " = '" + this.TargetDate + "'");

            foreach (var row in rows)
            {
                // Compare with Row Value
                if (row[columnName].ToString() == value.ToString())
                {
                    // 等しい
                    return true;
                }
            }

            // 等しくない
            return false;
        }

        #endregion

        #region HasData

        /// <summary>
        /// テーブルに存在するか
        /// </summary>
        /// <param name="tableName">対象のテーブル名</param>
        /// <param name="targetDate">対象日付</param>
        /// <returns></returns>
        public bool HasDataInTable(
            // 対象のテーブル名
            System.Data.DataTable tableName,
            // 対象日付
            string targetDate
        )
        {
            var rows = tableName.Select("WorkMonth" + " = '" + targetDate + "'");

            foreach (var row in rows)
            {
                // データあり
                return true;
            }

            // データなし
            return false;
        }

        #endregion

        #region HasData

        /// <summary>
        /// テーブルから値を取得
        /// </summary>
        /// <param name="tableName">対象のテーブル名</param>
        /// <param name="targetDate">対象日付</param>
        /// <param name="columnName">列名</param>
        /// <returns></returns>
        public object GetDataInTable(
            // 対象のテーブル名
            System.Data.DataTable tableName,
            // 対象日付
            string targetDate,
            // 列名
            string columnName
        )
        {
            var rows = tableName.Select("WorkMonth" + " = '" + targetDate + "'");

            foreach (var row in rows)
            {
                // データあり
                return row[columnName];
            }

            // データなし
            return null;
        }

        #endregion

    }
}
