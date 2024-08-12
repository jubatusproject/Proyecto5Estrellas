using Destructurama.Attributed;
using Jubatus.Common;

namespace Users.Api.Service.Models;

public record UsersEntity : IEntity
{
    [NotLogged]
    public Guid Id { get; init; }

    public string? FirstName { get; init; }

    public string? LastName { get; init; }

    public string? Alias { get; init; }

    [NotLogged]
    public string? Password { get; init; }

    public bool IsActive { get; init; }
}