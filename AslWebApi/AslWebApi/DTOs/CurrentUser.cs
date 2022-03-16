namespace AslWebApi.DTOs
{
    public class CurrentUser
    {
        public int UserInfoID { get; set; }

        public int UserID { get; set; }

        public string? BranchCD { get; set; }
        public string? UserName { get; set; }
        public string? UserType { get; set; }

        public string? LoginID { get; set; }

        public string? Ltude { get; set; }
        public string? UserPC { get; set; }
        public string? IPAddress { get; set; }

    }
}
