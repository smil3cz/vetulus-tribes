using GreenFoxAcademy.SpaceSettlers.Logging;
using System.Collections.Generic;

namespace GreenFoxAcademy.SpaceSettlers.Services.Interfaces
{
    public interface ILogsService
    {
        public List<LogData> RetrieveLogs();
    }
}
