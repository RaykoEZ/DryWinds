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
    public delegate void OnEncounterFinish();
    public class EncounterManager : MonoBehaviour
    {
        [SerializeField] protected EncounterUIHandler m_ui = default;
        [SerializeField] protected GameStateManager m_gameState = default;
        public event OnEncounterFinish OnEncounterFinished;
        void Start()
        {
            m_ui.OnEncounterFinished += OnFinish;
        }
        void OnFinish() 
        {
            OnEncounterFinished?.Invoke();
        }
        public void OnEncounter(EncounterDetail enc)
        {
            m_ui.BeginEncounter(enc, m_gameState.GetCurrent());            
        }
    }
}
