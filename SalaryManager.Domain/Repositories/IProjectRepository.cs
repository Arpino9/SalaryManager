namespace SalaryManager.Domain.Repositories;

public interface IProjectRepository
{
    ProjectEntity GetEntity(int id);

    IReadOnlyList<ProjectEntity> GetEntities();

    void Save(ProjectEntity entity);

    void Delete(int id);
}
