namespace SalaryManager.WPF.Models;

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
        this.GeneralOption.FontFamily_ItemSource.Value = ListUtils.ToReactiveCollection<string>(fonts.Families.Select(x => x.Name).ToList());
        this.GeneralOption.FontFamily_SelectedIndex.Value = 0;

        if (Shared.SavingExtension == "XML")
        {
            // Excelテンプレート
            this.GeneralOption.SelectExcelTempletePath_Text.Value = XMLLoader.FetchExcelTemplatePath();
            // SQLite
            this.GeneralOption.SelectSQLite_Text.Value = XMLLoader.FetchSQLitePath();

            this.GeneralOption.FontFamily_Text.Value = XMLLoader.FetchFontFamilyText();

            // 初期表示時にデフォルト明細を表示する
            this.GeneralOption.ShowDefaultPayslip_IsChecked.Value = XMLLoader.FetchShowDefaultPayslip();

            // フォント
            this.GeneralOption.Preview_FontFamily.Value = XMLLoader.FetchFontFamily();
            this.GeneralOption.FontSize_Value.Value = XMLLoader.FetchFontSize();

            var obj = EnumUtils.ToEnum(this.GeneralOption.HowToSaveImage_IsChecked.GetType(), XMLLoader.FetchHowToSaveImage());
            if (obj != null)
            {
                this.GeneralOption.HowToSaveImage_IsChecked.Value = (ViewModel_GeneralOption.HowToSaveImage)obj;
            }

            this.GeneralOption.ImageFolderPath_Text.Value = XMLLoader.FetchImageFolder();
            
            // 背景色
            this.Window_BackgroundColor = XMLLoader.FetchBackgroundColor();
            this.GeneralOption.Window_Background.Value = XMLLoader.FetchBackgroundColorBrush();
        }
        else
        {
            var json = JSONExtension.DeserializeSettings<JSONProperty_Settings>(FilePath.GetJSONDefaultPath());

            // Excelテンプレート
            this.GeneralOption.SelectExcelTempletePath_Text.Value = json.Excel.TemplatePath;
            // SQLite
            this.GeneralOption.SelectSQLite_Text.Value = json.SQLite.Path;

            // フォントファミリ
            this.GeneralOption.FontFamily_Text.Value = json.General.FontFamily;

            // 初期表示時にデフォルト明細を表示する
            this.GeneralOption.ShowDefaultPayslip_IsChecked.Value = json.General.ShowDefaultPayslip;

            // フォント
            this.GeneralOption.FontSize_Value.Value = json.General.FontSize;

            var obj = EnumUtils.ToEnum(this.GeneralOption.HowToSaveImage_IsChecked.Value.GetType(), json.General.HowToSaveImage);
            if (obj != null)
            {
                this.GeneralOption.HowToSaveImage_IsChecked.Value = (ViewModel_GeneralOption.HowToSaveImage)obj;
            }

            this.GeneralOption.ImageFolderPath_Text.Value = json.General.ImageFolderPath;

            //this.GeneralOption.Window_BackgroundColor = json.General.BackgroundColor_ColorCode;
            //this.GeneralOption.Window_Background = json.General.BackgroundColor;
        }

        this.GeneralOption.SelectFolder_IsEnabled.Value = this.GeneralOption.HowToSaveImage_IsChecked.Value == ViewModel_GeneralOption.HowToSaveImage.SavePath;
    }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <remarks>
    /// 値があればXMLから、なければconfigから取得する。
    /// </remarks>
    internal void Initialize_SpreadSheet()
    {
        this.SpreadSheetOption.SelectPrivateKey_Text.Value = XMLLoader.FetchPrivateKeyPath_SpreadSheet();
        this.SpreadSheetOption.SheetId_Text.Value          = XMLLoader.FetchSheetId();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <remarks>
    /// 値があればXMLから、なければconfigから取得する。
    /// </remarks>
    internal void Initialize_PDF()
    {
        this.PDFOption.Password_Text.Value = XMLLoader.FetchPDFPassword();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <remarks>
    /// 値があればXMLから、なければconfigから取得する。
    /// </remarks>
    internal void Initialize_Calendar()
    {
        this.CalendarOption.SelectPrivateKey_Text.Value = XMLLoader.FetchPrivateKeyPath_Calendar();
        this.CalendarOption.SelectCalendarID_Text.Value = XMLLoader.FetchCalendarId();
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

        this.GeneralOption.SelectSQLite_Text.Value = dialog.FileName;
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

        this.GeneralOption.SelectExcelTempletePath_Text.Value = dialog.FileName;
    }

    #endregion

    #region 認証ファイル

    /// <summary>
    /// 認証ファイル(SpreadSheet) - 開く
    /// </summary>
    internal void SelectPrivateKeyPath_SpreadSheet()
    {
        this.SpreadSheetOption.SelectPrivateKey_Text.Value = DialogUtils.SelectFile(string.Empty, "JSONファイル(*.json)|*.json");
    }

    /// <summary>
    /// 認証ファイル(Googleカレンダー) - 開く
    /// </summary>
    internal void SelectPrivateKeyPath_Calendar()
    {
        this.CalendarOption.SelectPrivateKey_Text.Value = DialogUtils.SelectFile(string.Empty, "JSONファイル(*.json)|*.json");
    }

    #endregion

    #region フォントファミリ

    /// <summary>
    /// フォントファミリ - SelectionChanged
    /// </summary>
    internal void FontFamily_SelectionChanged()
    {
        this.GeneralOption.Preview_FontFamily.Value = new System.Windows.Media.FontFamily(this.GeneralOption.FontFamily_Text.Value);
    }

    #endregion

    /// <summary>
    /// フォントファミリ - SelectionChanged
    /// </summary>
    internal void HowToSaveImage_SelectionChanged()
    {
        this.GeneralOption.SelectFolder_IsEnabled.Value = this.GeneralOption.HowToSaveImage_IsChecked.Value == ViewModel_GeneralOption.HowToSaveImage.SavePath;
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
            this.GeneralOption.Window_Background.Value = ColorUtils.ToWPFColor(dialog.Color);
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
            tag.SQLitePath                = this.GeneralOption.SelectSQLite_Text.Value;
            tag.ExcelTemplatePath         = this.GeneralOption.SelectExcelTempletePath_Text.Value;
            tag.FontFamily                = this.GeneralOption.FontFamily_Text.Value;
            tag.FontSize                  = this.GeneralOption.FontSize_Value.Value;
            tag.ShowDefaultPayslip        = this.GeneralOption.ShowDefaultPayslip_IsChecked.Value;
            tag.BackgroundColor_ColorCode = this.Window_BackgroundColor.Name;
            tag.ImageFolderPath           = this.GeneralOption.ImageFolderPath_Text.Value;

            var list = new List<string>()
            {
                this.Window_BackgroundColor.A.ToString(),
                this.Window_BackgroundColor.R.ToString(),
                this.Window_BackgroundColor.G.ToString(),
                this.Window_BackgroundColor.B.ToString()
            };

            tag.HowToSaveImage = this.GeneralOption.HowToSaveImage_IsChecked.ToString();

            tag.BackgroundColor = list.Combine();

            tag.PrivateKeyPath_SpreadSheet = this.SpreadSheetOption.SelectPrivateKey_Text.Value;
            tag.SheetId                    = this.SpreadSheetOption.SheetId_Text.Value;

            tag.PDFPassword = this.PDFOption.Password_Text.Value;

            tag.PrivateKeyPath_Calendar = this.CalendarOption.SelectPrivateKey_Text.Value;
            tag.CalendarId              = this.CalendarOption.SelectCalendarID_Text.Value;

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

        this.GeneralOption.ImageFolderPath_Text.Value = directory;
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
                FontFamily                = this.GeneralOption.FontFamily_Text.Value,
                FontSize                  = this.GeneralOption.FontSize_Value.Value,
                BackgroundColor           = list.Combine(),
                BackgroundColor_ColorCode = this.Window_BackgroundColor.Name,
                ShowDefaultPayslip        = this.GeneralOption.ShowDefaultPayslip_IsChecked.Value,
                HowToSaveImage            = this.GeneralOption.HowToSaveImage_IsChecked.ToString(),
                ImageFolderPath           = this.GeneralOption.ImageFolderPath_Text.Value,
            },
            SpreadSheet = new SpreadSheet
            {
                PrivateKeyPath = this.SpreadSheetOption.SelectPrivateKey_Text.Value,
                ID             = this.SpreadSheetOption.SheetId_Text.Value,
            },
            SQLite = new SQLite
            {
                Path = this.GeneralOption.SelectSQLite_Text.Value,
            },
            Excel = new Excel
            {
                TemplatePath = this.GeneralOption.SelectExcelTempletePath_Text.Value,
            },
            GoogleCalendar = new GoogleCalendar
            {
                PrivateKeyPath = this.CalendarOption.SelectPrivateKey_Text.Value,
                ID             = this.CalendarOption.SelectCalendarID_Text.Value,
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
        this.GeneralOption.SelectSQLite_Text.Value = FilePath.GetSQLiteDefaultPath();

        // Excelテンプレートパス
        this.GeneralOption.SelectExcelTempletePath_Text.Value = FilePath.GetExcelTempleteDefaultPath();

        // フォントファミリ
        this.GeneralOption.FontFamily_Text.Value    = Shared.FontFamily;
        this.GeneralOption.Preview_FontFamily.Value = new System.Windows.Media.FontFamily(Shared.FontFamily);

        // フォントサイズ
        this.GeneralOption.FontSize_Value.Value = decimal.Parse(Shared.FontSize);

        // 背景色
        this.Window_BackgroundColor = System.Drawing.SystemColors.ControlLight;
        this.GeneralOption.Window_Background.Value      = ColorUtils.ToWPFColor(System.Drawing.SystemColors.ControlLight);

        // PDFのパスワード
        this.PDFOption.Password_Text.Value = XMLLoader.FetchPDFPassword();
    }

    #endregion

}
