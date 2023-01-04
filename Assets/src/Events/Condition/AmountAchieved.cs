using System;
using UnityEngine;

namespace Curry.Events
{
    [Serializable]
    public class AmountAchieved : ICondition<int>
    {
        [SerializeField] string m_contextDescription = default;
        [SerializeField] int m_target = default;
        
        public virtual int Target
        {
            get { return m_target; }
        }

        public virtual int Progress
        {
            get; protected set;
        }

        public virtual bool Achieved
        {
            get 
            {
                return Progress >= Target;
            }
        }

        public string Description { get { return $"{m_contextDescription}: {Progress} / {Target}"; } }

        public virtual bool UpdateProgress(int progress)
        {
            Progress = progress;
            return Achieved;
        }
    }
}
