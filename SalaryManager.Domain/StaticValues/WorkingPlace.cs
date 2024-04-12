using System.Linq;
using System.Collections;
using System.Collections.Generic;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Repositories;

namespace SalaryManager.Domain.StaticValues
{
    /// <summary>
    /// Static Values - 就業場所
    /// </summary>
    public static class WorkingPlace
    {
        private static List<WorkingPlaceEntity> _entities = new List<WorkingPlaceEntity>();

        /// <summary>
        /// テーブル取得
        /// </summary>
        /// <param name="repository">Repository</param>
        /// <remarks>
        /// 競合防止のためlockをかけており、常に最新の情報が取得できる。
        /// </remarks>
        public static void Create(IWorkingPlaceRepository repository)
        {
            lock (((ICollection)_entities).SyncRoot)
            {
                _entities.Clear();
                _entities.AddRange(repository.GetEntities());
            }
        }

        /// <summary>
        /// 就業場所を取得
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>就業場所</returns>
        public static WorkingPlaceEntity Fetch(int id)
        {
            return _entities.Find(x => x.ID == id);
        }

        /// <summary>
        /// 昇順で取得する
        /// </summary>
        /// <returns>職歴</returns>
        public static IReadOnlyList<WorkingPlaceEntity> FetchByAscending()
        {
            return _entities.OrderBy(x => x.ID).ToList().AsReadOnly();
        }

        /// <summary>
        /// 降順で取得する
        /// </summary>
        /// <returns>職歴</returns>
        public static IReadOnlyList<WorkingPlaceEntity> FetchByDescending()
        {
            return _entities.OrderByDescending(x => x.ID).ToList().AsReadOnly();
        }
    }
}
