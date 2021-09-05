using Verse;

namespace ProjectPangaea
{
    public class PlantCategory : OrganismCategory
    {
        public PlantCategory(ThingDef thingDef) : base(thingDef)
        {
        }

        public override string GetSubTexPath() => "Plant";
    }
}
