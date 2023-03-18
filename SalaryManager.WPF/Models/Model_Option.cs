using SalaryManager.Domain;
using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.Domain.Modules.Logics;
using SalaryManager.Domain.StaticValues;
using SalaryManager.WPF.ViewModels;
using System.Windows.Forms;

namespace SalaryManager.WPF.Models
{
    public class Model_Option
    {

        #region Get Instance

        private static Model_Option model = null;

        public static Model_Option GetInstance()
        {
            if (model == null)
            {
                model = new Model_Option();
            }

            return model;
        }

        public Model_Option()
        {
            
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <remarks>
        /// 値があればXMLから、なければconfigから取得する。
        /// </remarks>
        internal void Initialize()
        {
            Options_General.Create();

            this.ViewModel.SelectExcelTempletePath_Text = Options_General.FetchExcelTemplatePath();
            this.ViewModel.SelectSQLite_Text            = Options_General.FetchSQLitePath();
        }

        #endregion

        /// <summary> ViewModel - 全般設定 </summary>
        internal ViewModel_GeneralOption ViewModel { get; set; }

        #region SQLite

        /// <summary>
        /// SQLite - 開く
        /// </summary>
        /// <remarks>
        /// 任意のディレクトリに配置されたSQLite.dbを選択させる。
        /// ただし、ファイル名はソリューション名と同じとする。
        /// </remarks>
        internal void SelectSQLitePath()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "SQLiteファイル(*.db)|*.db|全てのファイル(*.*)|*.*";

            var result = dialog.ShowDialog();

            if (dialog.SafeFileName != $"{FilePath.GetSolutionName()}.db")
            {
                DialogMessage.ShowErrorMessage("ファイル名が不正です。", "SQLiteファイル選択");
                return;
            }

            if (result == DialogResult.OK)
            {
                this.ViewModel.SelectSQLite_Text = dialog.FileName;
            }
        }

        /// <summary>
        /// SQLite - 初期値に戻す
        /// </summary>
        internal void SetDefault_SelectSQLitePath()
        {
            this.ViewModel.SelectSQLite_Text = FilePath.GetSQLiteDefaultPath();
        }

        #endregion

        #region Excelテンプレート

        /// <summary>
        /// Excelテンプレートパス - 開く
        /// </summary>
        internal void SelectExcelTemplatePath()
        {
            var path = DirectorySelector.Select("Excelのテンプレートが格納されているフォルダを指定してください。");
            this.ViewModel.SelectExcelTempletePath_Text = path;
        }

        /// <summary>
        /// Excelテンプレートパス - 初期値に戻す
        /// </summary>
        internal void SetDefault_SelectExcelTemplatePath()
        {
            this.ViewModel.SelectExcelTempletePath_Text = Shared.PathOutputPayslip;
        }

        #endregion

        #region 保存

        /// <summary>
        /// 保存
        /// </summary>
        internal void Save()
        {
            var setting = new Settings();

            using (var writer = new XMLWriter(Shared.XMLPath, setting.GetType()))
            {
                setting.SQLitePath        = this.ViewModel.SelectSQLite_Text;
                setting.ExcelTemplatePath = this.ViewModel.SelectExcelTempletePath_Text;

                writer.Serialize(setting);
            }
        }

        #endregion

    }
}
