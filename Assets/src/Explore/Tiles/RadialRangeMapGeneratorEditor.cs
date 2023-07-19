#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Curry.Explore
{
    // Inspector setup for RangeDisplayMasks to auto-generate some range tiles
    [CustomEditor(typeof(RadialRangeMapGenerator))]
    public class RadialRangeMapGeneratorEditor : Editor
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
            RadialRangeMapGenerator script = target as RadialRangeMapGenerator;
            maxRange = EditorGUILayout.IntField("Max Square Range to Generate:", maxRange);

            if (GUILayout.Button("Generate Square Radius Masks", GUILayout.Height(20)))
            {
                script.GenerateSquareRadiusMaps(maxRange);
                m_editorObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(script);
            }
            if (GUILayout.Button("Export generated range maps", GUILayout.Height(20))) 
            {
                script.ExportRangeMaps();
            }
        }
    }
}
#endif
