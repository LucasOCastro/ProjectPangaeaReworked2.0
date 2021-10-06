using Verse;
using System.Collections.Generic;
using System.Linq;

namespace ProjectPangaea
{
    public class PangaeaThingEntry
    {
        public ThingDef ThingDef { get; }
        public string Label => ThingDef.LabelCap;

        public ModExt_Extinct ExtinctExtension { get; }
        public bool IsExtinct => ExtinctExtension != null;

        public OrganismCategory Category { get; }

        private Dictionary<ResourceTypeDef, PangaeaResource> resources = new Dictionary<ResourceTypeDef, PangaeaResource>();
        public IEnumerable<PangaeaResource> AllResources => resources.Values;


        public PangaeaThingEntry(ThingDef thingDef)
        {

            ThingDef = thingDef;
            ExtinctExtension = thingDef.GetModExtension<ModExt_Extinct>();
            Category = OrganismCategory.For(thingDef, ExtinctExtension);

            foreach (ResourceTypeDef resourceType in DefDatabase<ResourceTypeDef>.AllDefs)
            {
                if (resourceType.addToAny || (resourceType.addToExtinct && IsExtinct))
                {
                    AddResourceOfType(resourceType);
                }
            }
        }

        public void AddResourceOfType(ResourceTypeDef resourceType)
        {
            if (!resources.ContainsKey(resourceType))
            {
                resources.Add(resourceType, new PangaeaResource(resourceType, this));
            }
        }

        public PangaeaResource GetResource(ResourceTypeDef def) => resources.TryGetValue(def);

        public bool TryGetResource(ResourceTypeDef def, out PangaeaResource resource) => resources.TryGetValue(def, out resource);

        public override string ToString()
        {
            return base.ToString() + " " + ThingDef;
        }
    }
}
