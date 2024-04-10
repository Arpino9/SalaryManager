using SalaryManager.Domain.Modules.Helpers;
using System.Drawing.Imaging;
using FormatException = SalaryManager.Domain.Exceptions.FormatException;

namespace SalaryManager.Domain.ValueObjects
{
    /// <summary>
    /// Value Object - ファイル拡張子
    /// </summary>
    public sealed record class FileExtensionValue
    {
        /// <summary> JPG形式 </summary>
        private static readonly FileExtensionValue JPG = new FileExtensionValue("jpg");

        /// <summary> GIF形式 </summary>
        private static readonly FileExtensionValue GIF = new FileExtensionValue("gif");

        /// <summary> PNG形式 </summary>
        private static readonly FileExtensionValue PNG = new FileExtensionValue("png");

        /// <summary> TIFF形式 </summary>
        private static readonly FileExtensionValue TIFF = new FileExtensionValue("tiff");

        /// <summary> PDF形式 </summary>
        private static readonly FileExtensionValue PDF = new FileExtensionValue("pdf");

        /// <summary> EXIF形式 </summary>
        private static readonly FileExtensionValue Exif = new FileExtensionValue("exif");

        public FileExtensionValue(string path)
        {
            var rawExtension = ImageUtils.ExtractFileExtension(path);

            this.Value = rawExtension.ToLower();
        }

        /// <summary> 値 </summary>
        public string Value;

        /// <summary>
        /// PDF形式か
        /// </summary>
        public bool IsPDF 
            => (this.Value == FileExtensionValue.PDF.Value);

        /// <summary>
        /// 画像形式か
        /// </summary>
        public bool IsImage 
            => (this.IsJPG || this.IsGIF || this.IsPNG || this.IsTIFF || this.IsExif);

        /// <summary>
        /// JPG形式か
        /// </summary>
        public bool IsJPG
            => (this.Value == FileExtensionValue.JPG.Value);    

        /// <summary>
        /// GIF形式か
        /// </summary>
        public bool IsGIF
            => (this.Value == FileExtensionValue.GIF.Value);

        /// <summary>
        /// PNG形式か
        /// </summary>
        public bool IsPNG
            => (this.Value == FileExtensionValue.PNG.Value);

        /// <summary>
        /// TIFF形式か
        /// </summary>
        public bool IsTIFF
            => (this.Value == FileExtensionValue.TIFF.Value);

        /// <summary>
        /// Exif形式か
        /// </summary>
        public bool IsExif
            => (this.Value == FileExtensionValue.Exif.Value);

        /// <summary>
        /// 画像フォーマット
        /// </summary>
        public ImageFormat ImageFormat
        {
            get
            {
                if (this.Value == FileExtensionValue.JPG.Value) 
                {
                    return ImageFormat.Jpeg;
                }

                if (this.Value == FileExtensionValue.GIF.Value)
                {
                    return ImageFormat.Gif;
                }

                if (this.Value == FileExtensionValue.PNG.Value)
                {
                    return ImageFormat.Png;
                }

                if (this.Value == FileExtensionValue.TIFF.Value)
                {
                    return ImageFormat.Tiff;
                }

                if (this.Value == FileExtensionValue.Exif.Value)
                {
                    return ImageFormat.Exif;
                }

                throw new FormatException("画像フォーマットの変換に失敗しました。");
            }
        }
    }
}
