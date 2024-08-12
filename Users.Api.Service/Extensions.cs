using System.Security.Cryptography;
using System.Text;
using Users.Api.Service.Dtos;
using Users.Api.Service.Models;

namespace Users.Api.Service;

public static class UsersExtensions
{
    public static UsersDto AsUsersDto(this UsersEntity usersEntity)
    {
        ArgumentNullException.ThrowIfNull(usersEntity);

        return new UsersDto()
        {
            Id = usersEntity.Id,
            FirstName = usersEntity.FirstName!,
            LastName = usersEntity.LastName!,
            Alias = usersEntity.Alias!,
            Password = usersEntity.Password!,
            IsActive = usersEntity.IsActive
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="plainText"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static async Task<string> EncryptUserPassword(this string plainText, string key)
    {
        byte[] iv = new byte[16];
        byte[] array;

        using (var aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;

#pragma warning disable CA5401 // Do not use CreateEncryptor with non-default IV
#pragma warning disable S3329 // Cipher Block Chaining IVs should be unpredictable
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
#pragma warning restore S3329 // Cipher Block Chaining IVs should be unpredictable
#pragma warning restore CA5401 // Do not use CreateEncryptor with non-default IV

            using MemoryStream memoryStream = new();
            using CryptoStream cryptoStream = new((Stream)memoryStream, encryptor, CryptoStreamMode.Write);
            using (StreamWriter streamWriter = new((Stream)cryptoStream))
            {
                await streamWriter.WriteAsync(plainText);
            }

            array = memoryStream.ToArray();
        }

        return Convert.ToBase64String(array);
    }
}