using System;
using System.Linq;
using SalaryManager.Domain.Entities;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.Infrastructure.Interface;
using SalaryManager.WPF.ViewModels;

namespace SalaryManager.WPF.Models
{
    /// <summary>
    /// Model - ヘッダー
    /// </summary>
    public sealed class Model_Header : IInputPayroll
    {
        #region Get Instance

        private static Model_Header model = null;
        
        public static Model_Header GetInstance()
        {
            if (model == null)
            {
                model = new Model_Header();
            }

            return model;
        }

        #endregion

        /// <summary> ViewModel - ヘッダー </summary>
        internal ViewModel_Header ViewModel { get; set; }

        /// <summary> Entity - ヘッダー </summary>
        private HeaderEntity Entity { get; set; }

        /// <summary>
        /// ←ボタン
        /// </summary>
        internal void Return()
        {
            var yearMonth = this.ViewModel.YearMonth.AddMonths(-1);

            this.ViewModel.Year  = yearMonth.Year;
            this.ViewModel.Month = yearMonth.Month;

            this.Reload();
        }

        /// <summary>
        /// →ボタン
        /// </summary>
        internal void Proceed()
        {
            var yearMonth = this.ViewModel.YearMonth.AddMonths(1);

            this.ViewModel.Year  = yearMonth.Year;
            this.ViewModel.Month = yearMonth.Month;

            this.Reload();
        }

        /// <summary>
        /// Reload
        /// </summary>
        public void Reload()
        {
            this.ViewModel.YearMonth = new DateTime(this.ViewModel.Year, this.ViewModel.Month, 1);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            var sqlite = new HeaderSQLite();
            var records = sqlite.GetEntities();

            this.Entity = records.Where(x => x.YearMonth.Year == DateTime.Today.Year
                                         && x.YearMonth.Month == DateTime.Today.Month)
                                .FirstOrDefault();


            if (this.Entity is null)
            {
                this.ViewModel.ID = records.Max(x => x.ID) + 1;
                this.ViewModel.YearMonth = DateTime.Today;
            }

            this.Refresh();
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

            // ID
            this.ViewModel.ID    = this.Entity.ID;
            // 年
            this.ViewModel.Year  = this.Entity.YearMonth.Year;
            // 月
            this.ViewModel.Month = this.Entity.YearMonth.Month;
            // 作成日
            this.ViewModel.CreateDate = this.Entity.CreateDate;
            // 更新日
            this.ViewModel.UpdateDate = this.Entity.UpdateDate;
        }

        /// <summary>
        /// クリア
        /// </summary>
        public void Clear()
        {
            // 年
            this.ViewModel.Year  = this.ViewModel.YearMonth.Year;
            // 月
            this.ViewModel.Month = this.ViewModel.YearMonth.Month;
            // 作成日
            this.ViewModel.CreateDate = DateTime.Today;
            // 更新日
            this.ViewModel.UpdateDate = DateTime.Today;
        }

        /// <summary>
        /// 登録
        /// </summary>
        public void Register()
        {
            var entity = new HeaderEntity(            
                this.ViewModel.ID,
                this.ViewModel.YearMonth,
                this.ViewModel.IsDefault,
                this.ViewModel.CreateDate,
                this.ViewModel.UpdateDate
            );

            var header = new HeaderSQLite();
            header.Save(entity);
        }

        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="transaction">トランザクション</param>
        public void Register(SQLiteTransaction transaction)
        {
            var entity = new HeaderEntity(
                 this.ViewModel.ID,
                 this.ViewModel.YearMonth,
                 this.ViewModel.IsDefault,
                 this.ViewModel.CreateDate,
                 this.ViewModel.UpdateDate
             );

            var header = new HeaderSQLite();
            header.Save(transaction, entity);
        }
    }
}
