using Verse;

namespace ProjectPangaea
{
    public class AnimalCategory : OrganismCategory
    {
        public PangaeaDiet Diet { get; }

        private AnimalType type;
        public AnimalType Type => type;

        public AnimalCategory(ThingDef thingDef) : base(thingDef)
        {
            Diet = thingDef.race.ResolvedDietCategory.ToPangaeaDiet();
        }

        public void OverrideAnimalType(AnimalType newType)
        {
            type = newType;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj) && obj is AnimalCategory category && category.Diet == Diet && category.Type == Type;
        }

        public override int GetHashCode()
        {
            return Gen.HashCombineStruct(Gen.HashCombineStruct(base.GetHashCode(), Diet), Type);
        }
    }
}
