﻿using AslWebApi.DAL.Models;
using AslWebApi.DAL.Repositories;
using AslWebApi.DTOs;

namespace AslWebApi.Services
{
    public interface ILogService
    {
        /// <summary>
        /// Inserts JSON serialized data of the given ID into log table
        /// </summary>
        /// <typeparam name="T">Table type</typeparam>
        /// <param name="UserID">UserID of the user that made the action</param>
        /// <param name="TableName">Table Name</param>
        /// <param name="logType">Log Type: Update, Delete, Login, Logout, etc</param>
        /// <param name="Id">Primary Key (ID) of the record</param>
        /// <returns></returns>
        public CLog? InsertLog<T>(int UserID, string TableName, string logType, int Id) where T : class;
        /// <summary>
        /// Inserts JSON serialized data of the given ID into log table
        /// </summary>
        /// <typeparam name="T">Table type</typeparam>
        /// <param name="TableName">Table Name</param>
        /// <param name="logType">Log Type: Update, Delete, Login, Logout, etc</param>
        /// <param name="Id">Primary Key (ID) of the record</param>
        /// <returns></returns>
        public Task<bool> InsertLogAsync<T>(string TableName, string logType, string Id) where T : class;
        /// <summary>
        /// Inserts JSON serialized data of the given ID into log table
        /// </summary>
        /// <typeparam name="T">Table type</typeparam>
        /// <param name="TableName">Table Name</param>
        /// <param name="logType">Log Type: Update, Delete, Login, Logout, etc</param>
        /// <param name="Id">Primary Key (ID) of the record</param>
        /// <returns></returns>
        public bool InsertLog<T>(string TableName, string logType, int Id) where T : class;

        /// <summary>
        /// Inserts JSON serialized data of the given ID into log table
        /// </summary>
        /// <typeparam name="T">Table type</typeparam>
        /// <param name="TableName">Table Name</param>
        /// <param name="logType">Log Type: Update, Delete, Login, Logout, etc</param>
        /// <param name="Id">Primary Key (ID) of the record</param>
        /// <returns></returns>
        public Task<bool> InsertLogAsync<T>(string TableName, string logType, int Id) where T : class;

        /// <summary>
        /// Inserts JSON serialized data of the given ID into log table
        /// </summary>
        /// <typeparam name="T">Table type</typeparam>
        /// <param name="TableName">Table Name</param>
        /// <param name="logType">Log Type: Update, Delete, Login, Logout, etc</param>
        /// <param name="Id">Primary Key (ID) of the record</param>
        /// <returns></returns>
        public bool InsertLog<T>(string TableName, string logType, string Id) where T : class;

        /// <summary>
        /// Inserts JSON serialized data of the given query into log table
        /// </summary>
        /// <typeparam name="T">Table type</typeparam>
        /// <param name="TableName">Table Name</param>
        /// <param name="logType">Log Type: Update, Delete, Login, Logout, etc</param>
        /// <param name="whereClause">Where clause to find the record from sql database.</param>
        /// <returns></returns>
        public bool InsertLogWithWhereClause<T>(string TableName, string logType, string whereClause) where T : class;
        /// <summary>
        /// Inserts JSON serialized data of the given query into log table
        /// </summary>
        /// <typeparam name="T">Table type</typeparam>
        /// <param name="TableName">Table Name</param>
        /// <param name="logType">Log Type: Update, Delete, Login, Logout, etc</param>
        /// <param name="whereClause">Where clause to find the record from sql database.</param>
        /// <returns></returns>
        public Task<bool> InsertLogWithWhereClauseAsync<T>(string TableName, string logType, string whereClause) where T : class;

    }

    public class LogService : ILogService
    {

        private IJsonDBService rdbService;
        private IGenericRepo<CLog> logRepo;
        private readonly GlobalFunctions _gFunc;
        private CurrentUser currentUser;
        public LogService(IJsonDBService _rdbService, IGenericRepo<CLog> _logRepo, GlobalFunctions gFunc)
        {
            rdbService = _rdbService;
            logRepo = _logRepo;
            _gFunc = gFunc;
            //currentUser = Task.Run(async () => await _gFunc.CurrentUser()).Result;

            currentUser = GlobalFunctions.CurrentUserS();
        }

