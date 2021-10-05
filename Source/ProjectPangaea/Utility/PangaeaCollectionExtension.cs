﻿using System;
using System.Linq;
using System.Collections.Generic;
using ProjectPangaea.Production;

namespace ProjectPangaea
{
    public static class PangaeaCollectionExtension
    {
        public static IEnumerable<T> Yield<T>(this T t)
        {
            yield return t;
        }

        public static void AllowParcelDataList(this PangaeaThingFilter filter, List<PortionData> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var ing = list[i];
                if (ing.thing != null)
                {
                    filter.SetAllow(ing.thing, true);
                }
                else if (ing.resource ?.Value != null)
                {
                    filter.SetAllow(ing.resource.Value, true);
                }
            }
        }
    }
}
