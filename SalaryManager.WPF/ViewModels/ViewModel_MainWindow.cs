using System;
using System.ComponentModel;
using System.Windows.Media;
using Reactive.Bindings;
using SalaryManager.Domain;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.Models;

namespace SalaryManager.WPF.ViewModels
{
    /// <summary>
    /// ViewModel - メイン画面
    /// </summary>
    public class ViewModel_MainWindow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        public ViewModel_MainWindow()
        {
            this.Model.ViewModel   = this;
            this.Header.MainWindow = this;
            this.WorkingReference.MainWindow = this;

            this.Model.Initialize();
 
            this.BindEvents();
        }

        /// <summary>
        /// イベント登録
        /// </summary>
        /// <remarks>
        /// Viewの指定したイベントと、発火させるメソッドを紐付ける。
        /// </remarks>
        public void BindEvents()
        {
            // 画面遷移時
            this.Window_Activated.Subscribe(_ => this.Model.Window_Activated());
            this.Window_Activated.Subscribe(_ => this.Header.Window_Activated());
            this.Window_Activated.Subscribe(_ => this.WorkPlace.Window_Activated());
            this.Window_Activated.Subscribe(_ => this.AnnualChart.Window_Activated());

            // メニュー - 編集
            this.EditCompany_Command.Subscribe(_ => this.Model.EditCompany());
            this.EditCareer_Command.Subscribe(_ => this.Model.EditCareer());
            this.EditWorkingPlace_Command.Subscribe(_ => this.Model.EditWorkingPlace());
            this.EditHome_Command.Subscribe(_ => this.Model.EditHome());
            this.EditHoliday_Command.Subscribe(_ => this.Model.EditHoliday());
            this.EditFileStorage_Command.Subscribe(_ => this.Model.EditFileSotrage());
            this.EditOption_Command.Subscribe(_ => this.Model.EditOption());

            // 読込
            this.ReadWorkSchedule_Command.Subscribe(_ => this.Model.ReadWorkSchedule());
            this.ReadDefaultPayslip_Command.Subscribe(_ => this.Model.ReadDefaultPayslip());
            this.ReadCSV_Command.Subscribe(_ => this.Model.ReadCSV());

            // 表示
            this.ShowCurrentPayslip_Command.Subscribe(_ => this.Model.ShowCurrentPayslip());

            // 出力
            this.OutputExcel_Command.Subscribe(_ => this.Model.OutputExcel());
            this.OutputSpreadSheet_Command.Subscribe(_ => this.Model.OutputSpreadSheet());

            // 保存
            this.SavePayslip_Command.Subscribe(_ => this.Model.SavePayslip());
            this.SavePayslip_Command.Subscribe(_ => this.AnnualChart.Reload());
            this.SaveDefaultPayslip_Command.Subscribe(_ => this.Header.SetDefaultPayslip());
            this.SaveDBBackup_Command.Subscribe(_ => this.Model.SaveDBBackup());
        }

        /// <summary> Model - ヘッダー </summary>
        public Model_MainWindow Model { get; set; } 
            = Model_MainWindow.GetInstance();

        /// <summary> Model - ヘッダ </summary>
        public Model_Header Header { get; set; } 
            = Model_Header.GetInstance(new HeaderSQLite());

        /// <summary> Model - 勤務場所 </summary>
        private Model_WorkPlace WorkPlace { get; set; } 
            = Model_WorkPlace.GetInstance();

        /// <summary> Model - 月収一覧 </summary>
        private Model_AnnualChart AnnualChart { get; set; } 
            = Model_AnnualChart.GetInstance();

        /// <summary> Model - 勤怠備考 </summary>
        private Model_WorkingReference WorkingReference { get; set; } 
            = Model_WorkingReference.GetInstance(new WorkingReferenceSQLite());

        #region Window

        /// <summary> Window - Background </summary>
        public ReactiveProperty<SolidColorBrush> Window_Background { get; }
            = new ReactiveProperty<SolidColorBrush>(new SolidColorBrush());

        /// <summary> Window - FontFamily </summary>
        public ReactiveProperty<FontFamily> Window_FontFamily { get; set; }
            = new ReactiveProperty<FontFamily>();

        /// <summary> Window - FontSize </summary>
        public ReactiveProperty<decimal> Window_FontSize { get; set; }
            = new ReactiveProperty<decimal>();

        /// <summary> Window - Title </summary>
        public ReactiveProperty<string> Window_Title { get; set; }
            = new ReactiveProperty<string>(Shared.SystemName);

        /// <summary> Window - Activated </summary>
        public ReactiveCommand Window_Activated { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region メニュー - 編集

        /// <summary> 会社マスタ - Command  </summary>
        public ReactiveCommand EditCompany_Command { get; private set; }
            = new ReactiveCommand();

        /// <summary> 経歴マスタ - Command </summary>
        public ReactiveCommand EditCareer_Command { get; private set; }
            = new ReactiveCommand();

        /// <summary> 就業時間マスタ - Command </summary>
        public ReactiveCommand EditWorkingPlace_Command { get; private set; } 
            = new ReactiveCommand();

        /// <summary> 自宅マスタ - Command </summary>
        public ReactiveCommand EditHome_Command { get; private set; }
            = new ReactiveCommand();

        /// <summary> 祝日マスタ - Command </summary>
        public ReactiveCommand EditHoliday_Command { get; private set; }
            = new ReactiveCommand();

        /// <summary> 添付ファイル - Command </summary>
        public ReactiveCommand EditFileStorage_Command { get; private set; }
            = new ReactiveCommand();

        /// <summary> オプション - Command </summary>
        public ReactiveCommand EditOption_Command { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region メニュー - 読込

        /// <summary> 勤怠表 - Command </summary>
        public ReactiveCommand ReadWorkSchedule_Command { get; private set; }
            = new ReactiveCommand();

        /// <summary> デフォルト明細を取得 - Command </summary>
        public ReactiveCommand ReadDefaultPayslip_Command { get; private set; }
            = new ReactiveCommand();

        /// <summary> CSV読込 - Command </summary>
        public ReactiveCommand ReadCSV_Command { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region メニュー - 表示

        /// <summary> 今月の明細 - Command </summary>
        public ReactiveCommand ShowCurrentPayslip_Command { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region メニュー - 出力

        /// <summary> Excel - Command </summary>
        public ReactiveCommand OutputExcel_Command { get; private set; }
            = new ReactiveCommand();

        /// <summary> SpreadSheet - Command </summary>
        public ReactiveCommand OutputSpreadSheet_Command { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region メニュー - 保存

        /// <summary> 給与明細 - Command </summary>
        public ReactiveCommand SavePayslip_Command { get; private set; }
            = new ReactiveCommand();

        /// <summary> デフォルト明細 - Command </summary>
        public ReactiveCommand SaveDefaultPayslip_Command { get; private set; }
            = new ReactiveCommand();

        /// <summary> DBのバックアップを作成する - Command </summary>
        public ReactiveCommand SaveDBBackup_Command { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 金額の比較用

        /// <summary> 金額の比較用 - Content </summary>
        public ReactiveProperty<string> PriceUpdown_Content { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 金額の比較用 - Foreground </summary>
        public ReactiveProperty<SolidColorBrush> PriceUpdown_Foreground { get; set; }
            = new ReactiveProperty<SolidColorBrush>();

        #endregion

    }
}
