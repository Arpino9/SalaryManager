using SalaryManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryManager.Domain.Repositories
{
    public interface IHeaderRepository
    {
        /// <summary>
        /// Get - ヘッダ
        /// </summary>
        /// <returns>Entity - 副業</returns>
        IReadOnlyList<HeaderEntity> GetEntities();

        HeaderEntity FetchDefault();
    }
}
