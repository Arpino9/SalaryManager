using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Reflection;
using SalaryManager.Domain.Modules.Logics;

namespace SalaryManager.Infrastructure.SQLite
{
    public class SQLiteHelper
    {
        /// <summary> SQLiteのバージョン </summary>
        private static readonly int Version = 3;

        /// <summary>
        /// SQLiteの接続文字列を取得する
        /// </summary>
        /// <returns>接続文字列</returns>
        private static string GetConnectionString()
        {
            var projectName = Assembly.GetExecutingAssembly().GetName().Name;
            var sqlitePath  = $"{FilePath.GetSolutionPath()}\\{projectName}\\{FilePath.GetSolutionName()}.db";

            return $"Data Source={sqlitePath};Version={SQLiteHelper.Version};";
        }

        internal static IReadOnlyList<T> Query<T>(
            string sql,
            Func<SQLiteDataReader, T> createEntity)
        {
            var result = new List<T>();

            var projectName = Assembly.GetExecutingAssembly().GetName().Name;
            var sqlitePath = $"{FilePath.GetSolutionPath()}\\{projectName}\\{FilePath.GetSolutionName()}.db";
            
            using (var connection = new SQLiteConnection(SQLiteHelper.GetConnectionString()))
            {
                using (var command = new SQLiteCommand(sql, connection))
                {
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(createEntity(reader));
                        }
                    }
                }
            }

            return result;
        }

        internal static T QuerySingle<T>(
            string sql,
            Func<SQLiteDataReader, T> createEntity,
            T nullEntity)
        {
            return QuerySingle<T>(sql, null, createEntity, nullEntity);
        }

        internal static T QuerySingle<T>(
            string sql,
            SQLiteParameter[] parameters,
            Func<SQLiteDataReader, T> createEntity,
            T nullEntity)
        {
            var result = new List<T>();

            using (var connection = new SQLiteConnection(SQLiteHelper.GetConnectionString()))
            using (var command = new SQLiteCommand(sql, connection))
            {
                connection.Open();
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return createEntity(reader);
                    }
                }
            }

            return nullEntity;
        }

        internal static IReadOnlyList<T> QueryPlural<T>(
            string sql,
            SQLiteParameter[] parameters,
            Func<SQLiteDataReader, T> createEntity)
        {
            var result = new List<T>();

            using (var connection = new SQLiteConnection(SQLiteHelper.GetConnectionString()))
            using (var command = new SQLiteCommand(sql, connection))
            {
                connection.Open();
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(createEntity(reader));
                    }
                }
            }

            return result;
        }

        [Obsolete("Transaction版を使うこと。")]
        internal static void Execute(
        string sql,
        SQLiteParameter[] parameters
        )
        {
            using (var connection = new SQLiteConnection(SQLiteHelper.GetConnectionString()))
            using (var command = new SQLiteCommand(sql, connection))
            {
                connection.Open();
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                command.ExecuteNonQuery();
            }
        }

        internal static void Execute(
            string insert,
            string update,
            SQLiteParameter[] parameters)
        {
            using (var connection = new SQLiteConnection(SQLiteHelper.GetConnectionString()))
            using (var command = new SQLiteCommand(update, connection))
            {
                connection.Open();

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                if (command.ExecuteNonQuery() < 1)
                {
                    command.CommandText = insert;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
