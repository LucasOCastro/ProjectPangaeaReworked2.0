using System;
using System.Collections.Generic;
using System.Xml;
using RimWorld;
using Verse;

namespace ProjectPangaea
{
    [Serializable]
    public class EnumDictionary<E, T> : Dictionary<E, T> where E : Enum
    {
        public EnumDictionary(T defaultValue = default, params E[] exceptions) : base()
        {
            E[] enumVals = (E[])Enum.GetValues(typeof(E));
            for (int i = 0; i < enumVals.Length; i++)
            {
                bool isException = false;
                for (int j = 0; j < (exceptions?.Length ?? 0); j++)
                {
                    if (enumVals[i].Equals(exceptions[j]))
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

        //For custom deserialization
        public void LoadDataFromXmlCustom(XmlNode xmlRoot)
        {
            for (int i = 0; i < xmlRoot.ChildNodes.Count; i++)
            {
                var node = xmlRoot.ChildNodes[i];

                string keyString = node.Name;
                E key = ParseHelper.FromString<E>(keyString);

                string valueString = node.FirstChild.Value;
                T value = ParseHelper.FromString<T>(valueString);

                if (key != null && value != null)
                {
                    this.SetOrAdd(key, value);
                }
            }
        }
    }
}
