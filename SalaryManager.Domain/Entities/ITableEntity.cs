using System.ComponentModel.DataAnnotations.Schema;

namespace SalaryManager.Domain.Entities;

/// <summary>
/// Interface - Entity - Table
/// </summary>
internal interface ITableEntity : IEntity
{
    /// <summary> ID </summary>
    [Column("ID")]
    public int ID { get; }
}
