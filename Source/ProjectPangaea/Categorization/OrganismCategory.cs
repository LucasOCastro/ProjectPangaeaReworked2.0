using Verse;

namespace ProjectPangaea
{
    public abstract class OrganismCategory
    {
        public ExtinctionStatus ExtinctionStatus { get; }

        public OrganismCategory(ThingDef thingDef)
        {
            ExtinctionStatus = thingDef.GetExtinctionStatus();
        }

        public override bool Equals(object obj)
        {
            return obj is OrganismCategory category && category.ExtinctionStatus == ExtinctionStatus;
        }

        public override int GetHashCode()
        {
            return ExtinctionStatus.GetHashCode();
        }

        public static OrganismCategory For(ThingDef thingDef, ModExt_Extinct extinctExtension)
        {
            if (thingDef.plant != null)
            {
                return new PlantCategory(thingDef);
            }
            return new AnimalCategory(thingDef);
        }
    }
}
