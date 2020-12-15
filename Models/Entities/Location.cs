using Microsoft.EntityFrameworkCore;

namespace GreenFoxAcademy.SpaceSettlers.Models.Entities
{
    [Owned]
    public class Location
    {
        public int PositionX { get; set; }
        public int PositionY { get; set; }
    }
}
