using System.Linq;
using System.Collections;
using System.Collections.Generic;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Repositories;
using System;
using SalaryManager.Domain.ValueObjects;

namespace SalaryManager.Domain.StaticValues
{
    /// <summary>
    /// Static Values - 職歴
    /// </summary>
    public static class Careers
    {
        private static List<CareerEntity> _entities = new List<CareerEntity>();

        /// <summary>
        /// テーブル取得
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
        /// 会社名を取得
        /// </summary>
        /// <param name="date">日付</param>
        /// <returns>会社名</returns>
        public static CompanyValue FetchCompany(DateTime date)
        {
            var entity = _entities.Find(x => x.WorkingStartDate.Value <= date &&
                                             date <= x.WorkingEndDate.Value);

            if (entity == null) 
            {
                return new CompanyValue(string.Empty);
            }

            return entity.CompanyName;
        }

        /// <summary>
        /// 会社名から社員番号を取得
        /// </summary>
        /// <param name="company">会社名</param>
        /// <returns>社員番号</returns>
        public static string FetchEmployeeNumber(CompanyValue company)
        {
            var entity = _entities.Find(x => x.CompanyName == company);

            if (entity == null)
            {
                return string.Empty;
            }

            return entity.EmployeeNumber;
        }

        /// <summary>
        /// 昇順で取得する
        /// </summary>
        /// <returns>職歴</returns>
        public static IReadOnlyList<CareerEntity> FetchByAscending()
        {
            return _entities.OrderBy(x => x.WorkingStartDate.Value).ToList().AsReadOnly();
        }

        /// <summary>
        /// 降順で取得する
        /// </summary>
        /// <returns>職歴</returns>
        public static IReadOnlyList<CareerEntity> FetchByDescending()
        {
            return _entities.OrderByDescending(x => x.WorkingStartDate.Value).ToList().AsReadOnly();
        }
    }
}
