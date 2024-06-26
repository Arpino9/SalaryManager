﻿namespace SalaryManager.Infrastructure.SpreadSheet;

/// <summary>
/// スプレッドシート - Reader
/// </summary>
public static class SpreadSheetReader
{
    /// <summary> 取得した値 </summary>
    public static IList<IList<object>> CellValues { get; set; }

    /// <summary>
    /// 読み込み
    /// </summary>
    /// <param name="sheetName">シート名</param>
    public static void ReadOutlineHeader(string sheetName)
    {
        using(var fileStream = new FileStream(XMLLoader.FetchPrivateKeyPath_SpreadSheet(), FileMode.Open, FileAccess.Read))
        {
            var googleCredential = GoogleCredential.FromStream(fileStream)
                                                   .CreateScoped(SheetsService.Scope.Spreadsheets);

            var initializer = new BaseClientService.Initializer() { HttpClientInitializer = googleCredential };
            var sheetsService = new SheetsService(initializer);

            var request  = sheetsService.Spreadsheets.Values.Get(XMLLoader.FetchSheetId(), GetRange(sheetName));
            var response = request.Execute();

            SpreadSheetReader.CellValues = response.Values;
        }
    }

    /// <summary>
    /// 出力範囲を取得
    /// </summary>
    /// <param name="sheetName">シート名</param>
    /// <returns>出力範囲</returns>
    private static string GetRange(string sheetName)
    {
        if (sheetName == SpreadSheetDefinition.SheetName.Outline)
        {
            return SpreadSheetDefinition.OutlineRange;
        }

        if (sheetName == SpreadSheetDefinition.SheetName.Detail)
        {
            return SpreadSheetDefinition.DetailRange;
        }

        return string.Empty;
    }
}
