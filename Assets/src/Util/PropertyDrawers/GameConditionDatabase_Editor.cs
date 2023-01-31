using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;

namespace Curry.Util
{
#if UNITY_EDITOR
    [CustomEditor(typeof(GameConditionDatabase))]
    public class GameConditionDatabase_Editor : Editor 
    {
        SerializedProperty conditions;
        ReorderableList condList;
        bool ConditionsAtMaximumSize => conditions.arraySize == 32;
        void OnEnable()
        {
            conditions = serializedObject.FindProperty("m_definedConditions");
            condList = new ReorderableList(serializedObject, conditions,false, true, !ConditionsAtMaximumSize, true);
            condList.onAddCallback = OnAdd;
            condList.drawElementCallback = OnDrawConditionElement;
            condList.drawHeaderCallback = DrawHeader;
        }
        void OnAdd(ReorderableList list)
        {
            if (!ConditionsAtMaximumSize)
            {
                ++conditions.arraySize;
                int newIndex = conditions.arraySize - 1;
                var newElement = conditions.GetArrayElementAtIndex(newIndex);
                newElement.stringValue = $"NewCondition{newIndex}";
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfRequiredOrScript();
            condList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
        void DrawHeader(Rect rect) 
        {
            EditorGUI.LabelField(rect, "Existing Flags (for in-game progress/accomplishment logging)");
        }
        void OnDrawConditionElement(Rect rect, int index, bool isActive, bool isFocused) 
        {
            var currentCondition = conditions.GetArrayElementAtIndex(index);
            DisplayFlagElement(rect, currentCondition, GUIContent.none, index);
        }

        void DisplayFlagElement(Rect rect, SerializedProperty property, GUIContent label, int index)
        {
            var labelRect = new Rect(
                rect.x,
                rect.y,
                0.25f * rect.width,
                rect.height);

            var nameFieldRect = new Rect(
                rect.x + 0.25f * rect.width,
                rect.y,
                0.5f * rect.width,
                rect.height);

            var flagRect = new Rect(
                rect.x + (0.8f * rect.width),
                rect.y,
                rect.width,
                rect.height);

            EditorGUIUtility.labelWidth = 0.25f * rect.width;
            GUIStyle centeredStyle = GUI.skin.GetStyle("Label");
            centeredStyle.alignment = TextAnchor.UpperCenter;
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUI.LabelField(labelRect, $"Condition {index}", centeredStyle);
                GUILayout.FlexibleSpace();
                property.stringValue = EditorGUI.TextField(nameFieldRect, property.stringValue);
                GUILayout.FlexibleSpace();
                // Makes the fields disabled / grayed out
                EditorGUI.BeginDisabledGroup(true);
                {
                    // In your case the best option would be a Vector3Field which handles the correct drawing
                    GUILayout.FlexibleSpace();
                    EditorGUI.IntField(flagRect, index);
                    GUILayout.FlexibleSpace();
                }
                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
#endif
}