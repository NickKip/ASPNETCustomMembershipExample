using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.Domain.Entities
{
    [Table(Name = "tblUsers")]
    public class User
    {
        [Column(IsDbGenerated = true, IsPrimaryKey = true, AutoSync = AutoSync.OnInsert)]
        public int UserId { get; set; }

        [Column]
        public string UserGuid { get; set; }

        [Column]
        public string Username { get; set; }

        [Column]
        public string Email { get; set; }

        [Column]
        public string PasswordHash { get; set; }

        [Column]
        public byte[] PasswordSalt { get; set; }

        [Column]
        public string SecurityQuestion { get; set; }

        [Column]
        public string SecurityAnswer { get; set; }

        [Column]
        public bool EmailVerified { get; set; }

        [Column]
        public DateTime SignUpDate { get; set; }

        [Column]
        public DateTime LastLoginDate { get; set; }

        [Column]
        public DateTime LastActiveTime { get; set; }
    }
}
