using UnityEngine;

namespace GameEditor
{
    public class LevelAsset : ConfigAsset
    {
        public static string AssetPath = "Assets/Config/Level/";
        
        [InspectorName("宽")]
        public int Width;
        [InspectorName("长")]
        public int Height;
    }
}