using Users.Api.Service.Dtos;
using Users.Api.Service.Models;

namespace Users.Api.Service;

public static class UsersExtensions
{
    public static UsersDto AsUsersDto(this UsersEntity usersEntity)
    {
        ArgumentNullException.ThrowIfNull(usersEntity);
        return new UsersDto(usersEntity.Id, usersEntity.FirstName!, usersEntity.LastName!, usersEntity.Alias!, usersEntity.Password!, usersEntity.IsActive);
    }
}