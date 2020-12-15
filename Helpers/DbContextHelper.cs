using GreenFoxAcademy.SpaceSettlers.Database;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace GreenFoxAcademy.SpaceSettlers.Helpers
{
    public class DbContextHelper
    {
        private readonly DbConnection? connection;

        public DbContextHelper()
        {
            connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
        }

        public ApplicationDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection).Options;
            var dbContext = new ApplicationDbContext(options);
            dbContext.Database.EnsureCreated();
            return dbContext;
        }
    }
}
