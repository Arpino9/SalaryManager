using System;
using SalaryManager.Domain.Entities;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.Infrastructure.Interface;
using SalaryManager.WPF.ViewModels;
using SalaryManager.Domain.StaticValues;

namespace SalaryManager.WPF.Models
{
    /// <summary>
    /// Model - 副業
    /// </summary>
    public class Model_SideBusiness : IInputPayslip
    {

        #region Get Instance

        private static Model_SideBusiness model = null;

        public static Model_SideBusiness GetInstance()
        {
            if (model == null)
            {
                model = new Model_SideBusiness();
            }

            return model;
        }

        #endregion

        /// <summary> ViewModel - ヘッダ </summary>
        internal ViewModel_Header Header { get; set; }

        /// <summary> ViewModel - 副業 </summary>
        internal ViewModel_SideBusiness ViewModel { get; set; }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="entityDate">取得する日付</param>
        public void Initialize(DateTime entityDate)
        {
            SideBusinesses.Create(new SideBusinessSQLite());

            this.ViewModel.Entity          = SideBusinesses.Fetch(entityDate.Year, entityDate.Month);
            this.ViewModel.Entity_LastYear = SideBusinesses.Fetch(entityDate.Year, entityDate.Month - 1);

            if (this.ViewModel.Entity is null)
            {
                // デフォルト明細
                this.ViewModel.Entity = SideBusinesses.FetchDefault();
            }

            this.Refresh();
        }

        /// <summary>
        /// リロード
        /// </summary>
        public void Reload()
        {
            // 副業
            var sideBusinessTable = new SideBusinessSQLite();
            this.ViewModel.Entity          = sideBusinessTable.GetEntity(this.Header.Year, this.Header.Month);
            this.ViewModel.Entity_LastYear = sideBusinessTable.GetEntity(this.Header.Year - 1, this.Header.Month);
            
            this.Refresh();
        }

        /// <summary>
        /// クリア
        /// </summary>
        public void Clear()
        {
            // 副業
            this.ViewModel.SideBusiness = default(double);
            // 臨時収入
            this.ViewModel.Perquisite   = default(double);
            // その他
            this.ViewModel.Others       = default(double);
            // 備考
            this.ViewModel.Remarks      = default(string);
        }

        /// <summary>
        /// 再描画
        /// </summary>
        public void Refresh()
        {
            var entity = this.ViewModel.Entity;

            if (entity is null)
            {
                this.Clear();
                return;
            }

            // 副業
            this.ViewModel.SideBusiness = entity.SideBusiness;
            // 臨時収入
            this.ViewModel.Perquisite   = entity.Perquisite;
            // その他
            this.ViewModel.Others       = entity.Others;
            // 備考
            this.ViewModel.Remarks      = entity.Remarks;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="transaction">トランザクション</param>
        public void Save(SQLiteTransaction transaction)
        {
            var entity = new SideBusinessEntity(
                this.Header.ID,
                this.Header.YearMonth,
                this.ViewModel.SideBusiness,
                this.ViewModel.Perquisite,
                this.ViewModel.Others,
                this.ViewModel.Remarks);

            var sideBusiness = new SideBusinessSQLite();
            sideBusiness.Save(transaction, entity);
        }
    }
}
