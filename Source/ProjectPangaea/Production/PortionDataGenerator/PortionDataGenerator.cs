
namespace ProjectPangaea.Production
{
    [System.Serializable]
    public abstract class PortionDataGenerator
    {
        protected int count;
        public abstract PortionData GenFor(PangaeaThingEntry entry);
    }
}
