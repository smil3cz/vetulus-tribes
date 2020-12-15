using System;
using System.ComponentModel.DataAnnotations;

namespace GreenFoxAcademy.SpaceSettlers.Models.Entities
{
    public class Token
    {
        [Key]
        public long Id { get; set; }
        public string Jwt { get; set; }
        public DateTime DateTime { get; set; }

        public Token()
        {
            DateTime = DateTime.UtcNow;
        }
    }
}
