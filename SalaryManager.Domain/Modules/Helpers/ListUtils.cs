namespace SalaryManager.Domain.Modules.Helpers;

/// <summary>
/// 拡張クラス - List
/// </summary>
public static class ListUtils
{
    /// <typeparam name="T">型パラメータ</typeparam>
    /// <param name="list">リスト</param>
    extension<T>(IEnumerable<T> list)
    {
        /// <summary>
        /// Observable Collectionに変換する
        /// </summary>
        /// <returns>ObservableCollection</returns>
        [Obsolete("一応残すが、ToReactiveCollection()メソッドを使うこと")]
        public ObservableCollection<T> ToObservableCollection()
            => new ObservableCollection<T>(list as List<T>);

        /// <summary>
        /// List → Reactive Collection
        /// </summary>
        /// <returns>Reactive Collection</returns>
        /// <remarks>
        /// 通常のList → ReactiveCollection変換。
        /// </remarks>
        public ReactiveCollection<T> ToReactiveCollection()
        {
            var reactiveCollection = new ReactiveCollection<T>();

            if (list.IsEmpty())
            {
                return new ReactiveCollection<T>();
            }

            foreach (var item in list)
            {
                reactiveCollection.Add(item);
            }

            return reactiveCollection;
        }

        /// <summary>
        /// コレクションが空かどうか調べる
        /// </summary>
        /// <returns>
        /// True : コレクションが空である / False: コレクションが空でない
        /// </returns>
        public bool IsEmpty()
            => (list.Any() == false);
    }
}
