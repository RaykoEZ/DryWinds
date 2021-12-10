﻿using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Curry.Events;
using TMPro;

namespace Curry.UI
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] CurryGameEventListener m_listener = default;
        [SerializeField] TextMeshProUGUI m_nameDisplay = default;
        [SerializeField] TextMeshProUGUI m_dialogueDisplay = default;
        [SerializeField] Animator m_boxAnim = default;
        [SerializeField] InputActionReference m_continueAction = default;
        Queue<string> m_dialogue;
        string m_currentLine;
        Coroutine m_displayingText;

        void OnEnable()
        {
            m_listener.Init();
        }
        void OnDisable()
        {
            m_listener.Shutdown();
        }
        void Update()
        {
            if(m_continueAction.action.triggered) 
            {
                Debug.Log("space");
                NextPage();
            }
        }

        public void OpenDialogue(Dialogue dialogue, bool diaplayName = true) 
        {
            m_dialogue?.Clear();
            m_currentLine = "";
            m_nameDisplay.text = diaplayName ? dialogue.Name : "";
            m_dialogueDisplay.text = "";
            m_dialogue = new Queue<string>(dialogue.DialogueLines);
            m_boxAnim.SetBool("BoxOn", true);
            NextPage();
        }

        public void OnDialogueTrigger(EventInfo info) 
        {
            bool diaplayName = info.Payload["displayName"] != null || 
                (bool)info.Payload["displayName"];
            Debug.Log("dialogue open: " + diaplayName);

            if (info.Payload["dialogue"] is Dialogue dialogue) 
            {
                OpenDialogue(dialogue, diaplayName);
            }
        }

        public void NextPage() 
        {
            m_dialogueDisplay.text = "";
            if(m_dialogue.Count == 0) 
            {
                EndDialogue();
            }
            else if (m_displayingText != null) 
            {
                // Skip letter by letter display
                // when player clicks [next] during display coroutine
                StopCoroutine(m_displayingText);
                // Display entire page immediately
                m_dialogueDisplay.text = m_currentLine;
            }
            else 
            {
                // start lett-by-letter
                m_currentLine = m_dialogue.Dequeue();
                m_displayingText = StartCoroutine(OnShowDialogue(m_currentLine));
            }
        }

        IEnumerator OnShowDialogue(string dialogue) 
        {
            char[] letters = dialogue.ToCharArray();
            foreach(char letter in letters) 
            {
                // Show text letter-by-letter
                m_dialogueDisplay.text += letter;
                yield return null;
            }
            m_displayingText = null;
        }

        void EndDialogue()
        {
            // end
            Debug.Log("End of talk.");
            m_boxAnim.SetBool("BoxOn", false);
        }
    }
}