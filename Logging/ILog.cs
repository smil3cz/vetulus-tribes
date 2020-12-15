using System.Threading.Tasks;

namespace GreenFoxAcademy.SpaceSettlers.Logging
{
    public interface ILog
    {
        public Task<int?> Log(LogData logData, int? logId);
    }
}
