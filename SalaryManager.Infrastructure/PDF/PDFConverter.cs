using System;
using System.Collections.Generic;
using Aspose.Pdf;
using Aspose.Pdf.Devices;
using SalaryManager.Domain.Exceptions;
using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.Domain.Repositories;
using SalaryManager.Infrastructure.XML;

namespace SalaryManager.Infrastructure.PDF
{
    /// <summary>
    /// PDF変換
    /// </summary>
    public sealed class PDFConverter : IPDFConverterRepository
    {
        /// <summary>
        /// PDFをPNG変換する
        /// </summary>
        /// <param name="path">パス</param>
        /// <returns>生成されたPNGパスのリスト</returns>
        public List<string> ConvertPDFIntoImage(string path)
        {
            try
            {
                // パスワードなし
                return this.AddPDFToImage(path, new Document(path));
            } 
            catch(InvalidPasswordException) 
            {
                // パスワードあり
                return this.AddPDFToImage(path, new Document(path, XMLLoader.FetchPDFPassword()));
            }
            catch (Exception ex) 
            {
                throw new FileReaderException("PDFの読み込みに失敗しました。", ex);
            }
        }

        /// <summary>
        /// PDF文書を開く
        /// </summary>
        /// <param name="path">PDFのパス</param>
        /// <param name="pdfDocument">PDFファイル</param>
        /// <returns></returns>
        private List<string> AddPDFToImage(string path, Document pdfDocument)
        {
            var filePaths = new List<string>();

            foreach (var page in pdfDocument.Pages)
            {
                // Pngデバイスを作成する
                var PngDevice = new PngDevice();

                var directory = ImageUtils.ExtractFileDirectory(path);

                // 特定のページを変換し、画像を保存してストリーミングする
                var filePath = directory + page.Number + "_out" + ".Png";

                PngDevice.Process(pdfDocument.Pages[page.Number], filePath);

                filePaths.Add(filePath);
            }

            return filePaths;
        }
    }
}
