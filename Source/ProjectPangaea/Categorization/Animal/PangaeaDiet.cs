using Verse;

namespace ProjectPangaea
{
    public enum PangaeaDiet { Carnivore, Herbivore }

    public static class PangaeaDietUtility
    {
        public static PangaeaDiet ToPangaeaDiet(this DietCategory diet)
        {
            switch (diet)
            {
                case DietCategory.Ovivorous:
                case DietCategory.Omnivorous:
                case DietCategory.Carnivorous:
                    return PangaeaDiet.Carnivore;
                case DietCategory.Dendrovorous:
                case DietCategory.Herbivorous:
                    return PangaeaDiet.Herbivore;
                default:
                    Log.Error("Tried to convert diet from NeverEats to pangaea diet. Returning omnivorous.");
                    return ToPangaeaDiet(DietCategory.Omnivorous);
            }
        }
    }

}
