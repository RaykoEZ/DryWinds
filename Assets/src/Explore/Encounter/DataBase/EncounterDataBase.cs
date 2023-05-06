using System.Collections.Generic;
using UnityEngine;
namespace Curry.Explore
{
    [CreateAssetMenu(fileName = "EncounterDatabase", menuName = "Curry/Encounter/Database", order = 2)]
    public class EncounterDataBase : ScriptableObject
    {
        [SerializeField] List<EncounterItem> m_encounters = default;
        public bool TryGetDetail(int index, out EncounterDetail detail)
        {
            bool ret = index > -1 && index < m_encounters.Count && m_encounters[index] != null;
            if (ret) 
            {
                detail = m_encounters[index].Detail;
            }
            else
            {
                Debug.LogWarning($"Detail in EncounterDatabase[{index}] is invalid or item is null");
                detail = new EncounterDetail();
            }
            return ret;
        }
    }
}
