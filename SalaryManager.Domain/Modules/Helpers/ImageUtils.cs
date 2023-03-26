using Aspose.Pdf;
using Aspose.Pdf.Devices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SalaryManager.Domain.Modules.Helpers
{
    public sealed class ImageUtils
    {
        /// <summary> 最大バイトサイズ </summary>
        private static readonly int Max_Byte_Size = 65535;

        
        public static string ExtractFileExtension(string path)
        {
            var startIndex = path.LastIndexOf('.');

            return path.Substring(startIndex + 1, path.Length - startIndex - 1);
        }

        /// <summary>
        /// ファイル名を抽出する
        /// </summary>
        /// <param name="path">パス</param>
        /// <returns>ファイル名</returns>
        public static string ExtractFileName(string path)
        {
            var startIndex = path.LastIndexOf("\\") + 1;

            return path.Substring(startIndex, path.Length - startIndex);
        }

        /// <summary>
        /// ファイル名を抽出する
        /// </summary>
        /// <param name="path">パス</param>
        /// <returns>ファイル名</returns>
        public static string ExtractFileDirectory(string path)
        {
            var startIndex = path.LastIndexOf("\\");

            return path.Substring(0, startIndex + 1);
        }

        /// <summary>
        /// 画像パスをBMP変換
        /// </summary>
        /// <param name="path">パス</param>
        /// <param name="format">フォーマット</param>
        /// <returns>BMP</returns>
        public static ImageSource ConvertPathToImage(string path, ImageFormat format)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new MemoryStream(ImageUtils.ConvertPathToBytes(path, format));
            image.EndInit();

            image.Freeze();

            return image;
        }

        /// <summary>
        /// 画像パスをバイト配列に変換
        /// </summary>
        /// <param name="path">パス</param>
        /// <param name="format">フォーマット</param>
        /// <returns>バイト配列</returns>
        public static byte[] ConvertPathToBytes(string path, ImageFormat format)
        {
            // 画像を読み込む
            System.Drawing.Image img = System.Drawing.Image.FromFile(path);

            // Jpeg形式でストリームを定義
            MemoryStream stream = new MemoryStream();
            img.Save(stream, format);

            // ストリームからバイト配列に書き込む
            var imgBytes = new Byte[Max_Byte_Size];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(imgBytes, 0, Max_Byte_Size);

            return imgBytes;
        }

        /// <summary>
        /// バイト配列をBMP変換
        /// </summary>
        /// <param name="byteImage">画像</param>
        /// <returns>BMP</returns>
        public static ImageSource ConvertBytesToImage(byte[] byteImage)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new MemoryStream(byteImage);
            image.EndInit();

            image.Freeze();

            return image;
        }

        public static List<string> ConvertPDFToImage(string path)
        {
            // ドキュメントを開く
            var pdfDocument = new Document(path);

            var filePaths = new List<string>();

            foreach (var page in pdfDocument.Pages)
            {
                // 解像度を定義する
                var resolution = new Resolution(300);

                // 指定された属性でPngデバイスを作成する
                // 幅、高さ、解像度
                var PngDevice = new PngDevice(500, 700, resolution);

                var directory = ExtractFileDirectory(path);

                // 特定のページを変換し、画像を保存してストリーミングする
                var filePath = directory + page.Number + "_out" + ".Png";

                PngDevice.Process(pdfDocument.Pages[page.Number], filePath);

                filePaths.Add(filePath);
            }

            return filePaths;
        }
    }
}
