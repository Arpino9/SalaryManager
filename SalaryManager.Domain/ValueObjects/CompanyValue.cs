namespace SalaryManager.Domain.ValueObjects
{
    /// <summary>
    /// Value Object - 会社
    /// </summary>
    public class CompanyValue : ValueObject<CompanyValue>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value">値</param>
        public CompanyValue(string value)
        {
            this.Text = value;
        }

        /// <summary> Text </summary>
        public string Text { get; }

        /// <summary> 未登録 </summary>
        public static CompanyValue Undefined = new CompanyValue(string.Empty);

        /// <summary>
        /// 株式会社か
        /// </summary>
        public bool IsInc
        {
            get
            {
                return (this.Text.Contains("株式会社") || 
                        this.Text.Contains("(株)") ||
                        this.Text.Contains("（株）"));
            }
        }

        /// <summary>
        /// 有限会社か
        /// </summary>
        public bool IsLimited
        {
            get
            {
                return (this.Text.Contains("有限会社") ||
                        this.Text.Contains("(有)") ||
                        this.Text.Contains("（有）"));
            }
        }

        /// <summary>
        /// 表示用
        /// </summary>
        public string DisplayValue
        {
            get
            {
                return (this == CompanyValue.Undefined ? "<未登録>" : this.Text);
            }
        }

        protected override bool EqualsCore(CompanyValue other)
        {
            return (this.Text == other.Text);
        }
    }
}
