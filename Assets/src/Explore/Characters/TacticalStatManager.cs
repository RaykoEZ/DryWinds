using Curry.Game;
using System.Collections.Generic;

namespace Curry.Explore
{
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
        public TacticalStats Result { get; protected set; }
        public virtual void Init(TacticalStats start) 
        {
            Result = start;
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
        }

        public virtual void AddModifier(IStatModifier<TacticalStats> mod)
        {
            m_toAdd.Add(mod);
        }
        protected virtual void AddModifier_Internal(IStatModifier<TacticalStats> mod)
        {
            if (mod == null)
            {
                return;
            }
            mod.OnModifierExpire += OnModifierExpire;
            mod.OnTrigger += OnModifierEffectTrigger;
            m_mods.Add(mod);
            Result = mod.Apply(Result);
        }

        protected virtual void RemoveExpiredModifier(IStatModifier<TacticalStats> mod)
        {
            if (mod == null)
            {
                return;
            }
            mod.OnModifierExpire -= OnModifierExpire;
            mod.OnTrigger -= OnModifierEffectTrigger;
            m_mods.Remove(mod);
            OnModExpire?.Invoke(mod);
        }
        protected virtual void OnModifierExpire(IStatModifier<TacticalStats> mod)
        {
            m_toRemove.Add(mod);
        }
        protected virtual void OnModifierEffectTrigger()
        {
            UpdateModifierValue();
            OnModTrigger?.Invoke();
        }
        protected virtual void UpdateModifierValue()
        {
            if (m_mods.Count == 0)
            {
                return;
            }
            else
            {
                // Apply all modifiera to base
                foreach (var mod in m_mods)
                {
                    Result = mod.Apply(Result);
                }
            }
        }
    }
}
