using System;

namespace SalaryManager.Domain.Exceptions
{
    /// <summary>
    /// ユーザ定義例外
    /// </summary>
    public class ExceptionBase : Exception
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">メッセージ</param>
        public ExceptionBase(string message) : base(message)
        {
   
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="kind">例外区分</param>
        public ExceptionBase(string message, ExceptionKind kind)
        {

            //TODO: ログ出力
#if Debug

            

#endif

        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="exception">元になった例外</param>
        public ExceptionBase(string message, Exception exception)
            : base(message, exception)
        {

        }

        /// <summary>
        /// 例外区分
        /// </summary>
        public enum ExceptionKind
        {
            Info,
            Warning,
            Error,
        }
    }
}
