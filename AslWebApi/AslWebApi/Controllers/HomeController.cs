using AslWebApi.DAL;
using AslWebApi.DAL.Models;
using AslWebApi.DAL.Repositories;
using AslWebApi.DTOs;
using AslWebApi.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AslWebApi.Controllers
{
    [Route("[controller]/[action]/")]
    public class HomeController : Controller
    {
        private readonly IGenericRepo<UserInfo> _userRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGenericRepo<CLog> _logRepo;
        private readonly DatabaseContext _db;
        private readonly CurrentUser _currentUser;

        public HomeController(IGenericRepo<UserInfo> userRepo, IHttpContextAccessor httpContextAccessor, IGenericRepo<CLog> logRepo, DatabaseContext db)
        {
            _userRepo = userRepo;
            _httpContextAccessor = httpContextAccessor;
            _logRepo = logRepo;
            _db = db;
            _currentUser = GlobalFunctions.CurrentUserS();
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(LoginModel loginModel)
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (loginModel is null || loginModel.UserName is null || loginModel.Password is null)
            {
                //ViewBag.ErrMsg = "Invalid request";
                return RedirectToActionPermanent("Login");
            }

            UserInfo? user =
                await _userRepo.GetOneByRawSqlAsync(
                    $"Select top 1 * From UserInfos where LoginID = '{loginModel.UserName}' and LoginPW = '{loginModel.Password}'");

            if (user is null)
            {
                ViewBag.ErrMsg = "Incorrect UserID or Password.";
                return View();
            }

            if (user.UserType == "SUPERADMIN")
            {
                string token = await getLoginToken(user);

                CookieOptions cookie = new CookieOptions();
                cookie.Expires = DateTime.Now.AddMinutes(120);
                CookieBuilder cb = new CookieBuilder();
                _httpContextAccessor!.HttpContext!.Response.Cookies.Append("AslWebApiCookie", token, cookie);

                return RedirectToActionPermanent("DashBoard");
            }
            else
            {
                ViewBag.ErrMsg = "Acces Denied";
                return View();
            }

        }

        /// <summary>
        /// create a token and return it.
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns>A token string for authentication / UA for unauthorized / BR for bad request</returns>
        [NonAction]
        private async Task<string> getLoginToken(UserInfo user)
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

            return tokenString;
        }


        [AuthorizeWithRedirect]
        public IActionResult DashBoard()
        {
            DashBoardVM vm = new DashBoardVM();
            vm.FromDtString = DateTime.Now.ToString("dd/MM/yyyy");
            vm.ToDtString = DateTime.Now.ToString("dd/MM/yyyy");
            return View(vm);
        }

        public IActionResult DashBoard1()
        {
            DashBoardVM vm = new DashBoardVM();
            vm.FromDtString = DateTime.Now.ToString("dd/MM/yyyy");
            vm.ToDtString = DateTime.Now.ToString("dd/MM/yyyy");
            return View(vm);
        }

        [AuthorizeWithRedirect]
        [HttpPost]
        public async Task<IActionResult> DashBoard(DashBoardVM vm)
        {


            int UserID = vm.UserID;
            List<CLog> logs = await _logRepo.GetAll().Where(l => l.UserID == UserID
                                                            && ((DateTime)l.LogTime!).Date >= vm.FromDt //&& ((DateTime)l.LogTime!).Date <= vm.ToDt
                                                            && l.TableName == "UserStates").OrderByDescending(l => l.LogTime).ToListAsync();
            List<UserState> userStates = new List<UserState>();

            if (vm.ToDt?.Date == DateTime.Now.Date)
            {
                UserState? currentState = await _db.UserStates.FirstOrDefaultAsync(us => us.UserID == UserID);
                if (currentState is not null)
                {
                    currentState.TimeTo = DateTime.Now;
                    currentState.Remarks = "Current State (Auto Generated)";
                    currentState.ClogID = -1;
                    userStates.Add(currentState);
                }
            }

            foreach (CLog log in logs)
            {
                UserState? state = JsonConvert.DeserializeObject<UserState>(log.LogData);
                state.ClogID = log.ClogID;
                if (state is not null)
                    userStates.Add(state);
            }
            vm.UserStates = (from us in userStates
                             where us.TimeTo <= vm.ToDt?.AddDays(1)
                             orderby us.TimeTo descending
                             select us).ToList();

            return View(vm);
        }


        [AuthorizeWithRedirect]
        [HttpPost]
        public async Task<IActionResult> DashBoard1(DashBoardVM vm)
        {


            int UserID = vm.UserID;
            List<CLog> logs = await _logRepo.GetAll().Where(l => l.UserID == UserID
                                                            && ((DateTime)l.LogTime!).Date >= vm.FromDt //&& ((DateTime)l.LogTime!).Date <= vm.ToDt
                                                            && l.TableName == "UserStates").OrderByDescending(l => l.LogTime).ToListAsync();
            List<UserState> userStates = new List<UserState>();

            if (vm.ToDt?.Date == DateTime.Now.Date)
            {
                UserState? currentState = await _db.UserStates.FirstOrDefaultAsync(us => us.UserID == UserID);
                if (currentState is not null)
                {
                    currentState.TimeTo = DateTime.Now;
                    currentState.Remarks = "Current State (Auto Generated)";
                    currentState.ClogID = -1;
                    userStates.Add(currentState);
                }
            }

            foreach (CLog log in logs)
            {
                UserState? state = JsonConvert.DeserializeObject<UserState>(log.LogData);
                state.ClogID = log.ClogID;
                if (state is not null)
                    userStates.Add(state);
            }
            vm.UserStates = (from us in userStates
                             where us.TimeTo <= vm.ToDt?.AddDays(1)
                             orderby us.TimeTo descending
                             select us).ToList();

            return View(vm);
        }

        [AuthorizeWithRedirect, HttpPost]
        public async Task<IActionResult> ScreenShots(long CLogID, int? UserID)
        {
            ScreenShotsVM vm = new ScreenShotsVM();

            UserState? state;

            if (CLogID == -1)
            {
               state =  await _db.UserStates.FirstOrDefaultAsync(s => s.UserID == UserID);
               if (state is not null) state.TimeTo = state.TimeTo ?? DateTime.Now;
            }
            else
            {
                CLog? log = await _db.CLogs.FindAsync(CLogID);
                state = JsonConvert.DeserializeObject<UserState>(log.LogData);
            }

            if (state is not null)
            {
                List<ScreenShot> screenshots = await (from ss in _db.ScreenShots
                                                      where ss.InTime >= state.TimeFrom && ss.InTime <= state.TimeTo
                                                      orderby ss.InTime descending
                                                      select ss).ToListAsync();
                vm.ScreenShots = screenshots;
                //    .ForEach(s =>
                //{
                //    string fullPath = s.DirPath + "\\" +  ss.FileName;
                //fullPath = (fullPath.Split("wwwroot"))[1];
                //});
            }

            return View(vm);
        }

        [AuthorizeWithRedirect, HttpPost]
        public async Task<IActionResult> ScreenShots1(long CLogID, int? UserID)
        {
            ScreenShotsVM vm = new ScreenShotsVM();

            UserState? state;

            if (CLogID == -1)
            {
                state = await _db.UserStates.FirstOrDefaultAsync(s => s.UserID == UserID);
                if (state is not null) state.TimeTo = state.TimeTo ?? DateTime.Now;
            }
            else
            {
                CLog? log = await _db.CLogs.FindAsync(CLogID);
                state = JsonConvert.DeserializeObject<UserState>(log.LogData);
            }

            if (state is not null)
            {
                List<ScreenShot> screenshots = await (from ss in _db.ScreenShots
                                                      where ss.InTime >= state.TimeFrom && ss.InTime <= state.TimeTo
                                                      orderby ss.InTime descending
                                                      select ss).ToListAsync();
                vm.ScreenShots = screenshots;
                //    .ForEach(s =>
                //{
                //    string fullPath = s.DirPath + "\\" +  ss.FileName;
                //fullPath = (fullPath.Split("wwwroot"))[1];
                //});
            }

            return View(vm);
        }

        [AuthorizeWithRedirect]
        public IActionResult LogOut()
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("AslWebApiCookie");
            return RedirectToAction("Login", "Home");
        }









        [HttpPost]
        public List<TwoValue> GetCompletionListUserName(string prefix)
        {
            // your code to query the database goes here
            List<TwoValue> result = new List<TwoValue>();

            string query = $@"
SELECT TOP 20 UserName AS Label, UserId AS Value
FROM UserInfos
WHERE UserName LIKE @SearchText + '%'
ORDER BY UserName
";

            SqlConnection conn = new SqlConnection(GlobalFunctions.ConnectionString);
            using (SqlCommand obj_Sqlcommand = new SqlCommand(query, conn))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                obj_Sqlcommand.Parameters.AddWithValue("@SearchText", prefix);
                SqlDataReader obj_result = obj_Sqlcommand.ExecuteReader();
                while (obj_result.Read())
                {
                    string label = obj_result["Label"].ToString().TrimEnd();
                    string value = obj_result["Value"].ToString().TrimEnd();
                    result.Add(new TwoValue() { Label = label, Value = value });
                }
            }
            if (conn.State != ConnectionState.Closed)
                conn.Close();
            //return Json(result);
            return result;
        }
    }
}
