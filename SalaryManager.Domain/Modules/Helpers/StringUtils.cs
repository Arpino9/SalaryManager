using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryManager.Domain.Modules.Helpers
{
    public static class StringUtils
    {
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
                if (list.Last() == item)
                {
                    return str += item;
                }

                str += item + ",";
            }

            return string.Empty;
        }

        /// <summary>
        /// 区切り文字ごとにリスト化する
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<string> Separate(string str)
        {
            return str.Split(',').ToList();
        }
    }
}
