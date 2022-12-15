using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DUTPS.Databases.Schemas.General;
using Microsoft.EntityFrameworkCore;


namespace DUTPS.Databases.Schemas.Authentication
{
    public class UserInfo : Table
    {
        [Key]
        [Column("user_id", Order = 1)]
        [Comment("primary key to identity the user")]
        public long UserId { set; get; }

        [StringLength(50)]
        [Column("name")]
        [Comment("name of user")]
        public string Name { get; set; }
        
        /// <summary>
        /// gender of user, defined in <see cref="DUTPS.Commons.CodeMaster.Gender"/>
        /// </summary>
        [Column("gender")]
        [Comment("gender of user")]
        public int? Gender { get; set; }

        [Column("birthday", TypeName = "date")]
        [Comment("birthday of user")]
        public DateTime? Birthday { get; set; }

        [StringLength(20)]
        [Column("phoneNumber")]
        [Comment("phone number of user")]
        public string PhoneNumber { get; set; }

        [StringLength(50)]
        [Column("class")]
        [Comment("class name of user")]
        public string Class { get; set; }

        [StringLength(10)]
        [Column("faculty_id")]
        [Comment("Faculty Id Of User")]
        public string FacultyId { get; set;}

        public virtual Faculty Faculty { set; get; }

        public virtual User User { set; get; }
    }
}