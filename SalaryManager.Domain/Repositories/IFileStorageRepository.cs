using SalaryManager.Domain.Entities;
using System.Collections.Generic;

namespace SalaryManager.Domain.Repositories
{
    /// <summary>
    /// Repository - 添付ファイル
    /// </summary>
    public interface IFileStorageRepository
    {
        IReadOnlyList<FileStorageEntity> GetEntities();

        void Save(FileStorageEntity entity);

        void Delete(int id);
    }
}
