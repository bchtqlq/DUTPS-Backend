using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DUTPS.Databases.Schemas.Authentication
{
    public class User : IdentityTable
    {
        [Required]
        [Column("username")]
        [Comment("username of user (sv id)")]
        [StringLength(512)]
        public string UserName { set; get; }

        [Required]
        [Column("email")]
        [Comment("email")]
        [StringLength(512)]
        public string Email { set; get; }

        [Column("password_hash")]
        [Comment("password hash")]
        public byte PasswordHash { set; get; }

        [Column("password_salt")]
        [Comment("password salt")]
        public byte PasswordSalt { set; get; }

        /// <summary>
        /// status of account of user, defined in <see cref="DUTPS.Commons.CodeMaster.AccountState"/>
        /// </summary>
        [Column("status")]
        [Comment("status of account of user")]
        public int Status { set; get; }

        /// <summary>
        /// more information of user
        /// </summary>
        public virtual UserInfo Information { set; get; }
    }
}