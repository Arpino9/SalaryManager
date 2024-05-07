using System;
using SalaryManager.Domain.Entities;
using SalaryManager.Infrastructure.Interface;
using SalaryManager.WPF.ViewModels;
using SalaryManager.Domain.StaticValues;
using SalaryManager.Domain.Modules.Logics;
using SalaryManager.Infrastructure.XML;
using SalaryManager.Domain.Repositories;

namespace SalaryManager.WPF.Models
{
    /// <summary>
    /// Model - 副業
    /// </summary>
    public class Model_SideBusiness : IInputPayslip
    {

        #region Get Instance

        private static Model_SideBusiness model = null;

        public static Model_SideBusiness GetInstance(ISideBusinessRepository repository)
        {
            if (model == null)
            {
                model = new Model_SideBusiness(repository);
            }

            return model;
        }

        #endregion

        /// <summary> Repository </summary>
        private ISideBusinessRepository _repository;

        public Model_SideBusiness(ISideBusinessRepository repository)
        {
            _repository = repository;
        }

        /// <summary> ViewModel - ヘッダ </summary>
        internal ViewModel_Header Header { get; set; }

        /// <summary> ViewModel - 副業 </summary>
        internal ViewModel_SideBusiness ViewModel { get; set; }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="entityDate">取得する日付</param>
        /// <remarks>
        /// 画面起動時に、項目を初期化する。
        /// </remarks>
        public void Initialize(DateTime entityDate)
        {
            SideBusinesses.Create(_repository);

            this.ViewModel.Entity          = SideBusinesses.Fetch(entityDate.Year, entityDate.Month);
            this.ViewModel.Entity_LastYear = SideBusinesses.Fetch(entityDate.Year, entityDate.Month - 1);

            var showDefaultPayslip = XMLLoader.FetchShowDefaultPayslip();

            if (this.ViewModel.Entity is null && showDefaultPayslip)
            {
                // デフォルト明細
                this.ViewModel.Entity = SideBusinesses.FetchDefault();
            }

            this.Refresh();
        }

        /// <summary>
        /// リロード
        /// </summary>
        /// <remarks>
        /// 該当月に副業額が存在すれば、各項目を再描画する。
        /// </remarks>
        public void Reload()
        {
            using (var cursor = new CursorWaiting())
            {
                SideBusinesses.Create(_repository);

                this.ViewModel.Entity          = SideBusinesses.Fetch(this.Header.Year_Text.Value, this.Header.Month_Text.Value);
                this.ViewModel.Entity_LastYear = SideBusinesses.Fetch(this.Header.Year_Text.Value - 1, this.Header.Month_Text.Value);
            
                this.Refresh();
            }   
        }

        /// <summary>
        /// クリア
        /// </summary>
        /// <remarks>
        /// 各項目を初期化する。
        /// </remarks>
        public void Clear()
        {
            // 副業
            this.ViewModel.SideBusiness_Text.Value = default(double);
            // 臨時収入
            this.ViewModel.Perquisite_Text.Value   = default(double);
            // その他
            this.ViewModel.Others_Text.Value       = default(double);
            // 備考
            this.ViewModel.Remarks_Text.Value      = default(string);
        }

        /// <summary>
        /// 再描画
        /// </summary>
        /// <remarks>
        /// 該当月に副業額が存在すれば、各項目を再描画する。
        /// </remarks>
        public void Refresh()
        {
            var entity = this.ViewModel.Entity;

            if (entity is null)
            {
                this.Clear();
                return;
            }

            // 副業
            this.ViewModel.SideBusiness_Text.Value = entity.SideBusiness;
            // 臨時収入
            this.ViewModel.Perquisite_Text.Value   = entity.Perquisite;
            // その他
            this.ViewModel.Others_Text.Value       = entity.Others;
            // 備考
            this.ViewModel.Remarks_Text.Value      = entity.Remarks;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="transaction">トランザクション</param>
        /// <remarks>
        /// SQLiteに接続し、入力項目を保存する。
        /// </remarks>
        public void Save(ITransactionRepository transaction)
        {
            var entity = new SideBusinessEntity(
                this.Header.ID,
                this.Header.YearMonth,
                this.ViewModel.SideBusiness_Text.Value,
                this.ViewModel.Perquisite_Text.Value,
                this.ViewModel.Others_Text.Value,
                this.ViewModel.Remarks_Text.Value);

            _repository.Save(transaction, entity);
        }
    }
}
