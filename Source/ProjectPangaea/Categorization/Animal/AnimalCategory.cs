using Verse;

namespace ProjectPangaea
{
    public class AnimalCategory : OrganismCategory
    {
        public PangaeaDiet Diet { get; }
        public AnimalType Type { get; }

        public AnimalCategory(ThingDef thingDef, AnimalType type) : base(thingDef)
        {
            Diet = thingDef.race.ResolvedDietCategory.ToPangaeaDiet();
            Type = type;
        }

        public override string GetSubTexPath() => (Type != AnimalType.Unspecified) ? $"{Diet}{Type}/{ExtinctionStatus}" : "Default";

        public override bool Equals(object obj)
        {
            return base.Equals(obj) && obj is AnimalCategory category && category.Diet == Diet && category.Type == Type;
        }

        public override int GetHashCode()
        {
            return Gen.HashCombineStruct(Gen.HashCombineStruct(base.GetHashCode(), Diet), Type.GetHashCode());
        }
    }
}
