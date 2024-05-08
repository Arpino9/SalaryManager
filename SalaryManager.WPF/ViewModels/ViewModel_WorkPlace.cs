using System;
using System.Windows.Media;
using Reactive.Bindings;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.Models;

namespace SalaryManager.WPF.ViewModels
{
    /// <summary>
    /// ViewModel - 勤務先
    /// </summary>
    public class ViewModel_WorkPlace
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewModel_WorkPlace()
        {
            this.Model.ViewModel               = this;
            this.WorkingReference.WorkPlace    = this;
            this.MainWindow.WorkPlace          = this;
            this.Allowance.ViewModel_WorkPlace = this;

            this.Model.Initialize(DateTime.Today);
        }

        /// <summary> Model </summary>
        public Model_WorkPlace Model { get; set; } 
            = Model_WorkPlace.GetInstance();

        /// <summary> Model - 勤怠備考 </summary>
        public Model_WorkingReference WorkingReference { get; set; } 
            = Model_WorkingReference.GetInstance(new WorkingReferenceSQLite());

        /// <summary> Model - メイン画面 </summary>
        public Model_MainWindow MainWindow { get; set; } 
            = Model_MainWindow.GetInstance();

        /// <summary> Model - 手当 </summary>
        public Model_Allowance Allowance { get; set; } 
            = Model_Allowance.GetInstance(new AllowanceSQLite());

        #region 背景色

        /// <summary> 背景色 - Background </summary>
        public ReactiveProperty<SolidColorBrush> Window_Background { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        #endregion

        #region 所属会社名

        /// <summary> 所属会社名 - Foreground </summary>
        public ReactiveProperty<SolidColorBrush> CompanyName_Foreground { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        /// <summary> 所属会社名 - Text </summary>
        public ReactiveProperty<string> CompanyName_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

        #region 勤務先

        /// <summary> 所属会社名 - Foreground </summary>
        public ReactiveProperty<SolidColorBrush> WorkPlace_Foreground { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        /// <summary> 勤務先 - Text </summary>
        public ReactiveProperty<string> WorkPlace_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

    }
}
