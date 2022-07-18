using AslWebApi.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace AslWebApi.DAL
{
    public class SeedData
    {
        /// <summary>
        /// Insert some seed data into the database UserInfo table
        /// </summary>
        /// <param name="context"></param>
        public static void SeedDatabase(DatabaseContext context)
        {
            context.Database.Migrate();

            if (!context.UserInfos.Any())
            {
                var User1 = new UserInfo()
                {
                    UserID = 10001,
                    UserName = "Alchemy Software",
                    Address = "Chittagong",
                    UserType = "SUPERADMIN",
                    EmailID = "asl@gmail.com",
                    MobNo = "123",
                    LoginID = "asl@gmail.com",
                    LoginBy = "EMAIL",
                    LoginPW = "123",
                    TimeFr = new TimeSpan(00, 00, 00),
                    TimeTo = new TimeSpan(23, 59, 00),
                    Status = "A",
                    InUserID = 10001,
                    InTime = DateTime.Now,
                    InIPAddress = GlobalFunctions.IpAddress(),
                    InLtude = "",
                    InUserPC = GlobalFunctions.UserPc(),
                };

                var User2 = new UserInfo()
                {
                    UserID = 10101,
                    UserName = "Rahim Uddin",
                    Address = "Chittagong",
                    UserType = "COMPADMIN",
                    EmailID = "rahim@gmail.com",
                    MobNo = "124",
                    LoginID = "rahim@gmail.com",
                    LoginBy = "EMAIL",
                    LoginPW = "123",
                    TimeFr = new TimeSpan(00, 00, 00),
                    TimeTo = new TimeSpan(23, 59, 00),
                    Status = "A",
                    InUserID = 10001,
                    InTime = DateTime.Now,
                    InIPAddress = GlobalFunctions.IpAddress(),
                    InLtude = "",
                    InUserPC = GlobalFunctions.UserPc(),
                };

                var User3 = new UserInfo()
                {
                    UserID = 10102,
                    UserName = "Karim Uddin",
                    Address = "Chittagong",
                    UserType = "USER",
                    EmailID = "karim@gmail.com",
                    MobNo = "125",
                    LoginID = "karim@gmail.com",
                    LoginBy = "EMAIL",
                    LoginPW = "123",
                    TimeFr = new TimeSpan(00, 00, 00),
                    TimeTo = new TimeSpan(23, 59, 00),
                    Status = "A",
                    InUserID = 10001,
                    InTime = DateTime.Now,
                    InIPAddress = GlobalFunctions.IpAddress(),
                    InLtude = "",
                    InUserPC = GlobalFunctions.UserPc(),
                };

                context.UserInfos.AddRange(User1, User2, User3);
                context.SaveChanges();
            }


        }
    }

}