        /// <summary>
        /// Inserts JSON serialized data of the given ID into log table
        /// </summary>
        /// <typeparam name="T">Table type</typeparam>
        /// <param name="UserID">UserID of the user that made the action</param>
        /// <param name="TableName">Table Name</param>
        /// <param name="logType">Log Type: Update, Delete, Login, Logout, etc</param>
        /// <param name="Id">Primary Key (ID) of the record</param>
        /// <returns></returns>
        public CLog? InsertLog<T>(int UserID, string TableName, string logType, int Id) where T : class
        {
            try
            {
                string logData = rdbService.GetKeyValuePairOfData<T>(Id);

                CLog cLog = new CLog();
                cLog.LogData = logData;
                cLog.LogTime = DateTime.Now;
                cLog.TableName = TableName;
                cLog.LogType = logType;
                cLog.UserID = UserID;
                cLog.IPAddress = GlobalFunctions.IpAddress();
                cLog.UserPC = GlobalFunctions.UserPc();
                return logRepo.Create(cLog);
                //return true;
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// Inserts JSON serialized data of the given ID into log table
        /// </summary>
        /// <typeparam name="T">Table type</typeparam>
        /// <param name="TableName">Table Name</param>
        /// <param name="logType">Log Type: Update, Delete, Login, Logout, etc</param>
        /// <param name="Id">Primary Key (ID) of the record</param>
        /// <returns></returns>
        public bool InsertLog<T>(string TableName, string logType, int Id) where T : class
        {
            try
            {
                string logData = rdbService.GetKeyValuePairOfData<T>(Id);
                CLog cLog = new CLog();
                cLog.LogData = logData;
                cLog.LogTime = DateTime.Now;
                cLog.TableName = TableName;
                cLog.LogType = logType;
                cLog.UserID = currentUser.UserID;
                cLog.IPAddress = GlobalFunctions.IpAddress();
                cLog.UserPC = GlobalFunctions.UserPc();
                CLog? log = logRepo.Create(cLog);
                return (log != null);
            }
            catch (Exception ex)
            {
                string ErrMsg = ex.Message;
                return false;
            }
        }
        /// <summary>
        /// Inserts JSON serialized data of the given ID into log table
        /// </summary>
        /// <typeparam name="T">Table type</typeparam>
        /// <param name="TableName">Table Name</param>
        /// <param name="logType">Log Type: Update, Delete, Login, Logout, etc</param>
        /// <param name="Id">Primary Key (ID) of the record</param>
        /// <returns></returns>
        public async Task<bool> InsertLogAsync<T>(string TableName, string logType, int Id) where T : class
        {
            try
            {
                //currentUser = await _gFunc.CurrentUserAsync();
                string logData = await  rdbService.GetKeyValuePairOfDataAsync<T>(Id);
                CLog cLog = new CLog();
                cLog.LogData = logData;
                cLog.LogTime = DateTime.Now;
                cLog.TableName = TableName;
                cLog.LogType = logType;
                cLog.UserID = currentUser.UserID;
                cLog.IPAddress = GlobalFunctions.IpAddress();
                cLog.UserPC = GlobalFunctions.UserPc();
                CLog? log = await logRepo.CreateAsync(cLog);
                return (log != null);
            }
            catch (Exception ex)
            {
                string ErrMsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Inserts JSON serialized data of the given ID into log table
        /// </summary>
        /// <typeparam name="T">Table type</typeparam>
        /// <param name="TableName">Table Name</param>
        /// <param name="logType">Log Type: Update, Delete, Login, Logout, etc</param>
        /// <param name="Id">Primary Key (ID) of the record</param>
        /// <returns></returns>
        public bool InsertLog<T>(string TableName, string logType, string Id) where T : class
        {
            try
            {
                string logData = rdbService.GetKeyValuePairOfData<T>(Id);
                CLog cLog = new CLog();
                cLog.LogData = logData;
                cLog.LogTime = DateTime.Now;
                cLog.TableName = TableName;
                cLog.LogType = logType;
                cLog.UserID = currentUser.UserID;
                cLog.IPAddress = GlobalFunctions.IpAddress();
                cLog.UserPC = GlobalFunctions.UserPc();
                CLog? log = logRepo.Create(cLog);
                return (log != null);
                //return true;
            }
            catch (Exception ex)
            {
                string ErrMsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Inserts JSON serialized data of the given ID into log table
        /// </summary>
        /// <typeparam name="T">Table type</typeparam>
        /// <param name="TableName">Table Name</param>
        /// <param name="logType">Log Type: Update, Delete, Login, Logout, etc</param>
        /// <param name="Id">Primary Key (ID) of the record</param>
        /// <returns></returns>
        public async Task<bool> InsertLogAsync<T>(string TableName, string logType, string Id) where T : class
        {
            try
            {
                //currentUser = await _gFunc.CurrentUserAsync();
                string logData = await rdbService.GetKeyValuePairOfDataAsync<T>(Id);
                CLog cLog = new CLog();
                cLog.LogData = logData;
                cLog.LogTime = DateTime.Now;
                cLog.TableName = TableName;
                cLog.LogType = logType;
                cLog.UserID = currentUser.UserID;
                cLog.IPAddress = GlobalFunctions.IpAddress();
                cLog.UserPC = GlobalFunctions.UserPc();
                CLog? log = await logRepo.CreateAsync(cLog);
                return (log != null);
                //return true;
            }
            catch (Exception ex)
            {
                string ErrMsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Inserts JSON serialized data of the given query into log table
        /// </summary>
        /// <typeparam name="T">Table type</typeparam>
        /// <param name="TableName">Table Name</param>
        /// <param name="logType">Log Type: Update, Delete, Login, Logout, etc</param>
        /// <param name="whereClause">Where clause to find the record from sql database.</param>
        /// <returns></returns>
        public bool InsertLogWithWhereClause<T>(string TableName, string logType, string whereClause) where T : class
        {
            string query = "";
            query += $"SELECT Top 1 * FROM {TableName} {whereClause}";
            try
            {
                string logData = rdbService.GetKeyValuePairOfDataWithSQL<T>(query);
                CLog cLog = new CLog();
                cLog.LogData = logData;
                cLog.LogTime = DateTime.Now;
                cLog.TableName = TableName;
                cLog.LogType = logType;
                cLog.UserID = currentUser.UserID;
                cLog.IPAddress = GlobalFunctions.IpAddress();
                cLog.UserPC = GlobalFunctions.UserPc();
                CLog? log = logRepo.Create(cLog);
                return (log != null);
                //return false;
            }
            catch (Exception ex)
            {
                string ErrMsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Inserts JSON serialized data of the given query into log table
        /// </summary>
        /// <typeparam name="T">Table type</typeparam>
        /// <param name="TableName">Table Name</param>
        /// <param name="logType">Log Type: Update, Delete, Login, Logout, etc</param>
        /// <param name="whereClause">Where clause to find the record from sql database.</param>
        /// <returns></returns>
        public async Task<bool> InsertLogWithWhereClauseAsync<T>(string TableName, string logType, string whereClause) where T : class
        {
            //currentUser = await _gFunc.CurrentUserAsync();
            string query = "";
            query += $"SELECT Top 1 * FROM {TableName} {whereClause}";
            try
            {
                string logData = await rdbService.GetKeyValuePairOfDataWithSQLAsync<T>(query);
                CLog cLog = new CLog();
                cLog.LogData = logData;
                cLog.LogTime = DateTime.Now;
                cLog.TableName = TableName;
                cLog.LogType = logType;
                cLog.UserID = currentUser.UserID;
                cLog.IPAddress = GlobalFunctions.IpAddress();
                cLog.UserPC = GlobalFunctions.UserPc();
                CLog? log = await logRepo.CreateAsync(cLog);
                return (log != null);
                //return false;
            }
            catch (Exception ex)
            {
                string ErrMsg = ex.Message;
                return false;
            }
        }
    }

}
