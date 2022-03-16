using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AslWebApi.DAL.Models
{
    public class UserInfo
    {
        [Key]
        public int UserInfoID { get; set; }

        #region Unique Keys as PK
        public int UserID { get; set; }
        #endregion

        #region Separate Unique Keys
        [MaxLength(15), Column(TypeName = "varchar(15)"), Required]
        public string MobNo { get; set; } = default!;

        [MaxLength(50), Column(TypeName = "varchar(50)"), Required]
        public string EmailID { get; set; } = default!;

        [MaxLength(50), Column(TypeName = "varchar(50)"), Required]
        public string LoginID { get; set; } = default!;
        #endregion

        #region other properties

        [MaxLength(50), Required]
        public string UserName { get; set; } = default!;
        [MaxLength(20), Column(TypeName = "varchar(20)"), Required]
        public string UserType { get; set; } = default!;

        [MaxLength(100)]
        public string? Address { get; set; } 
        [MaxLength(10), Column(TypeName = "time"), Required]
        public TimeSpan TimeFr { get; set; }
        [MaxLength(10), Column(TypeName = "time"), Required]
        public TimeSpan TimeTo { get; set; }
        [MaxLength(1), Column(TypeName = "varchar(1)"), Required]
        public string Status { get; set; } = default!;

        [MaxLength(5), Column(TypeName = "varchar(5)"), Required]
        public string LoginBy { get; set; } = default!;
        [MaxLength(100), Column(TypeName = "varchar(100)"), Required]
        public string LoginPW { get; set; } = default!;

        #endregion

        #region Common Properties
        public int? InUserID { get; set; }
        public int? UpUserID { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? InTime { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? UpTime { get; set; }
        [MaxLength(100), Column(TypeName = "varchar(100)")]
        public string? InLtude { get; set; }
        [MaxLength(100), Column(TypeName = "varchar(100)")]
        public string? UpLtude { get; set; }
        [MaxLength(50), Column(TypeName = "varchar(50)")]
        public string? InUserPC { get; set; }
        [MaxLength(50), Column(TypeName = "varchar(50)")]
        public string? UpUserPC { get; set; }
        [MaxLength(50), Column(TypeName = "varchar(50)")]
        public string? InIPAddress { get; set; }
        [MaxLength(50), Column(TypeName = "varchar(50)")]
        public string? UpIPAddress { get; set; }

        #endregion

    }
}
