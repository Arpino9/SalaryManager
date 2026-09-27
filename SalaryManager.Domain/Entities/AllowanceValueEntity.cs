namespace SalaryManager.Domain.Entities;

/// <summary>
/// Entity - 支給額
/// </summary>
/// <param name="id">ID</param>
/// <param name="yearMonth">年月</param>
/// <param name="basicSalary">基本給</param>
/// <param name="executiveAllowance">役職手当</param>
/// <param name="dependencyAllowance">扶養手当</param>
/// <param name="overtimeAllowance">時間外手当</param>
/// <param name="daysoffIncreased">休日割増</param>
/// <param name="nightworkIncreased">深夜割増</param>
/// <param name="housingAllowance">住宅手当</param>
/// <param name="lateAbsent">遅刻早退欠勤</param>
/// <param name="transportationExpenses">交通費</param>
/// <param name="prepaidRetirementPayment">前払退職金</param>
/// <param name="electricityAllowance">在宅手当</param>
/// <param name="specialAllowance">特別手当</param>
/// <param name="spareAllowance">予備</param>
/// <param name="remarks">備考</param>
/// <param name="totalSalary">支給総計</param>
/// <param name="totalDeductedSalary">差引支給額</param>
[Table("Allowance")]
public sealed record class AllowanceValueEntity(
    int id,
    DateOnly yearMonth,
    double basicSalary,
    double executiveAllowance,
    double dependencyAllowance,
    double overtimeAllowance,
    double daysoffIncreased,
    double nightworkIncreased,
    double housingAllowance,
    double lateAbsent,
    double transportationExpenses,
    double prepaidRetirementPayment,
    double electricityAllowance,
    double specialAllowance,
    double spareAllowance,
    string remarks,
    double totalSalary,
    double totalDeductedSalary) : ITableEntity
{
    /// <summary> ID </summary>
    [Column("ID")]
    public int ID => id;

    /// <summary> 年月 </summary>
    [Column("YearMonth")]
    public DateOnly YearMonth => yearMonth;

    /// <summary> 基本給 </summary>
    [Column("BasicSalary")]
    public MoneyValue BasicSalary => new MoneyValue(basicSalary);

    /// <summary> 役職手当 </summary>
    [Column("ExecutiveAllowance")]
    public MoneyValue ExecutiveAllowance => new MoneyValue(executiveAllowance);

    /// <summary> 扶養手当 </summary>
    [Column("DependencyAllowance")]
    public MoneyValue DependencyAllowance => new MoneyValue(dependencyAllowance);

    /// <summary> 時間外手当 </summary>
    [Column("OvertimeAllowance")]
    public MoneyValue OvertimeAllowance => new MoneyValue(overtimeAllowance);

    /// <summary> 休日割増 </summary>
    [Column("DaysoffIncreased")]
    public MoneyValue DaysoffIncreased => new MoneyValue(daysoffIncreased);

    /// <summary> 深夜割増 </summary>
    [Column("NightworkIncreased")]
    public MoneyValue NightworkIncreased => new MoneyValue(nightworkIncreased);

    /// <summary> 住宅手当 </summary>
    [Column("HousingAllowance")]
    public MoneyValue HousingAllowance => new MoneyValue(housingAllowance);

    /// <summary> 遅刻早退欠勤 </summary>
    [Column("LateAbsent")]
    public double LateAbsent => lateAbsent;

    /// <summary> 交通費 </summary>
    [Column("TransportationExpenses")]
    public MoneyValue TransportationExpenses => new MoneyValue(transportationExpenses);

    /// <summary> 在宅手当 </summary>
    [Column("ElectricityAllowance")]
    public MoneyValue ElectricityAllowance => new MoneyValue(electricityAllowance);

    /// <summary> 前払退職金 </summary>
    [Column("PrepaidRetirementPayment")]
    public MoneyValue PrepaidRetirementPayment => new MoneyValue(prepaidRetirementPayment);

    /// <summary> 特別手当 </summary>
    [Column("SpecialAllowance")]
    public double SpecialAllowance => specialAllowance;

    /// <summary> 予備 </summary>
    [Column("SpareAllowance")]
    public double SpareAllowance => spareAllowance;

    /// <summary> 備考 </summary>
    [Column("Remarks")]
    public string Remarks => remarks;

    /// <summary> 支給総計 </summary>
    [Column("TotalSalary")]
    public MoneyValue TotalSalary => new MoneyValue(totalSalary);

    /// <summary> 差引支給額 </summary>
    [Column("TotalDeductedSalary")]
    public MoneyValue TotalDeductedSalary => new MoneyValue(totalDeductedSalary);
}
