using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.Domain.Entities
{
    [Table(Name = "tblRoles")]
    public class Role
    {
        [Column(IsPrimaryKey = true, AutoSync = AutoSync.OnInsert, IsDbGenerated = true)]
        public int RoleId { get; set; }

        [Column]
        public string RoleName { get; set; }

        [Column]
        public string RoleDescription { get; set; }
    }

    [Table(Name = "tblUsersInRoles")]
    public class UsersInRoles
    {
        [Column(IsPrimaryKey = true, AutoSync = AutoSync.OnInsert, IsDbGenerated = true)]
        public int Id { get; set; }

        [Column]
        public int UserId { get; set; }

        [Column]
        public int RoleId { get; set; }

        private EntityRef<Role> _Role;
        [Association(Storage = "_Role", ThisKey = "RoleId", OtherKey = "RoleId")]
        public Role Role
        {
            set
            {
                _Role.Entity = value;
            }
            get
            {
                return _Role.Entity;
            }
        }
    }
}
