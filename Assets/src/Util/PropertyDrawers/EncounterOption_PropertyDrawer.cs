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
        float padding = 0.5f * EditorGUIUtility.singleLineHeight;
        GUIContent resultLabel = new GUIContent("Result(s) when Chosen");
        GUIContent effectLabel = new GUIContent("Effect(s) when Chosen");

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var result = property.FindPropertyRelative("m_encounterResult");
            var effect = property.FindPropertyRelative("m_effect");
            float nestedHeight = EditorGUIUtility.singleLineHeight; 
            nestedHeight += effect.isExpanded? TotalNestedModuleHeight(effect) : 0f;
            //set the height of the drawer by the field size and padding
            return (labelFieldHeight * numLabel) +
                (numTextArea * textAreaHeight) +
                EditorGUI.GetPropertyHeight(result) +
                EditorGUI.GetPropertyHeight(effect) + nestedHeight + padding;
        }
        // sum of all nested serialize properties under effects
        float TotalNestedModuleHeight(SerializedProperty property) 
        {
            float ret = 0f;
            if (property.objectReferenceValue is IEncounterModule module)
            {
                foreach (string name in module.SerializePropertyNames)
                {
                    ret += GetNestedModuleHeight(name, property);
                }
            }
            return ret;
        }
        // get the height of a single nested field for effect module
        float GetNestedModuleHeight(string nestedName, SerializedProperty property)
        {
            var objectRef = new SerializedObject(property.objectReferenceValue);
            objectRef.Update();
            var nested = objectRef.FindProperty(nestedName);
            return EditorGUI.GetPropertyHeight(nested);
        }
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            {
                var description = property.FindPropertyRelative("m_description");
                var summary = property.FindPropertyRelative("m_summary");
                var result = property.FindPropertyRelative("m_encounterResult");
                var effect = property.FindPropertyRelative("m_effect");

                EditorGUI.indentLevel = 0;
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
                    EditorGUI.GetPropertyHeight(result, resultLabel));

                EditorGUI.BeginProperty(resultRect, resultLabel, result);
                {
                    EditorGUI.PropertyField(
                        resultRect,
                        result,
                        resultLabel,
                        includeChildren: true);
                }
                EditorGUI.EndProperty();
                var effectRect = new Rect(
                    rect.x,
                    resultRect.y + (resultRect.height),
                    rect.width,
                    EditorGUI.GetPropertyHeight(effect, effectLabel));
                EditorGUI.BeginProperty(effectRect, effectLabel, effect);
                {
                    EditorGUI.PropertyField(effectRect, effect, effectLabel);
                    Rect nestedRect = effectRect;
                    nestedRect.y += EditorGUIUtility.singleLineHeight;
                    EffectPropertyField(effectRect, effect);
                }
                EditorGUI.EndProperty();
            }
            EditorGUI.EndProperty();
        }

        void EffectPropertyField(Rect rect, SerializedProperty property)
        {
            if (property.objectReferenceValue is IEncounterModule module)
            {
                foreach(string name in module.SerializePropertyNames) 
                {
                    rect = DisplayNestedProperty(rect, property, name);
                }
            }
        }

        Rect DisplayNestedProperty(Rect rect, SerializedProperty property, string nestedName) 
        {
            SerializedObject objectRef = new SerializedObject(property.objectReferenceValue);
            objectRef.Update();
            var nested = objectRef.FindProperty(nestedName);
            if (nested == null) return rect;
            GUIContent label = new GUIContent(nested.displayName);
            rect.y += EditorGUIUtility.singleLineHeight;
            // Indent foldout
            EditorGUI.indentLevel = 1;
            property.isExpanded = EditorGUI.Foldout(rect, property.isExpanded, "Effect Properties");
            if (property.isExpanded) 
            {
                Rect nestedRect = rect;
                nestedRect.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.indentLevel = 2;
                EditorGUI.BeginProperty(nestedRect, label, nested);
                {
                    EditorGUI.PropertyField(nestedRect, nested);
                }
                EditorGUI.EndProperty();
            }
            objectRef.ApplyModifiedProperties();
            return rect;
        }
    }
#endif

}