using AslWebApi.DAL.Models;

namespace AslWebApi.DTOs
{
    public class ScreenShotsVM
    {
        public int CLogID { get; set; }
        
        public List<ScreenShot>? ScreenShots { get; set; }

        public UserState? userState { get; set; }


    }
}
