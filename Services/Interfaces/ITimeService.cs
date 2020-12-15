using System.Threading.Tasks;

namespace GreenFoxAcademy.SpaceSettlers.Services
{
    public interface ITimeService
    {
        public Task UpdateResourceAmounts();
        public Task UpdateResourceAmount(int resourceType);
    }
}
