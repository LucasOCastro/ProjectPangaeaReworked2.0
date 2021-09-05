using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld;

namespace ProjectPangaea.Production
{
    public class Pangaea_CreateLifeRecipeExtension : Pangaea_ResourceRecipeExtension
    {
        public override PangaeaBillCounter GetBillCounter(RecipeDef recipe)
        {
            return new PangaeaBillCounter(recipe, PangaeaThingCategoryDefOf.PangaeaDNA);
        }

        public override List<PangaeaThingEntry> GetListerEntries(RecipeDef recipe)
        {
            return PangaeaDatabase.AllEntries.Where(e => !e.IsDNAOverriden && e.ThingDef.HasComp(typeof(CompEggLayer))).ToList();
        }
    }
}
