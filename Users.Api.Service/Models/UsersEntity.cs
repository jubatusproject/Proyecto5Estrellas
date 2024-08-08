using Jubatus.Common;

namespace Users.Api.Service.Models;

public record UsersEntity : IEntity
{
    public Guid Id { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Alias { get; init; }
    public string? Password { get; init; }
    public bool IsActive { get; init; }
}