using Verse;

namespace ProjectPangaea
{
    [System.Serializable]
    public class PangaeaOverride
    {
        public string label;
        public string description;

        public void Override(PangaeaResource resource)
        {
            if (!label.NullOrEmpty())
            {
                resource.overrideLabel = label;
            }

            if (!description.NullOrEmpty())
            {
                resource.overrideDescription = description;
            }
        }
    }
}
