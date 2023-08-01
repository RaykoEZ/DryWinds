using UnityEngine;
using UnityEngine.Timeline;

namespace Curry.Vfx
{
    // A simple component to listen to timeline signals, will add more signal events if needed
    public delegate void OnSignalTriggered();
    public class TimelineSignalListener : SignalReceiver
    {
        public event OnSignalTriggered OnTriggered;
        public void ReceiveSequenceSignal() 
        {
            OnTriggered?.Invoke();
        }
    }
}