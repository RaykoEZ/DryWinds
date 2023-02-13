#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Curry.Explore
{
    [CustomPropertyDrawer(typeof(AddCardToHand))]
    public class AddCardToHand_PropertyDrawer : PropertyDrawer 
    {
        protected ReorderableList m_cardList;
        protected SerializedProperty m_listProperty;
        protected bool m_cardListInitialized = false;
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var cards = property.FindPropertyRelative("m_cardsToAdd");
            return EditorGUI.GetPropertyHeight(cards, label);
        }
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.indentLevel = 0;
            if (!m_cardListInitialized) 
            {
                InitCardList(property);
            }
            m_cardList?.DoList(rect);
            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
            }
        }
        protected virtual void InitCardList(SerializedProperty property) 
        {
            m_cardListInitialized = true;
            m_listProperty = property.FindPropertyRelative("m_cardsToAdd");
            m_cardList = new ReorderableList(property.serializedObject, m_listProperty, true, true, true, true);
            // draw each card in list
            m_cardList.drawElementCallback = DrawCardInList;
            m_cardList.drawHeaderCallback = DrawListHeader;
        }
        // draw header 
        protected virtual void DrawListHeader(Rect rect) 
        {
            EditorGUI.LabelField(rect, "Card(s) to add to Hand");
        }

        protected virtual void DrawCardInList(Rect rect, int index, bool isActive, bool isFocused)
        {
            EditorGUI.PropertyField(rect, m_listProperty.GetArrayElementAtIndex(index));
        }
    }
#endif
}
