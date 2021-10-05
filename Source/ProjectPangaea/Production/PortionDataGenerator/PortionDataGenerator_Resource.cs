
namespace ProjectPangaea.Production
{
    public class PortionDataGenerator_Resource : PortionDataGenerator
    {
        public ResourceTypeDef resourceType;
        public override PortionData GenFor(PangaeaThingEntry entry)
        {
            var resource = entry.GetResource(resourceType);
            if (resource == null)
            {
                return null;
            }
            return new PortionData(resource, count);
        }
    }
}  
