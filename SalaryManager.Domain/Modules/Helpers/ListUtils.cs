using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SalaryManager.Domain.Modules.Helpers
{
    /// <summary>
    /// List拡張クラス
    /// </summary>
    public static class ListUtils
    {
        /// <summary>
        /// Observable Collectionに変換する
        /// </summary>
        /// <param name="dictionary">ディクショナリ</param>
        /// <returns>ObservableCollection</returns>
        public static ObservableCollection<KeyValuePair<TKey, TValue>> ToObservableCollection<TKey, TValue>(this ObservableCollection<KeyValuePair<TKey, TValue>> dictionary)
            => new ObservableCollection<KeyValuePair<TKey, TValue>>(dictionary);

        /// <summary>
        /// Observable Collectionに変換する
        /// </summary>
        /// <typeparam name="T">型パラメータ</typeparam>
        /// <param name="list">リスト</param>
        /// <returns>ObservableCollection</returns>
        public static ObservableCollection<T> ToObservableCollection<T>(ICollection<T> list)
            => new ObservableCollection<T>(list as List<T>);
    }
}
