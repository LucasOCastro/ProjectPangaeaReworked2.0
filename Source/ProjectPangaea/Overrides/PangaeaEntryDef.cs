using Verse;
using System.Xml;

namespace ProjectPangaea
{
    public class PangaeaEntryDef : Def
    {
        public EntrySettings settings;

        //For custom deserialization
        public void LoadDataFromXmlCustom(XmlNode xmlRoot)
        {
            settings = ParseHelper.FromString<EntrySettings>(xmlRoot.FirstChild.Value);
        }

    }
}
