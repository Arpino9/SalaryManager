﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Repositories;

namespace SalaryManager.Domain.StaticValues
{
    /// <summary>
    /// Static Values - 勤務備考
    /// </summary>
    public static class WorkingReferences
    {
        private static List<WorkingReferencesEntity> _entities = new List<WorkingReferencesEntity>();

        private static WorkingReferencesEntity _default;

        /// <summary>
        /// テーブル取得
        /// </summary>
        /// <param name="repository">Repository</param>
        /// <remarks>
        /// 競合防止のためlockをかけており、常に最新の情報が取得できる。
        /// </remarks>
        public static void Create(IWorkingReferencesRepository repository)
        {
            lock (((ICollection)_entities).SyncRoot)
            {
                _entities.Clear();
                _entities.AddRange(repository.GetEntities());

                _default = repository.GetDefault();
            }
        }

        /// <summary>
        /// 勤務備考を取得
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <returns>勤務備考</returns>
        public static WorkingReferencesEntity Fetch(int year, int month)
        {
            return _entities.Find(x => x.YearMonth.Year == year
                                    && x.YearMonth.Month == month);
        }

        /// <summary>
        /// 昇順で取得する
        /// </summary>
        /// <returns>勤務備考</returns>
        public static IReadOnlyList<WorkingReferencesEntity> FetchByAscending()
        {
            return _entities.OrderBy(x => x.YearMonth).ToList().AsReadOnly();
        }

        /// <summary>
        /// 降順で取得する
        /// </summary>
        /// <returns>勤務備考</returns>
        public static IReadOnlyList<WorkingReferencesEntity> FetchByDescending()
        {
            return _entities.OrderByDescending(x => x.YearMonth).ToList().AsReadOnly();
        }

        /// <summary>
        /// デフォルト明細を取得する
        /// </summary>
        /// <returns>デフォルト明細</returns>
        public static WorkingReferencesEntity FetchDefault()
        {
            return _default;
        }
    }
}
