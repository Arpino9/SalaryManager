using System;
using System.ComponentModel;
using System.Windows.Media;
using Reactive.Bindings;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.Models;

namespace SalaryManager.WPF.ViewModels
{
    /// <summary>
    /// ViewModel - ヘッダ
    /// </summary>
    public class ViewModel_Header : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel_Header()
        {
            this.MainWindow.Header = this.Model;

            this.Model.ViewModel         = this;
            this.Allowance.Header        = this;
            this.Deduction.Header        = this;
            this.WorkingReference.Header = this;
            this.SideBusiness.Header     = this;
            this.WorkPlace.Header        = this;
            this.AnnualCharts.Header     = this;

            this.BindEvents();

            this.Model.Initialize(DateTime.Today);
        }

        /// <summary>
        /// イベント登録
        /// </summary>
        /// <remarks>
        /// Viewの指定したイベントと、発火させるメソッドを紐付ける。
        /// Subscribe()メソッドのオーバーロードが正しく呼ばれないので、
        /// 名前空間に「using System;」を必ず入れること。
        /// </remarks>
        private void BindEvents()
        {
            // ←(戻る)
            this.Return_Command.Subscribe(_ => this.Model.Return());
            this.Return_Command.Subscribe(_ => this.Allowance.Reload());
            this.Return_Command.Subscribe(_ => this.Deduction.Reload());
            this.Return_Command.Subscribe(_ => this.WorkingReference.Reload());
            this.Return_Command.Subscribe(_ => this.SideBusiness.Reload());
            this.Return_Command.Subscribe(_ => this.WorkPlace.Reload());
            this.Return_Command.Subscribe(_ => this.AnnualCharts.Reload());

            // →(進む)
            this.Proceed_Command.Subscribe(_ => this.Model.Proceed());
            this.Proceed_Command.Subscribe(_ => this.Allowance.Reload());
            this.Proceed_Command.Subscribe(_ => this.Deduction.Reload());
            this.Proceed_Command.Subscribe(_ => this.WorkingReference.Reload());
            this.Proceed_Command.Subscribe(_ => this.SideBusiness.Reload());
            this.Proceed_Command.Subscribe(_ => this.WorkPlace.Reload());
            this.Proceed_Command.Subscribe(_ => this.AnnualCharts.Reload());

            // 年
            this.Year_Text.Subscribe(_ => this.Model.IsValid_Year());
            this.Year_TextChanged.Subscribe(_ => this.Model.Reload());
            this.Year_TextChanged.Subscribe(_ => this.Allowance.Reload());
            this.Year_TextChanged.Subscribe(_ => this.Deduction.Reload());
            this.Year_TextChanged.Subscribe(_ => this.WorkingReference.Reload());
            this.Year_TextChanged.Subscribe(_ => this.SideBusiness.Reload());
            this.Year_TextChanged.Subscribe(_ => this.WorkPlace.Reload());
            this.Year_TextChanged.Subscribe(_ => this.AnnualCharts.Reload());

            // 月
            this.Month_Text.Subscribe(_ => this.Model.IsValid_Month());
            this.Month_TextChanged.Subscribe(_ => this.Model.Reload());
            this.Month_TextChanged.Subscribe(_ => this.Allowance.Reload());
            this.Month_TextChanged.Subscribe(_ => this.Deduction.Reload());
            this.Month_TextChanged.Subscribe(_ => this.WorkingReference.Reload());
            this.Month_TextChanged.Subscribe(_ => this.SideBusiness.Reload());
            this.Month_TextChanged.Subscribe(_ => this.WorkPlace.Reload());
            this.Month_TextChanged.Subscribe(_ => this.AnnualCharts.Reload());
        }

        /// <summary> Model - ヘッダー </summary>
        public Model_Header Model { get; set; } 
            = Model_Header.GetInstance(new HeaderSQLite());

        /// <summary> Model - メイン画面 </summary>
        public Model_MainWindow MainWindow { get; set; } 
            = Model_MainWindow.GetInstance();

        /// <summary> Model - 月収一覧 </summary>
        public Model_AnnualChart AnnualCharts { get; set; } 
            = Model_AnnualChart.GetInstance();

        /// <summary> Model - 支給額 </summary>
        public Model_Allowance Allowance { get; set; } 
            = Model_Allowance.GetInstance(new AllowanceSQLite());

        /// <summary> Model - 控除額 </summary>
        public Model_Deduction Deduction { get; set; } 
            = Model_Deduction.GetInstance(new DeductionSQLite());

        /// <summary> Model - 勤務備考 </summary>
        public Model_WorkingReference WorkingReference { get; set; } 
            = Model_WorkingReference.GetInstance(new WorkingReferenceSQLite());

        /// <summary> Model - 勤務場所 </summary>
        public Model_WorkPlace WorkPlace { get; set; } 
            = Model_WorkPlace.GetInstance();

        /// <summary> Model - 副業 </summary>
        public Model_SideBusiness SideBusiness { get; set; } 
            = Model_SideBusiness.GetInstance(new SideBusinessSQLite());

        #region 背景色

        /// <summary> 背景色 - Background </summary>
        public ReactiveProperty<SolidColorBrush> Window_Background { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        #endregion

        /// <summary> ID </summary>
        public int ID { get; internal set; }

        /// <summary> 年月 </summary>
        public DateTime YearMonth { get; set; } = DateTime.Today;

        #region 年

        /// <summary> 年 - Text </summary>
        public ReactiveProperty<int> Year_Text { get; set; }
            = new ReactiveProperty<int>(DateTime.Now.Year);

        /// <summary> 年 - TextChanged </summary>
        public ReactiveCommand Year_TextChanged { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 月

        /// <summary> 月 - Text </summary>
        public ReactiveProperty<int> Month_Text { get; set; }
            = new ReactiveProperty<int>(DateTime.Now.Month);

        /// <summary> 月 - TextChanged </summary>
        public ReactiveCommand Month_TextChanged { get; private set; }
            = new ReactiveCommand();

        #endregion

        /// <summary> デフォルトか </summary>
        public bool IsDefault { get; set; }

        /// <summary> 作成日 </summary>
        public DateTime CreateDate { get; set; }

        /// <summary> 更新日 </summary>
        public DateTime UpdateDate { get; set; }

        #region 戻るボタン

        /// <summary> 戻る - Command </summary>
        public ReactiveCommand Return_Command { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 進むボタン

        /// <summary> 進む - Command </summary>
        public ReactiveCommand Proceed_Command { get; private set; }
            = new ReactiveCommand();

        #endregion

    }
}
