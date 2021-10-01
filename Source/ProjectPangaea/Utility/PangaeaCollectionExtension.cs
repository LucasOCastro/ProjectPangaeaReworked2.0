using System;
using System.Linq;
using System.Collections.Generic;

namespace ProjectPangaea
{
    public static class PangaeaCollectionExtension
    {
        public static IEnumerable<T> Yield<T>(this T t)
        {
            yield return t;
        }
    }
}
