using System;
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
        // set vfx and sequence assets and bind components to PlayableDirector
        public void SetupAsset(VisualEffectAsset vfx, TimelineAsset timeline) 
        {
            // prevent null and duplicate setup
            if (timeline != null && vfx != null &&
                Vfx.visualEffectAsset != vfx &&
                Director.playableAsset != timeline) 
            {
                Director.playableAsset = timeline;
                Vfx.visualEffectAsset = vfx;
                var tracks = Director.playableAsset.outputs;
                foreach (PlayableBinding binding in tracks)
                {
                    // Rebind all timeline tracks depending on its acccepted component type
                    Type bindingType = binding.outputTargetType;
                    if (bindingType == typeof(VisualEffect))
                    {
                        Director.SetGenericBinding(binding.sourceObject, Vfx);
                    }
                    else if (bindingType == typeof(AudioSource))
                    {
                        Director.SetGenericBinding(binding.sourceObject, GetComponent<AudioSource>());
                    }
                    else if (bindingType == typeof(SignalReceiver))
                    {
                        Director.SetGenericBinding(binding.sourceObject, Trigger);
                    }
                }
            }
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