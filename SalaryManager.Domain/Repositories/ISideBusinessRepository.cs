using SalaryManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryManager.Domain.Repositories
{
    /// <summary>
    /// Interface - 副業
    /// </summary>
    public interface ISideBusinessRepository
    {
        /// <summary>
        /// Get - 副業
        /// </summary>
        /// <returns>Entity - 副業</returns>
        IReadOnlyList<SideBusinessEntity> GetEntities();


        SideBusinessEntity GetEntity(int year, int month);
        SideBusinessEntity GetDefault();
    }
}
