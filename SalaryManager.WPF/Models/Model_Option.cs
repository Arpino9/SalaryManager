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
using SalaryManager.Infrastructure.JSON;

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
            // フォントファミリ
            var fonts = new InstalledFontCollection();
            this.GeneralOption.FontFamily_ItemSource = ListUtils.ToObservableCollection<string>(fonts.Families.Select(x => x.Name).ToList());

            if (Shared.SavingExtension == "XML")
            {
                // Excelテンプレート
                this.GeneralOption.SelectExcelTempletePath_Text = XMLLoader.FetchExcelTemplatePath();
                // SQLite
                this.GeneralOption.SelectSQLite_Text = XMLLoader.FetchSQLitePath();

                this.GeneralOption.FontFamily_Text = XMLLoader.FetchFontFamilyText();

                // 初期表示時にデフォルト明細を表示する
                this.GeneralOption.ShowDefaultPayslip_IsChecked = XMLLoader.FetchShowDefaultPayslip();

                // フォント
                this.GeneralOption.Preview_FontFamily = XMLLoader.FetchFontFamily();
                this.GeneralOption.FontSize_Value = XMLLoader.FetchFontSize();

                var obj = EnumUtils.ToEnum(this.GeneralOption.HowToSaveImage_IsChecked.GetType(), XMLLoader.FetchHowToSaveImage());
                if (obj != null)
                {
                    this.GeneralOption.HowToSaveImage_IsChecked = (HowToSaveImage)obj;
                }

                this.GeneralOption.ImageFolderPath_Text = XMLLoader.FetchImageFolder();
                
                // 背景色
                this.GeneralOption.Window_BackgroundColor = XMLLoader.FetchBackgroundColor();
                this.GeneralOption.Window_Background = XMLLoader.FetchBackgroundColorBrush();
            }
            else
            {
                var json = JSONExtension.DeserializeSettings();

                // Excelテンプレート
                this.GeneralOption.SelectExcelTempletePath_Text = json.Excel.TemplatePath;
                // SQLite
                this.GeneralOption.SelectSQLite_Text = json.SQLite.Path;

                // フォントファミリ
                this.GeneralOption.FontFamily_Text = json.General.FontFamily;

                // 初期表示時にデフォルト明細を表示する
                this.GeneralOption.ShowDefaultPayslip_IsChecked = json.General.ShowDefaultPayslip;

                // フォント
                this.GeneralOption.FontSize_Value = json.General.FontSize;

                var obj = EnumUtils.ToEnum(this.GeneralOption.HowToSaveImage_IsChecked.GetType(), json.General.HowToSaveImage);
                if (obj != null)
                {
                    this.GeneralOption.HowToSaveImage_IsChecked = (HowToSaveImage)obj;
                }

                this.GeneralOption.ImageFolderPath_Text = json.General.ImageFolderPath;

                //this.GeneralOption.Window_BackgroundColor = json.General.BackgroundColor_ColorCode;
                //this.GeneralOption.Window_Background = json.General.BackgroundColor;
            }

            this.GeneralOption.SelectFolder_IsEnabled = this.GeneralOption.HowToSaveImage_IsChecked == HowToSaveImage.SavePath;
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
        /// XML保存
        /// </summary>
        internal void SaveXML()
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

        /// <summary>
        /// JSON保存
        /// </summary>
        internal void SaveJSON()
        {
            var list = new List<string>()
            {
                this.GeneralOption.Window_BackgroundColor.A.ToString(),
                this.GeneralOption.Window_BackgroundColor.R.ToString(),
                this.GeneralOption.Window_BackgroundColor.G.ToString(),
                this.GeneralOption.Window_BackgroundColor.B.ToString()
            };

            var property = new JSONProperty_Settings()
            {
                General = new General
                {
                    FontFamily = this.GeneralOption.FontFamily_Text,
                    FontSize = this.GeneralOption.FontSize_Value,
                    BackgroundColor = StringUtils.Aggregate(list),
                    BackgroundColor_ColorCode = this.GeneralOption.Window_BackgroundColor.Name,
                    ShowDefaultPayslip = this.GeneralOption.ShowDefaultPayslip_IsChecked,
                    HowToSaveImage = this.GeneralOption.HowToSaveImage_IsChecked.ToString(),
                    ImageFolderPath = this.GeneralOption.ImageFolderPath_Text,
                },
                SpreadSheet = new SpreadSheet
                {
                    PrivateKeyPath = this.SpreadSheetOption.SelectPrivateKey_Text,
                    ID             = this.SpreadSheetOption.SheetId_Text,
                },
                SQLite = new SQLite
                {
                    Path = this.GeneralOption.SelectSQLite_Text,
                },
                Excel = new Excel
                {
                    TemplatePath = this.GeneralOption.SelectExcelTempletePath_Text,
                },
                GoogleCalendar = new GoogleCalendar
                {
                    PrivateKeyPath = this.CalendarOption.SelectPrivateKey_Text,
                    ID             = this.CalendarOption.SelectCalendarID_Text,
                },
                PDF = new PDF
                {
                    Password = string.Empty,
                }
            };

            property.SerializeToFile(FilePath.GetJSONDefaultPath());
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
