using System;
using UnityEngine;
using System.Collections;

namespace Curry.Explore
{
    public class EnemyAction : Phase
    {
        [SerializeField] EnemyManager m_enemies = default;      
        protected override Type NextState { get; set; } = typeof(TurnEnd);
        bool m_actionFinished = false;
        public override void Init()
        {
            m_enemies.OnActionFinish += OnEnemyActionFinish;
            base.Init();
        }
        void OnEnemyActionFinish() 
        {
            m_actionFinished = true;
        }
        protected override IEnumerator Evaluate_Internal()
        {
            m_actionFinished = false;
            if (!m_enemies.OnEnemyAction())
            {
                m_actionFinished = true;
            }
            yield return new WaitUntil(() => m_actionFinished);
            TransitionTo();
        }
    }
}