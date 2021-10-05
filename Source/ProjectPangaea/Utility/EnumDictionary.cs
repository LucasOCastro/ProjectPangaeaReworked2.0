using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using Verse;

namespace ProjectPangaea
{
    [Serializable]
    public class EnumDictionary<E, T> : Dictionary<E, T> where E : Enum
    {
        private static E[] enumVals = (E[])Enum.GetValues(typeof(E));
        private static Dictionary<string, E> stringToEnumDict = enumVals.ToDictionary(e => e.ToString().ToLower());
        public EnumDictionary(T defaultValue, params E[] exceptions) : base()
        {
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

        public EnumDictionary() :this(default)
        {
        }

        //For custom deserialization
        public void LoadDataFromXmlCustom(XmlNode xmlRoot)
        {
            for (int i = 0; i < xmlRoot.ChildNodes.Count; i++)
            {
                var node = xmlRoot.ChildNodes[i];

                string keyString = node.Name;
                E key = stringToEnumDict[keyString.ToLower()];

                T value = DirectXmlToObject.ObjectFromXml<T>(node, true);

                if (key != null && value != null)
                {
                    this.SetOrAdd(key, value);
                }
            }
        }
    }
}
