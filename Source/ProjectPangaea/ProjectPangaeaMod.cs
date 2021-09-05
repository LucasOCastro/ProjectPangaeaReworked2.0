using HugsLib;
using Verse;
using System.Linq;
using System.Collections.Generic;
using ProjectPangaea.Overrides;

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

        public override void DefsLoaded()
        {
            base.DefsLoaded();
            if (!ModIsActive)
            {
                return;
            }

            DNAGraphicsLister.Init();


            HashSet<ThingDef> alreadyAddedDefs = new HashSet<ThingDef>();
            foreach (var categoryDef in DefDatabase<PangaeaAnimalCategorizationDef>.AllDefs.Reverse())
            {
                foreach (var group in categoryDef.groups)
                {
                    foreach (var thingDef in group.animals)
                    {
                        if (alreadyAddedDefs.Contains(thingDef))
                        {
                            continue;
                        }
                        PangaeaDatabase.AddEntry(thingDef, group.animalType);
                        alreadyAddedDefs.Add(thingDef);
                    }
                }
            }

            foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
            {
                if (alreadyAddedDefs.Contains(thingDef))
                {
                    continue;
                }

                bool isPlant = thingDef.plant != null;
                bool isPawn = thingDef.race != null;
                if (!isPlant && !isPawn)
                {
                    continue;
                }

                ModExt_Extinct extinct = thingDef.GetModExtension<ModExt_Extinct>();
                if (extinct != null)
                {
                    PangaeaDatabase.AddEntry(thingDef, extinct.animalType);
                    alreadyAddedDefs.Add(thingDef);
                }
            }

            foreach (PangaeaOverrideDef overrideDef in DefDatabase<PangaeaOverrideDef>.AllDefs)
            {
                if (overrideDef.overridenThingDef == null)
                {
                    continue;
                }

                PangaeaDatabase.AddOrUpdateFromOverrideDef(overrideDef);
            }

            HasInitiatedDatabase = true;
            PangaeaSettings.UpdateSettings(Settings);
        }
    }

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
