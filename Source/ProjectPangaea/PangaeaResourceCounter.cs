using RimWorld;
using Verse;
using System.Collections.Generic;
using ProjectPangaea.Production;

namespace ProjectPangaea
{
    public class PangaeaResourceCounter : MapComponent
    {
		private Dictionary<PangaeaResource, int> countedAmounts = new Dictionary<PangaeaResource, int>();

		public PangaeaResourceCounter(Map map) : base(map)
        {
			foreach (var entry in PangaeaDatabase.AllEntries)
			{
				foreach (var resource in entry.AllResources)
                {
					countedAmounts.Add(resource, 0);
				}
			}
		}

		public int GetCount(PangaeaResource resource) => countedAmounts[resource];

		private void ResetResourceCounts()
		{
			foreach (var entry in PangaeaDatabase.AllEntries)
				foreach (var resource in entry.AllResources)
					countedAmounts[resource] = 0;
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
			if (ProjectPangaeaMod.Settings.counterIncludeNonStocked)
			{
				UpdateResourceCounts_IncludeNonStocked();
			}
            else
            {
				UpdateResourceCounts_StockedOnly();
			}
		}

		private void UpdateResourceCounts_StockedOnly()
		{
			ResetResourceCounts();

			List<SlotGroup> allGroupsListForReading = map.haulDestinationManager.AllGroupsListForReading;
			for (int i = 0; i < allGroupsListForReading.Count; i++)
			{
				foreach (Thing thing in allGroupsListForReading[i].HeldThings)
				{
					if (thing is PangaeaThing pangaeaThing && pangaeaThing.Resource != null)
					{
						countedAmounts[pangaeaThing.Resource] += pangaeaThing.stackCount;
					}
				}
			}
		}

		private void UpdateResourceCounts_IncludeNonStocked()
		{
			ResetResourceCounts();
			
			foreach (Thing thing in map.listerThings.ThingsInGroup(ThingRequestGroup.HaulableEver))
            {
				if (thing is PangaeaThing pangaeaThing && pangaeaThing.Resource != null)
				{
					countedAmounts[pangaeaThing.Resource] += pangaeaThing.stackCount;
				}
			}
		}
	}
}
