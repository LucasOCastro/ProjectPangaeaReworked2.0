using Verse;
using System.Collections.Generic;
using System.Linq;

namespace ProjectPangaea
{
    public class PangaeaThingEntry
    {
        public ThingDef ThingDef { get; }
        public string Label => ExtinctExtension?.ScientificName ?? ThingDef.LabelCap;

        public ModExt_Extinct ExtinctExtension { get; }
        public bool IsExtinct => ExtinctExtension != null;

        public OrganismCategory Category { get; }

        private Dictionary<ResourceTypeDef, PangaeaResource> resources = new Dictionary<ResourceTypeDef, PangaeaResource>();
        public IEnumerable<PangaeaResource> AllResources => resources.Values;


        private static List<ResourceTypeDef> generalResourceCache = DefDatabase<ResourceTypeDef>.AllDefs.Where(r=>r.addToAny).ToList();
        public PangaeaThingEntry(ThingDef thingDef)
        {
            ThingDef = thingDef;
            ExtinctExtension = thingDef.GetModExtension<ModExt_Extinct>();
            Category = OrganismCategory.For(thingDef, ExtinctExtension);

            foreach (var generalResourceType in generalResourceCache)
            {
                AddResourceOfType(generalResourceType);
            }
        }

        public void AddResourceOfType(ResourceTypeDef resourceType)
        {
            if (!resources.ContainsKey(resourceType))
            {
                resources.Add(resourceType, new PangaeaResource(resourceType, this));
            }
        }

        public PangaeaResource GetResourceOfCategory(ThingCategoryDef category)
        {
            return AllResources.First(r => r.ThingDef.IsWithinCategory(category));
        }
        public PangaeaResource GetResourceOfDef(ResourceTypeDef def) => resources.TryGetValue(def);

        public override string ToString()
        {
            return base.ToString() + " " + ThingDef;
        }
    }
}
