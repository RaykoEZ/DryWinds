using System.Collections.Generic;
using UnityEngine;
using Curry.Util;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Curry.Explore
{
    // A storage for all radial range patterns for game pieces in the game
    [CreateAssetMenu(fileName = "RadialRangeMapGenerator", menuName = "Curry/Range/Range map generator asset for radius", order = 1)]
    public class RadialRangeMapGenerator : ScriptableObject
    {
        [SerializeField] List<RangeMap> m_squareRadiusRangeMaps = new List<RangeMap>();
        string fileName = "Range_Rad";
        public RangeMap GetSquareRadiusMap(int range)
        {
            if (range < 0 || range > m_squareRadiusRangeMaps.Count)
            {
                return m_squareRadiusRangeMaps[0];
            }

            return m_squareRadiusRangeMaps[range];
        }
        public void GenerateSquareRadiusMaps(int maxRange)
        {
            m_squareRadiusRangeMaps.Clear();
            for (int i = 0; i < maxRange + 1; ++i)
            {
                RangeMap map = RangeMapping.GetNeighbourRangeMap(i);
                m_squareRadiusRangeMaps.Add(map);
            }
        }
#if UNITY_EDITOR
        public void ExportRangeMaps() 
        {
            string path = EditorUtility.SaveFilePanel("Save Range Asset to folder", "Asset/", fileName, "");
            string prefix = Application.dataPath;
            if (path.Length != 0 && path.StartsWith(prefix))
            {
                path = path.Substring(prefix.Length).TrimStart(Path.DirectorySeparatorChar);
            }
            for (int i = 0; i < m_squareRadiusRangeMaps.Count; i++)
            {
                var asset = CreateInstance<RangeMapAsset>();
                asset.Range = m_squareRadiusRangeMaps[i];
                AssetDatabase.CreateAsset(asset, $"Assets{path}_{i}.asset");
            }
        }
#endif
    }
}