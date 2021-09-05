using RimWorld;
using Verse;
using System.Collections.Generic;

namespace ProjectPangaea
{
    public class PangaeaResourceCounter : MapComponent
    {
		private Dictionary<PangaeaResource, int> countedAmounts = new Dictionary<PangaeaResource, int>();
		private Dictionary<ThingDef, int> countedCorpseAmounts = new Dictionary<ThingDef, int>();

		public PangaeaResourceCounter(Map map) : base(map)
        {
			HashSet<PangaeaResource> alreadyAddedResources = new HashSet<PangaeaResource>();
			foreach (var entry in PangaeaDatabase.AllEntries)
			{
				if (entry.DNA != null && !alreadyAddedResources.Contains(entry.DNA))
				{
					countedAmounts.Add(entry.DNA, 0);
					alreadyAddedResources.Add(entry.DNA);
				}
				if (entry.Fossil != null && !alreadyAddedResources.Contains(entry.Fossil))
				{
					countedAmounts.Add(entry.Fossil, 0);
					alreadyAddedResources.Add(entry.Fossil);
				}
				countedCorpseAmounts.Add(entry.ThingDef, 0);
			}
		}

		public int GetCount(PangaeaResource resource) => countedAmounts[resource];
		public int GetCorpseCount(PangaeaThingEntry entry) => GetCorpseCount(entry.ThingDef);
		public int GetCorpseCount(ThingDef animalThingDef) => countedCorpseAmounts[animalThingDef];

		public int GetCategoryCount(ThingCategoryDef category, PangaeaThingEntry entry)
        {
			if (category == ThingCategoryDefOf.Corpses)
			{
				return GetCorpseCount(entry);
			}
			PangaeaResource resource = entry.GetResourceOfCategory(category);
			return (resource != null) ? GetCount(resource) : -1;			
		}

		private void ResetResourceCounts()
		{
			foreach (var entry in PangaeaDatabase.AllEntries)
			{
				if (entry.DNA != null) countedAmounts[entry.DNA] = 0;
				if (entry.Fossil != null) countedAmounts[entry.Fossil] = 0;
				countedCorpseAmounts[entry.ThingDef] = 0;
			}
		}

        public override void MapComponentTick()
        {
			if (Find.TickManager.TicksGame % 204 == 0)
			{
				UpdateResourceCounts();
			}
		}

		public void UpdateResourceCounts()
		{
			ResetResourceCounts();
			foreach (Thing thing in GetResources())
            {
				if (thing is PangaeaResourceThingBase pangaeaThing && pangaeaThing.Resource != null)
				{
					countedAmounts[pangaeaThing.Resource] += pangaeaThing.stackCount;
				}
				else if (thing is Corpse corpse && corpse.InnerPawn.def.HasDNA())
				{
					countedCorpseAmounts[corpse.InnerPawn.def]++;
				}
			}
		}

		private IEnumerable<Thing> GetResources()
        {
			if (PangaeaSettings.BillUIOnlyConsidersStockpiled)
            {
				return GetStockpiledResources();
            }
			return map.listerThings.ThingsInGroup(ThingRequestGroup.HaulableEver);
		}

		private IEnumerable<Thing> GetStockpiledResources()
		{
			List<SlotGroup> allGroupsListForReading = map.haulDestinationManager.AllGroupsListForReading;
			for (int i = 0; i < allGroupsListForReading.Count; i++)
			{
				foreach (Thing heldThing in allGroupsListForReading[i].HeldThings)
				{
					yield return heldThing;
				}
			}
		}
	}
}
