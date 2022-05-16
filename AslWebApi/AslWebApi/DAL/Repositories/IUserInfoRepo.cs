using AslWebApi.DAL.Models;
using AslWebApi.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AslWebApi.DAL.Repositories;

public interface IUserRepo
{
    public UserInfo? Create(UserInfo user);
    public bool Delete(int userId);
    public bool Delete(UserInfo user);
    public IQueryable<UserInfo> GetAll();
    public UserInfo? Get(int UserID);
    public bool Update(UserInfo user);


    public Task<UserInfo?> CreateAsync(UserInfo user);
    public Task<bool> DeleteAsync(int userId);
    public Task<bool> DeleteAsync(UserInfo user);
    public Task<List<UserInfo>> GetAllListAsync();
    public Task<UserInfo?> GetAsync(int userId);
    public Task<bool> UpdateAsync(UserInfo user);
}

public class UserRepo : IUserRepo
{
    private DatabaseContext context;
    private CurrentUser? currentUser;
    public UserRepo(DatabaseContext dc)
    {
        context = dc;
        currentUser = GlobalFunctions.CurrentUserS();
    }

    /// <summary>
    /// Gets last User Id and create new UserId by incrementing the number.
    /// </summary>
    /// <returns>new User ID in int</returns>
    private int GenerateNewUserID()
    {
        int lastId =
             (from m in GetAll()
              orderby m.UserID descending
              select m.UserID).FirstOrDefault();
        if (lastId == 0) lastId = 10101;
        return lastId + 1;
    }

    /// <summary>
    /// Gets last User Id and create new UserId by incrementing the number.
    /// </summary>
    /// <returns>new User ID in int</returns>
    private async Task<int> GenerateNewUserIDAsync()
    {
        int lastId =
             await (from m in GetAll()
                    orderby m.UserID descending
                    select m.UserID).FirstOrDefaultAsync();
        if (lastId == 0) lastId = 10101;
        return lastId + 1;
    }


    public UserInfo? Create(UserInfo user)
    {
        try
        {
            if (user == null) return null;

            user.UserID = GenerateNewUserID();
            user.InTime = DateTime.Now;
            user.InIPAddress = GlobalFunctions.IpAddress();
            user.InUserPC = GlobalFunctions.UserPc();
            user.InUserID = currentUser?.UserID;

            context.UserInfos.Add(user);
            int rowAffected = context.SaveChanges();
            if (rowAffected > 0) return user;
            else return null;
        }
        catch
        {
            return null;
        }
    }
    public bool Delete(int UserID)
    {
        //bool logGenerated = _logService.InsertLogWithWhereClause<user>(TableName: "users", logType: "DELETE", whereClause: $"where UserID ={UserID}");
        //if (!logGenerated) return false;

        UserInfo? user = Get(UserID: UserID);
        if (user == null) return false;
        context.UserInfos.Remove(user);
        int rowAffected = context.SaveChanges();
        return rowAffected > 0;
    }
    public bool Delete(UserInfo user)
    {
        try
        {
            //bool logGenerated = _logService.InsertLog<user>(TableName: "userS", logType: "DELETE", user.userId);
            //if (!logGenerated) return false;

            context.ChangeTracker.Clear();
            context.Entry(user).State = EntityState.Deleted;

            int rowsAffected = context.SaveChanges();
            return rowsAffected > 0;
        }
        catch
        {
            return false;
        }
    }
    public IQueryable<UserInfo> GetAll()
    {
        return context.UserInfos.AsNoTracking().OrderBy(m => m.UserID);
    }
    public UserInfo? Get(int UserID)
    {
        return context.UserInfos.SingleOrDefault(us => us.UserID == UserID);
    }
    public bool Update(UserInfo user)
    {
        try
        {
            //bool logGenerated = _logService.InsertLog<user>(TableName: "userS", logType: "UPDATE", user.userId);
            //if (!logGenerated) return false;

            if (user == null) return false;

            user.UpTime = DateTime.Now;
            user.UpIPAddress = GlobalFunctions.IpAddress();
            user.UpUserPC = GlobalFunctions.UserPc();
            user.UpUserID = currentUser?.UserID;

            context.ChangeTracker.Clear();
            context.Entry(user).State = EntityState.Modified;
            int rowsAffected = context.SaveChanges();
            return rowsAffected > 0;
        }
        catch
        {
            return false;
        }
    }


    public async Task<List<UserInfo>> GetAllListAsync()
    {
        return await context.UserInfos.AsNoTracking().OrderBy(m => m.UserID).ToListAsync(); ;
    }
    public async Task<UserInfo?> GetAsync(int UserID)
    {
        return await context.UserInfos.AsNoTracking().FirstOrDefaultAsync(s => s.UserID == UserID);
    }
    public async Task<UserInfo?> CreateAsync(UserInfo user)
    {
        try
        {
            if (user == null) return null;

            user.UserID = await GenerateNewUserIDAsync();
            user.InTime = DateTime.Now;
            user.InIPAddress = GlobalFunctions.IpAddress();
            user.InUserPC = GlobalFunctions.UserPc();
            user.InUserID = currentUser?.UserID;


            context.UserInfos.Add(user);
            int rowsAffected = await context.SaveChangesAsync();
            if (rowsAffected > 0) return user;
            else return null;
        }
        catch
        {
            return null;
        }
    }
    public async Task<bool> DeleteAsync(int UserID)
    {
        //bool logGenerated = await _logService.InsertLogWithWhereClauseAsync<user>(TableName: "users", logType: "DELETE", whereClause: $"where userID = {UserID}");
        //if (!logGenerated) return false;

        UserInfo? user = Get(UserID: UserID);
        if (user == null) return false;
        context.UserInfos.Remove(user);
        int rowsAffected = await context.SaveChangesAsync();
        return rowsAffected > 0;
    }
    public async Task<bool> DeleteAsync(UserInfo user)
    {
        try
        {
            //bool logGenerated = await _logService.InsertLogAsync<user>(TableName: "userS", logType: "DELETE", user.userId);
            //if (!logGenerated) return false;

            context.ChangeTracker.Clear();
            context.Entry(user).State = EntityState.Deleted;

            int rowsAffected = await context.SaveChangesAsync();
            return rowsAffected > 0;
        }
        catch
        {
            return false;
        }
    }
    public async Task<bool> UpdateAsync(UserInfo user)
    {
        try
        {
            //bool logGenerated = await _logService.InsertLogAsync<user>(TableName: "userS", logType: "UPDATE",user.userId);
            //if (!logGenerated) return false;

            context.ChangeTracker.Clear();

            if (user == null) return false;

            user.UpTime = DateTime.Now;
            user.UpIPAddress = GlobalFunctions.IpAddress();
            user.UpUserPC = GlobalFunctions.UserPc();
            user.UpUserID = currentUser?.UserID;


            context.Entry(user).State = EntityState.Modified;
            int rowsAffected = await context.SaveChangesAsync();
            return rowsAffected > 0;
        }
        catch
        {
            return false;
        }
    }
}

