using DUTPS.Databases.Schemas.Authentication;
using DUTPS.Databases.Schemas.Vehicals;
using Microsoft.EntityFrameworkCore;

namespace DUTPS.Databases
{
    public static class ModelCreate
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

            modelBuilder.Entity<Vehical>()
                .HasOne(e => e.Customer)
                .WithMany(e => e.Vehicals)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CheckIn>()
                .HasOne(e => e.Customer)
                .WithMany(e => e.CustomerCheckIns)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<CheckIn>()
                .HasOne(e => e.Staff)
                .WithMany(e => e.StaffCheckIns)
                .HasForeignKey(e => e.StaffId)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<CheckIn>()
                .HasOne(e => e.Vehical)
                .WithMany(e => e.CheckIns)
                .HasForeignKey(e => e.VehicalId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CheckOut>()
                .HasOne(e => e.CheckIn)
                .WithOne(e => e.CheckOut)
                .HasForeignKey<CheckOut>(e => e.CheckInId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CheckOut>()
                .HasOne(e => e.Staff)
                .WithMany(e => e.StaffCheckOuts)
                .HasForeignKey(e => e.StaffId)
                .OnDelete(DeleteBehavior.NoAction);

            return modelBuilder;
        }
    }
}