﻿using System.Windows.Forms;
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
            this.SpreadSheetOption.SelectPrivateKey_Text = XMLLoader.FetchPrivateKeyPath();
            this.SpreadSheetOption.SheetId_Text = XMLLoader.FetchSheetId();
        }

        /// <summary> ViewModel - 全般設定 </summary>
        internal ViewModel_GeneralOption GeneralOption { get; set; }

        /// <summary> ViewModel - スプレッドシート設定 </summary>
        internal ViewModel_SpreadSheetOption SpreadSheetOption { get; set; }

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

        /// <summary>
        /// SQLite - 初期値に戻す
        /// </summary>
        internal void SetDefault_SelectSQLitePath()
        {
            this.GeneralOption.SelectSQLite_Text = FilePath.GetSQLiteDefaultPath();
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

        /// <summary>
        /// Excelテンプレートパス - 初期値に戻す
        /// </summary>
        internal void SetDefault_SelectExcelTemplatePath()
        {
            this.GeneralOption.SelectExcelTempletePath_Text = FilePath.GetExcelTempleteDefaultPath();
        }

        #endregion

        #region 認証ファイル

        /// <summary>
        /// 認証ファイル - 開く
        /// </summary>
        internal void SelectPrivateKeyPath()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "JSONファイル(*.json)|*.json";
            dialog.Title = "認証ファイルを指定してください";

            var result = dialog.ShowDialog();

            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.SpreadSheetOption.SelectPrivateKey_Text = dialog.FileName;
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

        /// <summary>
        /// フォントファミリ - 初期値に戻す
        /// </summary>
        internal void SetDefault_FontFamily()
        {
            this.GeneralOption.FontFamily_Text    = Shared.FontFamily;
            this.GeneralOption.Preview_FontFamily = new System.Windows.Media.FontFamily(Shared.FontFamily);
        }

        #endregion

        /// <summary>
        /// フォントファミリ - SelectionChanged
        /// </summary>
        internal void HowToSaveImage_SelectionChanged()
        {
            this.GeneralOption.SelectFolder_IsEnabled = this.GeneralOption.HowToSaveImage_IsChecked == HowToSaveImage.SavePath;
        }

        #region フォントサイズ

        /// <summary>
        /// フォントサイズ - 初期値に戻す
        /// </summary>
        internal void SetDefault_FontSize_Value()
        {
            this.GeneralOption.FontSize_Value = decimal.Parse(Shared.FontSize);
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
                this.GeneralOption.Window_Background      = ColorUtils.ToWPFColor(dialog.Color);
                this.GeneralOption.Window_BackgroundColor = dialog.Color;
            }
        }

        /// <summary>
        /// 背景色 - デフォルトに戻す
        /// </summary>
        internal void SetDefault_WindowBackground()
        {
            var defaultColor = SystemColors.ControlLight;

            this.GeneralOption.Window_BackgroundColor = defaultColor;
            this.GeneralOption.Window_Background      = ColorUtils.ToWPFColor(defaultColor);
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

                tag.PrivateKeyPath = this.SpreadSheetOption.SelectPrivateKey_Text;
                tag.SheetId        = this.SpreadSheetOption.SheetId_Text;

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

    }
}
