using AslWebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AslWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IScreenShotService _screenShotService;

        public FilesController(IScreenShotService screenShotService)
        {
            _screenShotService = screenShotService;
        }

        /// <summary>
        /// Saves the file in the server and insert file data into database.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> OnPostUploadAsync(IFormFile file)
        {
            bool uploaded = await _screenShotService.UploadSS(file);
            if (uploaded)
                return Ok(new { Size = file.Length });
            else return BadRequest();
        }
    }
}
