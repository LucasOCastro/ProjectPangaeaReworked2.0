
namespace ProjectPangaea.Production
{
    public class PortionDataGenerator_Corpse : PortionDataGenerator
    {
        public override PortionData GenFor(PangaeaThingEntry entry)
        {
            var corpseDef = entry.ThingDef.race?.corpseDef;
            if (corpseDef == null)
            {
                return null;
            }
            return new PortionData(corpseDef, count);
        }
    }
}