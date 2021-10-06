using Verse;
using System.Collections.Generic;
using System.Linq;

namespace ProjectPangaea
{
    public static class DebugActions
    {
        public static Command_Action GenAction(CompPangaeaResourceHolder resourceHolder)
        {
            Command_Action action = new Command_Action
            {
                defaultLabel = "Debug: Set Pangaea resource",
                action = () => Find.WindowStack.Add(new Dialog_DebugOptionListLister(GenMenuOptions(resourceHolder)))
            };
            return action;
        }

        private static IEnumerable<DebugMenuOption> GenMenuOptions(CompPangaeaResourceHolder resourceHolder)
        {
            HashSet<PangaeaResource> closedSet = new HashSet<PangaeaResource>();
            foreach (PangaeaResource resource in resourceHolder.Props.AllPossibleResources)
            {
                if (closedSet.Contains(resource))
                {
                    continue;
                }
                var option = new DebugMenuOption(resource.Label, DebugMenuOptionMode.Action, () => resourceHolder.Resource = resource);
                yield return option;
                closedSet.Add(resource);
            }
        }
    }
}