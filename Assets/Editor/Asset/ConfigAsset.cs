using UnityEngine;

namespace GameEditor
{
    public class ConfigAsset : ScriptableObject
    {
        public int ID;
        public string Name;

        public virtual string ShowName => $"{ID} {Name}";
    }
}