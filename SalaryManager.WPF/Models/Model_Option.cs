using System.Windows.Forms;
using SalaryManager.Domain;
using SalaryManager.Domain.Modules.Logics;
using SalaryManager.Domain.StaticValues;
using SalaryManager.WPF.ViewModels;
using System.Drawing.Text;
using System.Linq;
using SalaryManager.Domain.Modules.Helpers;
using System.Drawing;

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
            
            // Excelテンプレート
            this.ViewModel.SelectExcelTempletePath_Text = Options_General.FetchExcelTemplatePath();
            // SQLite
            this.ViewModel.SelectSQLite_Text            = Options_General.FetchSQLitePath();

            // フォントファミリ
            var fonts =  new InstalledFontCollection();
            this.ViewModel.FontFamily_ItemSource = ListUtil.ToObservableCollection<string>(fonts.Families.Select(x => x.Name).ToList());
            this.ViewModel.FontFamily_Text       = Options_General.FetchFontFamilyText();

            // 初期表示時にデフォルト明細を表示する
            this.ViewModel.ShowDefaultPayslip_IsChecked = Options_General.FetchShowDefaultPayslip();

            // フォント
            this.ViewModel.Preview_FontFamily     = Options_General.FetchFontFamily();

            // 背景色
            this.ViewModel.Window_BackgroundColor = Options_General.FetchBackgroundColor();
            this.ViewModel.Window_Background      = Options_General.FetchBackgroundColorBrush();
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
            dialog.Title  = "SQLiteデータベースを指定してください";

            var result = dialog.ShowDialog();

            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.ViewModel.SelectSQLite_Text = dialog.FileName;
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
            var dialog = new OpenFileDialog();
            dialog.Filter = "Excelファイル(*.xlsx)|*.xlsx|全てのファイル(*.*)|*.*";
            dialog.Title  = "Excelのテンプレートを指定してください";

            var result = dialog.ShowDialog();

            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.ViewModel.SelectExcelTempletePath_Text = dialog.FileName;
        }

        /// <summary>
        /// Excelテンプレートパス - 初期値に戻す
        /// </summary>
        internal void SetDefault_SelectExcelTemplatePath()
        {
            this.ViewModel.SelectExcelTempletePath_Text = FilePath.GetExcelTempleteDefaultPath();
        }

        #endregion

        #region フォントファミリ

        /// <summary>
        /// フォントファミリ - SelectionChanged
        /// </summary>
        internal void FontFamily_SelectionChanged()
        {
            this.ViewModel.Preview_FontFamily = new System.Windows.Media.FontFamily(this.ViewModel.FontFamily_Text);
        }

        /// <summary>
        /// フォントファミリ - 初期値に戻す
        /// </summary>
        internal void SetDefault_FontFamily()
        {
            this.ViewModel.FontFamily_Text    = Shared.FontFamily;
            this.ViewModel.Preview_FontFamily = new System.Windows.Media.FontFamily(Shared.FontFamily);
        }

        #endregion

        #region 背景色

        /// <summary>
        /// 背景色 - 色を選択
        /// </summary>
        internal void ChangeWindowBackground()
        {
            var dialog = new ColorDialog();
            var result = dialog.ShowDialog(); 

            if (result == DialogResult.OK) 
            {
                this.ViewModel.Window_Background      = ColorUtil.ToWPFColor(dialog.Color);
                this.ViewModel.Window_BackgroundColor = dialog.Color;
            }
        }

        /// <summary>
        /// 背景色 - デフォルトに戻す
        /// </summary>
        internal void SetDefault_WindowBackground()
        {
            var defaultColor = SystemColors.ControlLight;

            this.ViewModel.Window_BackgroundColor = defaultColor;
            this.ViewModel.Window_Background      = ColorUtil.ToWPFColor(defaultColor);
        }

        #endregion

        #region 保存

        /// <summary>
        /// 保存
        /// </summary>
        internal void Save()
        {
            var message = $"設定内容を保存しますか？";
            if (!DialogMessage.ShowConfirmingMessage(message, "保存"))
            {
                // キャンセル
                return;
            }

            var setting = new Settings();

            using (var writer = new XMLWriter(FilePath.GetXMLDefaultPath(), setting.GetType()))
            {
                setting.SQLitePath         = this.ViewModel.SelectSQLite_Text;
                setting.ExcelTemplatePath  = this.ViewModel.SelectExcelTempletePath_Text;
                setting.FontFamily         = this.ViewModel.FontFamily_Text;
                setting.ShowDefaultPayslip = this.ViewModel.ShowDefaultPayslip_IsChecked;
                setting.BackgroundColor    = this.ViewModel.Window_BackgroundColor.Name;
                setting.BackgroundColor_A  = this.ViewModel.Window_BackgroundColor.A.ToString();
                setting.BackgroundColor_R  = this.ViewModel.Window_BackgroundColor.R.ToString();
                setting.BackgroundColor_G  = this.ViewModel.Window_BackgroundColor.G.ToString();
                setting.BackgroundColor_B  = this.ViewModel.Window_BackgroundColor.B.ToString();

                writer.Serialize(setting);
            }
        }

        #endregion

    }
}
