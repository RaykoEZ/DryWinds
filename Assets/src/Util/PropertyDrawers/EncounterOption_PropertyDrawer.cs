#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

namespace Curry.Explore
{
    [CustomPropertyDrawer(typeof(EncounterOptionAttribute))]
    public class EncounterOption_PropertyDrawer : PropertyDrawer 
    {
        int numTextArea = 2;
        int numLabel = 2;
        float labelFieldHeight = 24f;
        float textAreaHeight = 64;
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var result = property.FindPropertyRelative("m_encounterResult");
            //set the height of the drawer by the field size and padding
            return (labelFieldHeight * numLabel) + (numTextArea * textAreaHeight) + 
                EditorGUI.GetPropertyHeight(result);
        }
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            { 
                var description = property.FindPropertyRelative("m_description");
                var summary = property.FindPropertyRelative("m_summary");
                var result = property.FindPropertyRelative("m_encounterResult");
                var labelRect = new Rect(
                    rect.x,
                    rect.y,
                    rect.width,
                    labelFieldHeight);
                EditorGUI.LabelField(labelRect, "Description");

                var textAreaRect = new Rect(
                    rect.x,
                    rect.y + labelFieldHeight,
                    rect.width,
                    textAreaHeight);
                description.stringValue = EditorGUI.TextArea(textAreaRect, description.stringValue);

                var sumLabelRect = new Rect(
                    rect.x,
                    textAreaRect.y + textAreaHeight,
                    rect.width,
                    labelFieldHeight);
                EditorGUI.LabelField(sumLabelRect, "Action Summanry");

                var sumRect = new Rect(
                    rect.x,
                    sumLabelRect.y + labelFieldHeight,
                    rect.width,
                    textAreaHeight);
                summary.stringValue = EditorGUI.TextArea(sumRect, summary.stringValue);

                var resultRect = new Rect(
                    rect.x,
                    sumRect.y + (textAreaHeight),
                    rect.width,
                    EditorGUI.GetPropertyHeight(result));
                
                EditorGUI.BeginProperty(resultRect, new GUIContent("Result"), result);
                {
                    EditorGUI.PropertyField(
                        resultRect,
                        result,
                        new GUIContent("Result"),
                        includeChildren: true);
                }
                EditorGUI.EndProperty();
            }
            EditorGUI.EndProperty();
        }
    }
#endif

}