using System.Collections.Generic;
using Verse;
using System;
using System.Linq;

namespace ProjectPangaea
{
    public class CompProperties_PangaeaResourceHolder : CompProperties
    {
        public Type resourceType;

        public CompProperties_PangaeaResourceHolder()
        {
            compClass = typeof(CompPangaeaResourceHolder);
        }

        public bool Allows(PangaeaResource resource) => resource.GetType() == resourceType;

        //TODO cache
        public PangaeaResource GetRandomResource() => GetAllPossibleEntries().RandomElement().GetResourceOfType(resourceType);
        public IEnumerable<PangaeaResource> GetAllPossibleResources() => GetAllPossibleEntries().Select(e => e.GetResourceOfType(resourceType));
        public IEnumerable<PangaeaThingEntry> GetAllPossibleEntries() => PangaeaDatabase.AllEntries.Where(e => e.GetResourceOfType(resourceType) != null);

        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            foreach (string error in base.ConfigErrors(parentDef))
                yield return error;
            if (resourceType == null || !typeof(PangaeaResource).IsAssignableFrom(resourceType))
                yield return "Invalid " + nameof(resourceType) + " in " + this;
        }
    }
}
