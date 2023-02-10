using System;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    [CreateAssetMenu(fileName = "Enc_", menuName = "Curry/Encounter/Item", order = 1)]
    public class EncounterItem : ScriptableObject 
    {
        [SerializeField] EncounterDetail m_detail = default;
        public EncounterDetail Detail => m_detail;
    }


}
