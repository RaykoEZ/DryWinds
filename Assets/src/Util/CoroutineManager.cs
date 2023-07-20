using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Curry.Util
{

    public class CoroutineManager : MonoBehaviour
    {
        protected delegate void OnCoroutineInterrupt(IEnumerator runThis);
        protected Stack<IEnumerator> m_coroutines = new Stack<IEnumerator>();
        protected IEnumerator m_currentCoroutine = default;
        protected OnCoroutineInterrupt m_onCoroutineInterrupted = default;

        bool m_coroutineInProgress = false;
        public bool CoroutineInProgress => m_coroutineInProgress;
        void OnEnable()
        {
            Init();
        }

        void OnDisable()
        {
            Shutdown();
        }

        protected virtual void Init()
        {
            m_onCoroutineInterrupted += OnCoroutineInterrupted;
        }

        protected virtual void Shutdown()
        {
            m_onCoroutineInterrupted -= OnCoroutineInterrupted;
        }

        public void ScheduleCoroutine(IEnumerator coroutine, bool interruptNow = false)
        {
            if (interruptNow)
            {
                m_onCoroutineInterrupted?.Invoke(coroutine);
            }
            else
            {
                m_coroutines.Push(coroutine);
            }
        }

        public void StartScheduledCoroutines()
        {
            if (CoroutineInProgress && m_coroutines.Count == 0)
            {
                return;
            }

            StartCoroutine(StartCurrentCoroutine());
        }

        public void StopCurrentCoroutine()
        {
            StopCoroutine(StartCurrentCoroutine());
            StopCoroutine(m_currentCoroutine);
        }

        void OnCoroutineInterrupted(IEnumerator runThis)
        {
            if (CoroutineInProgress)
            {
                StopCurrentCoroutine();
            }
            StartCoroutine(StartInterruptCoroutine(runThis));
        }

        IEnumerator StartCurrentCoroutine()
        {
            while (m_coroutines.Count > 0)
            {
                m_coroutineInProgress = true;
                m_currentCoroutine = m_coroutines.Pop();
                yield return StartCoroutine(m_currentCoroutine);
                m_coroutineInProgress = false;
            }
        }

        IEnumerator StartInterruptCoroutine(IEnumerator coroutine)
        {
            m_coroutineInProgress = true;
            m_currentCoroutine = coroutine;
            yield return StartCoroutine(coroutine);
            m_coroutineInProgress = false;
            StartCurrentCoroutine();
        }

    }
}
