using System.Windows.Controls;

namespace SalaryManager.Domain.Entities;

/// <summary>
/// Entity - 手当有無
/// </summary>
/// <param name="perfectAttendance">皆勤手当</param>
/// <param name="education">教育手当</param>
/// <param name="electricity">在宅手当</param>
/// <param name="certification">資格手当</param>
/// <param name="Overtime">時間外手当</param>
/// <param name="travel">出張手当</param>
/// <param name="housing">住宅手当</param>
/// <param name="food">食事手当</param>
/// <param name="lateNight">深夜手当</param>
/// <param name="area">地域手当</param>
/// <param name="commution">通勤手当</param>
/// <param name="prepaidRetirement">前払退職金</param>
/// <param name="dependency">扶養手当</param>
/// <param name="executive">役職手当</param>
/// <param name="special">特別手当</param>
public sealed class AllowanceExistenceEntity(
    bool perfectAttendance,
    bool education,
    bool electricity,
    bool certification,
    bool Overtime,
    bool travel,
    bool housing,
    bool food,
    bool lateNight,
    bool area,
    bool commution,
    bool prepaidRetirement,
    bool dependency,
    bool executive,
    bool special) : IEntity
{
    /// <summary> 皆勤手当 </summary>
    public AlternativeValue PerfectAttendance => new AlternativeValue(perfectAttendance);

    /// <summary> 教育手当 </summary>
    public AlternativeValue Education => new AlternativeValue(education);

    /// <summary> 在宅手当 </summary>
    public AlternativeValue Electricity => new AlternativeValue(electricity);

    /// <summary> 資格手当 </summary>
    public AlternativeValue Certification => new AlternativeValue(certification);

    /// <summary> 時間外手当 </summary>
    public AlternativeValue Overtime { get; } = new AlternativeValue(Overtime);

    /// <summary> 出張手当 </summary>
    public AlternativeValue Travel => new AlternativeValue(travel);

    /// <summary> 住宅手当 </summary>
    public AlternativeValue Housing => new AlternativeValue(housing);

    /// <summary> 食事手当 </summary>
    public AlternativeValue Food => new AlternativeValue(food);

    /// <summary> 深夜手当 </summary>
    public AlternativeValue LateNight => new AlternativeValue(lateNight);

    /// <summary> 地域手当 </summary>
    public AlternativeValue Area => new AlternativeValue(area);

    /// <summary> 通勤手当 </summary>
    public AlternativeValue Commution => new AlternativeValue(commution);

    /// <summary> 前払退職金 </summary>
    public AlternativeValue PrepaidRetirement => new AlternativeValue(prepaidRetirement);

    /// <summary> 扶養手当 </summary>
    public AlternativeValue Dependency => new AlternativeValue(dependency);

    /// <summary> 役職手当 </summary>
    public AlternativeValue Executive => new AlternativeValue(executive);

    /// <summary> 特別手当 </summary>
    public AlternativeValue Special => new AlternativeValue(special);
}