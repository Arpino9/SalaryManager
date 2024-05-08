using System;
using System.Windows.Media;
using Reactive.Bindings;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.Models;

namespace SalaryManager.WPF.ViewModels
{
    /// <summary>
    /// ViewModel - 控除額
    /// </summary>
    public class ViewModel_Deduction
    {
        public ViewModel_Deduction()
        {
            this.MainWindow.Deduction = this.Model;

            this.Allowance.ViewModel_Deduction = this;

            this.Model.ViewModel = this;
            this.Model.Initialize(DateTime.Today);

            this.BindEvent();
        }

        /// <summary>
        /// イベント登録
        /// </summary>
        /// <remarks>
        /// Viewの指定したイベントと、発火させるメソッドを紐付ける。
        /// Subscribe()メソッドのオーバーロードが正しく呼ばれないので、
        /// 名前空間に「using System;」を必ず入れること。
        /// </remarks>
        private void BindEvent()
        {
            // 初期状態
            this.Default_MouseLeave.Subscribe(_ => this.MainWindow.ComparePrice(0, 0));
            // 健康保険
            this.HealthInsurance_MouseMove.Subscribe(_ => this.MainWindow.ComparePrice(this.HealthInsurance_Text.Value, this.Model.Entity_LastYear?.HealthInsurance.Value));
            // 介護保険
            this.NursingInsurance_MouseMove.Subscribe(_ => this.MainWindow.ComparePrice(this.NursingInsurance_Text.Value, this.Model.Entity_LastYear?.NursingInsurance.Value));
            // 厚生年金
            this.WelfareAnnuity_MouseMove.Subscribe(_ => this.MainWindow.ComparePrice(this.WelfareAnnuity_Text.Value, this.Model.Entity_LastYear?.WelfareAnnuity.Value));
            // 雇用保険
            this.EmploymentInsurance_MouseMove.Subscribe(_ => this.MainWindow.ComparePrice(this.EmploymentInsurance_Text.Value, this.Model.Entity_LastYear?.EmploymentInsurance.Value));
            // 所得税
            this.IncomeTax_MouseMove.Subscribe(_ => this.MainWindow.ComparePrice(this.IncomeTax_Text.Value, this.Model.Entity_LastYear?.IncomeTax.Value));
            // 市町村税
            this.MunicipalTax_MouseMove.Subscribe(_ => this.MainWindow.ComparePrice(this.MunicipalTax_Text.Value, this.Model.Entity_LastYear?.MunicipalTax.Value));
            // 互助会
            this.FriendshipAssociation_MouseMove.Subscribe(_ => this.MainWindow.ComparePrice(this.FriendshipAssociation_Text.Value, this.Model.Entity_LastYear?.FriendshipAssociation.Value));
            // 年末調整他
            this.YearEndTaxAdjustment_MouseMove.Subscribe(_ => this.MainWindow.ComparePrice(this.YearEndTaxAdjustment_Text.Value, this.Model.Entity_LastYear?.YearEndTaxAdjustment));
            // 控除額計
            this.TotalDeduct_MouseMove.Subscribe(_ => this.MainWindow.ComparePrice(this.TotalDeduct_Text.Value, this.Model.Entity_LastYear?.TotalDeduct.Value));
        }

        /// <summary> Model - 控除額 </summary>
        public Model_Deduction Model { get; set; } 
            = Model_Deduction.GetInstance(new DeductionSQLite());

        /// <summary> Model - 支給額 </summary>
        public Model_Allowance Allowance { get; set; } 
            = Model_Allowance.GetInstance(new AllowanceSQLite());

        /// <summary> Model - メイン画面 </summary>
        public Model_MainWindow MainWindow { get; set; } 
            = Model_MainWindow.GetInstance();

        #region 初期状態

        /// <summary> 初期状態 - MouseMove </summary>
        public ReactiveCommand Default_MouseLeave { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 健康保険

        /// <summary> 健康保険 - Text </summary>
        public ReactiveProperty<double> HealthInsurance_Text { get; set; }
            = new ReactiveProperty<double>();

        /// <summary> 健康保険 - MouseMove </summary>
        public ReactiveCommand HealthInsurance_MouseMove { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 介護保険

        /// <summary> 介護保険 - Text </summary>
        public ReactiveProperty<double> NursingInsurance_Text { get; set; }
            = new ReactiveProperty<double>();

        /// <summary> 介護保険 - MouseMove </summary>
        public ReactiveCommand NursingInsurance_MouseMove { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 厚生年金

        /// <summary> 厚生年金 - Text </summary>
        public ReactiveProperty<double> WelfareAnnuity_Text { get; set; }
            = new ReactiveProperty<double>();

        /// <summary> 厚生年金 - MouseMove </summary>
        public ReactiveCommand WelfareAnnuity_MouseMove { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 雇用保険

        /// <summary> 雇用保険 - Text </summary>
        public ReactiveProperty<double> EmploymentInsurance_Text { get; set; }
            = new ReactiveProperty<double>();

        /// <summary> 雇用保険 - MouseMove </summary>
        public ReactiveCommand EmploymentInsurance_MouseMove { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 所得税

        /// <summary> 所得税 - Text </summary>
        public ReactiveProperty<double> IncomeTax_Text { get; set; }
            = new ReactiveProperty<double>();

        /// <summary> 所得税 - MouseMove </summary>
        public ReactiveCommand IncomeTax_MouseMove { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 市町村税

        /// <summary> 市町村税 - Text </summary>
        public ReactiveProperty<double> MunicipalTax_Text { get; set; }
            = new ReactiveProperty<double>();

        /// <summary> 市町村税 - MouseMove </summary>
        public ReactiveCommand MunicipalTax_MouseMove { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 互助会

        /// <summary> 互助会 - Text </summary>
        public ReactiveProperty<double> FriendshipAssociation_Text { get; set; }
            = new ReactiveProperty<double>();

        /// <summary> 互助会 - MouseMove </summary>
        public ReactiveCommand FriendshipAssociation_MouseMove { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 年末調整他

        /// <summary> 年末調整他 - Text </summary>
        public ReactiveProperty<double> YearEndTaxAdjustment_Text { get; set; }
            = new ReactiveProperty<double>();

        /// <summary> 年末調整他 - MouseMove </summary>
        public ReactiveCommand YearEndTaxAdjustment_MouseMove { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 控除額計

        /// <summary> 控除額計 - Foreground </summary>
        public ReactiveProperty<SolidColorBrush> TotalDeduct_Foreground { get; }
            = new ReactiveProperty<SolidColorBrush>(new SolidColorBrush(Colors.Red));

        /// <summary> 控除額計 - Text </summary>
        public ReactiveProperty<double> TotalDeduct_Text { get; set; }
            = new ReactiveProperty<double>();

        /// <summary> 控除額計 - MouseMove </summary>
        public ReactiveCommand TotalDeduct_MouseMove { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region 備考

        /// <summary> 備考 - Text </summary>
        public ReactiveProperty<string> Remarks_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

    }
}
