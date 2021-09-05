#if RWV13

using Verse;
using System.Collections.Generic;
using System.Linq;

namespace ProjectPangaea
{

    public static class Pangaea_DebugActions
    {
        [DebugAction("Project Pangaea", "Set Pangaea resource", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        public static void SetResource()
        {
            Thing thing = Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).FirstOrDefault(t => t is PangaeaResourceThingBase);//Where(t => t is PangaeaResourceThingBase).FirstOrDefault();
            if (thing == null)
            {
                return;
            }

            List<DebugMenuOption> actions;
            if (thing is DNAThing dnaThing)
            {
                actions = GetDnaActions(dnaThing);
            }
            else if (thing is FossilThing fossilThing)
            {
                actions = GetFossilActions(fossilThing);
            }
            else
            {
                return;
            }

            Find.WindowStack.Add(new Dialog_DebugOptionListLister(actions));
        }

        private static List<DebugMenuOption> GetDnaActions(DNAThing dnaThing)
        {
            List<DebugMenuOption> actions = new List<DebugMenuOption>();
            HashSet<DNA> closedSet = new HashSet<DNA>();
            foreach (PangaeaThingEntry entry in PangaeaDatabase.AllEntries)
            {
                if (entry.DNA == null || closedSet.Contains(entry.DNA))
                {
                    continue;
                }
                actions.Add(new DebugMenuOption(entry.DNA.Label, DebugMenuOptionMode.Action, delegate
                {
                    dnaThing.SetResource(entry.DNA);
                }));
                closedSet.Add(entry.DNA);
            }
            return actions;
        }

        private static List<DebugMenuOption> GetFossilActions(FossilThing fossilThing)
        {
            List<DebugMenuOption> actions = new List<DebugMenuOption>();
            foreach (PangaeaThingEntry entry in PangaeaDatabase.AllEntries)
            {
                if (entry.Fossil == null)
                {
                    continue;
                }
                actions.Add(new DebugMenuOption(entry.Fossil.Label, DebugMenuOptionMode.Action, delegate
                {
                    fossilThing.SetResource(entry.Fossil);
                }));
            }
            return actions;
        }
    }
}

#endif