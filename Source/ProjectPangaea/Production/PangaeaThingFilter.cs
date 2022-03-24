using Verse;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectPangaea.Production
{
    public class PangaeaThingFilter : ThingFilter
    {
        private HashSet<PangaeaResource> allowedResourcesHashset = new HashSet<PangaeaResource>();
        private List<PangaeaResource> allowedResourcesList = new List<PangaeaResource>();
        public IEnumerable<PangaeaResource> AllAllowedResources => allowedResourcesList;
        public PangaeaResource AnyAllowedResource => AllAllowedResources.FirstOrFallback();

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

        public PangaeaThingFilter()
        {
        }

        private Texture2D icon;
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
            icon = AnyAllowedResource?.Icon;
            if (icon.NullOrBad())
            {
                ThingDef def = AnyAllowedDef;
                icon = def.GetIcon() ?? BaseContent.BadTex;// def?.GetUIIconForStuff(RimWorld.GenStuff.DefaultStuffFor(def)) ?? BaseContent.BadTex;
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

        public bool Allows(PangaeaResource resource) => allowedResourcesHashset.Contains(resource);

        public bool AllowsResourceOfType(ResourceTypeDef type)
        {
            for (int i = 0; i < allowedResourcesList.Count; i++)
            {
                if (allowedResourcesList[i].ResourceDef == type)
                {
                    return true;
                }
            }
            return false;
        }

        public void SetAllow(PangaeaResource resource, bool allow)
        {
            if (allow)
            {
                allowedResourcesHashset.Add(resource);
                allowedResourcesList.Add(resource);
                SetAllow(resource.ResourceDef.thingDef, true);
            }
            else
            {
                allowedResourcesHashset.Remove(resource);
                allowedResourcesList.Remove(resource);
                DisallowIfNotNeeded(resource.ResourceDef.thingDef);
            }
        }

        private void DisallowIfNotNeeded(ThingDef thingDef)
        {
            foreach (var resource in AllAllowedResources)
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
                allowedResourcesHashset.Clear();
                allowedResourcesList.Clear();
                foreach (var resource in ptf.AllAllowedResources)
                {
                    SetAllow(resource, true);
                }
            }
        }

        private const string listExposeName = nameof(allowedResourcesList);
        public override void ExposeData()
        {
            base.ExposeData();
            if (Scribe.mode == LoadSaveMode.Saving) 
            {
                Scribe_PangaeaCollection.Look(ref allowedResourcesList, listExposeName);
            }
            else if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
                List<PangaeaResource> resourceList = null;
                Scribe_PangaeaCollection.Look(ref resourceList, listExposeName);
                for (int i  = 0; i < resourceList.Count; i++)
                {
                    SetAllow(resourceList[i], true);
                }
            }
        }
    }
}
