using UnityEngine;
using Curry.Events;
using System;
using UnityEngine.Tilemaps;

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
        [SerializeField] protected EncounterUIHandler m_ui = default;
        [SerializeField] protected GameStateManager m_gameState = default;
        [SerializeField] protected Tilemap m_locations = default;
        [SerializeField] protected TileBase m_clearTile = default;
        [SerializeField] protected EncounterIcon m_icon = default;
        Vector3 m_currentPos;
        void Start()
        {
            m_ui.OnEncounterFinished += OnFinish;
            m_icon.OnTrigger += TriggerEncounter;
        }
        void OnFinish() 
        {
            Vector3Int mapCoord = m_locations.WorldToCell(m_currentPos);
            m_locations.SetTile(mapCoord, m_clearTile);
            DisableEncounter();
        }
        public void TriggerEncounter() 
        {
            if (WorldTile.TryGetTile(m_locations, m_currentPos, out LocationTile e))
            {
                m_ui.BeginEncounter(e.GetEncounter(), m_gameState.GetCurrent());
            }
            else 
            {
                Debug.Log("No encounters here");
            }
        }
        // Remove active encounter icon
        public void DisableEncounter() 
        {
            m_icon?.Hide();
        }
        public void CheckForEncounter(Vector3 worldPos)
        {
            m_currentPos = worldPos;
            // If there are special events in this location, trigger them
            if (WorldTile.TryGetTile(m_locations, worldPos, out LocationTile _))
            {
                // Pop the encounter prompt icon if there is an encounter at this position
                m_icon?.Show();
            }     
        }
    }
}
