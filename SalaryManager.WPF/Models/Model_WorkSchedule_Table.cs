using SalaryManager.Infrastructure.Google_Calendar;
using SalaryManager.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryManager.WPF.Models
{
    public class Model_WorkSchedule_Table
    {
        public Model_WorkSchedule_Table()
        {
            CalendarReader.Read();
        }

        /// <summary> ViewModel - 勤務表 </summary>
        internal ViewModel_WorkSchedule_Table ViewModel { get; set; }
    }
}
