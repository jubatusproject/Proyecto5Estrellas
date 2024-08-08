using System.ComponentModel.DataAnnotations;
using Jubatus.Common;

namespace Users.Api.Service.Dtos;

public record UsersDto(Guid Id, string FirstName, string LastName, string Alias, string Password, bool IsActive) : IEntity;

public record NewUsersDto(
    [Required(ErrorMessage = UserStrings.ALIASISREQUIRED)]
    [MinLength(8, ErrorMessage = UserStrings.ALIASMINSIZE)]
    [MaxLength(16, ErrorMessage = UserStrings.ALIASMAXSIZE)]
    string Alias,

    string FirstName,
    string LastName,

    [Required(ErrorMessage = UserStrings.PASSWORDISREQUIRED)]
    [MinLength(8, ErrorMessage = UserStrings.PASSWORDMINSIZE)]
    [MaxLength(16, ErrorMessage = UserStrings.PASSWORDMAXSIZE)]
    string Password,

    bool IsActive = true
);

public record UpdUsersDto(
    [Required(ErrorMessage = UserStrings.ALIASISREQUIRED)]
    [MinLength(8, ErrorMessage = UserStrings.ALIASMINSIZE)]
    [MaxLength(16, ErrorMessage = UserStrings.ALIASMAXSIZE)]
    string Alias,

    string FirstName,
    string LastName,

    [Required(ErrorMessage = UserStrings.PASSWORDISREQUIRED)]
    [MinLength(8, ErrorMessage = UserStrings.PASSWORDMINSIZE)]
    [MaxLength(16, ErrorMessage = UserStrings.PASSWORDMAXSIZE)]
    string Password,

    bool IsActive = true
);
