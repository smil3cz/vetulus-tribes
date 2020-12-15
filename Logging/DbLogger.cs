using GreenFoxAcademy.SpaceSettlers.Database;
using System.Linq;
using System.Threading.Tasks;

namespace GreenFoxAcademy.SpaceSettlers.Logging
{
    public class DBLogger : ILog
    {
        private readonly ApplicationDbContext dbContext;

        public DBLogger(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int?> Log(LogData logData, int? logId)
        {
            if (logId == null)
            {
                dbContext.Logs.Add(logData);
                dbContext.SaveChanges();
                return dbContext.Logs.Where(l => l.Request.TraceIdentifier == logData.Request.TraceIdentifier).First().LogDataId;
            }
            var log = dbContext.Logs.FirstOrDefault(l => l.LogDataId == logId);
            log.Level = logData.Level;
            log.Response.Body = logData.Response.Body;
            log.Response.StatusCode = logData.Response.StatusCode;
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
            return null;
        }
    }
}

