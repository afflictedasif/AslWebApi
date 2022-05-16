using AslWebApi.DAL.Models;
using AslWebApi.DAL.Repositories;
using AslWebApi.DTOs;

namespace AslWebApi.Services;

public interface IFileUploader
{

    /// <summary>
    /// Saves file to server. File should be jpg or png only.
    /// </summary>
    /// <param name="file"></param>
    /// <returns>an array of folder path and file name</returns>
    public Task<string[]?> SaveFileAsync(IFormFile file);
    /// <summary>
    /// Delete the file from the given path
    /// </summary>
    /// <param name="filePath">Fully qualified path of the file</param>
    /// <returns></returns>
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

    #region Unused Codes
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
    #endregion

    /// <summary>
    /// Saves file to server. File should be jpg or png only.
    /// </summary>
    /// <param name="file"></param>
    /// <returns>an array of folder path and file name</returns>
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

    /// <summary>
    /// Delete the file from the given path
    /// </summary>
    /// <param name="filePath">Fully qualified path of the file</param>
    /// <returns></returns>
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
    /// <summary>
    /// Generate directory with userid and date as subdirectory
    /// </summary>
    /// <param name="rootPath"></param>
    /// <returns>Full path of the generated directory</returns>
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
    /// <summary>
    /// Generate uniqe file name with timestamp and guid with jpg file extenstion
    /// </summary>
    /// <returns>Generated file name</returns>
    private string GenerateFileName()
    {
        string guid = Guid.NewGuid().ToString();
        return DateTime.Now.ToString("HH-mm-ss") + "-" + guid.Substring(0, 8) + ".jpg";
    }

}

