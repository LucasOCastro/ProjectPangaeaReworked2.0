using System.Collections.Generic;
using Verse;
using System;
using System.Linq;

namespace ProjectPangaea
{
    public class CompProperties_PangaeaResourceHolder : CompProperties
    {
        public ResourceTypeDef resourceType;

        public CompProperties_PangaeaResourceHolder()
        {
            compClass = typeof(CompPangaeaResourceHolder);
        }

        public bool IsOfType(ResourceTypeDef type) => resourceType == type;
        public bool AllowsTypeOfResource(PangaeaResource resource) => IsOfType(resource.ResourceDef);

        public PangaeaResource GetRandomResource() => AllPossibleResources.RandomElement();
        
        private List<PangaeaResource> allPossibleResources;
        public List<PangaeaResource> AllPossibleResources
        {
            get
            {
                if (allPossibleResources == null)
                {
                    allPossibleResources = AllPossibleEntries.Select(e => e.GetResource(resourceType)).ToList();
                }
                return allPossibleResources;
            }
        }

        private List<PangaeaThingEntry> allPossibleEntries;
        public List<PangaeaThingEntry> AllPossibleEntries
        {
            get
            {
                if (allPossibleEntries == null)
                {
                    allPossibleEntries = PangaeaDatabase.AllEntries.Where(e => e.GetResource(resourceType) != null).ToList();
                }
                return allPossibleEntries;
            }
        }

        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            foreach (string error in base.ConfigErrors(parentDef))
                yield return error;
            if (resourceType == null)
                yield return "Invalid " + nameof(resourceType) + " in " + this;
            if (!typeof(PangaeaThing).IsAssignableFrom(parentDef.thingClass))
                yield return "thingClass of parentDef should be PangaeaThing on " + this;
        }
    }
}
