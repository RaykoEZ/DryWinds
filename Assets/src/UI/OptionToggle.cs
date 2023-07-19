﻿using System.Collections;
using UnityEngine;

namespace Assets.src.UI
{
    public class OptionToggle : MonoBehaviour
    {
        [SerializeField] CanvasGroup m_canvasGroup = default;
        protected bool IsOn = false;
        void Start()
        {
            IsOn = false;
            m_canvasGroup.alpha = 0f;
            m_canvasGroup.blocksRaycasts = false;
        }
        public void Toggle() 
        {
            IsOn = !IsOn;
            m_canvasGroup.alpha = IsOn ? 1f : 0f;
            m_canvasGroup.blocksRaycasts = IsOn;
        }
    }
}