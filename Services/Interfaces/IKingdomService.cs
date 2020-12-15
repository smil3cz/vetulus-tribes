using System.Threading.Tasks;
using GreenFoxAcademy.SpaceSettlers.Models.DTOs;
using GreenFoxAcademy.SpaceSettlers.Models.DTOs.KingdomDtos;
using GreenFoxAcademy.SpaceSettlers.Models.Entities;

namespace GreenFoxAcademy.SpaceSettlers.Services
{
    public interface IKingdomService
    {
        Task<Kingdom> GetKingdom();
        Task<Kingdom> GetKingdom(long id);
        Task<ResponseKingdomDto> ModifyKingdom(RequestKingdomDto kingdomDto);
        Task<Kingdom> GetKingdomByUser(long userId);
    }
}
