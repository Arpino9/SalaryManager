namespace SalaryManager.Domain.Entities;

/// <summary>
/// Entity - プロジェクト
/// </summary>
/// <param name="id">ID</param>
/// <param name="companyName">会社名</param>
/// <param name="projectName">プロジェクト名</param>
/// <param name="startDate">開始日</param>
/// <param name="endDate">終了日</param>
/// <param name="systemName">システム名</param>
/// <param name="language">言語</param>
/// <param name="database">データベース</param>
/// <param name="developmenSupportingTool">開発支援ツール</param>
/// <param name="otherTools">その他のツール</param>
/// <param name="role">役割</param>
/// <param name="member">メンバー</param>
/// <param name="assignedPhrase_RequireDefinition">要件定義担当</param>
/// <param name="assignedPhrase_BasicDesign">基本設計担当</param>
/// <param name="assignedPhrase_DetailDesign">詳細設計担当</param>
/// <param name="assignedPhrase_Development">開発担当</param>
/// <param name="assignedPhrase_UnitTest">単体テスト担当</param>
/// <param name="assignedPhrase_IntegrationTest">統合テスト担当</param>
/// <param name="assignedPhrase_SystemTest">システムテスト担当</param>
/// <param name="assignedPhrase_OperationTest">運用テスト担当</param>
/// <param name="assignedPhrase_Other">その他</param>
/// <param name="contents">内容</param>
/// <param name="remarks">備考</param>
[Table("Project")]
public sealed record class ProjectEntity(
    int id,
    string companyName,
    string projectName,
    DateTime startDate,
    DateTime endDate,
    string systemName,
    string language,
    string database,
    string developmenSupportingTool,
    string otherTools,
    string role,
    string member,
    bool assignedPhrase_RequireDefinition,
    bool assignedPhrase_BasicDesign,
    bool assignedPhrase_DetailDesign,
    bool assignedPhrase_Development,
    bool assignedPhrase_UnitTest,
    bool assignedPhrase_IntegrationTest,
    bool assignedPhrase_SystemTest,
    bool assignedPhrase_OperationTest,
    bool assignedPhrase_Other,
    string contents,
    string remarks) : ITableEntity
{
    /// <summary> ID </summary>
    [Column("ID")]
    public int ID => id;
    
    /// <summary> 会社名 </summary>
    [Column("CompanyName")]
    public string CompanyName => companyName;
    
    /// <summary> プロジェクト名 </summary>
    [Column("ProjectName")]
    public string ProjectName => projectName;

    /// <summary> 開始日 </summary>
    [Column("StartDate")]
    public WorkingDateValue StartDate => new WorkingDateValue(startDate);
    
    /// <summary> 終了日 </summary>
    [Column("EndDate")]
    public WorkingDateValue EndDate => new WorkingDateValue(endDate);
    
    /// <summary> システム名 </summary>
    [Column("SystemName")]
    public string SystemName => systemName;
    
    /// <summary> 言語 </summary>
    [Column("Language")]
    public string Language => language;
    
    /// <summary> データベース </summary>
    [Column("Database")]
    public string Database => database;

    /// <summary> 開発支援ツール </summary>
    [Column("DevelopmentSupportingTool")]
    public string DevelopmentSupportingTool => developmenSupportingTool;
    
    /// <summary> その他ツール </summary>
    [Column("OtherTools")]
    public string OtherTools => otherTools;
    
    /// <summary> 役割 </summary>
    [Column("Role")]
    public string Role => role;

    /// <summary> メンバー </summary>
    [Column("Member")]
    public string Member => member;

    /// <summary> 担当工程 - 要件定義 </summary>
    [Column("AssignedPhrase_RequireDefinition")]
    public AlternativeValue AssignedPhrase_RequireDefinition => new AlternativeValue(assignedPhrase_RequireDefinition);

    /// <summary> 担当工程 - 基本設計 </summary>
    [Column("AssignedPhrase_BasicDesign")]
    public AlternativeValue AssignedPhrase_BasicDesign => new AlternativeValue(assignedPhrase_BasicDesign);

    /// <summary> 担当工程 - 詳細設計 </summary>
    [Column("AssignedPhrase_DetailDesign")]
    public AlternativeValue AssignedPhrase_DetailDesign => new AlternativeValue(assignedPhrase_DetailDesign);

    /// <summary> 担当工程 - 開発 </summary>
    [Column("AssignedPhrase_Development")]
    public AlternativeValue AssignedPhrase_Development => new AlternativeValue(assignedPhrase_Development);

    /// <summary> 担当工程 - 単体テスト </summary>
    [Column("AssignedPhrase_UnitTest")]
    public AlternativeValue AssignedPhrase_UnitTest => new AlternativeValue(assignedPhrase_UnitTest);

    /// <summary> 担当工程 - 統合テスト </summary>
    [Column("AssignedPhrase_IntegrationTest")]
    public AlternativeValue AssignedPhrase_IntegrationTest => new AlternativeValue(assignedPhrase_IntegrationTest);

    /// <summary> 担当工程 - システムテスト </summary>
    [Column("AssignedPhrase_SystemTest")]
    public AlternativeValue AssignedPhrase_SystemTest => new AlternativeValue(assignedPhrase_SystemTest);
    
    /// <summary> 担当工程 - 運用テスト </summary>
    [Column("AssignedPhrase_OperationTest")]
    public AlternativeValue AssignedPhrase_OperationTest => new AlternativeValue(assignedPhrase_OperationTest);
    
    /// <summary> 担当工程 - その他 </summary>
    [Column("AssignedPhrase_Other")]
    public AlternativeValue AssignedPhrase_Other => new AlternativeValue(assignedPhrase_Other);

    /// <summary> 業務内容 </summary>
    [Column("Contents")]
    public string Contents => contents;
    
    /// <summary> 備考 </summary>
    [Column("Remarks")]
    public string Remarks => remarks;
}
