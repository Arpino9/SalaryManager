using System;

namespace SalaryManager.Domain.ValueObjects
{
    /// <summary>
    /// Value Object - 金額
    /// </summary>
    /// <remarks>
    /// 負数があり得る場合はプリミティブ型でよい。
    /// </remarks>
    public sealed record class MoneyValue
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="money">金額</param>
        /// <exception cref="ArgumentOutOfRangeException">値が0未満</exception>
        public MoneyValue(double money)
        {
            if (money < 0)
            {
                throw new Exceptions.FormatException("金額の値が不正です。");
            }

            this.Value = money;
        }

        /// <summary> 金額 </summary>
        public readonly double Value;

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns>¥マーク付の値</returns>
        public override string ToString() 
            => (this.Value.ToString("C"));
    }
}
