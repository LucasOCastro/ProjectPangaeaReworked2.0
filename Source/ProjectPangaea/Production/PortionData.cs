using Verse;
using UnityEngine;
using RimWorld;

namespace ProjectPangaea.Production
{
    [System.Serializable]
    public class PortionData
    {
        public PangaeaResourceReference resource;
        public ThingDef thing;
        public int count;

        private PangaeaThingFilter thingFilter;
        public PangaeaThingFilter ThingFilter
        {
            get
            {
                if (thingFilter == null)
                {
                    thingFilter = new PangaeaThingFilter();
                }
                if (thingFilter.AllowedDefCount == 0)
                {
                    if (thing != null) thingFilter.SetAllow(thing, true);
                    if (resource != null) thingFilter.SetAllow(resource?.Value, true);
                }
                return thingFilter;
            }
        }

        public ThingDef ResolvedThingDef => thing ?? resource?.Value?.ResourceDef.thingDef;

        public Graphic Graphic => resource?.Value?.Graphic ?? thing?.graphic ?? BaseContent.BadGraphic;

        Texture2D icon = null;
        public Texture2D Icon 
        {
            get
            {
                if (icon == null)
                {
                    ResolveIcon();
                }
                return icon;
            }
        }

        private void ResolveIcon()
        {
            icon = resource?.Value?.Icon;
            if (!icon.NullOrBad())
            {
                return;
            }
            icon = thing.GetIcon();
            if (!icon.NullOrBad())
            {
                return;
            }
            icon = ThingFilter.Icon ?? BaseContent.BadTex;
        }

        public PortionData()
        {
        }

        public PortionData(ThingDef thing, int count)
        {
            this.thing = thing;
            this.count = count;

            if (thing == null)
            {
                Log.Error("Tried to create " + nameof(PortionData) + " with null " + nameof(thing));
            }
        }

        public PortionData(PangaeaResourceReference resource, int count)
        {
            this.resource = resource;
            this.count = count;

            if (resource== null)
            {
                Log.Error("Tried to create " + nameof(PortionData) + " with null " + nameof(resource));
            }
        }

        public Thing MakeThing()
        {
            Thing result = null;
            if (thing != null)
            {
                result = ThingMaker.MakeThing(thing);
            }
            else if (resource?.Value != null)
            {
                result = resource.Value.MakeThing();
            }

            if (result != null)
            {
                result.stackCount = count;
            }
            return result;
        }

        public IngredientCount MakeIngredient()
        {
            if (ThingFilter.AllowedDefCount == 0)
            {
                return null;
            }

            IngredientCount ing = new IngredientCount() { filter = ThingFilter };
            ing.SetBaseCount(count);
            return ing;
        }

        private int lastCountCacheTicks = -1;
        private int lastCountCache = -1;
        public int CountInMap(Map map)
        {
            int currentTicks = GenTicks.TicksGame;
            if (currentTicks == lastCountCacheTicks)
            {
                return lastCountCache;
            }
            int count = 0;
            PangaeaResourceCounter pangCounter = map.GetComponent<PangaeaResourceCounter>();
            foreach (var resource in ThingFilter.AllAllowedResources)
            {
                count += pangCounter.GetCount(resource);
            }
            ResourceCounter resCounter = map.resourceCounter;
            foreach (var thing in ThingFilter.AllowedThingDefs)
            {
                var resHolder = thing.GetCompProperties<CompProperties_PangaeaResourceHolder>();
                if (resHolder != null && ThingFilter.AllowsResourceOfType(resHolder.resourceType))
                {
                    continue;
                }
                count += resCounter.GetCount(thing);
            }

            lastCountCache = count;
            lastCountCacheTicks = currentTicks;
            return count;
        }

        public override string ToString()
        {
            string label = resource?.Value?.Label ?? ThingFilter.Summary;
            return label + " x" + count;
        }
    }
}
