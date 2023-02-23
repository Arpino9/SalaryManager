using MySql.Data.MySqlClient;

namespace SalaryManager
{
    internal class MySqlCommand
    {
        internal MySqlConnection Connection;
        private string sql;
        private MySqlConnection connection;

        public MySqlCommand(string sql, MySqlConnection connection)
        {
            this.sql = sql;
            this.connection = connection;
        }
    }
}