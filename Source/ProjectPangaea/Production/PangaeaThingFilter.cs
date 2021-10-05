using Verse;
using System.Collections.Generic;

namespace ProjectPangaea.Production
{
    public class PangaeaThingFilter : ThingFilter
    {
        private HashSet<PangaeaResource> allowedResources = new HashSet<PangaeaResource>();
        public HashSet<PangaeaResource> AllowedResources => allowedResources;

        public PangaeaThingFilter(params PangaeaResource[] allowedResources)
        {
            if (allowedResources != null)
            {
                for (int i = 0; i < allowedResources.Length; i++)
                {
                    SetAllow(allowedResources[i], true);
                }
            }
        }
        public PangaeaThingFilter(IEnumerable<PangaeaResource> allowedResources)
        {
            foreach (var resource in allowedResources)
            {
                SetAllow(resource, true);
            }
        }

        public override bool Allows(Thing t)
        {
            if (!base.Allows(t))
            {
                return false;
            }

            var resourceHolder = t.TryGetComp<CompPangaeaResourceHolder>();
            return resourceHolder == null || Allows(resourceHolder.Resource);
        }

        public bool Allows(PangaeaResource resource) => AllowedResources.Contains(resource);

        public void SetAllow(PangaeaResource resource, bool allow)
        {
            if (allow)
            {
                AllowedResources.Add(resource);
                SetAllow(resource.ResourceDef.thingDef, true);
            }
            else
            {
                AllowedResources.Remove(resource);
                DisallowIfNotNeeded(resource.ResourceDef.thingDef);
            }
        }

        private void DisallowIfNotNeeded(ThingDef thingDef)
        {
            foreach (var resource in AllowedResources)
            {
                if (resource.ResourceDef.thingDef == thingDef)
                {
                    return;
                }
            }
            SetAllow(thingDef, false);
        }

        public override void CopyAllowancesFrom(ThingFilter other)
        {
            base.CopyAllowancesFrom(other);
            if (other is PangaeaThingFilter ptf)
            {
                AllowedResources.Clear();
                foreach (var resource in ptf.AllowedResources)
                {
                    SetAllow(resource, true);
                }
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            //TODO expos
            //Scribe_Collections.Look(ref allowedResources, "")
        }
    }
}
