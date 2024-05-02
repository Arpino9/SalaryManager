using System;

namespace SalaryManager.Domain.Entities
{
    /// <summary>
    /// Entity - 休祝日
    /// </summary>
    public sealed class HolidayEntity
    {
        public HolidayEntity(
            DateTime date,
            string name,
            string companyName,
            string remarks)
        {
            this.Date    = date;
            this.Name    = name;
            this.CompanyName = companyName;
            this.Remarks = remarks;
        }

        public DateTime Date { get; set; }
        public string Name { get; set; }

        public string CompanyName { get; set; }

        public string Remarks { get; set; }
    }
}
