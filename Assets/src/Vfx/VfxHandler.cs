using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.VFX;

namespace Curry.Vfx
{
    // To handle playing timeline animation sequences and propogate timeline signal callbacks
    [RequireComponent(typeof(PlayableDirector))]
    [RequireComponent(typeof(VisualEffect))]
    [RequireComponent(typeof(TimelineSignalListener))]
    public class VfxHandler : MonoBehaviour
    {
        public TimelineSignalListener Trigger => GetComponent<TimelineSignalListener>();
        public PlayableDirector Director => GetComponent<PlayableDirector>();
        public VisualEffect Vfx => GetComponent<VisualEffect>();
        protected bool m_trigger = false;
        void OnSequenceTriggered() 
        {
            m_trigger = true;
        }
        public IEnumerator PlaySequence() 
        {
            Trigger.OnTriggered += OnSequenceTriggered;
            Director?.Play();
            yield return new WaitUntil(() => m_trigger);
            m_trigger = false;
            Trigger.OnTriggered -= OnSequenceTriggered;
        }
    }
}