using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DUTPS.Databases
{
    public class IdentityTable : Table
    {
        [Key]
        [Column("id", Order = 1)]
        [Comment("primary key of table and auto increase")]
        public long Id { set; get; }
    }
}