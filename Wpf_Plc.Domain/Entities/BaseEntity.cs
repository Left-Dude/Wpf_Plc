namespace Wpf_Plc.Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime? Created { get; set; } = DateTime.UtcNow;
    public DateTime? Modified { get; set; } = DateTime.UtcNow;
}