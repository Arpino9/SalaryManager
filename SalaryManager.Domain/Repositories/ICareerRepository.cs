using System.Collections.Generic;
using SalaryManager.Domain.Entities;

namespace SalaryManager.Domain.Repositories
{
    /// <summary>
    /// Repositories - 職歴
    /// </summary>
    public interface ICareerRepository
    {
        /// <summary> 
        /// Get - 職歴
        /// </summary>
        /// <returns>Entity - 支給額</returns>
        IReadOnlyList<CareerEntity> GetEntities();

        /// <summary>
        /// Get - 職歴
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>職歴</returns>
        CareerEntity GetEntity(int id);
    }
}
