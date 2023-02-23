/// <summary>
/// Salary Manager
/// </summary>

namespace SalaryManager
{
    /// <summary>
    /// 副業
    /// </summary>
    public class ItemSideBusiness :
        // 給与明細
        ItemPaySlipAbstract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ItemSideBusiness()
        {
        }

        #region 副業収入

        private int _SideBusiness;

        /// <summary>
        /// 副業 - 副業収入
        /// </summary>
        public int? SideBusiness
        {
            get
            {
                return this._SideBusiness;
            }
            set
            {
                this._SideBusiness = value ?? 0;
            }
        }

        #endregion

        #region 臨時収入

        private int _Perquisite;

        /// <summary>
        /// 副業 - 副業収入
        /// </summary>
        public int? Perquisite
        {
            get
            {
                return this._Perquisite;
            }
            set
            {
                this._Perquisite = value ?? 0;
            }
        }

        #endregion

        #region その他

        private int _Others;

        /// <summary>
        /// 副業 - その他
        /// </summary>
        public int? Others
        {
            get
            {
                return this._Others;
            }
            set
            {
                this._Others = value ?? 0;
            }
        }

        #endregion

        #region 備考

        private string _Remarks;

        /// <summary>
        /// 備考
        /// </summary>
        public string Remarks
        {
            get
            {
                return this._Remarks;
            }
            set
            {
                this._Remarks = value;
            }
        }

        #endregion

        #region Reset

        /// <summary>
        /// Reset
        /// </summary>
        public override void Reset()
        {
            // 副業
            this.SideBusiness = default(int);
            // 臨時収入
            this.Perquisite   = default(int);
            // その他
            this.Others       = default(int);
            // 備考
            this.Remarks      = default(string);

        }

        #endregion

    }
}
