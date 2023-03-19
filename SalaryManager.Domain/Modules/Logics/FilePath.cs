using System.Reflection;

namespace SalaryManager.Domain.Modules.Logics
{
    /// <summary>
    /// ファイルパス
    /// </summary>
    public class FilePath
    {
        #region ソリューション

        /// <summary>
        /// ソリューションのパスを取得する
        /// </summary>
        /// <returns>ソリューションのパス</returns>
        public static string GetSolutionPath()
        {
            var appPath      = FilePath.GetAppPath();
            var solutionName = FilePath.GetSolutionName();
            var solutionPath = appPath.Substring(0, appPath.IndexOf(solutionName));

            return $"{solutionPath}{solutionName}";
        }

        /// <summary>
        /// ソリューション名を取得する
        /// </summary>
        /// <returns>ソリューション名</returns>
        public static string GetSolutionName()
        {
            var projectName = FilePath.GetProjectName();

            return (projectName.Substring(0, projectName.IndexOf(".")));
        }

        #endregion

        #region プロジェクト

        /// <summary>
        /// プロジェクト名を取得する
        /// </summary>
        /// <returns>プロジェクト名</returns>
        /// <remarks>
        /// このクラスが配置されているプロジェクト名であることに注意。
        /// </remarks>
        public static string GetProjectName()
        {
            return (Assembly.GetExecutingAssembly().GetName().Name);
        }

        #endregion

        #region exe

        /// <summary>
        /// exeのパスを取得する。
        /// </summary>
        /// <returns>exeファイルのパス</returns>
        public static string GetAppPath()
        {
            return (Assembly.GetExecutingAssembly().Location);
        }

        /// <summary>
        /// exeのフォルダパスを取得する。
        /// </summary>
        /// <returns>exeファイルのパス</returns>
        public static string GetAppFolderPath()
        {
            var exePath = Assembly.GetExecutingAssembly().Location;
            return (exePath.Substring(0, exePath.LastIndexOf(FilePath.GetSolutionName())));
        }

        #endregion

        /// <summary>
        /// SQLiteの初期パスを取得する
        /// </summary>
        /// <returns>SQLiteの初期パス</returns>
        public static string GetSQLiteDefaultPath()
        {
            var solutionName = FilePath.GetSolutionName();
            return $"{FilePath.GetSolutionPath()}\\{solutionName}.Infrastructure\\{solutionName}.db";
        }
    }
}
