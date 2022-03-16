using AslWebApi.DAL.Models;
using AslWebApi.DAL.Repositories;
using AslWebApi.DTOs;

namespace AslWebApi.Services;

public interface IFileUploader
{
    public Task<bool> SaveMultipleFilesAsync(List<IFormFile> files);
    public Task<string[]?> SaveFileAsync(IFormFile file);
    public bool DeleteFile(string filePath);


}


public class FileUploader : IFileUploader
{
    private CurrentUser? _currentUser;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileUploader(IWebHostEnvironment webHostEnvironment)
    {
        _currentUser = GlobalFunctions.CurrentUserS();
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<bool> SaveMultipleFilesAsync(List<IFormFile> files)
    {
        long size = files.Sum(f => f.Length);

        foreach (var formFile in files)
        {
            if (formFile.Length > 0)
            {
                //var filePath = Path.GetTempFileName();
                var filePath = Path.GetTempFileName();

                using var stream = File.Create(filePath);
                await formFile.CopyToAsync(stream);
            }
        }

        return true;
    }


    public async Task<string[]?> SaveFileAsync(IFormFile file)
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

                folderPath = GenerateFolders(rootPath);
                fileName = GenerateFileName();
                string filePath = $"{folderPath}\\{fileName}";

                using var stream = File.Create(filePath);
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

    public bool DeleteFile(string filePath)
    {
        try
        {
            FileInfo file = new FileInfo(filePath);
            if (!file.Exists) return false;
            file.Delete();
            return true;
        }
        catch
        {
            return false;
        }

    }

    private string GenerateFolders(string rootPath)
    {
        string userID = _currentUser!.UserID.ToString();
        string date = DateTime.Now.ToString("yyyy-MM-dd");
        string folderPath = $"{rootPath}\\{userID}\\{date}";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        return folderPath;
    }

    private string GenerateFileName()
    {
        string guid = Guid.NewGuid().ToString();
        return DateTime.Now.ToString("HH-mm-ss") + guid.Substring(0, 8) + ".jpg";
    }

}

