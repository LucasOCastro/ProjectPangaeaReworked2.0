using RimWorld;
using Verse;
using System;
using System.Text;

namespace ProjectPangaea
{
    public static class PangaeaEnumExtension
    {
        //public static string Translate(this AnimalType animalType) => ("Pangaea_" + animalType.ToString()).Translate();
        public static string Translate<E>(this E e) where E: Enum => ("Pangaea_" + e.ToString()).Translate();

        public static string AllEnumValuesTranslated<E>(string separator = "\n") where E: Enum
        {
            StringBuilder tx = new StringBuilder();
            E[] values = (E[])Enum.GetValues(typeof(E));
            for (int i = 0; i < values.Length; i++)
            {
                tx.Append(values[i].Translate());
                if (i < values.Length - 1)
                {
                    tx.Append(separator);
                }
            }
            return tx.ToString();
         }
    }
}
