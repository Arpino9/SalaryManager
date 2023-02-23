using SalaryManager.Domain.Entities;
using System.Collections.Generic;

namespace SalaryManager.Domain.Repositories
{
    public interface IAnnualChartRepository
    {
        /// <summary> 
        /// Get - 支給額
        /// </summary>
        /// <returns>Entity - 支給額</returns>
        IReadOnlyList<AnnualChartEntity> GetEntities();
    }
}
