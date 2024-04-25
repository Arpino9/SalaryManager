using System;
using System.Data.SQLite;
using SalaryManager.Domain.Repositories;

namespace SalaryManager.Infrastructure.SQLite
{
    /// <summary>
    /// SQLite - トランザクション版
    /// </summary>
    /// <remarks>
    /// SQLiteはTransactionScopeが使用できないため、こちらを利用する。
    /// </remarks>
    public class SQLiteTransaction : IDisposable, ITransactionRepository
    {
        /// <summary> 接続インスタンス </summary>
        private SQLiteConnection _connection;

        /// <summary> トランザクション </summary>
        private System.Data.SQLite.SQLiteTransaction _transaction;

        /// <summary> コマンド </summary>
        private SQLiteCommand _command;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public SQLiteTransaction()
        {
            _connection = new SQLiteConnection(SQLiteHelper.GetConnectionString());
            _connection.Open();

            _transaction = _connection.BeginTransaction();
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="update">UPDATE文</param>
        /// <param name="parameters">パラメーター</param>
        /// <exception cref="SQLiteException">更新に失敗した場合</exception>
        public void Execute(string update, SQLiteParameter[] parameters)
        {
            if (_connection is null)
            {
                return;
            }

            _command = new SQLiteCommand(update, _connection);
            _command.Parameters.AddRange(parameters);

            try
            {
                _command.ExecuteNonQuery();
            }
            catch (Exception ex) 
            {
                throw new SQLiteException("UPDATE文の実行に失敗しました。");
            }
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="insert">Insertt文</param>
        /// <param name="update">UPDATE文</param>
        /// <param name="parameters">パラメーター</param>
        /// <exception cref="SQLiteException">追加・更新に失敗した場合</exception>
        public void Execute(
        string insert,
        string update,
        SQLiteParameter[] parameters)
        {
            if (_connection is null)
            {
                return;
            }

            _command = new SQLiteCommand(update, _connection);
            _command.Parameters.AddRange(parameters);

            if (_command.ExecuteNonQuery() < 1)
            {
                _command.CommandText = insert;

                var result = _command.ExecuteNonQuery();
                if (result != 1)
                {
                    _transaction.Rollback();

                    throw new SQLiteException("データが更新できませんでした。");
                }
            }
        }

        /// <summary>
        /// コミット
        /// </summary>
        public void Commit() 
        {
            _transaction?.Commit();
            _command?.Dispose();
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            _transaction?.Dispose();

            _connection?.Close();
            _connection?.Dispose();
        }
    }
}
