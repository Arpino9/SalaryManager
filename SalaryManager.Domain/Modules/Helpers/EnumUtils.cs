using System;

namespace SalaryManager.Domain.Modules.Helpers
{
    /// <summary>
    /// Utility - 列挙型
    /// </summary>
    public sealed class EnumUtils
    {
        /// <summary>
        /// 列挙型に変換
        /// </summary>
        /// <param name="type">型</param>
        /// <param name="str">変換元の文字列</param>
        /// <returns>変換後の列挙型</returns>
        public static object ToEnum(Type type, string str)
        {
            if (str is null)
            {
                return null;
            }

            if (Enum.IsDefined(type, str) == false)
            {
                return null;
            }

            return Enum.Parse(type, str);
        }
    }
}
