using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AslWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectionController : ControllerBase
    {

        [HttpGet]
        public string Get()
        {
            return "OK";
        }
    }
}
