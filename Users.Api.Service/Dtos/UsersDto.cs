using System.ComponentModel.DataAnnotations;
using Destructurama.Attributed;
using Jubatus.Common;

namespace Users.Api.Service.Dtos;

/// <summary>
/// 
/// </summary>
public record AuthUserDto
{
    [Required]
    public string UserName { get; init; } = string.Empty;

    [Required]
    [NotLogged]
    public string Password { get; init; } = string.Empty;
}

/// <summary>
/// 
/// </summary>
public record UsersDto : IEntity
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

/// <summary>
/// 
/// </summary>
public record NewUsersDto
{
    [Required(ErrorMessage = ApiMessages.ALIASISREQUIRED)]
    [MinLength(8, ErrorMessage = ApiMessages.ALIASMINSIZE)]
    [MaxLength(16, ErrorMessage = ApiMessages.ALIASMAXSIZE)]
    public string? Alias { get; init; }

    public string? FirstName { get; init; }
    public string? LastName { get; init; }

    [NotLogged]
    [Required(ErrorMessage = ApiMessages.PASSWORDISREQUIRED)]
    [MinLength(8, ErrorMessage = ApiMessages.PASSWORDMINSIZE)]
    [MaxLength(16, ErrorMessage = ApiMessages.PASSWORDMAXSIZE)]
    public string? Password { get; init; }

    public bool IsActive { get; init; } = true;
}

/// <summary>
/// 
/// </summary>
public record UpdUsersDto
{
    [Required(ErrorMessage = ApiMessages.ALIASISREQUIRED)]
    [MinLength(8, ErrorMessage = ApiMessages.ALIASMINSIZE)]
    [MaxLength(16, ErrorMessage = ApiMessages.ALIASMAXSIZE)]
    public string? Alias { get; init; }

    public string? FirstName { get; init; }
    public string? LastName { get; init; }

    [NotLogged]
    [Required(ErrorMessage = ApiMessages.PASSWORDISREQUIRED)]
    [MinLength(8, ErrorMessage = ApiMessages.PASSWORDMINSIZE)]
    [MaxLength(16, ErrorMessage = ApiMessages.PASSWORDMAXSIZE)]
    public string? Password { get; init; }

    public bool IsActive { get; init; } = true;
};
