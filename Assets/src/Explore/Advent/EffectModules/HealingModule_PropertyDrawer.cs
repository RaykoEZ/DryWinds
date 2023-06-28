#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Curry.Explore
{
    [CustomPropertyDrawer(typeof(HealingModule))]
    public class HealingModule_PropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var healAmount = property.FindPropertyRelative("m_healAmount");
            return EditorGUI.GetPropertyHeight(healAmount, label);
        }
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            var healAmount = property.FindPropertyRelative("m_healAmount");
            healAmount.intValue = EditorGUI.IntSlider(rect, label, healAmount.intValue, 0, 999);          
        }
    }
}
#endif
