namespace ProjectPangaea.Overrides
{
    public enum OverrideType { Additive, Override, AdditiveOverride }

    public static class OverrideTypeUtility
    {
        public static bool IsOverride(this OverrideType ot)
        {
            switch (ot)
            {
                case OverrideType.Override:
                case OverrideType.AdditiveOverride:
                    return true;
                default: return false;
            }
        }
    }
}
