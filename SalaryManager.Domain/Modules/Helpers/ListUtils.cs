using Reactive.Bindings;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;

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
        /// <typeparam name="T">型パラメータ</typeparam>
        /// <param name="list">リスト</param>
        /// <returns>ObservableCollection</returns>
        public static ObservableCollection<T> ToObservableCollection<T>(ICollection<T> list)
            => new ObservableCollection<T>(list as List<T>);

        /// <summary>
        /// List → Reactive Collection
        /// </summary>
        /// <typeparam name="T">型パラメータ</typeparam>
        /// <param name="list">コレクション</param>
        /// <param name="reactiveCollection">Reactive Collection</param>
        /// <returns>Reactive Collection</returns>
        /// <remarks>
        /// 別のコレクションのSelectedItemなどでReactiveCollectionの中身を入れ替える場合、
        /// こちらのメソッドで引数に更新するReactiveCollectionを指定する。
        /// ※引数なしの方はViewへの通知がされないため
        /// </remarks>
        public static ReactiveCollection<T> ToReactiveCollection<T>(
            this IEnumerable<T> list, 
            ReactiveCollection<T> reactiveCollection)
        {
            if (list.Any() == false) return new ReactiveCollection<T>();

            reactiveCollection.Clear();

            foreach (var item in list)
            {
                reactiveCollection.Add(item);
            }

            return reactiveCollection;
        }

        /// <summary>
        /// List → Reactive Collection
        /// </summary>
        /// <typeparam name="T">型パラメータ</typeparam>
        /// <param name="list">コレクション</param>
        /// <returns>Reactive Collection</returns>
        /// <remarks>
        /// 通常のList → ReactiveCollection変換。
        /// </remarks>
        public static ReactiveCollection<T> ToReactiveCollection<T>(this IEnumerable<T> list)
        {
            var reactiveCollection = new ReactiveCollection<T>();

            if (list.Any() == false) return new ReactiveCollection<T>();

            foreach (var item in list)
            {
                reactiveCollection.Add(item);
            }

            return reactiveCollection;
        }
    }
}
