using SalaryManager.Domain.ValueObjects;

namespace SalaryManager.Domain.Entities
{
    /// <summary>
    /// Entity - 手当有無
    /// </summary>
    public class AllowanceExistenceEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="perfectAttendanceAllowance">皆勤手当</param>
        /// <param name="educationAllowance">教育手当</param>
        /// <param name="electricityAllowance">在宅手当</param>
        /// <param name="certificationAllowance">資格手当</param>
        /// <param name="overtimeAllowance">時間外手当</param>
        /// <param name="travelAllowance">出張手当</param>
        /// <param name="housingAllowance">住宅手当</param>
        /// <param name="foodAllowance">食事手当</param>
        /// <param name="lateNightAllowance">深夜手当</param>
        /// <param name="areaAllowance">地域手当</param>
        /// <param name="commutionAllowance">通勤手当</param>
        /// <param name="dependencyAllowance">扶養手当</param>
        /// <param name="executiveAllowance">役職手当</param>
        /// <param name="specialAllowance">特別手当</param>
        public AllowanceExistenceEntity(
            bool perfectAttendanceAllowance,
            bool educationAllowance,
            bool electricityAllowance,
            bool certificationAllowance,
            bool overtimeAllowance,
            bool travelAllowance,
            bool housingAllowance,
            bool foodAllowance,
            bool lateNightAllowance,
            bool areaAllowance,
            bool commutionAllowance,
            bool dependencyAllowance,
            bool executiveAllowance,
            bool specialAllowance)
        {
            this.PerfectAttendanceAllowance = new AlternativeValue(perfectAttendanceAllowance);
            this.EducationAllowance         = new AlternativeValue(educationAllowance);
            this.ElectricityAllowance       = new AlternativeValue(electricityAllowance);
            this.CertificationAllowance     = new AlternativeValue(certificationAllowance);
            this.OvertimeAllowance          = new AlternativeValue(overtimeAllowance);
            this.TravelAllowance            = new AlternativeValue(travelAllowance);
            this.HousingAllowance           = new AlternativeValue(housingAllowance);
            this.FoodAllowance              = new AlternativeValue(foodAllowance);
            this.LateNightAllowance         = new AlternativeValue(lateNightAllowance);
            this.AreaAllowance              = new AlternativeValue(areaAllowance);
            this.CommutionAllowance         = new AlternativeValue(commutionAllowance);
            this.DependencyAllowance        = new AlternativeValue(dependencyAllowance);
            this.ExecutiveAllowance         = new AlternativeValue(executiveAllowance);
            this.SpecialAllowance           = new AlternativeValue(specialAllowance);
        }

        /// <summary> 皆勤手当 </summary>
        public AlternativeValue PerfectAttendanceAllowance { get; }

        /// <summary> 教育手当 </summary>
        public AlternativeValue EducationAllowance { get; }

        /// <summary> 在宅手当 </summary>
        public AlternativeValue ElectricityAllowance { get; }

        /// <summary> 資格手当 </summary>
        public AlternativeValue CertificationAllowance { get; }

        /// <summary> 時間外手当 </summary>
        public AlternativeValue OvertimeAllowance { get; }

        /// <summary> 出張手当 </summary>
        public AlternativeValue TravelAllowance { get; }

        /// <summary> 住宅手当 </summary>
        public AlternativeValue HousingAllowance { get; }

        /// <summary> 食事手当 </summary>
        public AlternativeValue FoodAllowance { get; }

        /// <summary> 深夜手当 </summary>
        public AlternativeValue LateNightAllowance { get; }

        /// <summary> 地域手当 </summary>
        public AlternativeValue AreaAllowance { get; }

        /// <summary> 通勤手当 </summary>
        public AlternativeValue CommutionAllowance { get; }

        /// <summary> 扶養手当 </summary>
        public AlternativeValue DependencyAllowance { get; }

        /// <summary> 役職手当 </summary>
        public AlternativeValue ExecutiveAllowance { get; }

        /// <summary> 特別手当 </summary>
        public AlternativeValue SpecialAllowance { get; }
    }
}
