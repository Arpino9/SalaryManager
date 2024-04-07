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
        /// 職歴を取得
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>職歴</returns>
        public static WorkingPlaceEntity Fetch(int id)
        {
            return _entities.Find(x => x.ID == id);
        }

        /*/// <summary>
        /// 会社名を取得
        /// </summary>
        /// <param name="date">日付</param>
        /// <returns>会社名</returns>
        public static string FetchCompany(DateTime date)
        {
            var entity = _entities.Find(x => x.WorkingStartDate.Value <= date &&
                                             date <= x.WorkingEndDate.Value);

            if (entity == null) 
            {
                return CompanyNameValue.Undefined.DisplayValue;
            }

            return entity.CompanyName.DisplayValue;
        }

        /// <summary>
        /// 会社名から社員番号を取得
        /// </summary>
        /// <param name="company">会社名</param>
        /// <returns>社員番号</returns>
        public static string FetchEmployeeNumber(CompanyNameValue company)
        {
            var entity = _entities.Find(x => x.CompanyName == company);

            if (entity == null)
            {
                return string.Empty;
            }

            return entity.EmployeeNumber;
        }

        /// <summary>
        /// 会社名から支給有無を取得
        /// </summary>
        /// <param name="company">会社名</param>
        /// <returns>支給有無</returns>
        public static AllowanceExistenceEntity FetchAllowanceExistence(CompanyNameValue company)
        {
            var entity = _entities.Find(x => x.CompanyName == company);

            return entity?.AllowanceExistence;
        }*/

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
