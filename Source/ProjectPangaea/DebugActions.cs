using Verse;
using System.Collections.Generic;
using System;

namespace ProjectPangaea
{
    public static class DebugActions
    {
        public static Command_Action GenMenuListerAction(string label, IEnumerable<DebugMenuOption> actions)
        {
            return new Command_Action()
            {
                defaultLabel = label,
                action = () => Find.WindowStack.Add(new Dialog_DebugOptionListLister(actions))
            };
        }
        
        public static IEnumerable<DebugMenuOption> GenResourceMenuOptions(ResourceTypeDef resourceType, Action<PangaeaResource> action)
        {
            foreach (PangaeaResource resource in PangaeaDatabase.AllResourcesOfDef(resourceType))
            {
                var option = new DebugMenuOption(resource.Label, DebugMenuOptionMode.Action, () => action(resource));
                yield return option;
            }
        }
    }
}