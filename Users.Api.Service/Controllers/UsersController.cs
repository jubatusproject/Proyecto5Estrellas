using Asp.Versioning;
using Jubatus.Common;
using Microsoft.AspNetCore.Mvc;
using Users.Api.Service.Dtos;
using Users.Api.Service.Models;

namespace Users.Api.Service.Controllers;

[ApiController]
[ApiVersion(UserVersions.USERSV1)]
[Route(UserEndPoints.ROOTUSERS)]
public class UsersController : ControllerBase
{
    private readonly IRepository<UsersEntity> _usersRepository;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="usersRepository"></param>
    public UsersController(IRepository<UsersEntity> usersRepository)
    {
        _usersRepository = usersRepository;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [MapToApiVersion(UserVersions.USERSV1)]
    [Route(UserEndPoints.USRGETALLRECS)]
    public async Task<IActionResult> GetAllRecordsAsync()
    {
        var result = await _usersRepository.GetAllAsync().ConfigureAwait(false);

        if (_usersRepository.ResultCode == StatusCodes.Status200OK)
            return Ok(result.Select(item => item.AsUsersDto()));
        else
            return BadRequest(_usersRepository.ResultMessage);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [MapToApiVersion(UserVersions.USERSV1)]
    [Route(UserEndPoints.USRGETONERECS)]
    public async Task<IActionResult> GetOneRecordAsync([FromQuery] Guid id)
    {
        var result = await _usersRepository.GetAsync(id).ConfigureAwait(false);

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
    [MapToApiVersion(UserVersions.USERSV1)]
    [Route(UserEndPoints.USRCREATERECS)]
    public async Task<IActionResult> CreateRecordAsync([FromBody] NewUsersDto item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        UsersEntity user = new()
        {
            Id = Guid.NewGuid(),
            Alias = item.Alias,
            FirstName = item.FirstName,
            LastName = item.LastName,
            Password = item.Password,
            IsActive = item.IsActive
        };

        await _usersRepository.CreateAsync(user).ConfigureAwait(false);

        if (_usersRepository.ResultCode == StatusCodes.Status201Created)
            return Ok(user.AsUsersDto());
        else
            return BadRequest(_usersRepository.ResultMessage);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    [HttpPut]
    [MapToApiVersion(UserVersions.USERSV1)]
    [Route(UserEndPoints.USRUPDATERECS)]
    public async Task<IActionResult> UpdateRecordAsync([FromQuery] Guid id, [FromBody] UpdUsersDto item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var currentItem = await _usersRepository.GetAsync(id).ConfigureAwait(false);

        if (currentItem is null)
        {
            return NotFound();
        }

        UsersEntity updatedItem = currentItem with
        {
            Alias = item.Alias,
            FirstName = item.FirstName,
            LastName = item.LastName,
            Password = item.Password,
            IsActive = item.IsActive
        };

        await _usersRepository.UpdateAsync(updatedItem).ConfigureAwait(false);

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
    [MapToApiVersion(UserVersions.USERSV1)]
    [Route(UserEndPoints.USRDELETERECS)]
    public async Task<IActionResult> DeleteRecordAsync([FromQuery] Guid id)
    {
        await _usersRepository.RemoveAsync(id).ConfigureAwait(false);

        return _usersRepository.ResultCode switch
        {
            StatusCodes.Status204NoContent => Ok(),
            StatusCodes.Status404NotFound => NotFound(),
            _ => BadRequest(_usersRepository.ResultMessage)
        };
    }
}