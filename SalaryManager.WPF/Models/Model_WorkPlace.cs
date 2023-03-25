using System;
using System.Windows.Media;
using SalaryManager.Domain.Modules.Logics;
using SalaryManager.Domain.StaticValues;
using SalaryManager.Domain.ValueObjects;
using SalaryManager.Infrastructure.Interface;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.Infrastructure.XML;
using SalaryManager.WPF.ViewModels;

namespace SalaryManager.WPF.Models
{
    /// <summary>
    /// Model - 勤務場所
    /// </summary>
    public sealed class Model_WorkPlace : IInputPayslip
    {

        #region Get Instance

        private static Model_WorkPlace model = null;

        public static Model_WorkPlace GetInstance()
        {
            if (model == null)
            {
                model = new Model_WorkPlace();
            }

            return model;
        }

        #endregion

        /// <summary> ViewModel - 勤務先 </summary>
        internal ViewModel_WorkPlace ViewModel { get; set; }

        /// <summary> ViewModel - ヘッダ </summary>
        internal ViewModel_Header Header { get; set; }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="entityDate">取得する日付</param>
        /// <remarks>
        /// 画面起動時に、項目を初期化する。
        /// </remarks>
        public void Initialize(DateTime entityDate)
        {
            WorkingReferences.Create(new WorkingReferenceSQLite());
            Careers.Create(new CareerSQLite());

            this.ViewModel.Entity          = WorkingReferences.Fetch(entityDate.Year, entityDate.Month);
            this.ViewModel.Entity_LastYear = WorkingReferences.Fetch(entityDate.Year, entityDate.Month - 1);

            this.Window_Activated();

            if (this.ViewModel.Entity is null &&
                XMLLoader.FetchShowDefaultPayslip())
            {
                // デフォルト明細
                this.ViewModel.Entity = WorkingReferences.FetchDefault();
            }

            this.Refresh();
        }

        /// <summary>
        /// 画面起動時の処理
        /// </summary>
        internal void Window_Activated()
        {
            this.ViewModel.Window_Background = XMLLoader.FetchBackgroundColorBrush();
        }

        /// <summary>
        /// 再描画
        /// </summary>
        /// <remarks>
        /// 該当月に控除額が存在すれば、各項目を再描画する。
        /// </remarks>
        public void Refresh()
        {
            var entity = this.ViewModel.Entity;

            if (entity is null)
            {
                this.Clear();
                return;
            }

            // 勤務先
            this.ViewModel.WorkPlace = entity.WorkPlace;

            // 所属会社名
            Careers.Create(new CareerSQLite());

            var company = Careers.FetchCompany(new DateTime(this.Header.Year_Value, this.Header.Month_Value, 1));
            this.ViewModel.CompanyName = company;

            if (company == CompanyValue.Undefined.DisplayValue)
            {
                this.ViewModel.CompanyName_Foreground = new SolidColorBrush(Colors.Gray);
            }
            else
            {
                this.ViewModel.CompanyName_Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        /// <summary>
        /// リロード
        /// </summary>
        /// <remarks>
        /// 年月の変更時などに、該当月の項目を取得する。
        /// </remarks>
        public void Reload()
        {
            using (var cursor = new CursorWaiting())
            {
                WorkingReferences.Create(new WorkingReferenceSQLite());

                this.ViewModel.Entity          = WorkingReferences.Fetch(this.Header.Year_Value, this.Header.Month_Value);
                this.ViewModel.Entity_LastYear = WorkingReferences.Fetch(this.Header.Year_Value - 1, this.Header.Month_Value);

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
            // 所属会社名
            this.ViewModel.CompanyName            = CompanyValue.Undefined.DisplayValue;
            this.ViewModel.CompanyName_Foreground = new SolidColorBrush(Colors.Gray);
            // 勤務先
            this.ViewModel.WorkPlace = default(string);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="transaction">トランザクション</param>
        /// <see cref="Model_WorkingReference"/>
        /// <exception cref="NotImplementedException">未実装例外</exception>
        /// <remarks>
        /// 保存先は勤怠備考テーブルなので実装していない。
        /// </remarks>
        public void Save(SQLiteTransaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}
