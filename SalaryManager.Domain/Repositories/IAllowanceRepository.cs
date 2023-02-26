using SalaryManager.Domain.Entities;
using System.Collections.Generic;

namespace SalaryManager.Domain.Repositories
{
    /// <summary>
    /// Repositories - 支給額
    /// </summary>
    public interface IAllowanceRepository
    {
        /// <summary> 
        /// Get - 支給額
        /// </summary>
        /// <returns>Entity - 支給額</returns>
        IReadOnlyList<AllowanceEntity> GetEntities();

        /// <summary>
        /// Get - 支給額
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <returns>支給額</returns>
        AllowanceEntity GetEntiity(int year, int month);
    }
}
