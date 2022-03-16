using AslWebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AslWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IScreenShotService _screenShotService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IScreenShotService screenShotService)
        {
            _logger = logger;
            _screenShotService = screenShotService;
        }

        [Authorize]
        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            //var a = HttpContext;
            //var b = HttpHelper.HttpContext;
            var user = GlobalFunctions.CurrentUserS();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [Authorize]
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> OnPostUploadAsync(IFormFile file)
        //public async Task<IActionResult> OnPostUploadAsync([FromForm]FileObj fileobj)
        {
            var a = Request.Form;
            //IFormFile? file = fileobj.file;
            //IFormFile? file = a.Files[0];
            bool uploaded = await _screenShotService.UploadSS(file);
            if (uploaded)
                return Ok(new { Size = file.Length });
            else return BadRequest();
        }
    }

    public class FileObj
    {
        public IFormFile? file { get; set; }
    }
}