namespace SalaryManager.Domain.ValueObjects
{
    /// <summary>
    /// Value Object
    /// </summary>
    /// <typeparam name="T">型パラメーター</typeparam>
    public abstract class ValueObject<T> where T : ValueObject<T>
    {
        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj">オブジェクト</param>
        /// <returns>判定結果</returns>
        public override bool Equals(object obj)
        {
            var vo = obj as T;
            if (vo == null)
            {
                return false;
            }

            return EqualsCore(vo);
        }

        /// <summary>
        /// Operator
        /// </summary>
        /// <param name="vo1">ValueObject1</param>
        /// <param name="vo2">ValueObject2</param>
        /// <returns>比較結果</returns>
        public static bool operator ==(ValueObject<T> vo1,
            ValueObject<T> vo2)
        {
            return Equals(vo1, vo2);
        }

        /// <summary>
        /// Operator
        /// </summary>
        /// <param name="vo1">ValueObject1</param>
        /// <param name="vo2">ValueObject2</param>
        /// <returns>比較結果</returns>
        public static bool operator !=(ValueObject<T> vo1,
           ValueObject<T> vo2)
        {
            return !Equals(vo1, vo2);
        }

        /// <summary>
        /// Equals Core
        /// </summary>
        /// <param name="other"></param>
        /// <returns>比較結果</returns>
        protected abstract bool EqualsCore(T other);

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns>文字列</returns>
        public override string ToString()
        {
            return base.ToString();
        }

        /// <summary>
        /// Get Hash Code
        /// </summary>
        /// <returns>ハッシュ値</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
