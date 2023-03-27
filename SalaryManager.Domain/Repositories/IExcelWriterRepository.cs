using SalaryManager.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalaryManager.Domain.Repositories
{
    /// <summary>
    /// Repository - Excel書き込み
    /// </summary>
    public interface IExcelWriterRepository
    {
        /// <summary>
        /// Workbook保存
        /// </summary>
        /// <param name="directory">ディレクトリ</param>
        void CopyAsWorkbook(string directory);

        /// <summary>
        /// Write - ヘッダ
        /// </summary>
        /// <param name="entities">エンティティ</param>
        /// <returns>void</returns>
        Task WriteAllHeader(IReadOnlyList<HeaderEntity> entities);

        /// <summary>
        /// Write - 支給額
        /// </summary>
        /// <param name="entities">エンティティ</param>
        /// <returns>void</returns>
        Task WriteAllAllowance(IReadOnlyList<AllowanceValueEntity> entities);

        /// <summary>
        /// Write - 控除額
        /// </summary>
        /// <param name="entities">エンティティ</param>
        /// <returns>void</returns>
        Task WriteAllDeduction(IReadOnlyList<DeductionEntity> entities);

        /// <summary>
        /// Write - 勤務備考
        /// </summary>
        /// <param name="entities">エンティティ</param>
        /// <returns>void</returns>
        Task WriteAllWorkingReferences(IReadOnlyList<WorkingReferencesEntity> entities);

        /// <summary>
        /// Write - 副業
        /// </summary>
        /// <param name="entities">エンティティ</param>
        /// <returns>void</returns>
        Task WriteAllSideBusiness(IReadOnlyList<SideBusinessEntity> entities);

        /// <summary>
        /// スタイルの設定
        /// </summary>
        /// <returns>void</returns>
        Task SetStyle();
    }
}
