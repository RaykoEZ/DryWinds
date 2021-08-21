using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Skill
{
    [CreateAssetMenu(fileName = "TraceAsset", menuName = "Curry/Create an asset for a Trace", order = 1)]

    public class TraceAsset : ScriptableObject
    {
        [SerializeField] GameObject m_prefabRef = default;
        public GameObject Prefab { get { return m_prefabRef; } }
    }
}
