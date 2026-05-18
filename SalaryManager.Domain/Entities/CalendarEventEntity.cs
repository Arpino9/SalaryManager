namespace SalaryManager.Domain.Entities;

/// <summary>
/// Entity - Googleカレンダーのイベント
/// </summary>
/// <param name="title">タイトル</param>
/// <param name="startDate">開始日時</param>
/// <param name="endDate">終了日時</param>
/// <param name="place">場所</param>
/// <param name="description">説明</param>
public sealed class CalendarEventEntity(
    string title,
    DateTime startDate,
    DateTime endDate,
    string place,
    string description) : IEntity
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="title">タイトル</param>
    /// <param name="startDate">開始日時</param>
    /// <param name="endDate">終了日時</param>
    public CalendarEventEntity(
        string title,
        DateTime startDate,
        DateTime endDate) : this(title, startDate, endDate, string.Empty, string.Empty)
    {
            
    }

    /// <summary> タイトル </summary>
    public string Title => title;

    /// <summary> 開始日時 </summary>
    public DateTime StartDate => startDate;

    /// <summary> 終了日時 </summary>
    public DateTime EndDate => endDate;

    /// <summary> 場所 </summary>
    public string Place => place;

    /// <summary> 説明 </summary>
    public string Description => description;

    /// <summary> 所要時間 </summary>
    public TimeSpan TimeSpan => endDate - startDate;
}