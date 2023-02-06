using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Curry.UI;
using System.Collections.Generic;

namespace Curry.Explore
{
    public delegate void OnEncounterUpdate();
    public class EncounterUIHandler : MonoBehaviour
    {
        [SerializeField] Animator m_anim = default;
        [SerializeField] TextMeshProUGUI m_title = default;
        [SerializeField] Image m_background = default;
        [SerializeField] Transform m_optionParent = default;
        [SerializeField] DialogueManager m_dialogue = default;
        public event OnEncounterUpdate OnEncounterFinished;
        protected List<EncounterOption> m_currentOptions = new List<EncounterOption>();
        public void BeginEncounter(EncounterDetail detail, GameStateContext context) 
        {
            // Set backgound image
            // Set Title & Description text
            m_background.sprite = detail.CoverImage;
            m_title.text = detail.Title;
            // Instantiate all options, initialize them
            // Listen to on chosen to handle
            // dialogues and events when player chooses an option
            foreach(EncounterOption option in detail.Options) 
            {
                EncounterOption instance = Instantiate(option, m_optionParent);
                instance.Init(context);
                instance.OnChosen += OnOptionChosen; 
                m_currentOptions.Add(instance);
            }
            StartCoroutine(BeginAnimation(detail));
        }
        IEnumerator BeginAnimation(EncounterDetail detail) 
        {
            m_anim.SetBool("active", true);
            yield return new WaitForSeconds(0.5f);
            m_dialogue.DisplaySingle(detail.Description);
        }

        void OnOptionChosen(Dialogue chosen) 
        {
            foreach (EncounterOption option in m_currentOptions)
            {
                option.OnChosen -= OnOptionChosen;
                Destroy(option.gameObject);
            }
            m_currentOptions.Clear();
            // Options give text to display in dialogues
            // When dialogues finishes, trigger encounter effect
            StartCoroutine(OnChosen_Internal(chosen));
        }
        // Do dialogues
        IEnumerator OnChosen_Internal(Dialogue chosen) 
        {
            m_dialogue.OpenDialogue(chosen.Text, "");
            yield return new WaitForEndOfFrame();
            // call option effect when we finish dialogue
            yield return new WaitUntil(() => !m_dialogue.InProgress);
            yield return StartCoroutine(chosen.OnDialogueFinish);
            yield return new WaitForEndOfFrame();
            // finish after we yield from encounter effect
            OnFinish();
            yield return new WaitForEndOfFrame();
            m_anim.SetBool("active", false);
        }
        void OnFinish()
        {
            OnEncounterFinished?.Invoke();
        }
    }
}