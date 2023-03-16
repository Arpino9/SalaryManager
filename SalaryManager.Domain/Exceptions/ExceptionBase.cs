using System;
using System.Windows;

namespace SalaryManager.Domain.Exceptions
{
    /// <summary>
    /// ユーザ定義例外
    /// </summary>
    /// <remarks>
    /// デバッグ時のみメッセージ出力する。メッセージ表示後にログ書き出しを行う。
    /// </remarks>
    public abstract class ExceptionBase : Exception
    {
        private static readonly log4net.ILog _logger =
          log4net.LogManager.GetLogger(
              System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <remarks>
        /// 通常の例外
        /// </remarks>
        public ExceptionBase(string message) : 
            base(message)
        {

#if DEBUG

            MessageBox.Show(message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
             _logger.Error(message);
#endif

        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="kind">ログ区分</param>
        /// <remarks>
        /// ログ書き出しバージョン
        /// </remarks>
        public ExceptionBase(string message,  WriteLogKind kind)
        {

#if DEBUG

            switch (kind)
            {
                case WriteLogKind.Warn:
                    MessageBox.Show(message, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                    _logger.Warn(message); break;

                case WriteLogKind.Error:
                    MessageBox.Show(message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                    _logger.Error(message); break;

                case WriteLogKind.Fatal:
                    MessageBox.Show(message, "ヤバいエラー", MessageBoxButton.OK, MessageBoxImage.Error);
                    _logger.Fatal(message); break;
            }

#endif

        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="exception">元になった例外</param>
        /// <remarks>
        /// 内部例外ありバージョン
        /// </remarks>
        public ExceptionBase(string message, Exception exception)
            : base(message, exception)
        {

#if DEBUG

            MessageBox.Show(exception.ToString(), message, MessageBoxButton.OK);
            
            _logger.Error(message.ToString());

#endif

        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="exception">元になった例外</param>
        /// <remarks>
        /// 内部例外あり＆ログ書き出しバージョン
        /// </remarks>
        public ExceptionBase(string message, Exception exception, WriteLogKind kind)
            : base(message, exception)
        {

#if DEBUG

            switch (kind)
            {
                case WriteLogKind.Warn:
                    MessageBox.Show(exception.ToString(), message, MessageBoxButton.OK, MessageBoxImage.Warning);
                    _logger.Warn(message, exception); break;

                case WriteLogKind.Error:
                    MessageBox.Show(exception.ToString(), message, MessageBoxButton.OK, MessageBoxImage.Error);
                    _logger.Error(message, exception); break;

                case WriteLogKind.Fatal:
                    MessageBox.Show(exception.ToString(), message, MessageBoxButton.OK, MessageBoxImage.Error);
                    _logger.Fatal(message, exception); break;
            }

#endif

        }

        /// <summary>
        /// ログ区分
        /// </summary>
        public enum WriteLogKind
        {
            /// <summary> 警告 </summary>
            /// <remarks> 入力エラーなど </remarks>
            Warn,
            /// <summary> エラー </summary>
            /// <remarks> SQL接続エラーなど </remarks>
            Error,
            /// <summary> ヤバいエラー </summary>
            /// <remarks> システムの信頼性を脅かすエラー </remarks>
            Fatal
        }
    }
}
