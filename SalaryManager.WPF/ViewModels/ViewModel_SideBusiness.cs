using SalaryManager.Domain.Repositories;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

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
        public ViewModel_SideBusiness()
            : this(new SideBusinessSQLite())
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="allowance">支給額</param>
        public ViewModel_SideBusiness(ISideBusinessRepository allowance)
        {
            try
            {
                this.Model.ViewModel = this;

                this.Model.Initialize();
            }
            catch (Exception ex)
            {

            }
        }

        private Model_Header _model;

        private Model_SideBusiness Model = Model_SideBusiness.GetInstance();

        #region 副業

        private double _sideBusiness;

        /// <summary>
        /// 副業
        /// </summary>
        public double SideBusiness
        {
            get => this._sideBusiness;
            set
            {
                this._sideBusiness = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 臨時収入

        private double _perquisite;

        /// <summary>
        /// 臨時収入
        /// </summary>
        public double Perquisite
        {
            get => this._perquisite;
            set
            {
                this._perquisite = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region その他

        private double _others;

        /// <summary>
        /// その他
        /// </summary>
        public double Others
        {
            get => this._others;
            set
            {
                this._others = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region 備考

        private string _remarks;

        /// <summary>
        /// 備考
        /// </summary>
        public string Remarks
        {
            get => this._remarks;
            set
            {
                this._remarks = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

    }
}
