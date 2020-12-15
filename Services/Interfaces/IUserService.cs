using System.Net;
using System.Threading.Tasks;
using GreenFoxAcademy.SpaceSettlers.Models.DTOs;
using GreenFoxAcademy.SpaceSettlers.Models.Entities;

namespace GreenFoxAcademy.SpaceSettlers.Services
{
    public interface IUserService
    {
        Task<User> GetUser(UserDto userDto);
        Task RegUser(UserDto userDto);
        Task<User> AuthenticateUser(UserDto userDto);
        Task<string> GenerateJSONWebToken(User user);
        Task SaveToken(string token);
        Task<User> GetUser(long id);
        Task<HttpStatusCode> SendConfirmationEmail(User user);
        Task<bool> VerifyEmail(string verificationToken);
    }
}
