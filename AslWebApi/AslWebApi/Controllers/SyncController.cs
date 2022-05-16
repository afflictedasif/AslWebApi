using AslWebApi.DAL.Models;
using AslWebApi.DAL.Repositories;
using AslWebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AslWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyncController : ControllerBase
    {
        private readonly IGenericRepo<ScreenShot> _ssRepo;
        private readonly IGenericRepo<UserState> _userStateRepo;
        private readonly IGenericRepo<CLog> _logRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SyncController(IGenericRepo<ScreenShot> ssRepo, IGenericRepo<UserInfo> userRepo, IGenericRepo<UserState> userStateRepo, IUserStateService userStateService, IGenericRepo<CLog> logRepo, IUserRepo urepo, IWebHostEnvironment webHostEnvironment)
        {
            _ssRepo = ssRepo;
            _userStateRepo = userStateRepo;
            _logRepo = logRepo;
            _webHostEnvironment = webHostEnvironment;
        }

        [Authorize]
        [HttpGet, Route("getPreviousStates")]
        public async Task<IActionResult> getPreviousStates()
        {
            int UserID = GlobalFunctions.CurrentUserS().UserID;
            List<CLog> logs = await _logRepo.GetAll().Where(l => l.UserID == UserID 
                                                            && ((DateTime)l.LogTime!).Date == DateTime.Now.Date 
                                                            && l.TableName == "UserStates").OrderByDescending(l=> l.LogTime).ToListAsync();
            List<UserState> userStates = new List<UserState>();
            foreach (CLog log in logs)
            {
                UserState? state = JsonConvert.DeserializeObject<UserState>(log.LogData);
                if (state is not null)
                    userStates.Add(state);
            }
            return Ok(userStates);
        }


        /// <summary>
        /// Update userstate in the database
        /// </summary>
        /// <param name="userState"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost, Route("updateState")]
        public async Task<IActionResult> updateState([FromBody] UserState userState)
        {
            UserState? prevState = _userStateRepo.GetAll().FirstOrDefault(u => u.UserID == userState.UserID);
            if (prevState is null) return Ok();
            userState.UserStateId = prevState.UserStateId;
            bool result = await _userStateRepo.UpdateAsync(userState);
            if (result) return Ok();
            return BadRequest();
        }

        /// <summary>
        /// Insert Screenshot data into the database
        /// </summary>
        /// <param name="ss"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost, Route("addss")]
        public async Task<IActionResult> addss([FromBody] ScreenShot ss)
        {
            string rootPath = $"{_webHostEnvironment.WebRootPath}\\ScreenShots";

            ss.DirPath = rootPath + "\\" + ss.DirPath.Substring(6, ss.DirPath.Length - 6);
            ss.ScreenShotID = 0;
            ScreenShot? ssCreated = await _ssRepo.CreateAsync(ss);
            if (ssCreated is not null) return Ok();
            return BadRequest();
        }

        /// <summary>
        /// Inserts log into database
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost, Route("addLogs")]
        public async Task<IActionResult> addLogs([FromBody] CLog log)
        {

            CLog? logCreated = await _logRepo.CreateAsync(log);
            if (logCreated is not null) return Ok();
            return BadRequest();
        }


        /// <summary>
        /// Save Files to Server
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost, Route("Files")]
        public async Task<IActionResult> Files(IFormFile file)
        {
            await SaveFileAsync(file);
            //if (uploaded)
            return Ok(new { Size = file.Length });
            //else return BadRequest();
        }


        /// <summary>
        /// Check the file extensions, create directory from the db, then save the file in that.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [NonAction]
        private async Task<string[]?> SaveFileAsync(IFormFile file)
        {
            try
            {
                if (file.Length > (1024 * 1000 * 10)) return null;

                string[] permittedExtensions = { ".jpg", ".png" };
                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext)) return null;

                if (file.Length > 0)
                {
                    //var filePath = Path.GetTempFileName();
                    //string rootPath = Path.Combine(Directory.GetCurrentDirectory(), "ScreenShots");
                    string rootPath = $"{_webHostEnvironment.WebRootPath}\\ScreenShots";
                    string folderPath = "";
                    string fileName = "";

                    folderPath = GenerateFolders(rootPath, file.FileName);
                    fileName = file.FileName;
                    string filePath = $"{folderPath}\\{fileName}";

                    using var stream = System.IO.File.Create(filePath);
                    await file.CopyToAsync(stream);
                    string[] result = { folderPath, fileName };

                    return result;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get the directory name from database and create the directory if not exists
        /// </summary>
        /// <param name="rootPath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [NonAction]
        private string GenerateFolders(string rootPath, string fileName)
        {
            string dirPath = _ssRepo.GetAll().FirstOrDefault(s => s.FileName == fileName)!.DirPath;
            //string userID = _currentUser!.UserID.ToString();
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string folderPath = dirPath; //$"{rootPath}\\{dirPath}\\{date}";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            return folderPath;
        }

    }
}
