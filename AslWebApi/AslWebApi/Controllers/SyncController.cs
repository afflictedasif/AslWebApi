using AslWebApi.DAL.Models;
using AslWebApi.DAL.Repositories;
using AslWebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        [HttpPost, Route("updateState")]
        public async Task<IActionResult> updateState([FromBody] UserState userState)
        {
            UserState? prevState = _userStateRepo.GetAll().FirstOrDefault(u => u.UserID == userState.UserID);
            userState.UserStateId = prevState.UserStateId;
            bool result = await _userStateRepo.UpdateAsync(userState);
            if (result) return Ok();
            return BadRequest();
        }

        [Authorize]
        [HttpPost, Route("addss")]
        public async Task<IActionResult> addss([FromBody] ScreenShot ss)
        {
            string rootPath = $"{_webHostEnvironment.WebRootPath}\\ScreenShots";

            ss.DirPath = rootPath + "\\" + ss.DirPath.Substring(6, ss.DirPath.Length-6);
            ss.ScreenShotID = 0;
            ScreenShot? ssCreated = await _ssRepo.CreateAsync(ss);
            if (ssCreated is not null) return Ok();
            return BadRequest();
        }


        [Authorize]
        [HttpPost, Route("addLogs")]
        public async Task<IActionResult> addLogs([FromBody] CLog log)
        {

            CLog? logCreated = await _logRepo.CreateAsync(log);
            if (logCreated is not null) return Ok();
            return BadRequest();
        }

        [Authorize]
        [HttpPost, Route("Files")]
        public async Task<IActionResult> Files(IFormFile file)
        {
            await SaveFileAsync(file);
            //if (uploaded)
            return Ok(new { Size = file.Length });
            //else return BadRequest();
        }


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
