using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace GreenFoxAcademy.SpaceSettlers.Logging
{
    public class Request
    {
        [Key]
        public int RequestId { get; set; }
        public string? Path { get; set; }
        public string? Endpoint { get; set; }
        public string? TraceIdentifier { get; set; }
        public string? Body { get; set; }
        public int LogDataId { get; set; }
        [JsonIgnore]
        public LogData LogData { get; set; }

        public Request()
        {
        }
    }
}
