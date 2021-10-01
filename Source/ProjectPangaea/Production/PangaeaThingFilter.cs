using Verse;
using System.Collections.Generic;

namespace ProjectPangaea.Production
{
    public class PangaeaThingFilter : ThingFilter
    {
        public HashSet<PangaeaResource> AllowedResources { get; private set; } = new HashSet<PangaeaResource>();

        public PangaeaThingFilter(params PangaeaResource[] allowedResources)
        {
            if (allowedResources != null)
            {
                for (int i = 0; i < allowedResources.Length; i++)
                {
                    Allow(allowedResources[i]);
                }
            }
        }
        public PangaeaThingFilter(IEnumerable<PangaeaResource> allowedResources)
        {
            foreach (var resource in allowedResources)
            {
                Allow(resource);
            }
        }

        public void SyncAllowedEntries(PangaeaThingFilter other)
        {
            this.AllowedResources = other?.AllowedResources ?? new HashSet<PangaeaResource>(AllowedResources);
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

        public void Toggle(PangaeaResource resource)
        {
            if (AllowedResources.Contains(resource))
            {
                Disallow(resource);
            }
            else
            {
                Allow(resource);
            }
        }

        public void Allow(PangaeaResource resource)
        {
            AllowedResources.Add(resource);
            SetAllow(resource.ResourceDef.thingDef, true);
        }

        public void Disallow(PangaeaResource resource)
        {
            AllowedResources.Remove(resource);
            SetAllow(resource.ResourceDef.thingDef, false);
        }

        public override void CopyAllowancesFrom(ThingFilter other)
        {
            base.CopyAllowancesFrom(other);
            if (other is PangaeaThingFilter ptf)
            {
                AllowedResources.Clear();
                foreach (var resource in ptf.AllowedResources)
                {
                    Allow(resource);
                }
            }
        }
    }
}
