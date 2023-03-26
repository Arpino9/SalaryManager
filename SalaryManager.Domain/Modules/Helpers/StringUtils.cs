using System.Linq;
using System.Collections.Generic;

namespace SalaryManager.Domain.Modules.Helpers
{
    public static class StringUtils
    {
        /// <summary> 区切り文字 </summary>
        private static readonly char Separator = ',';

        /// <summary>
        /// 区切り文字をつける
        /// </summary>
        /// <param name="list">リスト</param>
        /// <returns>区切り文字付きの文字列</returns>
        public static string Aggregate(List<string> list)
        {
            var str = string.Empty;
            foreach(var item in list) 
            {
                str += item + StringUtils.Separator;
            }

            // 末尾の「,」は除外
            return str.Substring(0, str.Length - 1);
        }

        /// <summary>
        /// 区切り文字ごとにリスト化する
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<string> Separate(string str)
        {
            return str.Split(StringUtils.Separator).ToList();
        }

        /// <summary>
        /// ファイル名を抽出する
        /// </summary>
        /// <param name="path"パス></param>
        /// <returns>ファイル名</returns>
        public static string ExtractFileName(string path)
        {
            var startIndex = path.LastIndexOf("\\");

            return path.Substring(startIndex + "\\".Length, path.Length - startIndex - "\\".Length);
        }
    }
}
