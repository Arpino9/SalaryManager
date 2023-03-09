using Microsoft.VisualStudio.TestTools.UnitTesting;
using SalaryManager.Domain.Repositories;
using SalaryManager.WPF.UserControls;
using System.Collections.ObjectModel;
using System.Linq;

namespace SalaryManagerTest.Test
{
    [TestClass]
    public class UnitTest1
    {
        class MyClass
        {
            public int No { get; set; }
            public string Name { get; set; }
            public override string ToString()
            {
                return "No:" + No + ",Name:" + Name;
            }
        }

        private IAllowanceRepository _allowance;

        private static readonly string ConnectionString = @"Data Source=C:\Users\OKAJIMA\Desktop\勉強用\SQlite\SalaryManager.db;Version=3;";
        [TestMethod]
        public void TestMethod1()
        {
            ObservableCollection<MyClass> myCollection = new ObservableCollection<MyClass>
{
                new MyClass(){No=1,Name="ひかり"},
                new MyClass(){No=2,Name="こだま"},
                new MyClass(){No=3,Name="のぞみ"}
            };

            ObservableCollection<MyClass> orderedByName = new ObservableCollection<MyClass>(myCollection.OrderBy(n => n.Name));
        }
    }
}
