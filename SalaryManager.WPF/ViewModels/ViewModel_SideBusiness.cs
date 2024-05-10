using System;
using System.ComponentModel;
using Reactive.Bindings;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.Models;

namespace SalaryManager.WPF.ViewModels
{
    /// <summary>
    /// ViewModel - 副業
    /// </summary>
    public class ViewModel_SideBusiness : ViewModelBase
    {
        public override event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <exception cref="Exception">読込失敗時</exception>
        public ViewModel_SideBusiness()
        {
            this.MainWindow.SideBusiness = this.Model;

            this.Model.ViewModel = this;
            this.Model.Initialize(DateTime.Today);

            this.BindEvents();
        }

        protected override void BindEvents()
        {
            // Mouse Leave
            this.Default_MouseLeave.Subscribe(_ => this.MainWindow.ComparePrice(0, 0));
            // 副業
            this.SideBusiness_MouseMove.Subscribe(_ => this.MainWindow.ComparePrice(this.SideBusiness_Text.Value, this.Model.Entity_LastYear?.SideBusiness));
            // 臨時収入
            this.Perquisite_MouseMove.Subscribe(_ => this.MainWindow.ComparePrice(this.Perquisite_Text.Value, this.Model.Entity_LastYear?.Perquisite));
            // その他
            this.Others_MouseMove.Subscribe(_ => this.MainWindow.ComparePrice(this.Others_Text.Value, this.Model.Entity_LastYear?.Others));
        }

        /// <summary> Model </summary>
        public Model_SideBusiness Model { get; set; } 
            = Model_SideBusiness.GetInstance(new SideBusinessSQLite());

        /// <summary> Model - メイン画面 </summary>
        public Model_MainWindow MainWindow { get; set; }
            = Model_MainWindow.GetInstance();

        #region Mouse Leave

        /// <summary> MouseLeave - MouseLeave </summary>
        public ReactiveCommand Default_MouseLeave { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 副業

        /// <summary> 副業 - Text </summary>
        public ReactiveProperty<double> SideBusiness_Text { get; set; }
            = new ReactiveProperty<double>();

        /// <summary> 副業 - MouseMove </summary>
        public ReactiveCommand SideBusiness_MouseMove { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 臨時収入

        /// <summary> 臨時収入 - Text </summary>
        public ReactiveProperty<double> Perquisite_Text { get; set; }
            = new ReactiveProperty<double>();

        /// <summary> 臨時収入 - MouseMove </summary>
        public ReactiveCommand Perquisite_MouseMove { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region その他

        /// <summary> その他 - Text </summary>
        public ReactiveProperty<double> Others_Text { get; set; }
            = new ReactiveProperty<double>();

        /// <summary> その他 - MouseMove </summary>
        public ReactiveCommand Others_MouseMove { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 備考

        /// <summary> 備考 - Text </summary>
        public ReactiveProperty<string> Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

    }
}
