using GreenFoxAcademy.SpaceSettlers.Models.Entities;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GreenFoxAcademy.SpaceSettlers.Models.DTOs
{
    public class ResourceDTO
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ResourceType Type { get; set; }
        public int Amount { get; set; }
        public int NetGeneration { get; set; }

        public ResourceDTO()
        {
        }

        public ResourceDTO(Resource entity)
        {
            Type = entity.Type;
            Amount = entity.Amount;
            NetGeneration = entity.NetGeneration;
        }

        public Resource ToEntity(ResourceDTO resourceDto)
        {
            return new Resource()
            {
                Type = resourceDto.Type,
                Amount = resourceDto.Amount,
                NetGeneration = resourceDto.NetGeneration,
            };
        }
    }
    public enum ResourceType
    {
        [EnumMember(Value = "food")]
        food,
        [EnumMember(Value = "gold")]
        gold
    };
}
