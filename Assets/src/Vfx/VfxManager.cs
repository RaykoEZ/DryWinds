using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.VFX;
namespace Curry.Vfx
{
    public class VfxManager : MonoBehaviour 
    {
        [SerializeField] Transform m_vfxParent = default;
        [SerializeField] VfxSequencePlayer m_vfxRef = default;
        Queue<VfxHandle> m_readyToUse = new Queue<VfxHandle>();
        List<VfxHandle> m_inUse = new List<VfxHandle>();
        public delegate void OnVfxFinish(VfxHandle handle);
        public class VfxHandle 
        {
            protected VfxSequencePlayer m_player;
            public event OnVfxFinish OnFinish;
            public VfxHandle(VfxSequencePlayer player) 
            {
                m_player = player;
            }
            public void Init(VisualEffectAsset vfx, TimelineAsset timeline) 
            {
                m_player?.SetupAsset(vfx, timeline);
                // Clear all dangling external callbacks on setup
                OnFinish = null;
            }
            public IEnumerator PlayerVfx(Action onTrigger = null) 
            {
                yield return m_player?.PlayVfxSequence(onTrigger);
            }
            public void PlayVfx() 
            {
                m_player?.PlaySequence();
            }
            public void StopVfx()
            {
                m_player?.ResetVfx();
                OnFinish?.Invoke(this);
            }
        }
        public VfxHandle AddVfx(VisualEffectAsset vfx, TimelineAsset timeline) 
        {
            VfxHandle ret;
            if (m_readyToUse.Count > 0) 
            {
                ret = m_readyToUse.Dequeue();
            } 
            else 
            {
                ret = InstantiateVfx(vfx, timeline);
            }
            ret.Init(vfx, timeline);
            ret.OnFinish += DeallocateVfx;
            m_inUse.Add(ret);
            return ret;
        }
        void DeallocateVfx(VfxHandle vfx)
        {
            vfx.OnFinish -= DeallocateVfx;
            m_inUse.Remove(vfx);
            m_readyToUse.Enqueue(vfx);
        }
        // make new vfx objects if we don't have free ones in pool
        VfxHandle InstantiateVfx(VisualEffectAsset vfx, TimelineAsset timeline)
        {
            var ret = Instantiate(m_vfxRef, m_vfxParent, worldPositionStays: false);
            return new VfxHandle(ret);
        }
    }
}