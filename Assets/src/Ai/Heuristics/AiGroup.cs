using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Ai
{
    [Serializable]
    public struct AiGroupAwareness
    {
        public List<Vector2> PlacesOfInterest { get; set; }
        public Transform ConfirmedTarget { get; set; }

        public AiGroupAwareness(List<Vector2> interests, Transform target = null)
        {
            PlacesOfInterest = interests;
            ConfirmedTarget = target;
        }
    }

    [Serializable]
    public class AiGroup : MonoBehaviour
    {
        [SerializeField] Collider2D m_commsRange = default;
        protected List<BaseNpc> m_groupMembers = new List<BaseNpc>();
        protected AiGroupAwareness GroupAwareness { get; }

        protected virtual void AlertMembers() 
        {
        }
    }
}
