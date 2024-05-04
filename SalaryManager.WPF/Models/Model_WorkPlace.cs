using System;
using System.Linq;
using System.Windows.Media;
using SalaryManager.Domain.Modules.Logics;
using SalaryManager.Domain.Repositories;
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

        public Model_WorkPlace()
        {

        }

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

            var showDefaultPayslip = XMLLoader.FetchShowDefaultPayslip();

            if (this.ViewModel.Entity is null && showDefaultPayslip)
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
            this.ViewModel.Window_Background.Value = XMLLoader.FetchBackgroundColorBrush();
        }

        /// <summary>
        /// 再描画
        /// </summary>
        /// <remarks>
        /// 勤務先、所属会社名を再描画する。
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
            this.ViewModel.WorkPlace_Text.Value = entity.WorkPlace;

            // 所属会社名
            WorkingPlace.Create(new WorkingPlaceSQLite());
            Careers.Create(new CareerSQLite());

            var workingPlace = WorkingPlace.FetchByDate(new DateTime(this.Header.Year_Text.Value, this.Header.Month_Text.Value, 1));

            if (workingPlace.Any()) 
            {
                this.ViewModel.CompanyName_Text.Value = workingPlace.FirstOrDefault().DispatchingCompany.Text;
                this.ViewModel.WorkPlace_Text.Value   = workingPlace.FirstOrDefault().DispatchedCompany.Text;
            }
            else
            {
                this.ViewModel.CompanyName_Text.Value = CompanyNameValue.Undefined.DisplayValue;
                this.ViewModel.WorkPlace_Text.Value   = CompanyNameValue.Undefined.DisplayValue;
            }

            this.ViewModel.CompanyName_Foreground.Value = new SolidColorBrush(Colors.Black);
            this.ViewModel.WorkPlace_Foreground.Value   = new SolidColorBrush(Colors.Black);
        }

        /// <summary>
        /// リロード
        /// </summary>
        /// <remarks>
        /// 年月の変更時などに、該当月の項目を取得する。
        /// </remarks>
        public void Reload()
        {
            if (this.ViewModel is null) return;

            using (var cursor = new CursorWaiting())
            {
                WorkingReferences.Create(new WorkingReferenceSQLite());

                this.ViewModel.Entity          = WorkingReferences.Fetch(this.Header.Year_Text.Value, this.Header.Month_Text.Value);
                this.ViewModel.Entity_LastYear = WorkingReferences.Fetch(this.Header.Year_Text.Value - 1, this.Header.Month_Text.Value);

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
            this.ViewModel.CompanyName_Text.Value       = CompanyNameValue.Undefined.DisplayValue;
            this.ViewModel.CompanyName_Foreground.Value = new SolidColorBrush(Colors.Gray);
            // 勤務先
            this.ViewModel.WorkPlace_Text.Value = CompanyNameValue.Undefined.DisplayValue;
            this.ViewModel.WorkPlace_Foreground.Value = new SolidColorBrush(Colors.Gray);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="transaction">トランザクション</param>
        /// <see cref="Model_WorkingReference"/>
        /// <exception cref="NotImplementedException">未実装例外</exception>
        [Obsolete("保存先は勤怠備考テーブルなので実装していない。")]
        public void Save(ITransactionRepository transaction)
        {
            throw new NotImplementedException();
        }
    }
}
