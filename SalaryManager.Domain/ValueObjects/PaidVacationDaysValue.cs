﻿using System;

namespace SalaryManager.Domain.ValueObjects
{
    /// <summary>
    /// Value Object - 有給日数
    /// </summary>
    public sealed class PaidVacationDaysValue : ValueObject<PaidVacationDaysValue>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="paidVacationDays">有給日数</param>
        /// <exception cref="ArgumentOutOfRangeException">上限値を超えた場合</exception>
        public PaidVacationDaysValue(double paidVacationDays)
        {
            if (paidVacationDays < PaidVacationDaysValue.Minimum)
            {
                throw new ArgumentOutOfRangeException("有給日数の下限値を下回っています。");
            }

            if (paidVacationDays > PaidVacationDaysValue.Maximum)
            {
                throw new ArgumentOutOfRangeException("有給日数の上限値を超えています。");
            }

            this.Value = paidVacationDays;
        }

        /// <summary> 上限値 </summary>
        private static readonly int Maximum = 40;

        /// <summary> 下限値 </summary>
        private static readonly int Minimum = 0;

        /// <summary> 有給日数 </summary>
        public readonly double Value;

        /// <summary>
        /// To String
        /// </summary>
        /// <returns>金額</returns>
        public override string ToString()
        {
            return ($"有給日数：{this.Value}日");
        }

        /// <summary>
        /// Equals Core
        /// </summary>
        /// <param name="other">有給日数</param>
        /// <returns>有給日数</returns>
        protected override bool EqualsCore(PaidVacationDaysValue other)
        {
            return (this.Value == other.Value);
        }
    }
}