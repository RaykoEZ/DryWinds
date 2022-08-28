using System.Collections.Generic;
using UnityEngine;
using Curry.Events;

namespace Curry.Explore
{
    public class CrisisManager : MonoBehaviour
    {
        [SerializeField] CurryGameEventListener m_onCreateCrisis = default;
        List<ICrisis> m_crisisList = new List<ICrisis>();
        // Use this for initialization
        void OnEnable()
        {
            m_onCreateCrisis?.Init();
        }

        void OnDisable()
        {
            m_onCreateCrisis?.Shutdown();
        }


    }
}