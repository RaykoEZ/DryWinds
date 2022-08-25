using System.Collections;
using UnityEngine;

namespace Curry.Explore
{
    public delegate void OnOutOfTime(float hoursSpent);

    public class TimeManager : MonoBehaviour
    {
        [Range(1f, float.MaxValue - 1f)]
        [SerializeField] float m_hoursToClear = default;
        float m_hoursLeft;
        float m_hoursSpent;
        public OnOutOfTime OnOutOfTimeTrigger;
        public float HoursToClear { get { return m_hoursToClear; } }
        public float HoursLeft { get { return m_hoursLeft; } }

        // Use this for initialization
        void Awake()
        {
            ResetTime();
        }

        public void ResetTime()
        {
            m_hoursLeft = m_hoursToClear;
            m_hoursSpent = 0f;
        }

        public void AddTime(float hours) 
        {
            m_hoursLeft += hours;
        }

        // spend time and check if we run out of time
        public void SpendTime(float hoursToSpend, out bool enoughTime) 
        {
            enoughTime = m_hoursLeft >= hoursToSpend;
            if (enoughTime) 
            {
                m_hoursLeft -= hoursToSpend;
                m_hoursSpent += hoursToSpend;
            }
            
            if(Mathf.Approximately(m_hoursLeft, 0f)) 
            {
                OnOutOfTimeTrigger?.Invoke(m_hoursSpent);
            }
        }
    }
}