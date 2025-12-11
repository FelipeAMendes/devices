namespace Devices.Shared.Entities;

public interface IEntity
{
    public Guid Id { get; set; }
    public DateTime CreationTime { get; set; }
    public bool Removed { get; set; }
}
