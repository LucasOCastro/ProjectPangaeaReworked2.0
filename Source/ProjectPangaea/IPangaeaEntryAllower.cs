namespace ProjectPangaea
{
    public interface IPangaeaEntryAllower
    {
        void Allow(PangaeaThingEntry entry);
        void Disallow(PangaeaThingEntry entry);
        void Toggle(PangaeaThingEntry entry);
        bool Allows(PangaeaThingEntry entry);
    }
}
