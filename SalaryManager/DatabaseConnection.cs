/// <summary>
/// Salary Manager
/// </summary>

namespace SalaryManager
{
    /// <summary>
    /// データベース接続
    /// </summary>
    public class DatabaseConnection :
        System.IDisposable
    {

		#region Constructor

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="dataSource">Network Address</param>
		/// <param name="initialCatalog">データベース名</param>
		/// <param name="userID">ユーザID</param>
		/// <param name="password">パスワード</param>
		/// <param name="connectTimeout">接続タイムアウト [秒]</param>
		public DatabaseConnection(
			// Network Address
			string dataSource,
			// データベース名
			string initialCatalog,
			// ユーザID
			string userID,
			// パスワード
			string password,
			// 接続タイムアウト [秒]
			int connectTimeout
		)
		{
			// 接続文字列
			var builder =
				new System.Data.SqlClient.SqlConnectionStringBuilder()
				{
					// Network Address
					DataSource = dataSource,
					// データベース名
					InitialCatalog = initialCatalog,
					// ユーザID
					UserID = userID,
					// パスワード
					Password = password,
					// 接続タイムアウト [秒]
					ConnectTimeout = connectTimeout,
					// 複数のアクティブな結果セット = 有効
					MultipleActiveResultSets = true,
				};

			try
			{
				// Connection
				this.Connection =
					new System.Data.SqlClient.SqlConnection(
						// 接続文字列
						builder.ConnectionString
					);
			}
			catch (System.Data.SqlClient.SqlException)
			{
				throw;
			}
		}

		#endregion

		#region Connection

		/// <summary>
		/// Connection
		/// </summary>
		private readonly System.Data.SqlClient.SqlConnection Connection;

		#endregion

		#region Close

		/// <summary>
		/// Close
		/// </summary>
		public void Close()
        {
            Connection?.Close();
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            this.Close();
        }

        #endregion

    }
}
