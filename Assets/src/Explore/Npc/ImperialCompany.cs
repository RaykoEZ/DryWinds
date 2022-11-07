using System;
using UnityEngine;

namespace Curry.Explore
{
    // A basic enemy unit for now, may add more features
    public class ImperialCompany : TacticalEnemy, IEnemy
    {
        [SerializeField] AdventurerDetector m_detect = default;
        protected override void Awake()
        {
            base.Awake();
            m_detect.OnDetected += OnDetectEnter;
            m_detect.OnExitDetection += OnDetectExit;
        }

        void OnDetectEnter(Adventurer explorer) 
        {
            Debug.Log("target acquired");
            OnDetectReaction();
            OnCombat();
        }
        void OnDetectExit(Adventurer explorer)
        {
            Debug.Log("m8");
        }
    }
}
