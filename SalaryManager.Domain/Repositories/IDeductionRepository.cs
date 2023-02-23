using SalaryManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryManager.Domain.Repositories
{
    /// <summary>
    /// Repositories - 控除額
    /// </summary>
    public interface IDeductionRepository
    {
        /// <summary>
        /// Get - 控除額
        /// </summary>
        /// <returns>Entity - 控除額</returns>
        IReadOnlyList<DeductionEntity> GetEntities();

        /// <summary>
        /// Get - 控除額
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <returns>控除額</returns>
        DeductionEntity GetEntity(int year, int month);

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity">控除額</param>
        void Save(DeductionEntity entity);
    }
}
