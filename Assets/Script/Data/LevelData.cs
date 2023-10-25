using UnityEngine;

namespace ZOne
{
    [System.Serializable]
    public class LevelData
    {
        /// <summary>
        /// 地图宽高(每个单位为一个地格)
        /// </summary>
        public Vector2Int RectSize;
        
        /// <summary>
        /// 单个地格尺寸
        /// </summary>
        public float CellSize;
        
        /// <summary>
        /// 地格间距
        /// </summary>
        public float Spacing;
    }
}