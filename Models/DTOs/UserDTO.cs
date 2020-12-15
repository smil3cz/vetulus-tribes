using GreenFoxAcademy.SpaceSettlers.Models.Entities;
using Newtonsoft.Json;

namespace GreenFoxAcademy.SpaceSettlers.Models.DTOs
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class UserDto
    {
        public long? Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public long? KingdomId { get; set; }
        public string? KingdomName { get; set; }
        public string? Avatar { get; set; }
        public int? Points { get; set; }

        public UserDto()
        {
        }

        public UserDto(User user)
        {
            Id = user.Id;
            Username = user.Username;
            KingdomId = user.Kingdom.Id;
            Email = user.Email;
        }

        public User ToEntity(UserDto user)
        {
            return new User
            {
                Username = user.Username
            };
        }
    }
}
