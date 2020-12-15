using System;
using GreenFoxAcademy.SpaceSettlers.Models.DTOs;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GreenFoxAcademy.SpaceSettlers.Models.Entities
{
    public class Building
    {
        [Key]
        public long Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BuildingType Type { get; set; }
        public int Level { get; set; }
        public int HP { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime FinishedAt { get; set; }
        public long KingdomId { get; set; }
        public Kingdom Kingdom { get; set; }

        public Building()
        {

        }

        public Building(BuildingType type, Kingdom kingdom)
        {
            this.Level = 1;
            Type = type;
            Kingdom = kingdom;
            HP = Level * 100;
            StartedAt = DateTime.UtcNow;
            FinishedAt = DateTime.UtcNow;
        }
    }
}
