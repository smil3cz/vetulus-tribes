using GreenFoxAcademy.SpaceSettlers.Models.Entities;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GreenFoxAcademy.SpaceSettlers.Models.DTOs
{
    public enum ShipType
    {
        [EnumMember(Value = "StarFighter")]
        StarFighter,
        [EnumMember(Value = "Cruiser")]
        Cruiser
    }

    public class ShipDto
    {
        public long? Id { get; set; }
        public string? Type { get; set; }
        public int? Level { get; set; }
        public int? Hp { get; set; }
        public int? Attack { get; set; }
        public int? Defence { get; set; }
        public long? KingdomId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime FinishedAt { get; set; }
        public List<ShipDto>? Ships { get; set; }

        public ShipDto()
        {

        }

        public ShipDto(Ship ship)
        {
            Id = ship.Id;
            Type = ship.Type.ToString();
            Level = ship.Level;
            Hp = ship.Hp;
            Attack = ship.Attack;
            Defence = ship.Defence;
            StartedAt = ship.StartedAt;
            FinishedAt = ship.FinishedAt;
        }

        public static List<ShipDto> ToListShipDto(List<Ship> ships)
        {
            var shipsDto = new List<ShipDto>();
            foreach (var item in ships)
            {
                var shipDto = new ShipDto(item);
                shipsDto.Add(shipDto);
            }
            return shipsDto;
        }

        public static Ship ToEntity(ShipDto shipDto)
        {
            return new Ship()
            {
                Id = shipDto.Id ?? default,
                Type = (ShipType)Enum.Parse(typeof(ShipType), shipDto.Type),
                Level = shipDto.Level ?? default,
                Hp = shipDto.Hp ?? default,
                Attack = shipDto.Attack ?? default,
                Defence = shipDto.Defence ?? default,
                StartedAt = shipDto.StartedAt,
                FinishedAt = shipDto.FinishedAt
            };
        }
    }
}
