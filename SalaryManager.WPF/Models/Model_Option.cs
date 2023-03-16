using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.WPF.ViewModels;

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

        /// <summary> ViewModel - 全般設定 </summary>
        internal ViewModel_GeneralOption ViewModel { get; set; }

        internal void SelectExcelTemplatePath()
        {
            var path = DirectorySelector.Select("Excelのテンプレートが格納されているフォルダを指定してください。");
            this.ViewModel.ExcelTemplatePath_Text = path;
        }

        #endregion

    }
}
