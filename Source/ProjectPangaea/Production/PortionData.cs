using Verse;

namespace ProjectPangaea.Production
{
    [System.Serializable]
    public class PortionData
    {
        public PangaeaResourceReference resource;
        public ThingDef thing;
        public int count;

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
            ThingFilter filter = null;
            if (thing != null)
            {
                filter = new ThingFilter();
                filter.SetAllow(thing, true);
            }
            else if (resource?.Value != null)
            {
                PangaeaThingFilter pangFilter = new PangaeaThingFilter(resource.Value);
                filter = pangFilter;
            }

            if (filter == null)
            {
                return null;
            }

            IngredientCount ing = new IngredientCount() { filter = filter };
            ing.SetBaseCount(count);
            return ing;
        }
    }
}
