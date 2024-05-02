using SalaryManager.Domain.Entities;
using System.Collections.Generic;

namespace SalaryManager.Domain.Repositories
{
    public interface IHolidayRepository
    {
        /// <summary> 
        /// Get - 休祝日
        /// </summary>
        /// <returns>Entity - 休祝日</returns>
        IReadOnlyList<HolidayEntity> GetEntities();

        /// <summary>
        /// Get - 休祝日
        /// </summary>
        /// <returns>休祝日</returns>
        HolidayEntity GetEntity(int id);

        public void Save(
            HolidayEntity entity);

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="id">ID</param>
        public void Delete(int id);
    }
}
