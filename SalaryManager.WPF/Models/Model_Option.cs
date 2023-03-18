using System;
using SalaryManager.Domain;
using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.Domain.Modules.Logics;
using SalaryManager.Domain.StaticValues;
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

        /// <summary>
        /// 初期化
        /// </summary>
        internal void Initialize()
        {
            Options_General.Create();

            this.ViewModel.ExcelTemplatePath_Text = Options_General.FetchExcelTemplatePath();
        }

        #endregion

        #region Excelテンプレートパスの指定

        /// <summary> ViewModel - 全般設定 </summary>
        internal ViewModel_GeneralOption ViewModel { get; set; }

        internal void SelectExcelTemplatePath()
        {
            var path = DirectorySelector.Select("Excelのテンプレートが格納されているフォルダを指定してください。");
            this.ViewModel.ExcelTemplatePath_Text = path;
        }

        internal void SetDefault_SelectExcelTemplatePath()
        {
            this.ViewModel.ExcelTemplatePath_Text = Shared.PathOutputPayslip;
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
                setting.ExcelTemplatePath = this.ViewModel.ExcelTemplatePath_Text;

                writer.Serialize(setting);
            }
        }

        #endregion

    }
}
