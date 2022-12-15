using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DUTPS.Databases.Schemas.Authentication;
using Microsoft.EntityFrameworkCore;

namespace DUTPS.Databases.Schemas.General
{
    public class Faculty : Table
    {
        public Faculty()
        {
            Users = new HashSet<UserInfo>();
        }
        [Key]
        [StringLength(10)]
        [Column("id")]
        [Comment("Id of faculty")]
        public string Id { get; set; }
        
        [StringLength(256)]
        [Column("name")]
        [Comment("Name of Faculty")]
        public string Name { get; set; }

        public virtual HashSet<UserInfo> Users { get; set; }
    }
}