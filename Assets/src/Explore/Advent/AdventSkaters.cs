﻿using UnityEngine;

namespace Curry.Explore
{
    public class AdventSkaters : AdventCard
    {
        [SerializeField] GameObject m_spawnReference = default;
        protected void Start()
        {
            Activatable = true;
        }
        public override void Activate()
        {
            Debug.Log("Skate activate");
            OnExpend();
        }
    }
}
