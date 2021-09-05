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

        /// <returns>The texture path after "Things/Item/DNA/"</returns>
        public abstract string GetSubTexPath();

        public override bool Equals(object obj)
        {
            return obj is OrganismCategory category && category.ExtinctionStatus == ExtinctionStatus;
        }

        public override int GetHashCode()
        {
            return ExtinctionStatus.GetHashCode();
        }
    }
}
