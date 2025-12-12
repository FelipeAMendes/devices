using Devices.Shared.Validations;
using FluentValidation;

namespace Devices.Shared.Entities;

public abstract class Entity<T> : BaseValidation<T>, IEntity where T : IValidator, new()
{
    public Guid Id { get; set; }
    public DateTime CreationTime { get; set; }
    public bool Removed { get; set; }

    public Entity()
    {
        Id = Guid.NewGuid();
    }
}
