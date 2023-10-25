using System;
using System.Collections.Generic;
using System.Linq;
using Codice.Client.GameUI.Update;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SceneManagement;
using UnityEditor.Toolbars;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace GameEditor
{
    public class LevelEditorWindow : EditorWindow
    {
        [MenuItem("编辑器/关卡编辑器")]
        public static void ShowMapEditor()
        {
            EditorWindow wnd = GetWindow<LevelEditorWindow>();
            wnd.titleContent = new GUIContent("关卡编辑器");

            wnd.minSize = new Vector2(450, 200);
            wnd.maxSize = new Vector2(1920, 720);
        }

        private Dictionary<string, LevelAsset> m_LevelAssetDic;
        private List<string> m_LevelAssetCache;
        private LevelAsset m_CurEditAsset;
        private Editor m_AssetEditor;
        private List<string> LevelAssetList()
        {
            var levelAssets = AssetDatabase.FindAssets("t:LevelAsset");
            m_LevelAssetCache = new List<string>();
            m_LevelAssetDic = new Dictionary<string, LevelAsset>();
            foreach (var guid in levelAssets)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<LevelAsset>(path);
                m_LevelAssetCache.Add(asset.ShowName);
                m_LevelAssetDic[asset.ShowName] = asset;
            }
            return m_LevelAssetCache;
        }

        private DropdownField m_Dropdown;
        public void CreateGUI()
        {
            RebuildWindow();
        }

        private void RebuildWindow()
        {
            LevelAssetList();
            rootVisualElement.Clear();
            var toolbar = new Toolbar();
            int defaultIndex = 0;
            if (m_CurEditAsset)
            {
                defaultIndex = m_LevelAssetCache.FindIndex(m => m == m_CurEditAsset.ShowName);
            }
            m_Dropdown = new DropdownField("资源:", LevelAssetList(), defaultIndex, (s) =>
            {
                m_CurEditAsset = m_LevelAssetDic[s];
                return s;
            });
            toolbar.Add(m_Dropdown);
            
            var btnCreate = new Button();
            btnCreate.text = "创建";
            btnCreate.clicked += () =>
            {
                var asset = ScriptableObject.CreateInstance<LevelAsset>();
                int id = 1;
                for (int i = 1; i <= 999; i++)
                {
                    if (AssetDatabase.LoadAssetAtPath<LevelAsset>(LevelAsset.AssetPath + $"LevelAsset_{i}.asset") ==
                        null)
                    {
                        id = i;
                        break;
                    }
                }
                asset.ID = id;
                AssetDatabase.CreateAsset(asset, LevelAsset.AssetPath + $"LevelAsset_{id}.asset");
                AssetDatabase.SaveAssets();
                m_CurEditAsset = asset;
                RebuildWindow();
            };
            toolbar.Add(btnCreate);

            var btnSave = new Button();
            btnSave.text = "保存";
            btnSave.clicked += () =>
            {
                AssetDatabase.SaveAssets();
                RebuildWindow();
            };
            toolbar.Add(btnSave);
            
            rootVisualElement.Add(toolbar);
        }

        private Button m_ChangeScene;
        private void OnGUI()
        {
            if (!CheckScene())
            {
                return;
            }
            else
            {
                if (m_CurEditAsset)
                {
                    var editor = Editor.CreateEditor(m_CurEditAsset);
                    editor.OnInspectorGUI();
                }
            }
        }

        private bool CheckScene()
        {
            if (EditorSceneManager.GetActiveScene().name != "EditScene")
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.BeginVertical();
                    {
                        GUILayout.FlexibleSpace();
                        GUILayout.Label("切换到关卡编辑场景", GUIStyle.none);
                        if (GUILayout.Button("切换", GUILayout.Width(200)))
                        {
                            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                            {
                                EditorSceneManager.OpenScene("Assets/Scenes/EditScene.unity");
                            }
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndVertical();
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
                return false;
            }
            return true;
        }
    }
}