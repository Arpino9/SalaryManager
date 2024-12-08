namespace SalaryManager.Domain.Entities;

/// <summary>
/// Entity - 添付ファイル
/// </summary>
/// <param name="id">ID</param>
/// <param name="title">タイトル</param>
/// <param name="fileName">ファイル名</param>
/// <param name="image">画像</param>
/// <param name="remarks">備考</param>
/// <param name="createDate">作成日</param>
/// <param name="updateDate">更新日</param>
public sealed class FileStorageEntity(
    int id,
    string title,
    string fileName,
    byte[] image,
    string remarks,
    DateOnly createDate,
    DateOnly updateDate)
{
    /// <summary> ID </summary>
    public int ID => id;

    /// <summary> タイトル </summary>
    public string Title => title;

    /// <summary> ファイル名 </summary>
    public string FileName => fileName;

    /// <summary> 画像 </summary>
    public byte[] Image => image;

    /// <summary> 備考 </summary>
    public string Remarks => remarks;

    /// <summary> 作成日 </summary>
    public DateOnly CreateDate => createDate;

    /// <summary> 作成日 </summary>
    public DateOnly UpdateDate => updateDate;
}
