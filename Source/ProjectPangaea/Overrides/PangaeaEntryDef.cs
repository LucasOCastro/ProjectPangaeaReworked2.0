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
            for (int i = 0; i < xmlRoot.ChildNodes.Count; i++)
            {
                var node = xmlRoot.ChildNodes[i];
                if (node.Name == nameof(this.defName))
                {
                    string defName = node.Value;
                    this.defName = defName;
                    xmlRoot.RemoveChild(node);
                    break;
                }
            }
            settings = DirectXmlToObject.ObjectFromXml<EntrySettings>(xmlRoot, false);
        }

    }
}
