using Verse;

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
                    if (thing != null) thingFilter.SetAllow(thing, true);
                    else if (resource != null) thingFilter.SetAllow(resource?.Value, true);
                }
                return thingFilter;
            }
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
                throw new System.Exception("Tried to create " + nameof(PortionData) + " with null " + nameof(thing));
            }
        }

        public PortionData(PangaeaResourceReference resource, int count)
        {
            this.resource = resource;
            this.count = count;

            if (resource== null)
            {
                throw new System.Exception("Tried to create " + nameof(PortionData) + " with null " + nameof(resource));
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

        public override string ToString()
        {
            string label = resource?.Value?.Label ?? ThingFilter.Summary;
            return label + " x" + count;
        }
    }
}
