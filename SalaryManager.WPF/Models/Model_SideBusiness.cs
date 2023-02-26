using SalaryManager.Domain.Entities;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.Infrastructure.Interface;
using SalaryManager.WPF.ViewModels;
using System;
using System.Linq;

namespace SalaryManager.WPF.Models
{
    /// <summary>
    /// Model - 副業
    /// </summary>
    public class Model_SideBusiness : IInputPayroll
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

        /// <summary> Entity - 副業 </summary>
        private SideBusinessEntity Entity { get; set; }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            var sqlite  = new SideBusinessSQLite();
            var records = sqlite.GetEntities();

            this.Entity = records.Where(record => record.YearMonth.Year == DateTime.Today.Year
                                              && record.YearMonth.Month == DateTime.Today.Month)
                                 .FirstOrDefault(); ;

            if (this.Entity is null)
            {
                // レコードなし
                var header = new HeaderSQLite();
                var defaultEntity = header.GetDefaultEntity();

                if (defaultEntity != null)
                {
                    this.Entity = records.Where(record => record.ID == defaultEntity.ID)
                                         .FirstOrDefault();
                }
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
            this.Entity = sideBusinessTable.GetEntity(this.Header.Year, this.Header.Month);
            
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
            if (this.Entity is null)
            {
                this.Clear();
                return;
            }

            // 副業
            this.ViewModel.SideBusiness = this.Entity.SideBusiness;
            // 臨時収入
            this.ViewModel.Perquisite   = this.Entity.Perquisite;
            // その他
            this.ViewModel.Others       = this.Entity.Others;
            // 備考
            this.ViewModel.Remarks      = this.Entity.Remarks;
        }

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="transaction">トランザクション</param>
        public void Register(SQLiteTransaction transaction)
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
