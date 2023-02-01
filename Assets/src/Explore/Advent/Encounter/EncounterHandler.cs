using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Curry.UI;
namespace Curry.Explore
{
    public delegate void OnEncounterUpdate();
    public class EncounterHandler : MonoBehaviour
    {
        [SerializeField] Animator m_anim = default;
        [SerializeField] TextMeshProUGUI m_title = default;
        [SerializeField] TextMeshProUGUI m_description = default;
        [SerializeField] Image m_background = default;
        [SerializeField] Transform m_options = default;
        public event OnEncounterUpdate OnEncounterFinished;
        public void BeginEncounter(EncounterDetail detail) 
        { 
            // Set backgound image
            // Set Title & Description text
            // Instantiate all options, initialize them

            // Listen to on chosen to handle
            // dialogues and events when player chooses an option

        }

        void OnOptionChosen(Dialogue chosen) 
        { 
            
        }
        // Do dialogues
        IEnumerator OnChosen_Internal(Dialogue chosen) 
        {
            yield return null;
        }

        void OnFinish()
        {
            OnEncounterFinished?.Invoke();
        }
    }
}