using GreenFoxAcademy.SpaceSettlers.Models.Entities;
using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GreenFoxAcademy.SpaceSettlers.Models.DTOs
{
    public class BuildingDto
    {
        public long Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BuildingType Type { get; set; }
        public int Level { get; set; }
        public int HP { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime FinishedAt { get; set; }
        public BuildingDto()
        {
        }

        public BuildingDto(Building building)
        {
            Id = building.Id;
            Type = building.Type;
            Level = building.Level;
            HP = building.HP;
            StartedAt = building.StartedAt;
            FinishedAt = building.FinishedAt;
        }

        public Building ToEntity(BuildingDto buildingDto)
        {
            return new Building
            {
                Id = buildingDto.Id,
                Type = buildingDto.Type,
                Level = buildingDto.Level,
                HP = buildingDto.HP,
                StartedAt = buildingDto.StartedAt,
                FinishedAt = buildingDto.FinishedAt
            };
        }
    }
    public enum BuildingType
    {
        [EnumMember(Value = "townhall")]
        townhall,
        [EnumMember(Value = "farm")]
        farm,
        [EnumMember(Value = "mine")]
        mine,
        [EnumMember(Value = "barracks")]
        barracks
    }
}
