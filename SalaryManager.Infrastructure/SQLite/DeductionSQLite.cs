using System;
using System.Collections.Generic;
using System.Data.SQLite;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.Domain.Repositories;

namespace SalaryManager.Infrastructure.SQLite
{
    /// <summary>
    /// SQLite - 控除額
    /// </summary>
    /// <remarks>
    /// SQLiteの取得クラス
    /// </remarks>
    public class DeductionSQLite : IDeductionRepository
    {
        public IReadOnlyList<DeductionEntity> GetEntities()
        {
            string sql = @"
SELECT Id, 
YearMonth, 
HealthInsurance, 
NursingInsurance, 
WelfareAnnuity, 
EmploymentInsurance, 
IncomeTax, 
MunicipalTax,
FriendshipAssociation,
YearEndTaxAdjustment,
Remarks,
HealthInsurance
+ NursingInsurance
+ WelfareAnnuity
+ EmploymentInsurance
+ IncomeTax
+ MunicipalTax
+ FriendshipAssociation
+ YearEndTaxAdjustment AS TotalDeduct
FROM Deduction";

            return SQLiteHelper.Query(
                sql,
                reader =>
                {
                    return new DeductionEntity(
                                Convert.ToInt32(reader["Id"]),
                                Convert.ToDateTime(reader["YearMonth"]),
                                Convert.ToDouble(reader["HealthInsurance"]),
                                Convert.ToDouble(reader["NursingInsurance"]),
                                Convert.ToDouble(reader["WelfareAnnuity"]),
                                Convert.ToDouble(reader["EmploymentInsurance"]),
                                Convert.ToDouble(reader["IncomeTax"]),
                                Convert.ToDouble(reader["MunicipalTax"]),
                                Convert.ToDouble(reader["FriendshipAssociation"]),
                                Convert.ToDouble(reader["YearEndTaxAdjustment"]),
                                Convert.ToString(reader["Remarks"]),
                                Convert.ToDouble(reader["TotalDeduct"]));
                });
        }

        public DeductionEntity GetEntity(int year, int month)
        {
            string sql = @"
SELECT Id, 
YearMonth, 
HealthInsurance, 
NursingInsurance, 
WelfareAnnuity, 
EmploymentInsurance, 
IncomeTax, 
MunicipalTax,
FriendshipAssociation,
YearEndTaxAdjustment,
Remarks,
HealthInsurance
+ NursingInsurance
+ WelfareAnnuity
+ EmploymentInsurance
+ IncomeTax
+ MunicipalTax
+ FriendshipAssociation
+ YearEndTaxAdjustment AS TotalDeduct
FROM Deduction
Where YearMonth = @YearMonth";

            var args = new List<SQLiteParameter>()
            {
                new SQLiteParameter("YearMonth", year + "-" + month.ToString("D2") + "-" + "01"),
            };

            return SQLiteHelper.QuerySingle<DeductionEntity>(
                sql,
                args.ToArray(),
                reader =>
                {
                    return new DeductionEntity(
                                Convert.ToInt32(reader["Id"]),
                                Convert.ToDateTime(reader["YearMonth"]),
                                Convert.ToDouble(reader["HealthInsurance"]),
                                Convert.ToDouble(reader["NursingInsurance"]),
                                Convert.ToDouble(reader["WelfareAnnuity"]),
                                Convert.ToDouble(reader["EmploymentInsurance"]),
                                Convert.ToDouble(reader["IncomeTax"]),
                                Convert.ToDouble(reader["MunicipalTax"]),
                                Convert.ToDouble(reader["FriendshipAssociation"]),
                                Convert.ToDouble(reader["YearEndTaxAdjustment"]),
                                Convert.ToString(reader["Remarks"]),
                                Convert.ToDouble(reader["TotalDeduct"]));
                },
                null);
        }

