using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GameEditor
{
    [CustomEditor(typeof(CubeGroundGrid), true)]
    public class CustomGridEditor : Editor
    {
        private string[] guidList;
        private string[] pathList;
        private string[] buildList;
        private int selectIndex = 0;

        private void OnEnable()
        {
            CubeGroundGrid _target = target as CubeGroundGrid;
            guidList = AssetDatabase.FindAssets("t:prefab", new[] { "Assets/Prefab/Build" });
            buildList = new string[guidList.Length];
            pathList = new string[guidList.Length];
            for (int i = 0; i < guidList.Length; i++)
            {
                pathList[i] = AssetDatabase.GUIDToAssetPath(guidList[i]);
                var subPath = pathList[i].Split("/");
                buildList[i] = subPath.Last();
            }
            colorIndex = _target.TeamIndex;
        }

        private int colorIndex;
        private string[] colorList = new[] { "1", "2", "3" };
        public override void OnInspectorGUI()
        {
            CubeGroundGrid _target = target as CubeGroundGrid;
            base.OnInspectorGUI();
            serializedObject.Update();

            EditorGUILayout.BeginHorizontal();
            colorIndex = EditorGUILayout.Popup("初始阵营", colorIndex, colorList);
            if (colorIndex != _target.TeamIndex)
            {
                _target.TeamChange(colorIndex);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            selectIndex = EditorGUILayout.Popup("建筑", selectIndex, buildList);
            if (GUILayout.Button("选择"))
            {
                _target.BuildPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(pathList[selectIndex]);
                _target.Build();
                EditorUtility.SetDirty(_target);
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}