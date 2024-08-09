using System.ComponentModel.DataAnnotations;
using Jubatus.Common;

namespace Users.Api.Service.Dtos;

/// <summary>
/// 
/// </summary>
public record UsersDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Alias,
    string Password,
    bool IsActive
) : IEntity;

/// <summary>
/// 
/// </summary>
public record NewUsersDto(
    [Required(ErrorMessage = UserMessages.ALIASISREQUIRED)]
    [MinLength(8, ErrorMessage = UserMessages.ALIASMINSIZE)]
    [MaxLength(16, ErrorMessage = UserMessages.ALIASMAXSIZE)]
    string Alias,

    string FirstName,
    string LastName,

    [Required(ErrorMessage = UserMessages.PASSWORDISREQUIRED)]
    [MinLength(8, ErrorMessage = UserMessages.PASSWORDMINSIZE)]
    [MaxLength(16, ErrorMessage = UserMessages.PASSWORDMAXSIZE)]
    string Password,

    bool IsActive = true
);

/// <summary>
/// 
/// </summary>
public record UpdUsersDto(
    [Required(ErrorMessage = UserMessages.ALIASISREQUIRED)]
    [MinLength(8, ErrorMessage = UserMessages.ALIASMINSIZE)]
    [MaxLength(16, ErrorMessage = UserMessages.ALIASMAXSIZE)]
    string Alias,

    string FirstName,
    string LastName,

    [Required(ErrorMessage = UserMessages.PASSWORDISREQUIRED)]
    [MinLength(8, ErrorMessage = UserMessages.PASSWORDMINSIZE)]
    [MaxLength(16, ErrorMessage = UserMessages.PASSWORDMAXSIZE)]
    string Password,

    bool IsActive = true
);
