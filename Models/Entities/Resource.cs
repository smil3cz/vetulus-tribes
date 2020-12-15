using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GreenFoxAcademy.SpaceSettlers.Models.DTOs;

namespace GreenFoxAcademy.SpaceSettlers.Models.Entities
{
    public class Resource
    {
        [Key]
        public long Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ResourceType Type { get; set; }
        public int Amount { get; set; }
        public int MaxAmount { get; set; }
        public int NetGeneration { get; set; }
        public DateTime LastUpdated { get; set; }
        public long KingdomId { get; set; }
        public Kingdom Kingdom { get; set; }

        public Resource()
        {
        }

        public Resource(int resourceType)
        {
            Type = (ResourceType)resourceType;
        }

        public Resource(string resourceType)
        {
            Type = (ResourceType)Enum.Parse(typeof(ResourceType), resourceType);
        }
    }
}
