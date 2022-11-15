using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DUTPS.Databases.Schemas.Vehicals;
using Microsoft.EntityFrameworkCore;

namespace DUTPS.Databases.Schemas.Authentication
{
    public class User : IdentityTable
    {
        public User()
        {
            Vehicals = new HashSet<Vehical>();
            CustomerCheckIns = new HashSet<CheckIn>();
            StaffCheckIns = new HashSet<CheckIn>();
            StaffCheckOuts = new HashSet<CheckOut>();
        }
        [Required]
        [Column("username")]
        [Comment("username of user (sv id)")]
        [StringLength(512)]
        public string Username { set; get; }

        [Required]
        [Column("email")]
        [Comment("email")]
        [StringLength(512)]
        public string Email { set; get; }

        [Column("password_hash")]
        [Comment("password hash")]
        public byte[] PasswordHash { set; get; }

        [Column("password_salt")]
        [Comment("password salt")]
        public byte[] PasswordSalt { set; get; }

        /// <summary>
        /// status of account of user, defined in <see cref="DUTPS.Commons.CodeMaster.AccountState"/>
        /// </summary>
        [Column("status")]
        [Comment("status of account of user")]
        public int Status { set; get; }

        /// <summary>
        /// status of account of user, defined in <see cref="DUTPS.Commons.CodeMaster.Role"/>
        /// </summary>
        [Column("role")]
        [Comment("role of account of user")]
        public int Role { set; get; }

        /// <summary>
        /// more information of user
        /// </summary>
        public virtual UserInfo Information { set; get; }

        public virtual HashSet<Vehical> Vehicals { get; set; }

        public virtual HashSet<CheckIn> CustomerCheckIns { get; set; }

        public virtual HashSet<CheckIn> StaffCheckIns { get; set; }

        public virtual HashSet<CheckOut> StaffCheckOuts { get; set; }
    }
}