using System.Threading.Tasks;

namespace GreenFoxAcademy.SpaceSettlers.Tokens
{
    public interface ITokenManager
    {
        Task<bool> IsCurrentActiveToken();

        Task<bool> IsActiveAsync(string token);

        Task DeactivateCurrentAsync();

        Task DeactivateAsync(string token);
    }
}
