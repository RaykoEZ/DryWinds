using System;
using UnityEngine;

namespace Curry.Events
{
    [Serializable]
    public class IntAchieved : IProgress<int>
    {
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
                return Progress == Target;
            }
        }

        public virtual bool UpdateProgress(int progress)
        {
            Progress = progress;
            return Achieved;
        }
    }

    [SerializeField]
    public class ItemObtained : IProgress<string>
    {
        [SerializeField] string m_targetItemId = default;

        public virtual string Target => throw new NotImplementedException();

        public virtual string Progress => throw new NotImplementedException();

        public virtual bool Achieved => throw new NotImplementedException();

        public virtual bool UpdateProgress(string progress)
        {
            throw new NotImplementedException();
        }
    }
}
