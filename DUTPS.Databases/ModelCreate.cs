using DUTPS.Databases.Schemas.Authentication;
using Microsoft.EntityFrameworkCore;

namespace DUTPS.Databases
{
    public class ModelCreate
    {
        public static ModelBuilder OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Authentication

            modelBuilder.Entity<UserInfo>()
                .HasOne(e => e.User)
                .WithOne(e => e.Information)
                .HasForeignKey<UserInfo>(e => e.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<UserInfo>()
                .HasOne(e => e.Faculty)
                .WithMany(e => e.Users)
                .HasForeignKey(e => e.FacultyId)
                .OnDelete(DeleteBehavior.NoAction);
            #endregion
            return modelBuilder;
        }
    }
}