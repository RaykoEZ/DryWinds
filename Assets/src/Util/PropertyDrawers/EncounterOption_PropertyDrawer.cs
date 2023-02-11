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
        GUIContent resultLabel = new GUIContent("Result");
        GUIContent effectLabel = new GUIContent("Option Effects");

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var result = property.FindPropertyRelative("m_encounterResult");
            var effect = property.FindPropertyRelative("m_effect");
            //set the height of the drawer by the field size and padding
            return (labelFieldHeight * numLabel) +
                (numTextArea * textAreaHeight) +
                EditorGUI.GetPropertyHeight(result) +
                EditorGUI.GetPropertyHeight(effect);
        }
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            {
                var description = property.FindPropertyRelative("m_description");
                var summary = property.FindPropertyRelative("m_summary");
                var result = property.FindPropertyRelative("m_encounterResult");
                var effect = property.FindPropertyRelative("m_effect");
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
                        new GUIContent("Result"),
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
                    effectRect.height += EditorGUI.GetPropertyHeight(effect, effectLabel);
                    EffectPropertyField(effectRect, effect, effectLabel);
                }
                EditorGUI.EndProperty();
            }
            EditorGUI.EndProperty();
        }

        void EffectPropertyField(Rect rect, SerializedProperty property, GUIContent label)
        {
            rect = TryHealModule(rect, property, new GUIContent("Heal Player"));
            rect = TryHurtModule(rect, property, new GUIContent("Hurt Player"));
            TryAddCardModule(rect, property, new GUIContent("Add Card(s)"));
        }

        Rect TryHealModule(Rect rect, SerializedProperty property, GUIContent label) 
        {
            if (property.objectReferenceValue is IHealModule heal)
            {
                return DisplayNestedProperty(rect, property, heal.ModuleName, label);
            }
            return rect;
        }
        Rect TryHurtModule(Rect rect, SerializedProperty property, GUIContent label)
        {
            if (property?.objectReferenceValue is IHurtModule hurt)
            {
                return DisplayNestedProperty(rect, property, hurt.ModuleName, label);
            }
            return rect;          
        }
        Rect TryAddCardModule(Rect rect, SerializedProperty property, GUIContent label)
        {
            if (property.objectReferenceValue is IAddToHandModule addCard)
            {
                return DisplayNestedProperty(rect, property, addCard.ModuleName, label);
            }
            return rect;
        }
        Rect DisplayNestedProperty(Rect rect, SerializedProperty property, string nestedName, GUIContent label) 
        {
            var nested = property.FindPropertyRelative(nestedName);
            if (nested == null) return rect;
            rect.height += EditorGUI.GetPropertyHeight(nested);
            EditorGUI.BeginProperty(rect, label, nested);
            EditorGUI.PropertyField(rect, nested, label);
            EditorGUI.EndProperty();
            return rect;
        }
    }
#endif

}