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
using static SalaryManager.WPF.ViewModels.ViewModel_GeneralOption;

namespace SalaryManager.WPF.Models
{
    /// <summary>
    /// Model - オプション
    /// </summary>
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

        #endregion

        public Model_Option()
        {

        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <remarks>
        /// 値があればXMLから、なければconfigから取得する。
        /// </remarks>
        internal void Initialize_General()
        {
            // Excelテンプレート
            this.GeneralOption.SelectExcelTempletePath_Text = XMLLoader.FetchExcelTemplatePath();
            // SQLite
            this.GeneralOption.SelectSQLite_Text            = XMLLoader.FetchSQLitePath();

            // フォントファミリ
            var fonts =  new InstalledFontCollection();
            this.GeneralOption.FontFamily_ItemSource = ListUtils.ToObservableCollection<string>(fonts.Families.Select(x => x.Name).ToList());
            this.GeneralOption.FontFamily_Text       = XMLLoader.FetchFontFamilyText();

            // 初期表示時にデフォルト明細を表示する
            this.GeneralOption.ShowDefaultPayslip_IsChecked = XMLLoader.FetchShowDefaultPayslip();

            // フォント
            this.GeneralOption.Preview_FontFamily     = XMLLoader.FetchFontFamily();
            this.GeneralOption.FontSize_Value         = XMLLoader.FetchFontSize();

            var obj = EnumUtils.ToEnum(this.GeneralOption.HowToSaveImage_IsChecked.GetType(), XMLLoader.FetchHowToSaveImage());
            if (obj != null)
            {
                this.GeneralOption.HowToSaveImage_IsChecked = (HowToSaveImage)obj;
            }

            this.GeneralOption.ImageFolderPath_Text   = XMLLoader.FetchImageFolder();
            this.GeneralOption.SelectFolder_IsEnabled = this.GeneralOption.HowToSaveImage_IsChecked == HowToSaveImage.SavePath;

            // 背景色
            this.GeneralOption.Window_BackgroundColor = XMLLoader.FetchBackgroundColor();
            this.GeneralOption.Window_Background      = XMLLoader.FetchBackgroundColorBrush();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <remarks>
        /// 値があればXMLから、なければconfigから取得する。
        /// </remarks>
        internal void Initialize_SpreadSheet()
        {
            this.SpreadSheetOption.SelectPrivateKey_Text = XMLLoader.FetchPrivateKeyPath_SpreadSheet();
            this.SpreadSheetOption.SheetId_Text          = XMLLoader.FetchSheetId();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <remarks>
        /// 値があればXMLから、なければconfigから取得する。
        /// </remarks>
        internal void Initialize_PDF()
        {
            this.PDFOption.Password_Text = XMLLoader.FetchPDFPassword();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <remarks>
        /// 値があればXMLから、なければconfigから取得する。
        /// </remarks>
        internal void Initialize_Calendar()
        {
            this.CalendarOption.SelectPrivateKey_Text = XMLLoader.FetchPrivateKeyPath_Calendar();
            this.CalendarOption.SelectCalendarID_Text = XMLLoader.FetchCalendarId();
        }

        /// <summary> ViewModel - 全般設定 </summary>
        internal ViewModel_GeneralOption GeneralOption { get; set; }

        /// <summary> ViewModel - スプレッドシート設定 </summary>
        internal ViewModel_SpreadSheetOption SpreadSheetOption { get; set; }

        /// <summary> ViewModel - PDF設定 </summary>
        internal ViewModel_PDFOption PDFOption { get; set; }

        /// <summary> ViewModel - Googleカレンダー </summary>
        internal ViewModel_CalendarOption CalendarOption { get; set; }

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

            this.GeneralOption.SelectSQLite_Text = dialog.FileName;
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

            this.GeneralOption.SelectExcelTempletePath_Text = dialog.FileName;
        }

        #endregion

        #region 認証ファイル

        /// <summary>
        /// 認証ファイル(SpreadSheet) - 開く
        /// </summary>
        internal void SelectPrivateKeyPath_SpreadSheet()
        {
            this.SpreadSheetOption.SelectPrivateKey_Text = DialogUtils.SelectFile(string.Empty, "JSONファイル(*.json)|*.json");
        }

        /// <summary>
        /// 認証ファイル(Googleカレンダー) - 開く
        /// </summary>
        internal void SelectPrivateKeyPath_Calendar()
        {
            this.CalendarOption.SelectPrivateKey_Text = DialogUtils.SelectFile(string.Empty, "JSONファイル(*.json)|*.json");
        }

        #endregion

        #region フォントファミリ

        /// <summary>
        /// フォントファミリ - SelectionChanged
        /// </summary>
        internal void FontFamily_SelectionChanged()
        {
            this.GeneralOption.Preview_FontFamily = new System.Windows.Media.FontFamily(this.GeneralOption.FontFamily_Text);
        }

        #endregion

        /// <summary>
        /// フォントファミリ - SelectionChanged
        /// </summary>
        internal void HowToSaveImage_SelectionChanged()
        {
            this.GeneralOption.SelectFolder_IsEnabled = this.GeneralOption.HowToSaveImage_IsChecked == HowToSaveImage.SavePath;
        }

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
                this.GeneralOption.Window_Background      = ColorUtils.ToWPFColor(dialog.Color);
                this.GeneralOption.Window_BackgroundColor = dialog.Color;
            }
        }

        #endregion

        #region 保存

        /// <summary>
        /// 保存
        /// </summary>
        internal void Save()
        {
            if (!Domain.Modules.Logics.Message.ShowConfirmingMessage("設定内容を保存しますか？", "保存"))
            {
                // キャンセル
                return;
            }

            var tag = new XMLTag();

            using (var writer = new XMLWriter(FilePath.GetXMLDefaultPath(), tag.GetType()))
            {
                tag.SQLitePath                = this.GeneralOption.SelectSQLite_Text;
                tag.ExcelTemplatePath         = this.GeneralOption.SelectExcelTempletePath_Text;
                tag.FontFamily                = this.GeneralOption.FontFamily_Text;
                tag.FontSize                  = this.GeneralOption.FontSize_Value;
                tag.ShowDefaultPayslip        = this.GeneralOption.ShowDefaultPayslip_IsChecked;
                tag.BackgroundColor_ColorCode = this.GeneralOption.Window_BackgroundColor.Name;
                tag.ImageFolderPath           = this.GeneralOption.ImageFolderPath_Text;

                var list = new List<string>()
                {
                    this.GeneralOption.Window_BackgroundColor.A.ToString(),
                    this.GeneralOption.Window_BackgroundColor.R.ToString(),
                    this.GeneralOption.Window_BackgroundColor.G.ToString(),
                    this.GeneralOption.Window_BackgroundColor.B.ToString()
                };

                tag.HowToSaveImage = this.GeneralOption.HowToSaveImage_IsChecked.ToString();

                tag.BackgroundColor = StringUtils.Aggregate(list);

                tag.PrivateKeyPath_SpreadSheet = this.SpreadSheetOption.SelectPrivateKey_Text;
                tag.SheetId                    = this.SpreadSheetOption.SheetId_Text;

                tag.PDFPassword = this.PDFOption.Password_Text;

                tag.PrivateKeyPath_Calendar = this.CalendarOption.SelectPrivateKey_Text;
                tag.CalendarId              = this.CalendarOption.SelectCalendarID_Text;

                writer.Serialize(tag);
            }
        }

        /// <summary>
        /// フォルダを開く
        /// </summary>
        internal void OpenFolder()
        {
            var directory = DialogUtils.SelectDirectory("取得元のフォルダを選択してください。");

            if (string.IsNullOrEmpty(directory))
            {
                return;
            }

            this.GeneralOption.ImageFolderPath_Text = directory;
        }

        #endregion

        #region 初期値に戻す

        /// <summary>
        /// 初期値に戻す
        /// </summary>
        internal void SetDefault()
        {
            // SQLite
            this.GeneralOption.SelectSQLite_Text = FilePath.GetSQLiteDefaultPath();

            // Excelテンプレートパス
            this.GeneralOption.SelectExcelTempletePath_Text = FilePath.GetExcelTempleteDefaultPath();

            // フォントファミリ
            this.GeneralOption.FontFamily_Text    = Shared.FontFamily;
            this.GeneralOption.Preview_FontFamily = new System.Windows.Media.FontFamily(Shared.FontFamily);

            // フォントサイズ
            this.GeneralOption.FontSize_Value = decimal.Parse(Shared.FontSize);

            // 背景色
            this.GeneralOption.Window_BackgroundColor = SystemColors.ControlLight;
            this.GeneralOption.Window_Background      = ColorUtils.ToWPFColor(SystemColors.ControlLight);

            // PDFのパスワード
            this.PDFOption.Password_Text = XMLLoader.FetchPDFPassword();
        }

        #endregion

    }
}
