using RimWorld;
using Verse;
using System.Collections.Generic;
using ProjectPangaea.Production;

namespace ProjectPangaea
{
    public class PangaeaResourceCounter : MapComponent
    {
		private Dictionary<PangaeaResource, int> countedAmounts = new Dictionary<PangaeaResource, int>();
		private Dictionary<ThingDef, int> countedCorpseAmounts = new Dictionary<ThingDef, int>();

		public PangaeaResourceCounter(Map map) : base(map)
        {
			foreach (var entry in PangaeaDatabase.AllEntries)
			{
				foreach (var resource in entry.AllResources)
                {
					countedAmounts.Add(resource, 0);
				}
				countedCorpseAmounts.Add(entry.ThingDef, 0);
			}
		}

		public int GetCount(PangaeaResource resource) => countedAmounts[resource];
		public int GetCorpseCount(PangaeaThingEntry entry) => GetCorpseCount(entry.ThingDef);
		public int GetCorpseCount(ThingDef animalThingDef) => countedCorpseAmounts[animalThingDef];

		private void ResetResourceCounts()
		{
			foreach (var entry in PangaeaDatabase.AllEntries)
			{
				foreach (var resource in entry.AllResources)
					countedAmounts[resource] = 0;
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
			foreach (Thing thing in map.listerThings.ThingsInGroup(ThingRequestGroup.HaulableEver))
            {
				if (thing is PangaeaThing pangaeaThing && pangaeaThing.Resource != null)
				{
					countedAmounts[pangaeaThing.Resource] += pangaeaThing.stackCount;
				}
				else if (thing is Corpse corpse && PangaeaDatabase.GetOrNull(corpse.InnerPawn.def) != null)
				{
					countedCorpseAmounts[corpse.InnerPawn.def]++;
				}
			}
		}
	}
}
