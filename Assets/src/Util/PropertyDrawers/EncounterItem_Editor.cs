#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Curry.Explore;

namespace Curry.Util
{
    [CustomEditor(typeof(EncounterItem))]
    public class EncounterItem_Editor : Editor 
    {
        SerializedProperty detail;
        SerializedProperty options;
        ReorderableList optionList;
        void OnEnable()
        {
            detail = serializedObject.FindProperty("m_detail");
            options = detail.FindPropertyRelative("m_options");
            optionList = new ReorderableList(serializedObject, options, true, true, true, true);
            optionList.drawElementCallback = OnDrawConditionElement;
            optionList.drawHeaderCallback = DrawHeader;
        }

        void DrawHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Encounter Options:");
        }
        void OnDrawConditionElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            var current = options.GetArrayElementAtIndex(index);
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUI.PropertyField(rect, current);
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
#endif