using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    [CreateAssetMenu(fileName = "Asset", menuName = "Curry/Create an asset for a Prefab", order = 1)]

    public class PrefabAsset : ScriptableObject
    {
        [SerializeField] GameObject m_prefabRef = default;
        public GameObject Prefab { get { return m_prefabRef; } }
    }
}
