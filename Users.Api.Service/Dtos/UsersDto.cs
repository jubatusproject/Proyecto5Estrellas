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
    [Required(ErrorMessage = ApiMessages.AliasIsRequired)]
    [MinLength(8, ErrorMessage = ApiMessages.AliasMinSize)]
    [MaxLength(16, ErrorMessage = ApiMessages.AliasMaxSize)]
    public string? Alias { get; init; }

    public string? FirstName { get; init; }
    public string? LastName { get; init; }

    [NotLogged]
    [Required(ErrorMessage = ApiMessages.PasswordIsRequired)]
    [MinLength(8, ErrorMessage = ApiMessages.PasswordMinSize)]
    [MaxLength(16, ErrorMessage = ApiMessages.PasswordMaxSize)]
    public string? Password { get; init; }

    public bool IsActive { get; init; } = true;
}

/// <summary>
/// 
/// </summary>
public record UpdUsersDto
{
    [Required(ErrorMessage = ApiMessages.AliasIsRequired)]
    [MinLength(8, ErrorMessage = ApiMessages.AliasMinSize)]
    [MaxLength(16, ErrorMessage = ApiMessages.AliasMaxSize)]
    public string? Alias { get; init; }

    public string? FirstName { get; init; }
    public string? LastName { get; init; }

    [NotLogged]
    [Required(ErrorMessage = ApiMessages.PasswordIsRequired)]
    [MinLength(8, ErrorMessage = ApiMessages.PasswordMinSize)]
    [MaxLength(16, ErrorMessage = ApiMessages.PasswordMaxSize)]
    public string? Password { get; init; }

    public bool IsActive { get; init; } = true;
};
