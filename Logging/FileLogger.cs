using DotNext.Threading;
using GreenFoxAcademy.SpaceSettlers.Helpers;
using Newtonsoft.Json;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace GreenFoxAcademy.SpaceSettlers.Logging
{
    public class FileLogger : ILog
    {
        private readonly AsyncReaderWriterLock fileLock = new AsyncReaderWriterLock();
        private string FilePath { get => AppSettings.LogFilePath; }

        public async Task<int?> Log(LogData logData, int? logId)
        {
            if (logId == null)
            {
                fileLock.EnterUpgradeableReadLockAsync(CancellationToken.None);
                var newLogId = (File.ReadAllLinesAsync(FilePath).Result.Length / 2) + 1;
                fileLock.EnterWriteLockAsync(CancellationToken.None);
                try
                {
                    using StreamWriter writer = new StreamWriter(FilePath, true);
                    var log = JsonConvert.SerializeObject(logData);
                    await writer.WriteLineAsync(newLogId + "|" + log);
                    writer.Close();
                }
                finally
                {
                    fileLock.DisposeAsync();
                }
                return newLogId;
            }
            else
            {
                fileLock.EnterWriteLockAsync(CancellationToken.None);
                try
                {
                    using StreamWriter writer = new StreamWriter(FilePath, true);
                    var log = JsonConvert.SerializeObject(logData);
                    await writer.WriteLineAsync(logId + "|" + log);
                    writer.Close();
                }
                finally
                {
                    fileLock.DisposeAsync();
                }
                return null;
            }
        }
    }
}
