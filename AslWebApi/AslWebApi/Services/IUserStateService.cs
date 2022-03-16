using AslWebApi.DAL.Models;
using AslWebApi.DAL.Repositories;

namespace AslWebApi.Services;

public interface IUserStateService
{
    public bool ChangeUserState(UserState userState);
    public Task<bool> ChangeUserStateAsync(UserState userState);

    public UserState? CreateUserState(UserState userState);
    public Task<UserState?> CreateUserStateAsync(UserState userState);

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

        bool logGenerated = await _logService.InsertLogAsync<UserState>(TableName: "UserStateS", logType: "UPDATE", userState.UserStateId);
        if (!logGenerated) return false;

        return await _userStateRepo.UpdateAsync(userState);
    }

    public UserState? CreateUserState(UserState userState)
    {
        if (userState == null) return null;
        return _userStateRepo.Create(userState);
    }

    public async Task<UserState?> CreateUserStateAsync(UserState userState)
    {
        if (userState == null) return null;
        return await _userStateRepo.CreateAsync(userState);
    }
}

