using Verse;
using System.Collections.Generic;

namespace ProjectPangaea.Production.Splicing
{
    public class PangaeaDirectRecipeDef : Def
    {
        public DirectRecipeSettings settings;

        public override IEnumerable<string> ConfigErrors()
        {
            foreach (string error in base.ConfigErrors())
                yield return error;

            foreach (string error in processList(settings.ingredients)) yield return error;
            foreach (string error in processList(settings.results)) yield return error;
            IEnumerable<string> processList(List<DirectRecipeSettings.PortionData> list)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    var splice = list[i];
                    if (splice.resource != null && splice.thing != null)
                    {
                        yield return $"splice portion with index {i} has both resource and thing.";
                    }
                    else if (splice?.resource?.Value is null && splice.thing is null)
                    {
                        yield return $"splice portion with index {i} has neither resource nor thing.";
                    }
                }

            }
        }

        //For custom deserialization
        public void LoadDataFromXmlCustom(System.Xml.XmlNode xmlRoot)
        {
            settings = ParseHelper.FromString<DirectRecipeSettings>(xmlRoot.FirstChild.Value);
        }
    }
}
