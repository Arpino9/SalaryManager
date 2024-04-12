using Newtonsoft.Json;
using SalaryManager.Domain.Exceptions;
using SalaryManager.Domain.Modules.Logics;
using System;
using System.IO;

namespace SalaryManager.Infrastructure.JSON
{
    /// <summary>
    /// JSON Writer
    /// </summary>
    public static class JSONExtension
    {
        /// <summary>
        /// Jsonファイルへの出力
        /// </summary>
        /// <typeparam name="T">データ型</typeparam>
        /// <param name="obj">オブジェクト</param>
        /// <param name="path">ファイルパス</param>
        public static void SerializeToFile<T>(this T obj, string path)
        {
            try
            {
                using (var sw = new StreamWriter(path, false, System.Text.Encoding.UTF8))
                {
                    // JSON データにシリアライズ
                    var jsonData = JsonConvert.SerializeObject(obj, Formatting.Indented);

                    // JSON データをファイルに書き込み
                    sw.Write(jsonData);
                }
            }
            catch (Exception ex)
            {
                throw new FileWriterException("JSONの書き込みに失敗しました。", ex);
            }
        }

        /// <summary>
        /// Jsonファイルの読み込み
        /// </summary>
        /// <returns>Jsonデータ</returns>
        /// <exception cref="FileReaderException">読み込み失敗</exception>
        public static JSONProperty_Settings DeserializeSettings()
        {
            try
            {
                using (var sr = new StreamReader(FilePath.GetJSONDefaultPath(), System.Text.Encoding.UTF8))
                {
                    // 変数 jsonReadData にファイルの内容を代入 
                    var jsonReadData = sr.ReadToEnd();

                    // デシリアライズして person にセット
                    return JsonConvert.DeserializeObject<JSONProperty_Settings>(jsonReadData);
                }
            }
            catch (Exception ex)
            {
                throw new FileReaderException("JSONの読み込みに失敗しました。", ex);
            }
        }
    }
}
