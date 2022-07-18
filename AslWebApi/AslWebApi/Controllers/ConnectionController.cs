using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AslWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectionController : ControllerBase
    {

        /// <summary>
        /// return string, this method is used for connection status checking.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Get()
        {
            GlobalFunctions.WriteToFile("Get connection hit");
            return "OK";
        }
    }
}
