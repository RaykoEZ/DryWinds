using System;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    [CreateAssetMenu(fileName = "Enc_", menuName = "Curry/Encounter/New Encounter Entry", order = 1)]
    public class EncounterEntry : ScriptableObject 
    {
        [SerializeField] EncounterDetail m_detail = default;
        public EncounterDetail Detail => m_detail;
    }
}
