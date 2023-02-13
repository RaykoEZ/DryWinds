using UnityEngine;
using Curry.Events;
using System;

namespace Curry.Explore
{
    [Serializable]
    public class EncounterInfo : EventInfo
    {
        [SerializeField] int m_encounterId = default;
        public int EncounterId 
        { get { return m_encounterId; } protected set { m_encounterId = value; } }
        public EncounterInfo(int id)
        {
            EncounterId = id;
        }
    }
    public class EncounterManager : MonoBehaviour
    {
        [SerializeField] protected EncounterDataBase m_db = default;
        [SerializeField] protected CurryGameEventListener m_onEncounter = default;
        [SerializeField] protected EncounterUIHandler m_ui = default;
        [SerializeField] protected GameStateManager m_gameState = default;
        void Awake()
        {
            m_onEncounter?.Init();
        }
        public void OnEncounter(EventInfo info)
        {
            if (info is EncounterInfo e && m_db.TryGetDetail(e.EncounterId, out EncounterDetail detail))
            {
                m_ui.BeginEncounter(detail, m_gameState.GetCurrent());
            }
        }
    }

}
