using GreenFoxAcademy.SpaceSettlers.Models.Entities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace GreenFoxAcademy.SpaceSettlers.Models.DTOs
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ResponseKingdomDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public long UserId { get; set; }
        public Location? Location { get; set; }
        public List<BuildingDto>? Buildings { get; set; }
        public List<ResourceDTO>? Resources { get; set; }
        public List<ShipDto>? Ships { get; set; }

        public ResponseKingdomDto()
        {

        }

        public ResponseKingdomDto(Kingdom kingdom)
        {
            Id = kingdom.Id;
            Name = kingdom.Name;
            UserId = kingdom.UserId;
            Location = kingdom.Location;
            Buildings = kingdom.Buildings.Select(b => new BuildingDto(b)).ToList();
            Resources = kingdom.Resources.Select(r => new ResourceDTO(r)).ToList();
            Ships = kingdom.Ships.Select(s => new ShipDto(s)).ToList();
        }

        public Kingdom ToEntity(ResponseKingdomDto kingdom)
        {
            return new Kingdom
            {
                Id = kingdom.Id,
                Name = kingdom.Name,
                UserId = kingdom.UserId,
                Location = kingdom.Location
            };
        }
    }
}
