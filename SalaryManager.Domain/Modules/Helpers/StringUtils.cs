namespace SalaryManager.Domain.Modules.Helpers;

/// <summary>
/// 拡張クラス - String
/// </summary>
public static class StringUtils
{
    /// <summary> 区切り文字 </summary>
    private static readonly char Delimiter = ',';

    /// <param name="list">リスト</param>
    extension(List<string> list)
    {
        /// <summary>
        /// 区切り文字をつける
        /// </summary>
        /// <returns>区切り文字付きの文字列</returns>
        public string Combine()
        {
            var str = string.Empty;
            foreach (var item in list)
            {
                str += item + StringUtils.Delimiter;
            }

            // 末尾の「,」は除外
            return str.Substring(0, str.Length - 1);
        }
    }

    /// <param name="str">文字列</param>
    extension(string str)
    {
        /// <summary>
        /// 区切り文字ごとにリスト化する
        /// </summary>
        /// <returns></returns>
        public List<string> Separate()
            => str.Split(StringUtils.Delimiter).ToList();
    }
}
