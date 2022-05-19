using AslWebApi.DAL.Models;

namespace AslWebApi.DTOs
{
    public class DashBoardVM
    {
        public int UserID { get; set; }
        public string UserName { get; set; }

        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);

        public DateTime? FromDt
        {
            get => DateTime.Parse(FromDtString!, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
        }
        public string? FromDtString { get; set; }
        public DateTime? ToDt
        {
            get => DateTime.Parse(ToDtString!, dateformat, System.Globalization.DateTimeStyles.AssumeLocal);
        }
        public string? ToDtString { get; set; }

        public List<UserState> UserStates { get; set; }
    }
}
