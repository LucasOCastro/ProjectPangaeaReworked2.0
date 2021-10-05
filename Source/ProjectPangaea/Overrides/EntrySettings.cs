using System.Collections.Generic;

namespace ProjectPangaea
{
    [System.Serializable]
    public class EntrySettings
    {
        public PangaeaEntryFilter applyFilter;
        public AnimalType animalType = default;
        public List<ResourceTypeDef> resourceTypes = new List<ResourceTypeDef>();
        public Dictionary<ResourceTypeDef, PangaeaOverride> resourceOverrides = new Dictionary<ResourceTypeDef, PangaeaOverride>();

        public void TryOverride(PangaeaThingEntry entry)
        {
            if (!applyFilter.Allows(entry))
            {
                return;
            }

            if (animalType != default && entry.Category is AnimalCategory animalCategory)
            {
                animalCategory.OverrideAnimalType(animalType);
            }

            foreach (var overrideKeyValue in resourceOverrides)
            {
                var resource = entry.GetResource(overrideKeyValue.Key);
                if (resource != null)
                {
                    overrideKeyValue.Value.Override(resource);
                }
            }

            foreach (var resourceType in resourceTypes)
            {
                entry.AddResourceOfType(resourceType);
            }
        }
    }
}
