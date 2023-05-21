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
    public delegate void OnEncounterFinish();
    public class EncounterManager : MonoBehaviour
    {
        [SerializeField] protected EncounterUIHandler m_ui = default;
        [SerializeField] protected GameStateManager m_gameState = default;
        [SerializeField] protected Tilemap m_locations = default;
        [SerializeField] protected TileBase m_clearTile = default;
        public event OnEncounterFinish OnEncounterFinished;
        Vector3 m_currentPos;
        void Start()
        {
            m_ui.OnEncounterFinished += OnFinish;
        }
        void OnFinish() 
        {
            Vector3Int mapCoord = m_locations.WorldToCell(m_currentPos);
            m_locations.SetTile(mapCoord, m_clearTile);
            OnEncounterFinished?.Invoke();
        }
        public bool OnEncounter(Vector3 worldPos)
        {
            // If there are special events in this location, trigger them
            if (WorldTile.TryGetTile(m_locations, worldPos, out LocationTile e))
            {
                m_currentPos = worldPos;
                // Remove events after drawing those special event cards
                m_ui.BeginEncounter(e.GetEncounter(), m_gameState.GetCurrent());
                return true;
            }
            else
            {
                return false;
            }       
        }
    }
}
