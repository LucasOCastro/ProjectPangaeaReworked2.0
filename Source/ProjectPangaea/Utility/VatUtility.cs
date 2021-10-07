using RimWorld;
using Verse;
using UnityEngine;

namespace ProjectPangaea
{
    public static class DateUtility
    {
        public static int GestationTicks(this ThingDef thingDef)
        {
            return Mathf.RoundToInt(AnimalProductionUtility.GestationDaysEach(thingDef) * GenDate.TicksPerDay);
        }

        public static string ToStringGestationTicks(this ThingDef thingDef)
        {
            return GestationTicks(thingDef).ToString();
        }

        public static string GestationPeriod(this ThingDef thingDef)
        {
            return GestationTicks(thingDef).ToStringTicksToPeriod();
        }

        public static int VatGestationTicks(this ThingDef thingDef)
        {
            return Mathf.RoundToInt(GestationTicks(thingDef) * ProjectPangaeaMod.Settings.embryoVatTimeMultiplier);
        }

        public static string VatGestationPeriod(this ThingDef thingDef)
        {
            return VatGestationTicks(thingDef).ToStringTicksToPeriod();
        }
    }

}
