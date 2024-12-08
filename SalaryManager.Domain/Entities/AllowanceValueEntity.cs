namespace SalaryManager.Domain.Entities;

/// <summary>
/// Entity - 支給額
/// </summary>
/// <param name="id">ID</param>
/// <param name="yearMonth">年月</param>
/// <param name="basicSalary">基本給</param>
/// <param name="executiveAllowance">役職手当</param>
/// <param name="dependencyAllowance">扶養手当</param>
/// <param name="OvertimeAllowance">時間外手当</param>
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
public sealed class AllowanceValueEntity(
    int id,
    DateOnly yearMonth,
    double basicSalary,
    double executiveAllowance,
    double dependencyAllowance,
    double OvertimeAllowance,
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
    double totalDeductedSalary)
{
    /// <summary> ID </summary>
    public int ID => id;

    /// <summary> 年月 </summary>
    public DateOnly YearMonth => yearMonth;

    /// <summary> 基本給 </summary>
    public MoneyValue BasicSalary => new MoneyValue(basicSalary);

    /// <summary> 役職手当 </summary>
    public MoneyValue ExecutiveAllowance => new MoneyValue(executiveAllowance);

    /// <summary> 扶養手当 </summary>
    public MoneyValue DependencyAllowance => new MoneyValue(dependencyAllowance);

    /// <summary> 時間外手当 </summary>
    public MoneyValue OvertimeAllowance { get; } = new MoneyValue(OvertimeAllowance);

    /// <summary> 休日割増 </summary>
    public MoneyValue DaysoffIncreased => new MoneyValue(daysoffIncreased);

    /// <summary> 深夜割増 </summary>
    public MoneyValue NightworkIncreased => new MoneyValue(nightworkIncreased);

    /// <summary> 住宅手当 </summary>
    public MoneyValue HousingAllowance => new MoneyValue(housingAllowance);

    /// <summary> 遅刻早退欠勤 </summary>
    public double LateAbsent => lateAbsent;

    /// <summary> 交通費 </summary>
    public MoneyValue TransportationExpenses => new MoneyValue(transportationExpenses);

    /// <summary> 在宅手当 </summary>
    public MoneyValue ElectricityAllowance => new MoneyValue(electricityAllowance);

    /// <summary> 前払退職金 </summary>
    public MoneyValue PrepaidRetirementPayment => new MoneyValue(prepaidRetirementPayment);

    /// <summary> 特別手当 </summary>
    public double SpecialAllowance => specialAllowance;

    /// <summary> 予備 </summary>
    public double SpareAllowance => spareAllowance;

    /// <summary> 備考 </summary>
    public string Remarks => remarks;

    /// <summary> 支給総計 </summary>
    public MoneyValue TotalSalary => new MoneyValue(totalSalary);

    /// <summary> 差引支給額 </summary>
    public MoneyValue TotalDeductedSalary => new MoneyValue(totalDeductedSalary);

}