using SalaryManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryManager.Domain.Repositories
{
    public interface IFileStorageRepository
    {
        IReadOnlyList<FileStorageEntity> GetEntities();

        void Save(FileStorageEntity entity);

        void Delete(int id);
    }
}
