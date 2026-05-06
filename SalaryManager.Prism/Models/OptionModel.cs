using SalaryManager.WPF.ViewModels;

namespace SalaryManager.Prism.Models;

/// <summary>
/// Model - オプション(PDF)
/// </summary>
public sealed class OptionModel : ModelBase<GeneralOptionViewModel>
{
    #region Get Instance

    private static OptionModel model = null;

    public static OptionModel GetInstance()
    {
        if (model == null)
        {
            model = new OptionModel();
        }

        return model;
    }

    #endregion

    public OptionModel()
    {

    }

    /// <summary> 背景色</summary>
    internal System.Drawing.Color Window_BackgroundColor { get; set; } = System.Drawing.SystemColors.ControlLight;

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
        this.ViewModel.FontFamily_ItemSource = ListUtils.ToObservableCollection<string>(fonts.Families.Select(x => x.Name).ToList());
        this.ViewModel.FontFamily_SelectedIndex = 0;

        if (Shared.SavingExtension == "XML")
        {
            // Excelテンプレート
            this.ViewModel.SelectExcelTempletePath_Text = XMLLoader.FetchExcelTemplatePath();
            // SQLite
            this.ViewModel.SelectSQLite_Text = XMLLoader.FetchSQLitePath();

            this.ViewModel.FontFamily_Text = XMLLoader.FetchFontFamilyText();

            // 初期表示時にデフォルト明細を表示する
            this.ViewModel.ShowDefaultPayslip_IsChecked = XMLLoader.FetchShowDefaultPayslip();

            // フォント
            this.ViewModel.Preview_FontFamily = base.ConvertToWpfFontFamily(XMLLoader.FetchFontFamily());
            this.ViewModel.FontSize_Value = XMLLoader.FetchFontSize();

            var obj = EnumUtils.ToEnum(this.ViewModel.HowToSaveImage_IsChecked.GetType(), XMLLoader.FetchHowToSaveImage());
            if (obj != null)
            {
                this.ViewModel.HowToSaveImage_IsChecked = (GeneralOptionViewModel.HowToSaveImage)obj;
            }

            this.ViewModel.ImageFolderPath_Text = XMLLoader.FetchImageFolder();

            // 背景色
            this.Window_BackgroundColor = XMLLoader.FetchBackgroundColor();
            this.ViewModel.Window_Background = ConvertToBrush(XMLLoader.FetchBackgroundColorBrush());
        }
        else
        {
            var json = JSONExtension.DeserializeSettings<JSONProperty_Settings>(FilePath.GetJSONDefaultPath());

            // Excelテンプレート
            this.ViewModel.SelectExcelTempletePath_Text = json.Excel.TemplatePath;
            // SQLite
            this.ViewModel.SelectSQLite_Text = json.SQLite.Path;

            // フォントファミリ
            this.ViewModel.FontFamily_Text = json.General.FontFamily;

            // 初期表示時にデフォルト明細を表示する
            this.ViewModel.ShowDefaultPayslip_IsChecked = json.General.ShowDefaultPayslip;

            // フォント
            this.ViewModel.FontSize_Value = json.General.FontSize;

            var obj = EnumUtils.ToEnum(this.ViewModel.HowToSaveImage_IsChecked.GetType(), json.General.HowToSaveImage);
            if (obj != null)
            {
                this.ViewModel.HowToSaveImage_IsChecked = (GeneralOptionViewModel.HowToSaveImage)obj;
            }

            this.ViewModel.ImageFolderPath_Text = json.General.ImageFolderPath;

            //this.GeneralOption.Window_BackgroundColor = json.General.BackgroundColor_ColorCode;
            //this.GeneralOption.Window_Background = json.General.BackgroundColor;
        }

        this.ViewModel.SelectFolder_IsEnabled = this.ViewModel.HowToSaveImage_IsChecked == GeneralOptionViewModel.HowToSaveImage.SavePath;
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
        this.SpreadSheetOption.SheetId_Text = XMLLoader.FetchSheetId();
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
    internal override GeneralOptionViewModel ViewModel { get; set; }

    /// <summary> ViewModel - スプレッドシート設定 </summary>
    internal SpreadSheetOptionViewModel SpreadSheetOption { get; set; }

    /// <summary> ViewModel - PDF設定 </summary>
    internal PDFOptionViewModel PDFOption { get; set; }

    /// <summary> ViewModel - Googleカレンダー </summary>
    internal CalendarOptionViewModel CalendarOption { get; set; }

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
        /*var dialog = new OpenFileDialog();
        dialog.Filter = "SQLiteファイル(*.db)|*.db|全てのファイル(*.*)|*.*";
        dialog.Title  = "SQLiteデータベースを指定してください";*/
        var dialog = new OpenFileDialog
        {
            Filter = "SQLiteファイル(*.db)|*.db|全てのファイル(*.*)|*.*",
            Title = "SQLiteデータベースを指定してください"
        };

        var result = dialog.ShowDialog();

        if (result != System.Windows.Forms.DialogResult.OK)
        {
            return;
        }

        this.ViewModel.SelectSQLite_Text = dialog.FileName;
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
        dialog.Title = "Excelのテンプレートを指定してください";

        var result = dialog.ShowDialog();

        if (result != System.Windows.Forms.DialogResult.OK)
        {
            return;
        }

