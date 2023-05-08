using System.Collections.Generic;
using UnityEngine;
using System;
using Curry.Util;
using Curry.Game;

namespace Curry.Explore
{
    [Serializable]
    public class CallBackup : BaseAbility, ILimitedUse
    {
        [SerializeField] ReinforcementSignal m_signal = default;
        [SerializeField] Reinforcement_EffectResource m_reinforcement = default;
        [SerializeField] ReinforcementPool m_unitsToSpawn = default;
        [SerializeField] int m_maxUses = default;
        int m_useLeft = 0;
        public int UsesLeft { get { return m_useLeft; } protected set { m_useLeft = value; } }
        public bool Usable => UsesLeft > 0;
        public override AbilityContent Content
        {
            get
            {
                var ret = base.Content;
                ret.Name = "Call Baclup";
                ret.Description = $"Requests reinforcement on an adjacent position.";
                return ret;
            }
        }

        public void Refresh()
        {
            UsesLeft = m_maxUses;
        }
        public virtual void Try(object worldPosition, out bool used)
        {
            used = Usable;
            if (Usable && worldPosition is Vector3 position) 
            {

                CallReinforcement(position);
                --UsesLeft;
            }
        }
        protected virtual void CallReinforcement(Vector3 position)
        {
            RangeMap adjacentOffset = RangeMap.Adjacent;
            List<Vector3> spawnPositions = adjacentOffset.ApplyRangeOffsets(position);
            List<Vector3> unoccupied = new List<Vector3>();
            foreach (Vector3 p in spawnPositions)
            {
                Collider2D hit = Physics2D.OverlapCircle(p, 0.1f, m_reinforcement.ReinforcementModule.DoNotSpawnOn);
                if (!hit)
                {
                    unoccupied.Add(p);
                }
            }
            if (unoccupied.Count > 0)
            {
                // we intialize the spawned signal
                Action<PoolableBehaviour> setup = (instance) => 
                {
                    var info = m_unitsToSpawn.GetRandomTargets(1);
                    ReinforcementSignal signal = instance as ReinforcementSignal;
                    signal?.Setup(info[0]);
                };
                int rand = UnityEngine.Random.Range(0, unoccupied.Count);
                m_reinforcement.ReinforcementModule.ApplyEffect(unoccupied[rand], m_signal, setup);
            }
        }
    }
}
