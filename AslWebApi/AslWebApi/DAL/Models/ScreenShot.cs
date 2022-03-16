using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AslWebApi.DAL.Models
{
    public class ScreenShot
    {
        [Key]
        public long ScreenShotID { get; set; }

        public int UserID { get; set; }

        [MaxLength(200), Column(TypeName = "varchar(200)"), Required]
        public string DirPath { get; set; } = default!;
        [MaxLength(50), Column(TypeName = "varchar(50)"), Required]
        public string FileName { get; set; } = default!;

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
