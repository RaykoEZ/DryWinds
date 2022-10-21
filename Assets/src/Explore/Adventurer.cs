using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Events;

namespace Curry.Explore
{
    [Serializable]
    public struct AdventurerStats
    {
        [SerializeField] string m_name;
        [Range(1, 3)]
        [SerializeField] int m_scoutRange;
        [SerializeField] Transform m_adventurer;
        public string Name { get { return m_name; } }
        public int ScoutRange { get { return m_scoutRange; } }
        public Vector3 WorldPosition { get { return m_adventurer.position; } }

        public AdventurerStats(AdventurerStats stats) 
        {
            m_name = stats.Name;
            m_scoutRange = stats.ScoutRange;
            m_adventurer = stats.m_adventurer;
        }
    }

    public class PlayerInfo : EventInfo
    {
        public AdventurerStats PlayerStats { get; protected set; }
        public PlayerInfo(AdventurerStats stats) 
        {
            PlayerStats = stats;
        }
    }

    // A basic character for adventure mode
    public class Adventurer : MonoBehaviour 
    {
        [SerializeField] AdventurerStats m_stats = default;
        [SerializeField] CurryGameEventListener m_onMove = default;
        // Pings player status to trigger card draw events etc
        [SerializeField] CurryGameEventTrigger m_onPlayerPing = default;
        [SerializeField] CurryGameEventTrigger m_onPlayerDraw = default;
        public AdventurerStats Stats { get { return m_stats; } }
        void Awake()
        {
            m_onMove?.Init();
        }
        void Start()
        {
            // Ping once at start
            PlayerInfo info = new PlayerInfo(new AdventurerStats(m_stats));
            m_onPlayerPing?.TriggerEvent(info);
        }
        public void Move(EventInfo info) 
        {
            if (info == null) return;

            Vector3 target;
            // Depending on event param type, set target target position 
            if (info is PositionInfo select) 
            {
                target = select.WorldPosition;
                target.z = transform.position.z;
                StartCoroutine(Move_Internal(target));
            }
        }

        IEnumerator Move_Internal(Vector3 target) 
        {
            RaycastHit2D hit = Physics2D.Linecast(
                    transform.position,
                    target,
                    LayerMask.GetMask("Obstacles"));
            // Check for walls
            if (hit) 
            { 
                yield break; 
            }

            float duration = 1f;
            float timeElapsed = 0f;
            while (timeElapsed <= duration) 
            {
                timeElapsed += Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, target, timeElapsed / duration);
                yield return null;
            }
            PlayerInfo info = new PlayerInfo(new AdventurerStats(m_stats));
            m_onPlayerDraw?.TriggerEvent(info);
            m_onPlayerPing?.TriggerEvent(info);
        }
    }
}
