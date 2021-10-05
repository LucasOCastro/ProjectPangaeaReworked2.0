using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPangaea.Production
{
    public static class PangaeaRecipeGen
    {
        public static void GenAndRegisterRecipesFor(PangaeaThingEntry entry)
        {
            foreach (var recipeDef in PangaeaRecipeLister.AllRecipeExtensions)
            {
                foreach (var procedural in recipeDef.proceduralRecipeDefs)
                {
                }
            }
        }
    }
}
