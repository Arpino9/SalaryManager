using Microsoft.VisualStudio.TestTools.UnitTesting;
using SalaryManager.Domain.Repositories;
using SalaryManager.Infrastructure.SQLite;
using System;
using System.Data.SQLite;
using System.Linq;

namespace SalaryManagerTest.Test
{
    [TestClass]
    public class UnitTest1
    {

        private IAllowanceRepository _allowance;

        private static readonly string ConnectionString = @"Data Source=C:\Users\OKAJIMA\Desktop\勉強用\SQlite\SalaryManager.db;Version=3;";
        [TestMethod]
        public void TestMethod1()
        {


            /*try
            {
                string sql = @"
SELECT Id, 
YearMonth, 
BasicSalary, 
ExecutiveAllowance, 
DependencyAllowance, 
OvertimeAllowance, 
DaysoffIncreased, 
NightworkIncreased ,
HousingAllowance,
LateAbsent,
TransportationExpenses,
SpecialAllowance,
SpareAllowance,
Remarks,
TotalSalary,
TotalDeductedSalary
FROM Allowance";

                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        connection.Open();

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                //result.Add(createEntity(reader));
                            }
                        }
                    }
                }
            }catch (Exception ex)
            {

            }*/

        }
    }
}
