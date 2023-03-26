using System.Drawing.Imaging;
using FormatException = SalaryManager.Domain.Exceptions.FormatException;

namespace SalaryManager.Domain.ValueObjects
{
    /// <summary>
    /// Value Object - ファイル拡張子
    /// </summary>
    public sealed class FileExtensionValue : ValueObject<FileExtensionValue>
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

        public FileExtensionValue(string rawExtension)
        {
            this.Value = rawExtension.ToLower();
        }

        /// <summary>
        /// PDF形式か
        /// </summary>
        public bool IsPDF
        {
            get
            {
                return this.Value == FileExtensionValue.PDF.Value;
            }
        }

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

        /// <summary> 値 </summary>
        public string Value;

        protected override bool EqualsCore(FileExtensionValue other)
        {
            return this.Value == other.Value;
        }
    }
}
