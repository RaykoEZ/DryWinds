using Curry.Events;
using Curry.Util;
using UnityEngine;

namespace Curry.UI
{
    public abstract class GameMessageDisplay : MonoBehaviour 
    {
        [SerializeField] protected Animator m_anim = default;
        [SerializeField] protected CurryGameEventListener m_onMessageTrigger = default;
        [SerializeField] protected CoroutineManager m_coroutine = default;
        // Use this for initialization
        void Start()
        {
            m_onMessageTrigger.Init();
        }
        public abstract void OnGameMessage(EventInfo info);
    }
}