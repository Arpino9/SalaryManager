using System;

namespace SalaryManager.Domain.Entities
{
    /// <summary>
    /// Entity - 副業
    /// </summary>
    public sealed class SideBusinessEntity
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="yearMonth">年月</param>
        /// <param name="sideBusiness">副業収入</param>
        /// <param name="perquisite">臨時収入</param>
        /// <param name="other">その他</param>
        /// <param name="remarks">備考</param>
        public SideBusinessEntity(
            int id,
            DateTime yearMonth,
            double sideBusiness,
            double perquisite,
            double other,
            string remarks)
        {
            this.ID           = id;
            this.YearMonth    = yearMonth;
            this.SideBusiness = sideBusiness;
            this.Perquisite   = perquisite;
            this.Others        = other;
            this.Remarks      = remarks;
        }

        /// <summary> ID </summary>
        public int ID { get; set; }

        /// <summary> 年月 </summary>
        public DateTime YearMonth { get; }

        /// <summary> 副業収入 </summary>
        public double SideBusiness { get; }

        /// <summary> 臨時収入 </summary>
        public double Perquisite { get; }

        /// <summary> その他 </summary>
        public double Others { get; }

        /// <summary> 備考 </summary>
        public string Remarks { get; }
    }
}
