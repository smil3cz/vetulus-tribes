using System;
using System.ComponentModel.DataAnnotations;

namespace GreenFoxAcademy.SpaceSettlers.Logging
{
    public class LogData
    {
        [Key]
        public int LogDataId { get; set; }
        public DateTime Time { get; set; }
        public string? Username { get; set; }
        public string Level { get; set; }
        public Request? Request { get; set; }
        public Response? Response { get; set; }

        public LogData()
        {
            Request = new Request();
            Response = new Response();
        }
    }
}
