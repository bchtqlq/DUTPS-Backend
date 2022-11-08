using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using DUTPS.Databases.Schemas.Authentication;
using DUTPS.Databases.Schemas.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DUTPS.Databases
{
    public class DataContext : DbContext
    {
        public DataContext(
            DbContextOptions<DataContext> options
        ) : base(options)
        {
        }

        #region Add table to DB
        #region Authentication
        /// <summary>
        /// authusr<br/>table to save user who will use system
        /// </summary>
        public virtual DbSet<User> Users { get; set; }
        /// <summary>
        /// authusrinfo<br/>table to save more information of user
        /// </summary>
        public virtual DbSet<UserInfo> UserInfos { get; set; }
        #endregion

        #region General
        /// <summary>
        /// system_exception_logs<br/>Table to saves faculties of school
        /// </summary>
        public virtual DbSet<Faculty> Faculties { get; set; }
        #endregion
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            QueryFilter.HasQueryFilter(modelBuilder);
            ModelCreate.OnModelCreating(modelBuilder);
        }

        public DbConnection GetConnection()
        {
            DbConnection _connection = Database.GetDbConnection();
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
            return _connection;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override int SaveChanges()
        {
            OnBeforeSaving();
            return base.SaveChanges();
        }

        public async Task RollbackAsync(IDbContextTransaction transaction)
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync();
            }
        }

        public void Rollback(IDbContextTransaction transaction)
        {
            if (transaction != null)
            {
                transaction.Rollback();
            }
        }

        private void OnBeforeSaving()
        {
            // If have change database
            if (ChangeTracker.HasChanges())
            {
                // get additional information
                DateTime now = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                foreach (var entry in ChangeTracker.Entries())
                {
                    try
                    {
                        if (entry.Entity is Table root)
                        {
                            switch (entry.State)
                            {
                                // If it is a new insert, change the created information
                                case EntityState.Added:
                                    {

                                        root.CreatedAt = now;
                                        root.UpdatedAt = null;
                                        root.DeletedAt = null;
                                        root.DelFlag = false;
                                        break;
                                    }
                                // If it is a modifine, change the updated information
                                case EntityState.Modified:
                                    {
                                        if (root.DelFlag)
                                        {
                                            root.DeletedAt = now;
                                        }
                                        else
                                        {
                                            root.UpdatedAt = now;
                                            root.DeletedAt = null;
                                        }
                                        break;
                                    }
                                // If it is delete, change to soft delete
                                case EntityState.Deleted:
                                    {
                                        if (!root.ForceDel)
                                        {
                                            entry.State = EntityState.Modified;
                                            root.DeletedAt = now;
                                            root.DelFlag = true;
                                        }
                                        break;
                                    }
                            }
                        }
                    }
                    catch { }
                }
            }
        }
    }
}