using UnityEngine;
namespace Curry.Explore
{
    public delegate void OutOfTime();
    public delegate void TimeSpent(int timeSpent, int timeLeft);
    public abstract class TimeManager : MonoBehaviour 
    {
        public event OutOfTime OnOutOfTimeTrigger;
        public event TimeSpent OnTimeSpent;
        public abstract void AddTime(int time);
        // spend time and check if we run out of time
        public abstract bool TrySpendTime(int timeToSpend);
        public abstract void SpendTime(int timeToSpend);
        public abstract void SetTime(int time);
        public abstract void SetMaxTime(int maxTime);
        public abstract int TimeToClear { get; }
        public abstract int TimeLeftToClear { get; }
        protected void OnOutOfTime() 
        {
            OnOutOfTimeTrigger?.Invoke();
        }
        protected void NotifyTimeSpent(int spent) 
        {
            OnTimeSpent?.Invoke(spent, TimeLeftToClear);
        }
    }
}