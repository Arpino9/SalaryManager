using System.Windows.Forms;
using SalaryManager.Domain;
using SalaryManager.Domain.Modules.Logics;
using SalaryManager.WPF.ViewModels;
using System.Drawing.Text;
using System.Linq;
using SalaryManager.Domain.Modules.Helpers;
using System.Drawing;
using System.Collections.Generic;
using SalaryManager.Infrastructure.XML;

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
            // Excelテンプレート
            this.ViewModel.SelectExcelTempletePath_Text = XMLLoader.FetchExcelTemplatePath();
            // SQLite
            this.ViewModel.SelectSQLite_Text            = XMLLoader.FetchSQLitePath();

            // フォントファミリ
            var fonts =  new InstalledFontCollection();
            this.ViewModel.FontFamily_ItemSource = ListUtils.ToObservableCollection<string>(fonts.Families.Select(x => x.Name).ToList());
            this.ViewModel.FontFamily_Text       = XMLLoader.FetchFontFamilyText();

            // 初期表示時にデフォルト明細を表示する
            this.ViewModel.ShowDefaultPayslip_IsChecked = XMLLoader.FetchShowDefaultPayslip();

            // フォント
            this.ViewModel.Preview_FontFamily     = XMLLoader.FetchFontFamily();

            // 背景色
            this.ViewModel.Window_BackgroundColor = XMLLoader.FetchBackgroundColor();
            this.ViewModel.Window_Background      = XMLLoader.FetchBackgroundColorBrush();
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
                this.ViewModel.Window_Background      = ColorUtils.ToWPFColor(dialog.Color);
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
            this.ViewModel.Window_Background      = ColorUtils.ToWPFColor(defaultColor);
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

            var tag = new XMLTag();

            using (var writer = new XMLWriter(FilePath.GetXMLDefaultPath(), tag.GetType()))
            {
                tag.SQLitePath                = this.ViewModel.SelectSQLite_Text;
                tag.ExcelTemplatePath         = this.ViewModel.SelectExcelTempletePath_Text;
                tag.FontFamily                = this.ViewModel.FontFamily_Text;
                tag.ShowDefaultPayslip        = this.ViewModel.ShowDefaultPayslip_IsChecked;
                tag.BackgroundColor_ColorCode = this.ViewModel.Window_BackgroundColor.Name;

                var list = new List<string>()
                {
                    this.ViewModel.Window_BackgroundColor.A.ToString(),
                    this.ViewModel.Window_BackgroundColor.R.ToString(),
                    this.ViewModel.Window_BackgroundColor.G.ToString(),
                    this.ViewModel.Window_BackgroundColor.B.ToString()
                };

                tag.BackgroundColor = StringUtils.Aggregate(list);

                writer.Serialize(tag);
            }
        }

        #endregion

    }
}
