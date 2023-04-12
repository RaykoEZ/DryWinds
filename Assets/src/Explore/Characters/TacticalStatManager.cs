using Curry.Game;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    // handles modifiers applied by cards, items, attacks 
    public class TacticalStatManager : IModifierContainer<TacticalStats>
    {
        protected List<IStatModifier<TacticalStats>> m_mods = 
            new List<IStatModifier<TacticalStats>>();
        protected List<IStatModifier<TacticalStats>> m_toRemove =
            new List<IStatModifier<TacticalStats>>();
        protected List<IStatModifier<TacticalStats>> m_toAdd =
            new List<IStatModifier<TacticalStats>>();
        public IReadOnlyList<IStatModifier<TacticalStats>> Modifiers => m_mods;
        public event OnModifierExpire<TacticalStats> OnModExpire;
        public event OnModifierTrigger<TacticalStats> OnModTrigger;
        TacticalStats m_base;
        TacticalStats m_current;
        public TacticalStats Current { get { return m_current; } protected set { m_current = value; } }
        public virtual void Init(TacticalStats start) 
        {
            m_base = start;
            m_current = m_base;
        } 
        public void OnMovementFinish() 
        {
            foreach (var mod in m_mods)
            {
                if (mod is IMovementElement<TacticalStats> movement)
                {
                    movement.OnCharacterMoved(Current);
                }
            }
            UpdateModifierState();
        }

        public int CalculateDamage(int hitVal) 
        {
            int ret = hitVal;
            foreach(var mod in m_mods) 
            { 
                if(mod is IDamageModifier damageModifier) 
                {
                    ret = damageModifier.Apply(ret);
                }
            }
            UpdateModifierState();
            return Mathf.Max(0, ret);
        }
        public void SetMaxHp(int maxHp) 
        {
            if (maxHp < 1) return;
            m_current.MaxHp = maxHp;
        }
        public void TakeDamage(int damage) 
        {
            if (damage < 0) return;
            int result = m_current.Hp - damage;
            m_current.Hp = Mathf.Max(result, 0);
        }
        public void RecoverHp(int recover) 
        {
            if (recover < 0) return;
            int result = m_current.Hp + recover;
            m_current.Hp = Mathf.Min(result,m_current.MaxHp);      
        }
        public void SetMovementRange(int range) 
        {
            m_current.MoveRange = Mathf.Clamp(range, 0, 3);
        }
        public void SetSpeed(int speed) 
        {
            m_current.Speed = speed;
        }
        public void SetVisibility(ObjectVisibility newVal) 
        {
            m_current.Visibility = newVal;
        }
        public virtual void OnTimeElapsed(int dt) 
        {
            foreach (IStatModifier<TacticalStats> mod in m_mods)
            {
                if(mod is ITimedElement<int> timer)
                {
                    timer.OnTimeElapsed(dt);
                }
            }
            UpdateModifierState();
        }
        public virtual void ApplyModifier(IStatModifier<TacticalStats> mod)
        {
            m_toAdd.Add(mod);
        }
        protected virtual void OnModifierExpire(IStatModifier<TacticalStats> mod)
        {
            m_toRemove.Add(mod);
        }
        protected virtual void AddModifier_Internal(IStatModifier<TacticalStats> mod)
        {
            if (mod == null)
            {
                return;
            }
            mod.OnExpire += OnModifierExpire;
            mod.OnTrigger += OnModifierEffectTrigger;
            m_mods.Add(mod);
        }

        protected virtual void RemoveExpiredModifier(IStatModifier<TacticalStats> mod)
        {
            if (mod == null)
            {
                return;
            }
            mod.OnExpire -= OnModifierExpire;
            mod.OnTrigger -= OnModifierEffectTrigger;
            m_mods.Remove(mod);
            OnModExpire?.Invoke(mod);
        }
        protected virtual void OnModifierEffectTrigger(IStatModifier<TacticalStats> mod)
        {
            UpdateModifierState();
            OnModTrigger?.Invoke(mod);
        }
        protected virtual void UpdateModifierState()
        {
            // Clear all expired mods this frame
            foreach (IStatModifier<TacticalStats> expired in m_toRemove)
            {
                RemoveExpiredModifier(expired);
            }
            m_toRemove.Clear();

            //Add all new modifiers
            foreach (IStatModifier<TacticalStats> newMod in m_toAdd)
            {
                AddModifier_Internal(newMod);
            }
            m_toAdd.Clear();
            // Reset to base stat
            Current = m_base;
            // if there are modifiers, reapply them to reset stats
            if (m_mods.Count == 0)
            {
                return;
            }
            else
            {
                // Apply all modifiera to base
                foreach (var mod in m_mods)
                {
                    Current = mod.Apply(Current);
                }
            }
        }
    }
}
