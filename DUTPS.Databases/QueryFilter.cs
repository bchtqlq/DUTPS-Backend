using DUTPS.Databases.Schemas.Authentication;
using DUTPS.Databases.Schemas.General;
using Microsoft.EntityFrameworkCore;

namespace DUTPS.Databases
{
    public static class QueryFilter
    {
        public static ModelBuilder HasQueryFilter(ModelBuilder modelBuilder)
        {
            #region Authentication
            modelBuilder.Entity<User>().HasQueryFilter(x => !x.DelFlag);
            modelBuilder.Entity<UserInfo>().HasQueryFilter(x => !x.DelFlag);
            #endregion
            #region General
            modelBuilder.Entity<Faculty>().HasQueryFilter(x => !x.DelFlag);
            #endregion
            return modelBuilder;
        }
    }
}