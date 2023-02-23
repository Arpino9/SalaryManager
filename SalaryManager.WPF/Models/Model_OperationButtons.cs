using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.ViewModels;

namespace SalaryManager.WPF.Models
{
    /// <summary>
    /// Model - 操作ボタン
    /// </summary>
    public class Model_OperationButtons
    {
        private static Model_OperationButtons model = null;

        public static Model_OperationButtons GetInstance()
        {
            if (model == null)
            {
                model = new Model_OperationButtons();
            }

            return model;
        }

        /// <summary> ViewModel - ヘッダ </summary>
        public ViewModel_Header Header { get; set; }

        #region デフォルトに設定

        /// <summary>
        /// Set Default
        /// </summary>
        internal void SetDefault()
        {
            // 前回のデフォルト設定を解除する
            var header = new HeaderSQLite();
            var defaultEntity = header.GetDefaultEntity();

            if (defaultEntity != null)
            {
                defaultEntity.IsDefault = false;
                header.Save(defaultEntity);
            }

            // 今回のデフォルト設定を登録する
            this.Header.IsDefault = true;
        }

        #endregion
    }
}
