﻿#if UNITY_EDITOR
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Curry.Util
{
    // Property drawer for our game condition flags so we can edit the field
    // like enum flags in inspector
    [CustomPropertyDrawer(typeof(GameConditionAttribute))]
    public class GameConditionDefinition_PropertyDrawer : PropertyDrawer
    {
        int fieldAmount = 2;
        float fieldSize = 16;
        float padding = 2;
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            //set the height of the drawer by the field size and padding
            return (fieldSize * fieldAmount) + (padding * fieldAmount);
        }
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            //divide all field heights by the field amount..then minus the padding
            rect.height /= fieldAmount; 
            rect.height -= padding;
            var flag = property.FindPropertyRelative("m_flag");
            var condition = property.FindPropertyRelative("m_conditionRef");
            var conditionRect = new Rect(
                rect.x,
                rect.y,
                rect.width,
                rect.height);
            var flagRect = new Rect(
                rect.x,
                rect.y + rect.height,
                rect.width,
                rect.height);

                EditorGUI.ObjectField(conditionRect, condition);
                if ( condition.objectReferenceValue != null && 
                condition.objectReferenceValue is GameConditionDatabase dataSet)
                {
                    flag.intValue = LayerMaskField(dataSet, flagRect, label.text, flag.intValue);
                }
        }

        int LayerMaskField(GameConditionDatabase attribute, Rect position, string label, int layermask)
        {
            return attribute.FieldToMask(EditorGUI.MaskField(position, label, attribute.MaskToField(layermask),
                attribute.ExistingFlags.ToArray()));
        }
    }
}
#endif