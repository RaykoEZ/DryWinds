using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    [CreateAssetMenu(fileName = "SkillAsset", menuName = "Curry/Create an asset for a Skill", order = 1)]

    public class Asset : ScriptableObject
    {
        [SerializeField] GameObject m_prefabRef = default;
        public GameObject Prefab { get { return m_prefabRef; } }
    }
}
