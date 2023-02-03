using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Curry.UI;
using System.Collections.Generic;

namespace Curry.Explore
{
    public delegate void OnEncounterUpdate();
    public class EncounterHandler : MonoBehaviour
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
            m_dialogue.DisplaySingle(detail.Description);
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
        }

        void OnOptionChosen(Dialogue chosen) 
        {
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
        }

        void OnFinish()
        {
            OnEncounterFinished?.Invoke();
            foreach (EncounterOption option in m_currentOptions)
            {
                option.OnChosen -= OnOptionChosen;
                Destroy(option);
            }
            m_currentOptions.Clear();
        }
    }
}