using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DUTPS.Databases
{
    public class Table
    {
        [Column("created_at")]
        [Comment("the time that the record was inserted")]
        public DateTime CreatedAt { set; get; }

        [Column("updated_at")]
        [Comment("record's last update time")]
        public DateTime? UpdatedAt { set; get; }

        /// <summary>
        /// the time that the record was deleted
        /// </summary>
        [Column("deleted_at")]
        [Comment("the time that the record was deleted")]
        public DateTime? DeletedAt { set; get; }

        /// <summary>
        /// true = deleted; false = available
        /// </summary>
        [Column("del_flag")]
        [Comment("true = deleted; false = available")]
        public bool DelFlag { set; get; }

        /// <summary>
        /// force physical delete
        /// </summary>
        [NotMapped]
        public bool ForceDel { set; get; } = false;
    }
}