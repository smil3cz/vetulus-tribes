using System;
using GreenFoxAcademy.SpaceSettlers.Models.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenFoxAcademy.SpaceSettlers.Models.Entities
{
    public class Ship
    {
        [Key]
        public long Id { get; set; }
        public ShipType Type { get; set; }
        public int Level { get; set; }
        public int Hp { get; set; }
        public int Attack { get; set; }
        public int Defence { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime FinishedAt { get; set; }
        [ForeignKey("KingdomId")]
        public long KingdomId { get; set; }
        public Kingdom Kingdom { get; set; }

        public Ship()
        {

        }

        public Ship(ShipType type, Kingdom kingdom)
        {
            Level = 1;
            Type = type;
            Kingdom = kingdom;
            if (type == ShipType.Cruiser)
            {
                Hp = 25;
                Attack = 2;
                Defence = 2;
            }
            else
            {
                Hp = 10;
                Attack = 1;
                Defence = 1;
            }
        }
    }
}
