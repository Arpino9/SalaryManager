using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SalaryManager.Domain.Modules.Helpers
{
    public static class ListUtils
    {
        /// <summary>
        /// Observable Collectionに変換する
        /// </summary>
        /// <typeparam name="T">型パラメータ</typeparam>
        /// <param name="list">リスト</param>
        /// <returns>ObservableCollection</returns>
        public static ObservableCollection<T> ToObservableCollection<T>(IList<T> list)
        {
            return new ObservableCollection<T>(list as List<T>);
        }

        /// <summary>
        /// Observable Collectionに変換する
        /// </summary>
        /// <typeparam name="T">型パラメータ</typeparam>
        /// <param name="list">リスト</param>
        /// <returns>ObservableCollection</returns>
        public static ObservableCollection<T> ToObservableCollection<T>(List<T> list)
        {
            return new ObservableCollection<T>(list as List<T>);
        }
    }
}
