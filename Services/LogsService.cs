using GreenFoxAcademy.SpaceSettlers.Database;
using GreenFoxAcademy.SpaceSettlers.Logging;
using GreenFoxAcademy.SpaceSettlers.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace GreenFoxAcademy.SpaceSettlers.Services
{
    public class LogsService : ILogsService
    {
        private readonly ApplicationDbContext dbContext;

        public LogsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<LogData> RetrieveLogs()
        {
            return dbContext.Logs.ToList();
        }
    }
}
