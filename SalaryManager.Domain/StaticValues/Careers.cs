using System.Linq;
using System.Collections;
using System.Collections.Generic;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Repositories;

namespace SalaryManager.Domain.StaticValues
{
    /// <summary>
    /// Static Values - 職歴
    /// </summary>
    public static class Careers
    {
        private static List<CareerEntity> _entities = new List<CareerEntity>();

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="repository">Repository</param>
        /// <remarks>
        /// 競合防止のためlockをかけており、常に最新の情報が取得できる。
        /// </remarks>
        public static void Create(ICareerRepository repository)
        {
            lock (((ICollection)_entities).SyncRoot)
            {
                _entities.Clear();
                _entities.AddRange(repository.GetEntities());
            }
        }

        /// <summary>
        /// 職歴を取得
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>職歴</returns>
        public static CareerEntity Fetch(int id)
        {
            return _entities.Find(x => x.ID == id);
        }

        /// <summary>
        /// 昇順で取得する
        /// </summary>
        /// <returns>職歴</returns>
        public static IReadOnlyList<CareerEntity> FetchByAscending()
        {
            return _entities.OrderBy(x => x.WorkingStartDate).ToList().AsReadOnly();
        }

        /// <summary>
        /// 降順で取得する
        /// </summary>
        /// <returns>職歴</returns>
        public static IReadOnlyList<CareerEntity> FetchByDescending()
        {
            return _entities.OrderByDescending(x => x.WorkingStartDate).ToList().AsReadOnly();
        }
    }
}
