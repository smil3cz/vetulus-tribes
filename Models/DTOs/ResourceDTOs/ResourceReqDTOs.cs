namespace GreenFoxAcademy.SpaceSettlers.Models.DTOs
{
    public class ResourceReqDTO
    {
        public long KingdomId { get; set; }
        public int ResourceType { get; set; }

        public ResourceReqDTO()
        {
        }

        public ResourceReqDTO(long kingdomId)
        {
            KingdomId = kingdomId;
        }

        public ResourceReqDTO(long kingdomId, int resourceType) : this(kingdomId)
        {
            ResourceType = resourceType;
        }
    }
}
