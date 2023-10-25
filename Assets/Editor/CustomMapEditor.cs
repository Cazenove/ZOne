using UnityEditor;
using UnityEngine;

namespace GameEditor
{
    [CustomEditor(typeof(MapEditor), true)]
    public class CustomMapEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            MapEditor _target = target as MapEditor;
            base.OnInspectorGUI();
            serializedObject.Update();
            if (GUILayout.Button("刷新"))
            {
                _target.Rebuild();
            }
        }
    }
}