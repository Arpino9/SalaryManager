namespace SalaryManager.Infrastructure.SQLite;

public class ProjectSQLite : IProjectRepository
{
    public IReadOnlyList<ProjectEntity> GetEntities()
    {
        string sql = @"
SELECT ID, 
CompanyName,
ProjectName, 
StartDate,
EndDate,
SystemName,
Language,
Database,
DevelopmentSupportingTool,
OtherTools,
Role,
Member,
AssignedPhrase_RequireDefinition,
AssignedPhrase_BasicDesign,
AssignedPhrase_DetailDesign,
AssignedPhrase_Development,
AssignedPhrase_UnitTest,
AssignedPhrase_IntegrationTest,
AssignedPhrase_SystemTest,
AssignedPhrase_OperationTest,
AssignedPhrase_Other,
Contents,
Remarks
from Project";

        return SQLiteHelper.Query(
            sql,
            reader =>
            {
                return new ProjectEntity(
                            Convert.ToInt32(reader["ID"]),
                            Convert.ToString(reader["CompanyName"]),
                            Convert.ToString(reader["ProjectName"]),
                            Convert.ToDateTime(reader["StartDate"]),
                            Convert.ToDateTime(reader["EndDate"]),
                            Convert.ToString(reader["SystemName"]),
                            Convert.ToString(reader["Language"]),
                            Convert.ToString(reader["Database"]),
                            Convert.ToString(reader["DevelopmentSupportingTool"]),
                            Convert.ToString(reader["OtherTools"]),
                            Convert.ToString(reader["Role"]),
                            Convert.ToString(reader["Member"]),
                            Convert.ToBoolean(reader["AssignedPhrase_RequireDefinition"]),
                            Convert.ToBoolean(reader["AssignedPhrase_BasicDesign"]),
                            Convert.ToBoolean(reader["AssignedPhrase_DetailDesign"]),
                            Convert.ToBoolean(reader["AssignedPhrase_Development"]),
                            Convert.ToBoolean(reader["AssignedPhrase_UnitTest"]),
                            Convert.ToBoolean(reader["AssignedPhrase_IntegrationTest"]),
                            Convert.ToBoolean(reader["AssignedPhrase_SystemTest"]),
                            Convert.ToBoolean(reader["AssignedPhrase_OperationTest"]),
                            Convert.ToBoolean(reader["AssignedPhrase_Other"]),
                            Convert.ToString(reader["Contents"]),
                            Convert.ToString(reader["Remarks"]));
            });
    }

    public ProjectEntity GetEntity(int id)
    {
        string sql = @"
SELECT ID, 
CompanyName,
ProjectName, 
StartDate,
EndDate,
SystemName,
Language,
Database,
DevelopmentSupportingTool,
OtherTools,
Role,
Member,
AssignedPhrase_RequireDefinition,
AssignedPhrase_BasicDesign,
AssignedPhrase_DetailDesign,
AssignedPhrase_Development,
AssignedPhrase_UnitTest,
AssignedPhrase_IntegrationTest,
AssignedPhrase_SystemTest,
AssignedPhrase_OperationTest,
AssignedPhrase_Other,
Contents,
Remarks
FROM Project
Where ID = @ID";

        var args = new List<SQLiteParameter>()
        {
            new SQLiteParameter("ID", id),
        };

        return SQLiteHelper.QuerySingle<ProjectEntity>(
            sql,
            args.ToArray(),
            reader =>
            {
                return new ProjectEntity(
                            Convert.ToInt32(reader["ID"]),
                            Convert.ToString(reader["CompanyName"]),
                            Convert.ToString(reader["ProjectName"]),
                            Convert.ToDateTime(reader["StartDate"]),
                            Convert.ToDateTime(reader["EndDate"]),
                            Convert.ToString(reader["SystemName"]),
                            Convert.ToString(reader["Language"]),
                            Convert.ToString(reader["Database"]),
                            Convert.ToString(reader["DevelopmentSupportingTool"]),
                            Convert.ToString(reader["OtherTools"]),
                            Convert.ToString(reader["Role"]),
                            Convert.ToString(reader["Member"]),
                            Convert.ToBoolean(reader["AssignedPhrase_RequireDefinition"]),
                            Convert.ToBoolean(reader["AssignedPhrase_BasicDesign"]),
                            Convert.ToBoolean(reader["AssignedPhrase_DetailDesign"]),
                            Convert.ToBoolean(reader["AssignedPhrase_Development"]),
                            Convert.ToBoolean(reader["AssignedPhrase_UnitTest"]),
                            Convert.ToBoolean(reader["AssignedPhrase_IntegrationTest"]),
                            Convert.ToBoolean(reader["AssignedPhrase_SystemTest"]),
                            Convert.ToBoolean(reader["AssignedPhrase_OperationTest"]),
                            Convert.ToBoolean(reader["AssignedPhrase_Other"]),
                            Convert.ToString(reader["Contents"]),
                            Convert.ToString(reader["Remarks"]));
            },
            null);
    }

