using UnityEditor;
using UnityEngine;

namespace GameEditor
{
    [CustomPropertyDrawer(typeof(RectSize))]
    public class RectSizeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int originalIndentLevel = EditorGUI.indentLevel;
            float originalLabelWidth = EditorGUIUtility.labelWidth;
            
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            position.width = position.width / 2f;
            EditorGUIUtility.labelWidth = position.width / 2f;
            EditorGUI.indentLevel = 1;
            EditorGUI.PropertyField(position, property.FindPropertyRelative("width"));
            position.x += position.width;
            EditorGUI.PropertyField(position, property.FindPropertyRelative("height"));
            EditorGUI.EndProperty();
            
            EditorGUI.indentLevel = originalIndentLevel;
            EditorGUIUtility.labelWidth = originalLabelWidth;
        }
    }
}