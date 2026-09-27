namespace SalaryManager.Domain.ValueObjects;

/// <summary>
/// Value Object - ファイル拡張子
/// </summary>
public sealed record class FileExtensionValue
{
    /// <summary> JPG形式 </summary>
    private static readonly FileExtensionValue JPG = new("jpg");

    /// <summary> GIF形式 </summary>
    private static readonly FileExtensionValue GIF = new("gif");

    /// <summary> PNG形式 </summary>
    private static readonly FileExtensionValue PNG = new("png");

    /// <summary> TIFF形式 </summary>
    private static readonly FileExtensionValue TIFF = new("tiff");

    /// <summary> PDF形式 </summary>
    private static readonly FileExtensionValue PDF = new("pdf");

    /// <summary> EXIF形式 </summary>
    private static readonly FileExtensionValue Exif = new("exif");

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
    public bool IsPDF => (this.Value == FileExtensionValue.PDF.Value);

    /// <summary>
    /// 画像形式か
    /// </summary>
    public bool IsImage => (this.IsJPG || this.IsGIF || this.IsPNG || this.IsTIFF || this.IsExif);

    /// <summary>
    /// JPG形式か
    /// </summary>
    public bool IsJPG => (this.Value == FileExtensionValue.JPG.Value);    

    /// <summary>
    /// GIF形式か
    /// </summary>
    public bool IsGIF => (this.Value == FileExtensionValue.GIF.Value);

    /// <summary>
    /// PNG形式か
    /// </summary>
    public bool IsPNG => (this.Value == FileExtensionValue.PNG.Value);

    /// <summary>
    /// TIFF形式か
    /// </summary>
    public bool IsTIFF => (this.Value == FileExtensionValue.TIFF.Value);

    /// <summary>
    /// Exif形式か
    /// </summary>
    public bool IsExif => (this.Value == FileExtensionValue.Exif.Value);

    /// <summary>
    /// 画像フォーマット
    /// </summary>
    public IImageEncoder ImageEncoder
    {
        get
        {
            if (this.Value == FileExtensionValue.JPG.Value)
            {
                return new JpegEncoder();
            }

            if (this.Value == FileExtensionValue.GIF.Value)
            {
                return new GifEncoder();
            }

            if (this.Value == FileExtensionValue.PNG.Value)
            {
                return new PngEncoder();
            }

            if (this.Value == FileExtensionValue.TIFF.Value)
            {
                return new TiffEncoder();
            }

            throw new Exceptions.FormatException("画像フォーマットの変換に失敗しました。");
        }
    }
}
