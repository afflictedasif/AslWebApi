using AslWebApi.DAL.Models;
using AslWebApi.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AslWebApi.Services;

public interface IUserStateService
{
    /// <summary>
    /// Get the previous state of the user and update its timeTo field. then Log entry made, then new userState update is applied.
    /// </summary>
    /// <param name="userState"></param>
    /// <returns>boolean</returns>
    public bool ChangeUserState(UserState userState);
    /// <summary>
    /// Get the previous state of the user and update its timeTo field. then Log entry made, then new userState update is applied.
    /// </summary>
    /// <param name="userState"></param>
    /// <returns>boolean</returns>
    public Task<bool> ChangeUserStateAsync(UserState userState);

    public Task<bool> ChangeUserStateWithoutLogAsync(UserState userState);
    /// <summary>
    /// Inserts new UserState
    /// </summary>
    /// <param name="userState"></param>
    /// <returns>Inserted userState object</returns>
    public UserState? CreateUserState(UserState userState);
    /// <summary>
    /// Inserts new UserState
    /// </summary>
    /// <param name="userState"></param>
    /// <returns>Inserted userState object</returns>
    public Task<UserState?> CreateUserStateAsync(UserState userState);

    public Task<UserState> GetLastStateAsync(int UserID);

}

public class UserStateService : IUserStateService
{
    private readonly IUserStateRepo _userStateRepo;
    private readonly ILogService _logService;

    public UserStateService(IUserStateRepo userStateRepo, ILogService logService)
    {
        _userStateRepo = userStateRepo;
        _logService = logService;
    }

    /// <summary>
    /// Get the previous state of the user and update its timeTo field. then Log entry made, then new userState update is applied.
    /// </summary>
    /// <param name="userState"></param>
    /// <returns>boolean</returns>
    public bool ChangeUserState(UserState userState)
    {
        try
        {
            UserState? prevState = _userStateRepo.Get(userState.UserID);
            if (prevState == null) return false;

            if (prevState.TimeTo == null)
            {
                if (prevState.TimeFrom != null && prevState.TimeFrom.Value.Date == DateTime.Now.Date)
                {
                    prevState.TimeTo = DateTime.Now;
                    if (!_userStateRepo.Update(prevState)) return false;
                }
            }

            bool logGenerated = _logService.InsertLog<UserState>(TableName: "UserStateS", logType: "UPDATE", userState.UserStateId);
            if (!logGenerated) return false;

            return _userStateRepo.Update(userState);
        }
        catch
        {
            return false;
        }

    }


    /// <summary>
    /// Get the previous state of the user and update its timeTo field. then Log entry made, then new userState update is applied.
    /// </summary>
    /// <param name="userState"></param>
    /// <returns>boolean</returns>
    public async Task<bool> ChangeUserStateAsync(UserState userState)
    {
        UserState? prevState = await _userStateRepo.GetAsync(userState.UserID);
        if (prevState == null) return false;

        if (prevState.TimeTo == null)
        {
            if (prevState.TimeFrom != null && prevState.TimeFrom.Value.Date == DateTime.Now.Date)
            {
                prevState.TimeTo = DateTime.Now;
                if (!await _userStateRepo.UpdateAsync(prevState)) return false;
            }
        }
        userState.UserStateId = prevState.UserStateId;

        bool logGenerated = await _logService.InsertLogAsync<UserState>(TableName: "UserStateS", logType: "UPDATE", userState.UserStateId);
        if (!logGenerated) return false;

        return await _userStateRepo.UpdateAsync(userState);
    }


    public async Task<bool> ChangeUserStateWithoutLogAsync(UserState userState)
    {
        UserState? prevState = await _userStateRepo.GetAsync(userState.UserID);
        if (prevState == null) return false;

        userState.UserStateId = prevState.UserStateId;

        return await _userStateRepo.UpdateAsync(userState);
    }

    /// <summary>
    /// Inserts new UserState
    /// </summary>
    /// <param name="userState"></param>
    /// <returns>Inserted userState object</returns>
    public UserState? CreateUserState(UserState userState)
    {
        if (userState == null) return null;
        return _userStateRepo.Create(userState);
    }

    /// <summary>
    /// Inserts new UserState
    /// </summary>
    /// <param name="userState"></param>
    /// <returns>Inserted userState object</returns>
    public async Task<UserState?> CreateUserStateAsync(UserState userState)
    {
        if (userState == null) return null;
        return await _userStateRepo.CreateAsync(userState);
    }
    /// <summary>
    /// Return the UserState of the user
    /// </summary>
    /// <param name="userState"></param>
    /// <returns>userState object</returns>
    public async Task<UserState?> GetLastStateAsync(int UserID)
    {
        return await _userStateRepo.GetAll().FirstOrDefaultAsync(s => s.UserID == UserID);
    }
}

