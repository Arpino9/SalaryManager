namespace SalaryManager.Domain.Entities;

/// <summary>
/// Entity - 添付ファイル
/// </summary>
public sealed class FileStorageEntity
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="fileName">ファイル名</param>
    /// <param name="image">画像</param>
    /// <param name="remarks">備考</param>
    /// <param name="createDate">作成日</param>
    /// <param name="updateDate">更新日</param>
    public FileStorageEntity(
        int id,
        string title,
        string fileName,
        byte[] image,
        string remarks,
        DateTime createDate,
        DateTime updateDate)
    {
        this.ID         = id;
        this.Title      = title;
        this.FileName   = fileName;
        this.Image      = image;
        this.Remarks    = remarks;
        this.CreateDate = createDate;
        this.UpdateDate = updateDate;
    }

    /// <summary> ID </summary>
    public int ID { get; }

    /// <summary> タイトル </summary>
    public string Title { get; }

    /// <summary> ファイル名 </summary>
    public string FileName { get; }

    /// <summary> 画像 </summary>
    public byte[] Image { get; }

    /// <summary> 備考 </summary>
    public string Remarks { get; }

    /// <summary> 作成日 </summary>
    public DateTime CreateDate { get; }

    /// <summary> 作成日 </summary>
    public DateTime UpdateDate { get; }

}
