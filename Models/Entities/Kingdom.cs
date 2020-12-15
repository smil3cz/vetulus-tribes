using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GreenFoxAcademy.SpaceSettlers.Models.Entities
{
    public class Kingdom
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public Location? Location { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public List<Building> Buildings { get; set; }
        public List<Resource> Resources { get; set; }
        public List<Ship> Ships { get; set; }
    }
}
