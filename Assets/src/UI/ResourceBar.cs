using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Curry.UI
{
    public class ResourceBar : MonoBehaviour
    {
        [SerializeField] Slider m_slider = default;

        public void SetBarValue(float val) 
        {
            m_slider.value = val;
        }

        public void SetMaxValue(float val) 
        {
            m_slider.maxValue = val;
        }
    }

}


