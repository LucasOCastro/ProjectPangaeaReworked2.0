
namespace ProjectPangaea.Production
{
    public class PortionDataGenerator_Egg : PortionDataGenerator
    {
        public override PortionData GenFor(PangaeaThingEntry entry)
        {
            var eggLayer = entry.ThingDef.GetCompProperties<RimWorld.CompProperties_EggLayer>();
            if (eggLayer == null)
            {
                return null;
            }
            return new PortionData(eggLayer.eggFertilizedDef, count);
        }
    }
}  