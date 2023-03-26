using System;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SalaryManager.Domain.Modules.Helpers
{
    /// <summary>
    /// Utility - 画像関連
    /// </summary>
    public sealed class ImageUtils
    {
        /// <summary> 最大バイトサイズ </summary>
        private static readonly int Max_Byte_Size = 65535;

        /// <summary>
        /// ファイルの拡張子を抽出する
        /// </summary>
        /// <param name="path">パス</param>
        /// <returns>拡張子</returns>
        /// <remarks>
        /// 値オブジェクトと連動して使うことを推奨。
        /// </remarks>
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
            using(var image = System.Drawing.Image.FromFile(path))
            {
                // Jpeg形式でストリームを定義
                MemoryStream stream = new MemoryStream();
                image.Save(stream, format);

                // ストリームからバイト配列に書き込む
                var imgBytes = new Byte[Max_Byte_Size];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(imgBytes, 0, Max_Byte_Size);

                return imgBytes;
            }
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
    }
}
