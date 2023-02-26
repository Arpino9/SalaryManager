﻿using System;
using System.Linq;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Logics;
using SalaryManager.Infrastructure.Interface;
using SalaryManager.Infrastructure.SQLite;
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
        /// エンティティの取得
        /// </summary>
        /// <param name="year">取得する年</param>
        /// <param name="month">取得する月</param>
        private void FetchEntity(int year, int month)
        {
            var sqlite  = new HeaderSQLite();
            var records = sqlite.GetEntities();

            this.Entity = records.Where(x => x.YearMonth.Year  == year
                                          && x.YearMonth.Month == month)
                                 .FirstOrDefault();

            if (this.Entity is null)
            {
                this.ViewModel.ID = records.Max(x => x.ID) + 1;
            }
        }

        /// <summary>
        /// Reload
        /// </summary>
        /// <remarks>
        /// ヘッダ情報が存在すれば、ヘッダ情報を更新する。
        /// ここで「Refresh()」を呼び出すと再帰呼出になるので注意！
        /// </remarks>
        public void Reload()
        {
            this.FetchEntity(this.ViewModel.Year, this.ViewModel.Month);

            if (this.Entity is null)
            {
                // 新規追加
                this.ViewModel.CreateDate = DateTime.Today;
                this.ViewModel.UpdateDate = DateTime.Today;
            }
            else
            {
                // 更新
                this.ViewModel.ID         = this.Entity.ID;
                this.ViewModel.CreateDate = this.Entity.CreateDate;
                this.ViewModel.UpdateDate = this.Entity.UpdateDate;
            }

            this.ViewModel.YearMonth = new DateTime(this.ViewModel.Year, this.ViewModel.Month, 1);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="entityDate">取得する日付</param>
        public void Initialize(DateTime entityDate)
        {
            this.FetchEntity(entityDate.Year, entityDate.Month);

            if (this.Entity is null)
            {
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

            this.ViewModel.ID         = this.Entity.ID;
            this.ViewModel.Year       = this.Entity.YearMonth.Year;
            this.ViewModel.Month      = this.Entity.YearMonth.Month;
            this.ViewModel.CreateDate = this.Entity.CreateDate;
            this.ViewModel.UpdateDate = this.Entity.UpdateDate;
        }

        /// <summary>
        /// クリア
        /// </summary>
        public void Clear()
        {
            this.ViewModel.Year       = this.ViewModel.YearMonth.Year;
            this.ViewModel.Month      = this.ViewModel.YearMonth.Month;
            this.ViewModel.CreateDate = DateTime.Today;
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

        #region デフォルトに設定

        /// <summary>
        /// デフォルトに設定
        /// </summary>
        internal void SetDefault()
        {
            var confirmingMessage = $"{this.ViewModel.Year}年{this.ViewModel.Month}月の給与明細をデフォルト明細として設定しますか？";
            if (!DialogMessageUtils.ShowConfirmingMessage(confirmingMessage, this.ViewModel.Title))
            {
                // キャンセル
                return;
            }

            // 前回のデフォルト設定を解除する
            var header = new HeaderSQLite();
            var defaultEntity = header.GetDefaultEntity();

            if (defaultEntity != null)
            {
                defaultEntity.IsDefault = false;
                header.Save(defaultEntity);
            }

            // 今回のデフォルト設定を登録する
            this.ViewModel.IsDefault = true;
        }

        #endregion

    }
}
