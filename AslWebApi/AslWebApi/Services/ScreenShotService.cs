using AslWebApi.DAL.Models;
using AslWebApi.DAL.Repositories;
using AslWebApi.DTOs;

namespace AslWebApi.Services
{
    public interface IScreenShotService
    {
        public Task<bool> UploadSS(IFormFile file);
        public Task<bool> DeleteSS(ScreenShot ss);

    }
    public class ScreenShotService : IScreenShotService
    {
        private readonly IGenericRepo<ScreenShot> _ssRepo;
        private readonly IFileUploader _uploader;
        private CurrentUser _currentUser;
        public ScreenShotService(IGenericRepo<ScreenShot> ssRepo, IFileUploader uploader)
        {
            _ssRepo = ssRepo;
            _uploader = uploader;
            _currentUser = GlobalFunctions.CurrentUserS();
        }

        public async Task<bool> DeleteSS(ScreenShot ss)
        {
            if (ss == null) return false;
            if (!await _ssRepo.DeleteAsync(ss)) return false;
            string filePath = $"{ss.DirPath}\\{ss.FileName}";
            return _uploader.DeleteFile(filePath);
        }

        public async Task<bool> UploadSS(IFormFile file)
        {
            string[]? result = await _uploader.SaveFileAsync(file);
            if (result == null) return false;

            string dir = result[0]!;
            string fileName = result[1]!;

            ScreenShot ss = new ScreenShot()
            {
                UserID = _currentUser.UserID,
                DirPath = dir,
                FileName = fileName,
                InTime = DateTime.Now,
                InUserID = _currentUser.UserID,
                InUserPC = GlobalFunctions.UserPc(),
                InIPAddress = GlobalFunctions.IpAddress()
            };
            ScreenShot? ssCreated = await _ssRepo.CreateAsync(ss);
            if (ssCreated == null)
            {
                string filePath = $"{dir}\\{fileName}";
                _uploader.DeleteFile(filePath);
                return false;
            }
            return true;
        }
    }
}
