#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Curry.Explore
{
    [CustomPropertyDrawer(typeof(DealDamageTo))]
    public class DealDamageTo_PropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var damage = property.FindPropertyRelative("m_baseDamage");
            return EditorGUI.GetPropertyHeight(damage, label);
        }
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            var damage = property.FindPropertyRelative("m_baseDamage");
            label.text = "Base " + nameof(damage);
            damage.intValue = EditorGUI.IntSlider(rect, label, damage.intValue, 0, 999);
        }
    }
#endif
}
