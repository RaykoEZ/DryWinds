using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Curry.UI;
using System.Collections.Generic;
using System;

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
        [SerializeField] EncounterOptionDisplay m_optionRef = default;
        public event OnEncounterUpdate OnEncounterFinished;
        protected List<EncounterOptionDisplay> m_currentOptions = new List<EncounterOptionDisplay>();
        GameStateContext m_contextRef;
        public void BeginEncounter(EncounterDetail detail, GameStateContext context) 
        {
            m_contextRef = context;
            // Set backgound image
            // Set Title & Description text
            m_background.sprite = detail.CoverImage;
            m_title.text = detail.Title;
            // Instantiate all options, initialize them
            // Listen to on chosen to handle
            // dialogues and events when player chooses an option
            foreach(EncounterOptionAttribute option in detail.Options) 
            {
                if (option.OptionDetail.IsOptionAvailable(context)) 
                {
                    EncounterOptionDisplay instance = Instantiate(m_optionRef, m_optionParent);
                    instance.Init(option, context);
                    instance.OnChosen += OnOptionChosen;
                    m_currentOptions.Add(instance);
                }
            }
            StartCoroutine(BeginAnimation(detail));
        }
        IEnumerator BeginAnimation(EncounterDetail detail) 
        {
            m_anim.SetBool("active", true);
            yield return new WaitForSeconds(0.5f);
            m_dialogue.DisplaySingle(detail.Description);
        }
        void OnOptionChosen(EncounterOptionAttribute chosen) 
        {
            foreach (EncounterOptionDisplay option in m_currentOptions)
            {
                option.OnChosen -= OnOptionChosen;
                Destroy(option.gameObject);
            }
            m_currentOptions.Clear();
            // Options give text to display in dialogues
            // When dialogues finishes, trigger encounter effect
            StartCoroutine(OnChosen_Internal(chosen.OptionDetail));
        }
        // Do dialogues
        IEnumerator OnChosen_Internal(EncounterOption outcome) 
        {
            var content = outcome.GetOutcomeContent();
            m_dialogue.OpenDialogue(content.Dialogue, "");
            // call option effect when we finish dialogue
            yield return new WaitUntil(() => !m_dialogue.InProgress);
            yield return StartCoroutine(EncounterOption.Activate(m_contextRef, content.Effects));
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