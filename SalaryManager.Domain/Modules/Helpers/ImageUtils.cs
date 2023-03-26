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
            Image img = Image.FromFile(path);

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
    }
}
