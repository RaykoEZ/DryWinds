using System;
using System.Collections.Generic;

namespace Curry.Events 
{
    [Serializable]
    public class EventInfo 
    {
        public Dictionary<string, object> Payload { get; protected set; }
        // Method to call after resolving this event
        public Action OnFinishedCallback { get; protected set; }

        public EventInfo(Dictionary<string, object> payload = null, Action onFinishCallback = null) 
        {
            Payload = payload;
            OnFinishedCallback = onFinishCallback;
        }
    }

}

