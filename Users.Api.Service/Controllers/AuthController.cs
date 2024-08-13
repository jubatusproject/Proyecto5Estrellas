using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Users.Api.Service.Dtos;
using Users.Api.Service.Settings;
using System.Security.Cryptography;
using Jubatus.Common.Serilog;
using Users.Api.Service.Models;

namespace Users.Api.Service.Controllers;

[Authorize]
[ApiController]
[ApiVersion(ApiVersions.AuthUserV1)]
[Route(ApiEndPoints.AuthUsers)]
public class AuthController : ControllerBase
{
    #region private data

    private readonly IConfiguration _configuration;

    #endregion
    #region constructor

    public AuthController(IConfiguration configuration) => _configuration = configuration;

    #endregion
    #region private methods

    #endregion
    #region http methods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="authUser"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost]
    [MapToApiVersion(ApiVersions.AuthUserV1)]
    [Route(ApiEndPoints.CheckUsersAuth)]
    public async Task<ActionResult> AuthenticateAsync([FromBody] AuthUserDto authUser)
    {
        using Serilog.Core.Logger log = Serilogger.GetLogger();

        try
        {
            JwtSettings? jwtSettings = _configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
            ArgumentNullException.ThrowIfNull(jwtSettings);

            string authPassword = await authUser.Password.EncryptUserPassword(jwtSettings.JwtKey!);

            if (jwtSettings.AuthUser == authUser.UserName && jwtSettings.AuthPass == authPassword)
            {
                log.Debug($"User <{authUser.UserName}> is trying to authenticate.");

                var tokenHandle = new JwtSecurityTokenHandler();
                byte[] tokenKey = Encoding.UTF8.GetBytes(jwtSettings.JwtKey!);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, authUser.UserName??"")
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(10),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
                };

                SecurityToken token = tokenHandle.CreateToken(tokenDescriptor);
                return Ok(new TokensModel { Token = tokenHandle.WriteToken(token) });
            }
            else
            {
                return Unauthorized();
            }
        }
        catch (System.Exception ex)
        {
            log.Error(ex, "Authenticate: Has thrown an exception: <{Excepcion}>.", ex);
            return Unauthorized();
        }
    }

    #endregion
}
