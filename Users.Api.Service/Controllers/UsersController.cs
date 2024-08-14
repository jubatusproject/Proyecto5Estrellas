using Asp.Versioning;
using Jubatus.Common;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Users.Api.Service.Dtos;
using Users.Api.Service.Models;
using Destructurama;
using Microsoft.AspNetCore.Authorization;
using Jubatus.Common.Serilog;
using Users.Api.Service.Settings;

namespace Users.Api.Service.Controllers;

[Authorize]
[ApiController]
[ApiVersion(ApiVersions.UsersApiV1)]
[Route(ApiEndPoints.RootUsers)]
public class UsersController : ControllerBase
{
    private readonly IRepository<UsersEntity> _usersRepository;

    private readonly IConfiguration _configuration;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="usersRepository"></param>
    /// <param name="configuration"></param>
    public UsersController(IRepository<UsersEntity> usersRepository, IConfiguration configuration)
    {
        _usersRepository = usersRepository;
        _configuration = configuration;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [MapToApiVersion(ApiVersions.UsersApiV1)]
    [Route(ApiEndPoints.UserGetAllRecs)]
    public async Task<IActionResult> GetAllRecordsAsync()
    {
        IReadOnlyCollection<UsersEntity> result = await _usersRepository.GetAllAsync().ConfigureAwait(false);
        using Serilog.Core.Logger log = Serilogger.GetLogger();

        log.Debug("GetAllRecordsAsync( returns: {Count} records ), resultCode: {Code}, resultMessage: {Msg}",
            result.Count, _usersRepository.ResultCode, _usersRepository.ResultMessage);

        return _usersRepository.ResultCode == StatusCodes.Status200OK ?
            Ok(result.Select(item => item.AsUsersDto())) : BadRequest(_usersRepository.ResultMessage);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [MapToApiVersion(ApiVersions.UsersApiV1)]
    [Route(ApiEndPoints.UserGetOneRecs)]
    public async Task<IActionResult> GetOneRecordAsync([FromQuery] Guid id)
    {
        UsersEntity result = await _usersRepository.GetAsync(id).ConfigureAwait(false);
        using Serilog.Core.Logger log = Serilogger.GetLogger();

        log.Debug("GetOneRecordAsync( returns: {@Record} ), resultCode: {Code}, resultMessage: {Msg}",
            result, _usersRepository.ResultCode, _usersRepository.ResultMessage);

        return _usersRepository.ResultCode switch
        {
            StatusCodes.Status200OK => Ok(result.AsUsersDto()),
            StatusCodes.Status404NotFound => NotFound(),
            _ => BadRequest(_usersRepository.ResultMessage)
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    [HttpPost]
    [MapToApiVersion(ApiVersions.UsersApiV1)]
    [Route(ApiEndPoints.UserCreateRecs)]
    public async Task<IActionResult> CreateRecordAsync([FromBody] NewUsersDto item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        JwtSettings? jwtSettings = _configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
        ArgumentNullException.ThrowIfNull(jwtSettings);

        UsersEntity newUser = new()
        {
            Id = Guid.NewGuid(),
            Alias = item.Alias,
            FirstName = item.FirstName,
            LastName = item.LastName,
            Password = await item.Password!.EncryptUserPassword(jwtSettings.JwtKey!),
            IsActive = item.IsActive
        };

        await _usersRepository.CreateAsync(newUser).ConfigureAwait(false);
        using Serilog.Core.Logger log = Serilogger.GetLogger();

        log.Debug("CreateRecordAsync( creates: {@Record} ), resultCode: {Code}, resultMessage: {Msg}",
            newUser, _usersRepository.ResultCode, _usersRepository.ResultMessage);

        return _usersRepository.ResultCode == StatusCodes.Status201Created ?
            Ok(newUser.AsUsersDto()) : BadRequest(_usersRepository.ResultMessage);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    [HttpPut]
    [MapToApiVersion(ApiVersions.UsersApiV1)]
    [Route(ApiEndPoints.UserUpdateRecs)]
    public async Task<IActionResult> UpdateRecordAsync([FromQuery] Guid id, [FromBody] UpdUsersDto item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        UsersEntity? currentItem = await _usersRepository.GetAsync(id).ConfigureAwait(false);

        if (currentItem is null)
        {
            return NotFound();
        }

        JwtSettings? jwtSettings = _configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
        ArgumentNullException.ThrowIfNull(jwtSettings);

        UsersEntity updatedItem = currentItem with
        {
            Alias = item.Alias,
            FirstName = item.FirstName,
            LastName = item.LastName,
            Password = await item.Password!.EncryptUserPassword(jwtSettings.JwtKey!),
            IsActive = item.IsActive
        };

        await _usersRepository.UpdateAsync(updatedItem).ConfigureAwait(false);
        using Serilog.Core.Logger log = Serilogger.GetLogger();

        log.Debug("UpdateRecordAsync( updates: {@Record} ), resultCode: {Code}, resultMessage: {Msg}",
            updatedItem, _usersRepository.ResultCode, _usersRepository.ResultMessage);

        return _usersRepository.ResultCode switch
        {
            StatusCodes.Status204NoContent => Ok(updatedItem.AsUsersDto()),
            StatusCodes.Status404NotFound => NotFound(),
            _ => BadRequest(_usersRepository.ResultMessage)
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [MapToApiVersion(ApiVersions.UsersApiV1)]
    [Route(ApiEndPoints.UserDeleteRecs)]
    public async Task<IActionResult> DeleteRecordAsync([FromQuery] Guid id)
    {
        await _usersRepository.RemoveAsync(id).ConfigureAwait(false);
        using Serilog.Core.Logger log = Serilogger.GetLogger();

        log.Debug("DeleteRecordAsync( resultCode: {Code}, resultMessage: {Msg} )",
            _usersRepository.ResultCode, _usersRepository.ResultMessage);

        return _usersRepository.ResultCode switch
        {
            StatusCodes.Status204NoContent => Ok(),
            StatusCodes.Status404NotFound => NotFound(),
            _ => BadRequest(_usersRepository.ResultMessage)
        };
    }
}