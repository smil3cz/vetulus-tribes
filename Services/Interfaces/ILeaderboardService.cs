using GreenFoxAcademy.SpaceSettlers.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenFoxAcademy.SpaceSettlers.Services.Interfaces
{
    public interface ILeaderboardService
    {
        Task<List<LeaderDto>> GetLeaderboard(string leaderboardType);
    }
}
