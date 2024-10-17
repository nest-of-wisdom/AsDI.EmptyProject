using AsDI.Attributes;
using AsDI.Core.Config;
using Microsoft.EntityFrameworkCore;

namespace AsDI.EmptyProject.Repositories
{
    [Service]
    public partial class DbEntities : DbContext
    {
        private readonly string connectionString;

        public DbEntities([Value("AsDI.DataSource.ConnectionString")] string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(connectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }

    [Configuration]
    public class ContextConfig
    {
        public DbContext GetContext()
        {
            return AsDIContext.New<DbEntities>();
        }
    }

}
