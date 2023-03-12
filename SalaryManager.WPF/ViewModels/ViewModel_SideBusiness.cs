using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SalaryManager.Domain.Entities;
using SalaryManager.WPF.Converter;
using SalaryManager.WPF.Models;

namespace SalaryManager.WPF.ViewModels
{
    /// <summary>
    /// ViewModel - 副業
    /// </summary>
    public class ViewModel_SideBusiness : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var d = PropertyChanged;
            d?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <exception cref="Exception">読込失敗時</exception>
        public ViewModel_SideBusiness()
        {
            try
            {
                this.MainWindow.SideBusiness = this.Model;

                this.Model.ViewModel = this;
                this.Model.Initialize(DateTime.Today);

                this.BindEvent();
            }
            catch (Exception ex)
            {
                throw new Exception("副業テーブルの読込に失敗しました。", ex);
            }
        }

        /// <summary>
        /// Bind Event
        /// </summary>
        private void BindEvent()
        {
            // Mouse Leave
            this.MouseLeave_Action   = new RelayCommand(() => this.MainWindow.ComparePrice(0, 0));
            // 副業
            this.SideBusiness_Action = new RelayCommand(() => this.MainWindow.ComparePrice(this.SideBusiness_Value, this.Entity_LastYear?.SideBusiness));
            // 臨時収入
            this.Perquisite_Action   = new RelayCommand(() => this.MainWindow.ComparePrice(this.Perquisite_Value,   this.Entity_LastYear?.Perquisite));
            // その他
            this.Others_Action       = new RelayCommand(() => this.MainWindow.ComparePrice(this.Others_Value,       this.Entity_LastYear?.Others));
        }

        /// <summary> Model </summary>
        public Model_SideBusiness Model { get; set; } = Model_SideBusiness.GetInstance();

        /// <summary> Model - メイン画面 </summary>
        public Model_MainWindow MainWindow { get; set; } = Model_MainWindow.GetInstance();

        /// <summary> Entity - 勤務備考 </summary>
        public SideBusinessEntity Entity { get; set; }

        /// <summary> Entity - 勤務備考 (昨年度) </summary>
        public SideBusinessEntity Entity_LastYear { get; set; }

        #region Mouse Leave

        /// <summary>
        /// MouseLeave - Action
        /// </summary>
        public RelayCommand MouseLeave_Action { get; private set; }

        #endregion

        #region 副業

        private double _sideBusiness_Value;

        /// <summary>
        /// 副業 - Value
        /// </summary>
        public double SideBusiness_Value
        {
            get => this._sideBusiness_Value;
            set
            {
                this._sideBusiness_Value = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 副業 - Action
        /// </summary>
        public RelayCommand SideBusiness_Action { get; private set; }

        #endregion

        #region 臨時収入

        private double _perquisite_Value;

        /// <summary>
        /// 臨時収入 - Value
        /// </summary>
        public double Perquisite_Value
        {
            get => this._perquisite_Value;
            set
            {
                this._perquisite_Value = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 臨時収入 - Action
        /// </summary>
        public RelayCommand Perquisite_Action { get; private set; }

        #endregion

        #region その他

        private double _others_Value;

        /// <summary>
        /// その他 - Value
        /// </summary>
        public double Others_Value
        {
            get => this._others_Value;
            set
            {
                this._others_Value = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// その他 - Action
        /// </summary>
        public RelayCommand Others_Action { get; private set; }

        #endregion

        #region 備考

        private string _remarks_Text;

        /// <summary>
        /// 備考 - Text
        /// </summary>
        public string Remarks_Text
        {
            get => this._remarks_Text;
            set
            {
                this._remarks_Text = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

    }
}
