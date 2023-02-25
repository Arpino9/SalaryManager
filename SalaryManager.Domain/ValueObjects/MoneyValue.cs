using System;

namespace SalaryManager.Domain.ValueObjects
{
    /// <summary>
    /// Value Object - 金額
    /// </summary>
    /// <remarks>
    /// 負数があり得る場合はプリミティブ型でよい。
    /// </remarks>
    public sealed class MoneyValue : ValueObject<MoneyValue>
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
                throw new ArgumentOutOfRangeException("金額の値が不正です。");
            }

            this.Value = money;
        }

        /// <summary>
        /// 金額
        /// </summary>
        public readonly double Value;

        /// <summary>
        /// To String
        /// </summary>
        /// <returns>金額</returns>
        public override string ToString()
        {
            return (this.Value.ToString("C"));
        }

        /// <summary>
        /// Equals Core
        /// </summary>
        /// <param name="other">金額</param>
        /// <returns>金額</returns>
        protected override bool EqualsCore(MoneyValue other)
        {
            return (this.Value == other.Value);
        }
    }
}
