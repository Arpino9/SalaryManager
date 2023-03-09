namespace SalaryManager.Domain.ValueObjects
{
    /// <summary>
    /// Value Object - 二者択一
    /// </summary>
    public class AlternativeValue : ValueObject<AlternativeValue>
    {
        /// <summary> ○ </summary>
        public static readonly AlternativeValue Valid = new AlternativeValue(true);

        /// <summary> × </summary>
        public static readonly AlternativeValue Invalid = new AlternativeValue(false);

        public AlternativeValue(bool value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Value 
        /// </summary>
        private readonly bool Value;

        /// <summary>
        /// Text
        /// </summary>
        public string Text
        {
            get
            {
                return (this == AlternativeValue.Valid ? "○" : "×");
            }
        }

        /// <summary>
        /// To String
        /// </summary>
        /// <returns>文字列</returns>
        public override string ToString()
        {
            return (this.Text);
        }

        protected override bool EqualsCore(AlternativeValue other)
        {
            return this.Value == other.Value;
        }
    }
}
