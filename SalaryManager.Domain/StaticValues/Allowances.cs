using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Sqlite;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Exceptions;
using SalaryManager.Domain.Repositories;

namespace SalaryManager.Domain.StaticValues
{
    /// <summary>
    /// Static Values - 支給額
    /// </summary>
    public static class Allowances
    {
        private static List<AllowanceValueEntity> _entities = new List<AllowanceValueEntity>();

        private static AllowanceValueEntity _default;

        /// <summary>
        /// テーブル取得
        /// </summary>
        /// <param name="repository">Repository</param>
        /// <remarks>
        /// 競合防止のためlockをかけており、常に最新の情報が取得できる。
        /// </remarks>
        public static void Create(IAllowanceRepository repository)
        {
            lock (((ICollection)_entities).SyncRoot)
            {
                _entities.Clear();

                try
                {
                    _entities.AddRange(repository.GetEntities());

                    _default = repository.GetDefault();
                }
                catch(SqliteException ex)
                {
                    throw new DatabaseException("支給額テーブルの読込に失敗しました。", ex);
                }
            }
        }

        /// <summary>
        /// 支給額を取得
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <returns>支給額</returns>
        public static AllowanceValueEntity Fetch(int year, int month) 
        {
            return _entities.Find(x => x.YearMonth.Year  == year
                                    && x.YearMonth.Month == month);
        }

        /// <summary>
        /// 昇順で取得する
        /// </summary>
        /// <returns>支給額</returns>
        public static IReadOnlyList<AllowanceValueEntity> FetchByAscending()
        {
            return _entities.OrderBy(x => x.YearMonth).ToList().AsReadOnly();
        }

        /// <summary>
        /// 降順で取得する
        /// </summary>
        /// <returns>支給額</returns>
        public static IReadOnlyList<AllowanceValueEntity> FetchByDescending()
        {
            return _entities.OrderByDescending(x => x.YearMonth).ToList().AsReadOnly(); 
        }

        /// <summary>
        /// デフォルト明細を取得する
        /// </summary>
        /// <returns>デフォルト明細</returns>
        public static AllowanceValueEntity FetchDefault()
        {
            return _default;
        }
    }
}
