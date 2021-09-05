using System;
using System.Collections.Generic;

namespace ProjectPangaea
{
    public class EnumDictionary<E, T> : Dictionary<E, T> where E : Enum
    {
        public EnumDictionary(T defaultValue = default, params E[] exceptions) : base()
        {
            E[] enumVals = (E[])Enum.GetValues(typeof(E));
            for (int i = 0; i < enumVals.Length; i++)
            {
                bool isException = false;
                foreach (E e in exceptions)
                {
                    if (enumVals[i].Equals(e))
                    {
                        isException = true;
                        break;
                    }
                }

                if (!isException)
                {
                    this.Add(enumVals[i], defaultValue);
                }
            }
        }
    }
}