        /// <summary>
        /// Get - 控除額(デフォルト)
        /// </summary>
        /// <returns>控除額</returns>
        public DeductionEntity GetDefault()
        {
            string sql = @"
SELECT D.Id, 
D.YearMonth, 
D.HealthInsurance, 
D.NursingInsurance, 
D.WelfareAnnuity, 
D.EmploymentInsurance, 
D.IncomeTax, 
D.MunicipalTax,
D.FriendshipAssociation,
D.YearEndTaxAdjustment,
D.Remarks,
D.HealthInsurance
+ D.NursingInsurance
+ D.WelfareAnnuity
+ D.EmploymentInsurance
+ D.IncomeTax
+ D.MunicipalTax
+ D.FriendshipAssociation
+ D.YearEndTaxAdjustment AS TotalDeduct
FROM Deduction D
INNER JOIN YearMonth YM ON D.YearMonth = YM.YearMonth
WHERE YM.IsDefault = True";

            return SQLiteHelper.QuerySingle<DeductionEntity>(
                sql,
                reader =>
                {
                    return new DeductionEntity(
                                Convert.ToInt32(reader["Id"]),
                                Convert.ToDateTime(reader["YearMonth"]),
                                Convert.ToDouble(reader["HealthInsurance"]),
                                Convert.ToDouble(reader["NursingInsurance"]),
                                Convert.ToDouble(reader["WelfareAnnuity"]),
                                Convert.ToDouble(reader["EmploymentInsurance"]),
                                Convert.ToDouble(reader["IncomeTax"]),
                                Convert.ToDouble(reader["MunicipalTax"]),
                                Convert.ToDouble(reader["FriendshipAssociation"]),
                                Convert.ToDouble(reader["YearEndTaxAdjustment"]),
                                Convert.ToString(reader["Remarks"]),
                                Convert.ToDouble(reader["TotalDeduct"]));
                },
                null);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="transaction">トランザクション</param>
        /// <param name="entity">エンティティ</param>
        public void Save(SQLiteTransaction transaction, DeductionEntity entity)
        {
            string insert = @"
insert into Deduction
(Id, YearMonth, HealthInsurance, NursingInsurance, WelfareAnnuity,
EmploymentInsurance, IncomeTax, MunicipalTax, FriendshipAssociation,
YearEndTaxAdjustment, Remarks)
values
(@Id, @YearMonth, @HealthInsurance, @NursingInsurance, @WelfareAnnuity,
@EmploymentInsurance, @IncomeTax, @MunicipalTax, @FriendshipAssociation,
@YearEndTaxAdjustment, @Remarks)
";

            string update = @"
update Deduction
set YearMonth             = @YearMonth, 
    HealthInsurance       = @HealthInsurance, 
    NursingInsurance      = @NursingInsurance, 
    WelfareAnnuity        = @WelfareAnnuity, 
    EmploymentInsurance   = @EmploymentInsurance, 
    IncomeTax             = @IncomeTax, 
    MunicipalTax          = @MunicipalTax, 
    FriendshipAssociation = @FriendshipAssociation, 
    YearEndTaxAdjustment  = @YearEndTaxAdjustment, 
    Remarks               = @Remarks
where Id = @Id
";

            var args = new List<SQLiteParameter>()
            {
                new SQLiteParameter("Id", entity.ID),
                new SQLiteParameter("YearMonth", DateUtils.ConvertToSQLiteValue(entity.YearMonth)),
                new SQLiteParameter("HealthInsurance", entity.HealthInsurance.Value),
                new SQLiteParameter("NursingInsurance", entity.NursingInsurance.Value),
                new SQLiteParameter("WelfareAnnuity", entity.WelfareAnnuity.Value),
                new SQLiteParameter("EmploymentInsurance", entity.EmploymentInsurance.Value),
                new SQLiteParameter("IncomeTax", entity.IncomeTax.Value),
                new SQLiteParameter("MunicipalTax", entity.MunicipalTax.Value),
                new SQLiteParameter("FriendshipAssociation", entity.FriendshipAssociation.Value),
                new SQLiteParameter("YearEndTaxAdjustment", entity.YearEndTaxAdjustment),
                new SQLiteParameter("Remarks", entity.Remarks),
            };

            transaction.Execute(insert, update, args.ToArray());
        }

        public void Save(ITransactionRepository transaction, DeductionEntity entity)
            => this.Save((SQLiteTransaction)transaction, entity);
    }
}
