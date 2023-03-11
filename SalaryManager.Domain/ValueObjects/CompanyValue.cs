﻿namespace SalaryManager.Domain.ValueObjects
{
    /// <summary>
    /// Value Object - 会社名
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

        /// <summary>
        /// Text 
        /// </summary>
        public string Text { get; }

        /// <summary> 未登録 </summary>
        public static CompanyValue Undefined = new CompanyValue("<未登録>");

        /// <summary>
        /// 株式会社か
        /// </summary>
        public bool IsInc
        {
            get
            {
                return (this.Text.StartsWith("株式会社") || 
                        this.Text.EndsWith("株式会社"));
            }
        }

        /// <summary>
        /// 有限会社か
        /// </summary>
        public bool IsLimited
        {
            get
            {
                return (this.Text.StartsWith("有限会社") ||
                        this.Text.EndsWith("有限会社"));
            }
        }

        public override string ToString()
        {
            return (this == Undefined ? Undefined.Text : base.ToString());
        }

        protected override bool EqualsCore(CompanyValue other)
        {
            return this.Text == other.Text;
        }
    }
}
