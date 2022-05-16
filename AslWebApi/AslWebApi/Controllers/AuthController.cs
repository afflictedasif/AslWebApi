using AslWebApi.DAL.Models;
using AslWebApi.DAL.Repositories;
using AslWebApi.DTOs;
using AslWebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AslWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IGenericRepo<UserInfo> _userRepo;
        private readonly IUserStateService _userStateService;
        private readonly IGenericRepo<CLog> _logRepo;
        private readonly IUserRepo _urepo;

        public AuthController(IGenericRepo<UserInfo> userRepo, IUserStateService userStateService, IGenericRepo<CLog> logRepo, IUserRepo urepo)
        {
            _userRepo = userRepo;
            _userStateService = userStateService;
            _logRepo = logRepo;
            _urepo = urepo;
        }

        /// <summary>
        /// Validate given loginModel with database, Creates a authentication token and return it.
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns>Authentication Token</returns>
        [AllowAnonymous]
        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            string result = await SignIn(loginModel);
            switch (result)
            {
                case "UA": return Unauthorized();
                case "BR": return BadRequest();
                default: return Ok(new { Token = result });
            }
        }

        /// <summary>
        /// Validate the user, insert log, insert or update userState, create a token and return it.
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns>A token string for authentication / UA for unauthorized / BR for bad request</returns>
        [NonAction]
        private async Task<string> SignIn(LoginModel loginModel)
        {
            if (loginModel == null)
            {
                //return BadRequest("Invalid client request");
                return "BR";
            }

            UserInfo? user =
                await _userRepo.GetOneByRawSqlAsync(
                    $"Select top 1 * From UserInfos where LoginID = '{loginModel.UserName}' and LoginPW = '{loginModel.Password}'");

            if (user != null)
            {
                CurrentUser currentUser = new CurrentUser()
                {
                    LoginID = user.LoginID,
                    UserName = user.UserName,
                    UserID = user.UserID,
                    UserInfoID = user.UserInfoID,
                    UserType = user.UserType,
                    IPAddress = GlobalFunctions.IpAddress(),
                    UserPC = GlobalFunctions.UserPc(),
                    Ltude = "",
                };

                var claims = new[] {
                    //new Claim(ClaimTypes.Name, user.UserName),
                    //new Claim(ClaimTypes.Role, user.Role),
                    new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(currentUser)),
                    new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                };

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: "http://localhost:7110",
                    audience: "http://localhost:7110",
                    claims: claims,
                    expires: DateTime.Now.AddHours(2),
                    signingCredentials: signinCredentials
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

                CLog cLog = new CLog()
                {
                    TableName = "Login",
                    LogType = "LOGIN",
                    LogData = "Sign In",
                    LogTime = DateTime.Now,
                    UserID = user.UserID,
                    UserPC = currentUser.UserPC,
                    IPAddress = currentUser.IPAddress
                };
                await _logRepo.CreateAsync(cLog);


                if (await _userStateService.GetLastStateAsync(user.UserID) is null)
                {
                    UserState userState = new UserState()
                    {
                        UserID = user.UserID,
                        CurrentState = "Working",
                        TimeFrom = DateTime.Now,
                        Remarks = "Signed In"
                    };
                    await _userStateService.CreateUserStateAsync(userState);
                }
                else
                {
                    UserState userState = new UserState()
                    {
                        UserID = user.UserID,
                        CurrentState = "Working",
                        TimeFrom = DateTime.Now,
                        Remarks = "Signed In"
                    };
                    await _userStateService.ChangeUserStateAsync(userState);
                }

                //return Ok(new { Token = tokenString });
                return tokenString;
            }
            else
            {
                //return Unauthorized();
                return "UA";
            }
        }

        /// <summary>
        /// Change User State
        /// </summary>
        /// <param name="userState"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost, Route("changeState")]
        public async Task<IActionResult> ChangeState([FromBody] UserState userState)
        {
            bool result = await _userStateService.ChangeUserStateAsync(userState);
            if (result) return Ok();
            return BadRequest();
        }

        /// <summary>
        /// Create User and return token for authentication
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns>Login token if user is successfully created</returns>
        [AllowAnonymous]
        [HttpPost, Route("signup")]
        public async Task<IActionResult> SignUp([FromBody] UserInfo userInfo)
        {
            UserInfo? userCreated = await _urepo.CreateAsync(userInfo);
            if (userCreated is null) return BadRequest();
            string result = await SignIn(new LoginModel() { UserName = userCreated.LoginID, Password = userCreated.LoginPW });
            switch (result)
            {
                case "UA": return Unauthorized();
                case "BR": return BadRequest();
                default: return Ok(new { Token = result });
            }
        }

        /// <summary>
        /// Identify the user and return the last state of the user.
        /// </summary>
        /// <returns>UserState object</returns>
        [Authorize]
        [HttpGet,Route("lastState")]
        public async Task<IActionResult> GetLastState()
        {
            CurrentUser user = GlobalFunctions.CurrentUserS();
            UserState? state = await _userStateService.GetLastStateAsync(user.UserID);
            if (state is null) return NotFound();
            else return Ok(state);
        }

    }

}