    public void Save(ProjectEntity entity)
    {
        string insert = @"
insert into Project
(ID, 
CompanyName,
ProjectName, 
StartDate,
EndDate,
SystemName,
Language,
Database,
DevelopmentSupportingTool,
OtherTools,
Role,
Member,
AssignedPhrase_RequireDefinition,
AssignedPhrase_BasicDesign,
AssignedPhrase_DetailDesign,
AssignedPhrase_Development,
AssignedPhrase_UnitTest,
AssignedPhrase_IntegrationTest,
AssignedPhrase_SystemTest,
AssignedPhrase_OperationTest,
AssignedPhrase_Other,
Contents,
Remarks)
values
(@ID, 
@CompanyName, 
@ProjectName, 
@StartDate, 
@EndDate, 
@SystemName, 
@Language, 
@Database, 
@DevelopmentSupportingTool,
@OtherTools, 
@Role, 
@Member, 
@AssignedPhrase_RequireDefinition, 
@AssignedPhrase_BasicDesign, 
@AssignedPhrase_DetailDesign, 
@AssignedPhrase_Development, 
@AssignedPhrase_UnitTest, 
@AssignedPhrase_IntegrationTest, 
@AssignedPhrase_SystemTest, 
@AssignedPhrase_OperationTest, 
@AssignedPhrase_Other, 
@Contents, 
@Remarks)
";

        string update = @"
update Project
set ID             = @ID, 
    CompanyName    = @CompanyName, 
    ProjectName    = @ProjectName, 
    StartDate      = @StartDate, 
    EndDate        = @EndDate, 
    SystemName     = @SystemName, 
    Language       = @Language, 
    Database       = @Database, 
    DevelopmentSupportingTool = @DevelopmentSupportingTool, 
    OtherTools     = @OtherTools, 
    Role           = @Role, 
    Member         = @Member, 
    AssignedPhrase_RequireDefinition = @AssignedPhrase_RequireDefinition, 
    AssignedPhrase_BasicDesign = @AssignedPhrase_BasicDesign, 
    AssignedPhrase_DetailDesign = @AssignedPhrase_DetailDesign, 
    AssignedPhrase_Development = @AssignedPhrase_Development, 
    AssignedPhrase_UnitTest = @AssignedPhrase_UnitTest, 
    AssignedPhrase_IntegrationTest = @AssignedPhrase_IntegrationTest, 
    AssignedPhrase_SystemTest = @AssignedPhrase_SystemTest, 
    AssignedPhrase_OperationTest = @AssignedPhrase_OperationTest, 
    AssignedPhrase_Other = @AssignedPhrase_Other, 
    Contents       = @Contents, 
    Remarks        = @Remarks
where ID = @ID
";

        var args = new List<SQLiteParameter>()
        {
            new SQLiteParameter("ID",             entity.ID),
            new SQLiteParameter("CompanyName",    entity.CompanyName),
            new SQLiteParameter("ProjectName",    entity.ProjectName),
            new SQLiteParameter("StartDate",      entity.StartDate.Value),
            new SQLiteParameter("EndDate",        entity.EndDate.Value),
            new SQLiteParameter("SystemName",     entity.SystemName),
            new SQLiteParameter("Language",       entity.Language),
            new SQLiteParameter("Database",       entity.Database),
            new SQLiteParameter("DevelopmentSupportingTool",       entity.DevelopmentSupportingTool),
            new SQLiteParameter("OtherTools",     entity.OtherTools),
            new SQLiteParameter("Role",           entity.Role),
            new SQLiteParameter("Member",         entity.Member),
            new SQLiteParameter("AssignedPhrase_RequireDefinition", entity.AssignedPhrase_RequireDefinition.Value),
            new SQLiteParameter("AssignedPhrase_BasicDesign", entity.AssignedPhrase_BasicDesign.Value),
            new SQLiteParameter("AssignedPhrase_DetailDesign", entity.AssignedPhrase_DetailDesign.Value),
            new SQLiteParameter("AssignedPhrase_Development", entity.AssignedPhrase_Development.Value),
            new SQLiteParameter("AssignedPhrase_UnitTest", entity.AssignedPhrase_UnitTest.Value),
            new SQLiteParameter("AssignedPhrase_IntegrationTest", entity.AssignedPhrase_IntegrationTest.Value),
            new SQLiteParameter("AssignedPhrase_SystemTest", entity.AssignedPhrase_SystemTest.Value),
            new SQLiteParameter("AssignedPhrase_OperationTest", entity.AssignedPhrase_OperationTest.Value),
            new SQLiteParameter("AssignedPhrase_Other", entity.AssignedPhrase_Other.Value),
            new SQLiteParameter("Contents",       entity.Contents),
            new SQLiteParameter("Remarks",        entity.Remarks),
        };

        SQLiteHelper.Execute(insert, update, args.ToArray());
    }

    public void Delete(int id)
    {
        string delete = @"
Delete From Project
where ID = @ID
";

        var args = new List<SQLiteParameter>()
        {
            new SQLiteParameter("ID", id),
        };

        SQLiteHelper.Execute(delete, args.ToArray());
    }
}
