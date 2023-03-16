using System;

namespace SalaryManager.Domain.Exceptions
{
    /// <summary>
    /// ユーザ定義例外 - データベース接続
    /// </summary>
    public sealed class DatabaseException : ExceptionBase
    {
        public DatabaseException(string message, WriteLogKind kind) :
            base(message, kind)
        {

        }

        public DatabaseException(string message, Exception ex, WriteLogKind kind) : 
            base(message, ex, kind)
        {

        }
    }
}
