using System.Collections.Generic;

namespace GreenFoxAcademy.SpaceSettlers.Helpers
{
    public class PriceCollection
    {
        public Dictionary<string, int>? PriceList;
        public PriceCollection()
        {
            PriceList = new Dictionary<string, int>
            {
                { "farm", 250},
                { "barracks", 500},
                { "mine", 750},
                { "upgradeBuilding", 100},
                { "upgradePrice", 10},
                { "starfighter", 30},
                { "cruiser", 50}
            };
        }
    }
}
