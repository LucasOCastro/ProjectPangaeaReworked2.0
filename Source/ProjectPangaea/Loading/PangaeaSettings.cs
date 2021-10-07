using Verse;
using UnityEngine;

namespace ProjectPangaea
{
    public class PangaeaSettings : ModSettings
    {
        public bool assignRandomOwnerToNullResource = false;
        //TODO more complex configurable spawning settings
        public bool spawnExtinctAnimals = false;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref assignRandomOwnerToNullResource, nameof(assignRandomOwnerToNullResource));
            Scribe_Values.Look(ref spawnExtinctAnimals, nameof(spawnExtinctAnimals));
        }

        public void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            listing.CheckboxLabeled("PangaeaSetting_RandomResourceToNullName".Translate(),
                ref assignRandomOwnerToNullResource,
                tooltip: "PangaeaSetting_RandomResourceToNullDescription".Translate()
                );

            listing.CheckboxLabeled("PangaeaSetting_WildAnimalsName".Translate(),
                ref spawnExtinctAnimals,
                tooltip: "PangaeaSetting_WildAnimalsDescription".Translate()
                );
        }
    }
}
