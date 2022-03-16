using AslWebApi.DAL.Models;
using AslWebApi.DTOs;
using AslWebApi.Services;
using Microsoft.EntityFrameworkCore;

namespace AslWebApi.DAL.Repositories
{
    public interface IUserStateRepo
    {
        public UserState? Create(UserState userState);
        public bool Delete(int UserStateId);
        public bool Delete(UserState userState);
        public IQueryable<UserState> GetAll();
        public UserState? Get(int UserID);
        public bool Update(UserState userState);


        public Task<UserState?> CreateAsync(UserState userState);
        public Task<bool> DeleteAsync(int userStateId);
        public Task<bool> DeleteAsync(UserState userState);
        public Task<List<UserState>> GetAllListAsync();
        public Task<UserState?> GetAsync(int userStateId);
        public Task<bool> UpdateAsync(UserState userState);
    }

    public class UserStateRepo : IUserStateRepo
    {
        private DatabaseContext context;
        private CurrentUser? currentUser;
        public UserStateRepo(DatabaseContext dc)
        {
            context = dc;
            currentUser = GlobalFunctions.CurrentUserS();
        }
        public UserState? Create(UserState userState)
        {
            try
            {
                if (userState == null) return null;

                userState.InTime = DateTime.Now;
                userState.InIPAddress = GlobalFunctions.IpAddress();
                userState.InUserPC = GlobalFunctions.UserPc();
                userState.InUserID = currentUser?.UserID;

                context.UserStates.Add(userState);
                int rowAffected = context.SaveChanges();
                if (rowAffected > 0) return userState;
                else return null;
            }
            catch 
            {
                return null;
            }
        }
        public bool Delete(int UserID)
        {
            //bool logGenerated = _logService.InsertLogWithWhereClause<UserState>(TableName: "UserStates", logType: "DELETE", whereClause: $"where UserID ={UserID}");
            //if (!logGenerated) return false;

            UserState? userState = Get(UserID: UserID);
            if (userState == null) return false;
            context.UserStates.Remove(userState);
            int rowAffected = context.SaveChanges();
            return rowAffected > 0;
        }
        public bool Delete(UserState userState)
        {
            try
            {
                //bool logGenerated = _logService.InsertLog<UserState>(TableName: "UserStateS", logType: "DELETE", userState.UserStateId);
                //if (!logGenerated) return false;

                context.ChangeTracker.Clear();
                context.Entry(userState).State = EntityState.Deleted;

                int rowsAffected = context.SaveChanges();
                return rowsAffected > 0;
            }
            catch 
            {
                return false;
            }
        }
        public IQueryable<UserState> GetAll()
        {
            return context.UserStates.AsNoTracking().OrderBy(m => m.UserID);
        }
        public UserState? Get(int UserID)
        {
            return context.UserStates.SingleOrDefault(us => us.UserID == UserID);
        }
        public bool Update(UserState userState)
        {
            try
            {
                //bool logGenerated = _logService.InsertLog<UserState>(TableName: "UserStateS", logType: "UPDATE", userState.UserStateId);
                //if (!logGenerated) return false;

                if (userState == null) return false;

                userState.UpTime = DateTime.Now;
                userState.UpIPAddress = GlobalFunctions.IpAddress();
                userState.UpUserPC = GlobalFunctions.UserPc();
                userState.UpUserID = currentUser?.UserID;

                context.ChangeTracker.Clear();
                context.Entry(userState).State = EntityState.Modified;
                int rowsAffected = context.SaveChanges();
                return rowsAffected > 0;
            }
            catch 
            {
                return false;
            }
        }


        public async Task<List<UserState>> GetAllListAsync()
        {
            return await context.UserStates.AsNoTracking().OrderBy(m => m.UserID).ToListAsync(); ;
        }
        public async Task<UserState?> GetAsync(int UserID)
        {
            return await context.UserStates.AsNoTracking().FirstOrDefaultAsync(s => s.UserID == UserID);
        }
        public async Task<UserState?> CreateAsync(UserState userState)
        {
            try
            {
                if (userState == null) return null;

                userState.InTime = DateTime.Now;
                userState.InIPAddress = GlobalFunctions.IpAddress();
                userState.InUserPC = GlobalFunctions.UserPc();
                userState.InUserID = currentUser?.UserID;


                context.UserStates.Add(userState);
                int rowsAffected = await context.SaveChangesAsync();
                if (rowsAffected > 0) return userState;
                else return null;
            }
            catch
            {
                return null;
            }
        }
        public async Task<bool> DeleteAsync(int UserID)
        {
            //bool logGenerated = await _logService.InsertLogWithWhereClauseAsync<UserState>(TableName: "UserStates", logType: "DELETE", whereClause: $"where userID = {UserID}");
            //if (!logGenerated) return false;

            UserState? userState = Get(UserID:UserID);
            if (userState == null) return false;
            context.UserStates.Remove(userState);
            int rowsAffected = await context.SaveChangesAsync();
            return rowsAffected > 0;
        }
        public async Task<bool> DeleteAsync(UserState userState)
        {
            try
            {
                //bool logGenerated = await _logService.InsertLogAsync<UserState>(TableName: "UserStateS", logType: "DELETE", userState.UserStateId);
                //if (!logGenerated) return false;

                context.ChangeTracker.Clear();
                context.Entry(userState).State = EntityState.Deleted;

                int rowsAffected = await context.SaveChangesAsync();
                return rowsAffected > 0;
            }
            catch 
            {
                return false;
            }
        }
        public async Task<bool> UpdateAsync(UserState userState)
        {
            try
            {
                //bool logGenerated = await _logService.InsertLogAsync<UserState>(TableName: "UserStateS", logType: "UPDATE",userState.UserStateId);
                //if (!logGenerated) return false;

                context.ChangeTracker.Clear();

                if (userState == null) return false;

                userState.UpTime = DateTime.Now;
                userState.UpIPAddress = GlobalFunctions.IpAddress();
                userState.UpUserPC = GlobalFunctions.UserPc();
                userState.UpUserID = currentUser?.UserID;


                context.Entry(userState).State = EntityState.Modified;
                int rowsAffected = await context.SaveChangesAsync();
                return rowsAffected > 0;
            }
            catch 
            {
                return false;
            }
        }
    }
}