        this.ViewModel.SelectExcelTempletePath_Text = dialog.FileName;
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
        this.ViewModel.Preview_FontFamily = new System.Windows.Media.FontFamily(this.ViewModel.FontFamily_Text);
    }

    #endregion

    /// <summary>
    /// フォントファミリ - SelectionChanged
    /// </summary>
    internal void HowToSaveImage_SelectionChanged()
    {
        this.ViewModel.SelectFolder_IsEnabled = this.ViewModel.HowToSaveImage_IsChecked == GeneralOptionViewModel.HowToSaveImage.SavePath;
    }

    #region 背景色

    /// <summary>
    /// 背景色 - 色を選択
    /// </summary>
    internal void ChangeWindowBackground()
    {
        var dialog = new ColorDialog();
        var result = dialog.ShowDialog();

        if (result == System.Windows.Forms.DialogResult.OK)
        {
            var wpfColor = ColorUtils.ToWPFColor(dialog.Color);
            this.ViewModel.Window_Background = ConvertToBrush(wpfColor);
            this.Window_BackgroundColor = dialog.Color;
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
            tag.SQLitePath = this.ViewModel.SelectSQLite_Text;
            tag.ExcelTemplatePath = this.ViewModel.SelectExcelTempletePath_Text;
            tag.FontFamily = this.ViewModel.FontFamily_Text;
            tag.FontSize = this.ViewModel.FontSize_Value;
            tag.ShowDefaultPayslip = this.ViewModel.ShowDefaultPayslip_IsChecked;
            tag.BackgroundColor_ColorCode = this.Window_BackgroundColor.Name;
            tag.ImageFolderPath = this.ViewModel.ImageFolderPath_Text;

            var list = new List<string>()
            {
                this.Window_BackgroundColor.A.ToString(),
                this.Window_BackgroundColor.R.ToString(),
                this.Window_BackgroundColor.G.ToString(),
                this.Window_BackgroundColor.B.ToString()
            };

            tag.HowToSaveImage = this.ViewModel.HowToSaveImage_IsChecked.ToString();

            tag.BackgroundColor = list.Combine();

            tag.PrivateKeyPath_SpreadSheet = this.SpreadSheetOption.SelectPrivateKey_Text;
            tag.SheetId = this.SpreadSheetOption.SheetId_Text;

            tag.PDFPassword = this.PDFOption.Password_Text;

            tag.PrivateKeyPath_Calendar = this.CalendarOption.SelectPrivateKey_Text;
            tag.CalendarId = this.CalendarOption.SelectCalendarID_Text;

            writer.Serialize(tag);
        }
    }

    /// <summary>
    /// フォルダを開く
    /// </summary>
    internal void SelectFolder()
    {
        var directory = DialogUtils.SelectDirectory("取得元のフォルダを選択してください。");

        if (string.IsNullOrEmpty(directory))
        {
            return;
        }

        this.ViewModel.ImageFolderPath_Text = directory;
    }

    /// <summary>
    /// JSON保存
    /// </summary>
    internal void SaveJSON()
    {
        var list = new List<string>()
        {
            this.Window_BackgroundColor.A.ToString(),
            this.Window_BackgroundColor.R.ToString(),
            this.Window_BackgroundColor.G.ToString(),
            this.Window_BackgroundColor.B.ToString()
        };

        var property = new JSONProperty_Settings()
        {
            General = new General
            {
                FontFamily = this.ViewModel.FontFamily_Text,
                FontSize = this.ViewModel.FontSize_Value,
                BackgroundColor = list.Combine(),
                BackgroundColor_ColorCode = this.Window_BackgroundColor.Name,
                ShowDefaultPayslip = this.ViewModel.ShowDefaultPayslip_IsChecked,
                HowToSaveImage = this.ViewModel.HowToSaveImage_IsChecked.ToString(),
                ImageFolderPath = this.ViewModel.ImageFolderPath_Text,
            },
            SpreadSheet = new SpreadSheet
            {
                PrivateKeyPath = this.SpreadSheetOption.SelectPrivateKey_Text,
                ID = this.SpreadSheetOption.SheetId_Text,
            },
            SQLite = new SQLite
            {
                Path = this.ViewModel.SelectSQLite_Text,
            },
            Excel = new Excel
            {
                TemplatePath = this.ViewModel.SelectExcelTempletePath_Text,
            },
            GoogleCalendar = new GoogleCalendar
            {
                PrivateKeyPath = this.CalendarOption.SelectPrivateKey_Text,
                ID = this.CalendarOption.SelectCalendarID_Text,
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
        this.ViewModel.SelectSQLite_Text = FilePath.GetSQLiteDefaultPath();

        // Excelテンプレートパス
        this.ViewModel.SelectExcelTempletePath_Text = FilePath.GetExcelTempleteDefaultPath();

        // フォントファミリ
        this.ViewModel.FontFamily_Text = Shared.FontFamily;
        this.ViewModel.Preview_FontFamily = new System.Windows.Media.FontFamily(Shared.FontFamily);

        // フォントサイズ
        this.ViewModel.FontSize_Value = decimal.Parse(Shared.FontSize);

        // 背景色
        this.Window_BackgroundColor = System.Drawing.SystemColors.ControlLight;
        this.ViewModel.Window_Background = ConvertToBrush(ColorUtils.ToWPFColor(System.Drawing.SystemColors.ControlLight));

        // PDFのパスワード
        this.PDFOption.Password_Text = XMLLoader.FetchPDFPassword();
    }

    /// <summary>
    /// System.Drawing.Color を SolidColorBrush に変換
    /// </summary>
    /// <param name="drawingColor">System.Drawing.Color</param>
    /// <returns>SolidColorBrush</returns>
    public static System.Windows.Media.SolidColorBrush ConvertToBrush(System.Drawing.Color drawingColor)
    {
        return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(
            drawingColor.A,
            drawingColor.R,
            drawingColor.G,
            drawingColor.B
        ));
    }

    #endregion
}
