using HugsLib;
using Verse;
using System.Linq;
using System.Collections.Generic;

namespace ProjectPangaea
{
    public class ProjectPangaeaMod : ModBase
    {
        public override string ModIdentifier => this.ModContentPack.PackageId;

        public static bool HasInitiatedDatabase { get; private set; }

        public ProjectPangaeaMod()
        {
        }

        public override void SettingsChanged()
        {
            base.SettingsChanged();
            PangaeaSettings.UpdateSettings(Settings);
        }

        private bool ShouldAddToDB(ThingDef thingDef)
        {
            if (thingDef.plant == null && !thingDef.race.Animal)
            {
                return false;
            }
            return thingDef.IsExtinct();
        }

        public override void DefsLoaded()
        {
            base.DefsLoaded();
            if (!ModIsActive)
            {
                return;
            }

            ResourceGraphicLister.Init();

            //Add initial entries (currently only extinct)
            //TODO add setting for all blablabla
            foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
            {
                if (ShouldAddToDB(thingDef))
                {
                    PangaeaDatabase.AddEntry(thingDef);
                }
            }

            //Overrides the entries from the custom defs and adds if missing
            OverrideHelper.Init();
            OverrideHelper.DoOverrides();

            //Adds any extinct resources to extinct entries
            List<ResourceTypeDef> extinctResourceTypes = DefDatabase<ResourceTypeDef>.AllDefs.Where(r => r.addToExtinct).ToList();
            foreach (var extinctEntry in PangaeaDatabase.AllEntries.Where(e => e.IsExtinct))
            {
                for (int i = 0; i < extinctResourceTypes.Count; i++)
                {
                    extinctEntry.AddResourceOfType(extinctResourceTypes[i]);
                }
            }

            HasInitiatedDatabase = true;
            PangaeaSettings.UpdateSettings(Settings);
        }
    }

    //TODO settings shouldnt be static, dumbass
    public static class PangaeaSettings
    {
        public static bool AssignRandomPawnToNullResource { get; private set; }
        public static bool SpawnExtinctAnimals { get; private set; }
        public static bool SpawnExtinctPlants { get; private set; }
        public static bool BillUIOnlyConsidersStockpiled { get; private set; }

        public static void UpdateSettings(HugsLib.Settings.ModSettingsPack settings)
        {
            AssignRandomPawnToNullResource = settings.GetHandle<bool>("PangaeaRandomForNull",
                "PangaeaSetting_RandomResourceToNullName".Translate(),
                "PangaeaSetting_RandomResourceToNullDescription".Translate()
                );

            SpawnExtinctAnimals = settings.GetHandle<bool>("PangaeaSpawnExtinctAnimals",
                "PangaeaSetting_WildAnimalsName".Translate(),
                "PangaeaSetting_WildAnimalsDescription".Translate()
                );

            SpawnExtinctPlants = settings.GetHandle<bool>("PangaeaSpawnExtinctPlants",
                "PangaeaSetting_WildPlantsName".Translate(),
                "PangaeaSetting_WildPlantsDescription".Translate()
                );

            BillUIOnlyConsidersStockpiled = settings.GetHandle<bool>("PangaeaUIConsiderStockpiled",
                "PangaeaSetting_UIConsiderStockpiledName".Translate(),
                "PangaeaSetting_UIConsiderStockpiledDescription".Translate(),
                defaultValue: true
                );
        }
    }
}
