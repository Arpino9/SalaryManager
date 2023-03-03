using SalaryManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryManager.Domain.Repositories
{
    /// <summary>
    /// Interface - 勤務備考
    /// </summary>
    public interface IWorkingReferencesRepository
    {
        /// <summary>
        /// Get - 勤務備考
        /// </summary>
        /// <returns>Entity - 勤務備考</returns>
        IReadOnlyList<WorkingReferencesEntity> GetEntities();

        /// <summary>
        /// Get - 勤務備考
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <returns>勤務備考</returns>
        WorkingReferencesEntity GetEntity(int year, int month);

        WorkingReferencesEntity GetDefault();
    }
}
