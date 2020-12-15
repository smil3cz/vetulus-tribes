using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GreenFoxAcademy.SpaceSettlers.Logging
{
    public class Response
    {
        [Key]
        public int ResponseId { get; set; }
        public int StatusCode { get; set; }
        public string? Body { get; set; }
        public int LogDataId { get; set; }
        [JsonIgnore]
        public LogData LogData { get; set; }
    }
}
