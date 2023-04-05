using Aspose.Pdf;
using Aspose.Pdf.Devices;
using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.Domain.Repositories;
using System.Collections.Generic;

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
            // ドキュメントを開く
            var pdfDocument = new Document(path);

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
