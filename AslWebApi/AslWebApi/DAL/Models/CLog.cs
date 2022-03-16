using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AslWebApi.DAL.Models
{
    public class CLog
    {
        [Key]
        public long ClogID { get; set; }
        [Required, MaxLength(50), Column(TypeName = "varchar(50)")]
        public string TableName { get; set; } = default!;
        [Required, MaxLength(6), Column(TypeName = "varchar(6)")]
        public string LogType { get; set; } = default!;
        public string LogData { get; set; } = default!;
        [Required, Column(TypeName = "smalldatetime")]
        public DateTime? LogTime { get; set; }
        public int UserID { get; set; }



        [MaxLength(50), Column(TypeName = "varchar(50)")]
        public string? Ltude { get; set; }
        [MaxLength(50), Column(TypeName = "varchar(50)")]
        public string? UserPC { get; set; }
        [MaxLength(50), Column(TypeName = "varchar(50)")]
        public string? IPAddress { get; set; }
    }
}
