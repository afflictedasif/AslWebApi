using AslWebApi.DAL.Models;
using AslWebApi.DTOs;
using AslWebApi.Services;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Net;

namespace AslWebApi
{
    public class GlobalFunctions
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GlobalFunctions(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        #region Unused Codes

        public CurrentUser? CurrentUser()
        {
            UserInfo? user = null;

            var usr = _httpContextAccessor?.HttpContext?.User.Claims.FirstOrDefault(c => c.Type.Contains("userdata"));

            if (usr == null)
            {
                CurrentUser mockUser = new CurrentUser()
                {
                    LoginID = "",
                    UserName = "Mock User",
                    UserID = 10101,
                    UserInfoID = 100,
                    UserType = "SUPERADMIN",
                    IPAddress = GlobalFunctions.IpAddress(),
                    UserPC = GlobalFunctions.UserPc(),
                    Ltude = "",
                };
                return mockUser;
            }

            user = JsonConvert.DeserializeObject<UserInfo>(usr.Value);
            CurrentUser currentUser = new CurrentUser()
            {
                LoginID = user!.LoginID,
                UserName = user!.UserName,
                UserID = user!.UserID,
                UserInfoID = user!.UserInfoID,
                UserType = user!.UserType,
                IPAddress = GlobalFunctions.IpAddress(),
                UserPC = GlobalFunctions.UserPc(),
                Ltude = "",
            };
            return currentUser;
        }

        #endregion


        #region Static objects and functions

        //Local connection
        public static string ConnectionString = new SqlConnectionStringBuilder { DataSource = "(local)", InitialCatalog = "AslWebApiDB", UserID = "sa", Password = "12233445", MultipleActiveResultSets = true, ConnectTimeout = 0, Pooling = true, MinPoolSize = 0, MaxPoolSize = 4000 }.ToString();
        public static IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);


        /// <summary>
        /// Return local Ip Address
        /// </summary>
        /// <returns></returns>
        public static string IpAddress()
        {
            //var feature = HttpHelper.HttpContext.Features.Get<IHttpConnectionFeature>();
            //string LocalIPAddr = feature?.LocalIpAddress?.ToString();
            //return LocalIPAddr;

            //string a =  HttpHelper.HttpContext.Connection.RemoteIpAddress.ToString();
            //return a;

            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            return ipAddress.ToString();
        }

        /// <summary>
        /// Return Local Computer Name
        /// </summary>
        /// <returns></returns>
        public static string UserPc()
        {
            return Dns.GetHostName();
        }

        /// <summary>
        /// Gets current user data from the httpContext and return partial data from it.
        /// </summary>
        /// <returns>Current User object</returns>
        public static CurrentUser CurrentUserS()
        {
            UserInfo? user = null;

            var usr = HttpHelper.HttpContext?.User.Claims.FirstOrDefault(c => c.Type.Contains("userdata"));

            if (usr == null)
            {
                //if user is loggin in or signing up
                CurrentUser mockUser = new CurrentUser()
                {
                    LoginID = "",
                    UserName = "Anonymous",
                    UserID = 00000,
                    UserInfoID = 000,
                    UserType = "Anonymous",
                    IPAddress = GlobalFunctions.IpAddress(),
                    UserPC = GlobalFunctions.UserPc(),
                    Ltude = "",
                };
                return mockUser;
            }

            user = JsonConvert.DeserializeObject<UserInfo>(usr.Value);
            CurrentUser currentUser = new CurrentUser()
            {
                LoginID = user!.LoginID,
                UserName = user!.UserName,
                UserID = user!.UserID,
                UserInfoID = user!.UserInfoID,
                UserType = user!.UserType,
                IPAddress = IpAddress(),
                UserPC = UserPc(),
                Ltude = "",
            };
            return currentUser;
        }

        #endregion

    }

    public class TwoValue
    {
        public string Label { get; set; }
        public string Value { get; set; }
    }
}
