using SalaryManager.Domain.Entities;
using System.Collections.Generic;

namespace SalaryManager.Domain.Repositories
{
    /// <summary>
    /// Repository - 勤務備考
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
