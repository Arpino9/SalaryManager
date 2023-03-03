using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Repositories;

namespace SalaryManager.Domain.StaticValues
{
    /// <summary>
    /// Static Values - 控除額
    /// </summary>
    public static class Deductions
    {
        private static List<DeductionEntity> _entities = new List<DeductionEntity>();

        private static DeductionEntity _default;

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="repository">Repository</param>
        /// <remarks>
        /// 競合防止のためlockをかけており、常に最新の情報が取得できる。
        /// </remarks>
        public static void Create(IDeductionRepository repository)
        {
            lock (((ICollection)_entities).SyncRoot)
            {
                _entities.Clear();
                _entities.AddRange(repository.GetEntities());

                _default = repository.GetDefault();
            }
        }

        /// <summary>
        /// 控除額を取得
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <returns>控除額</returns>
        public static DeductionEntity Fetch(int year, int month)
        {
            return _entities.Find(x => x.YearMonth.Year == year
                                    && x.YearMonth.Month == month);
        }

        /// <summary>
        /// 昇順で取得する
        /// </summary>
        /// <returns>控除額</returns>
        public static IReadOnlyList<DeductionEntity> FetchByAscending()
        {
            return _entities.OrderBy(x => x.YearMonth).ToList().AsReadOnly();
        }

        /// <summary>
        /// 降順で取得する
        /// </summary>
        /// <returns>控除額</returns>
        public static IReadOnlyList<DeductionEntity> FetchByDescending()
        {
            return _entities.OrderByDescending(x => x.YearMonth).ToList().AsReadOnly();
        }

        /// <summary>
        /// デフォルト明細を取得する
        /// </summary>
        /// <returns>デフォルト明細</returns>
        public static DeductionEntity FetchDefault()
        {
            return _default;
        }
    }
}
