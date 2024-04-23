using SalaryManager.Domain.Entities;
using System.Collections.Generic;

namespace SalaryManager.Domain.Repositories
{
    public interface IHomeRepository
    {
        /// <summary> 
        /// Get - 自宅
        /// </summary>
        /// <returns>Entity - 自宅</returns>
        IReadOnlyList<HomeEntity> GetEntities();

        /// <summary>
        /// Get - 自宅
        /// </summary>
        /// <returns>自宅</returns>
        HomeEntity GetEntity(int id);

        public void Save(
            HomeEntity entity);

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="id">ID</param>
        public void Delete(int id);
    }
}
