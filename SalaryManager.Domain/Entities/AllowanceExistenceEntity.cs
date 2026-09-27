namespace SalaryManager.Domain.Entities;

/// <summary>
/// Entity - 手当有無
/// </summary>
/// <param name="id">皆勤手当</param>
/// <param name="perfectAttendance">皆勤手当</param>
/// <param name="education">教育手当</param>
/// <param name="electricity">在宅手当</param>
/// <param name="certification">資格手当</param>
/// <param name="overtime">時間外手当</param>
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
[Table("Allowance")]
public sealed record class AllowanceExistenceEntity(
    int id,
    bool perfectAttendance,
    bool education,
    bool electricity,
    bool certification,
    bool overtime,
    bool travel,
    bool housing,
    bool food,
    bool lateNight,
    bool area,
    bool commution,
    bool prepaidRetirement,
    bool dependency,
    bool executive,
    bool special) : ITableEntity
{
    /// <summary> 皆勤手当 </summary>
    [Column("ID")]
    public int ID => id;

    /// <summary> 皆勤手当 </summary>
    [Column("PerfectAttendance")]
    public AlternativeValue PerfectAttendance => new AlternativeValue(perfectAttendance);

    /// <summary> 教育手当 </summary>
    [Column("Education")]
    public AlternativeValue Education => new AlternativeValue(education);

    /// <summary> 在宅手当 </summary>
    [Column("Electricity")]
    public AlternativeValue Electricity => new AlternativeValue(electricity);

    /// <summary> 資格手当 </summary>
    [Column("Certification")]
    public AlternativeValue Certification => new AlternativeValue(certification);

    /// <summary> 時間外手当 </summary>
    [Column("Overtime")]
    public AlternativeValue Overtime => new AlternativeValue(overtime);

    /// <summary> 出張手当 </summary>
    [Column("Travel")]
    public AlternativeValue Travel => new AlternativeValue(travel);

    /// <summary> 住宅手当 </summary>
    [Column("Housing")]
    public AlternativeValue Housing => new AlternativeValue(housing);

    /// <summary> 食事手当 </summary>
    [Column("Food")]
    public AlternativeValue Food => new AlternativeValue(food);

    /// <summary> 深夜手当 </summary>
    [Column("LateNight")]
    public AlternativeValue LateNight => new AlternativeValue(lateNight);

    /// <summary> 地域手当 </summary>
    [Column("Area")]
    public AlternativeValue Area => new AlternativeValue(area);

    /// <summary> 通勤手当 </summary>
    [Column("Commution")]
    public AlternativeValue Commution => new AlternativeValue(commution);

    /// <summary> 前払退職金 </summary>
    [Column("PrepaidRetirement")]
    public AlternativeValue PrepaidRetirement => new AlternativeValue(prepaidRetirement);

    /// <summary> 扶養手当 </summary>
    [Column("Dependency")]
    public AlternativeValue Dependency => new AlternativeValue(dependency);

    /// <summary> 役職手当 </summary>
    [Column("Executive")]
    public AlternativeValue Executive => new AlternativeValue(executive);

    /// <summary> 特別手当 </summary>
    [Column("Special")]
    public AlternativeValue Special => new AlternativeValue(special);
}
