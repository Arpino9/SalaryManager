using SalaryManager.Domain.Entities;
using System.Collections.Generic;

namespace SalaryManager.Domain.Repositories
{
    public interface ICompanyRepository
    {
        /// <summary> 
        /// Get - 会社
        /// </summary>
        /// <returns>Entity - 会社</returns>
        IReadOnlyList<CompanyEntity> GetEntities();

        /// <summary>
        /// Get - 会社
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <returns>支給額</returns>
        CompanyEntity GetEntity(int id);

        public void Save(
            CompanyEntity entity);

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="id">ID</param>
        public void Delete(int id);
    }
}
