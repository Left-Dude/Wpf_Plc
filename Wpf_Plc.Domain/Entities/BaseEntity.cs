namespace Wpf_Plc.Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime? Created { get; set; } = DateTime.UtcNow;
    public DateTime? Modified { get; set; } = DateTime.UtcNow;
}