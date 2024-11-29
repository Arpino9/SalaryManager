using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.PixelFormats;
using Image = SixLabors.ImageSharp.Image;

namespace SalaryManager.Domain.Modules.Helpers;

/// <summary>
/// Utility - 画像関連
/// </summary>
public sealed class ImageUtils
{
    /// <summary> 最大バイトサイズ </summary>
    /// <remarks> 画像が途中までしか表示されない場合、ここの数値を増やす </remarks>
    private static readonly int Max_Byte_Size = 999999;

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
    /// ファイル名(拡張子なし)を抽出する
    /// </summary>
    /// <param name="path">パス</param>
    /// <returns>ファイル名(拡張子なし)</returns>
    /// <remarks>
    /// 拡張子の文字数 = 拡張子入りファイル名 - (拡張子の文字数 + ドット)
    /// </remarks>
    public static string ExtractFileNameWithoutExtension(string path)
    {
        var startIndex = path.LastIndexOf("\\") + 1;
        var extensionLength = ExtractFileExtension(path).Length + 1;

        return path.Substring(startIndex, path.Length - startIndex - extensionLength);
    }

    /// <summary>
    /// ファイル名(拡張子入り)を抽出する
    /// </summary>
    /// <param name="path">パス</param>
    /// <returns>ファイル名(拡張子入り)</returns>
    public static string ExtractFileNameWithExtension(string path)
    {
        var startIndex = path.LastIndexOf("\\") + 1;

        return path.Substring(startIndex, path.Length - startIndex);
    }

    /// <summary>
    /// ディレクトリ名を抽出する
    /// </summary>
    /// <param name="path">パス</param>
    /// <returns>ファイル名</returns>
    public static string ExtractFileDirectory(string path)
    {
        var startIndex = path.LastIndexOf("\\");

        return path.Substring(0, startIndex + 1);
    }

    /// <summary>
    /// 画像パスをBMP形式に変換
    /// </summary>
    /// <param name="path">画像のパス</param>
    /// <returns>BMP形式のメモリストリーム</returns>
    public static MemoryStream ConvertPathToImage(string path)
    {
        // 画像を読み込む
        using var image = Image.Load<Rgba32>(path);

        // BMP形式に変換してメモリストリームに保存
        var memoryStream = new MemoryStream();
        image.Save(memoryStream, new BmpEncoder());

        // ストリームの位置を先頭に戻す
        memoryStream.Seek(0, SeekOrigin.Begin);

        return memoryStream;
    }

    /// <summary>
    /// 画像パスをバイト配列に変換
    /// </summary>
    /// <param name="path">パス</param>
    /// <param name="format">フォーマット</param>
    /// <returns>バイト配列</returns>
    public static byte[] ConvertPathToBytes(string path, IImageEncoder encoder)
    {
        // 画像を読み込む
        using var image = Image.Load(path);

        // メモリストリームに保存
        using var stream = new MemoryStream();
        image.Save(stream, encoder);

        // バイト配列として返す
        return stream.ToArray();
    }

    /// <summary>
    /// バイト配列をBMP形式に変換
    /// </summary>
    /// <param name="byteImage">画像データのバイト配列</param>
    /// <returns>BMP形式のメモリストリーム</returns>
    public static MemoryStream ConvertBytesToBmpStream(byte[] byteImage)
    {
        // バイト配列から画像を読み込む
        using var image = Image.Load<Rgba32>(byteImage);

        // BMP形式に変換してメモリストリームに保存
        var memoryStream = new MemoryStream();
        image.Save(memoryStream, new BmpEncoder());

        // ストリームの位置を先頭に戻す
        memoryStream.Seek(0, SeekOrigin.Begin);

        return memoryStream;
    }
}
