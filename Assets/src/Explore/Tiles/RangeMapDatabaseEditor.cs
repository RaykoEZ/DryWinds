using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Curry.Explore
{
#if UNITY_EDITOR
    // Inspector setup for RangeDisplayMasks to auto-generate some range tiles
    [CustomEditor(typeof(RangeMapDatabase))]
    public class RangeMapDatabaseEditor : Editor
    {
        int maxRange = 0;

        SerializedObject m_editorObject;

        void OnEnable()
        {
            m_editorObject = new SerializedObject(target);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            RangeMapDatabase script = target as RangeMapDatabase;
            maxRange = EditorGUILayout.IntField("Max Square Range to Generate:", maxRange);

            if (GUILayout.Button("Generate Square Radius Masks", GUILayout.Height(20)))
            {
                script.GenerateSquareRadiusMaps(maxRange);
                m_editorObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(script);
            }
        }

    }
#endif

}
