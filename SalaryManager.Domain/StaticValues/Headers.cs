using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Repositories;

namespace SalaryManager.Domain.StaticValues
{
    public static class Headers
    {
        private static List<HeaderEntity> _entities = new List<HeaderEntity>();

        private static HeaderEntity _default;

        /// <summary>
        /// テーブル取得
        /// </summary>
        /// <param name="repository">Repository</param>
        /// <remarks>
        /// 競合防止のためlockをかけており、常に最新の情報が取得できる。
        /// </remarks>
        public static void Create(IHeaderRepository repository)
        {
            lock (((ICollection)_entities).SyncRoot)
            {
                _entities.Clear();
                _entities.AddRange(repository.GetEntities());

                _default = repository.FetchDefault();
            }
        }

        /// <summary>
        /// 副業額を取得
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <returns>副業額</returns>
        public static HeaderEntity Fetch(int year, int month)
        {
            return _entities.Find(x => x.YearMonth.Year  == year
                                    && x.YearMonth.Month == month);
        }

        /// <summary>
        /// 昇順で取得する
        /// </summary>
        /// <returns>ヘッダ情報</returns>
        public static IReadOnlyList<HeaderEntity> FetchByAscending()
        {
            return _entities.OrderBy(x => x.YearMonth).ToList().AsReadOnly();
        }

        /// <summary>
        /// 降順で取得する
        /// </summary>
        /// <returns>ヘッダ情報</returns>
        public static IReadOnlyList<HeaderEntity> FetchByDescending()
        {
            return _entities.OrderByDescending(x => x.YearMonth).ToList().AsReadOnly();
        }

        /// <summary>
        /// デフォルト明細を取得する
        /// </summary>
        /// <returns>デフォルト明細</returns>
        public static HeaderEntity FetchDefault()
        {
            return _default;
        }
    }
}
