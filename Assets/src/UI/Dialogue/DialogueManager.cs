using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Curry.Game;
using TMPro;

namespace Curry.UI
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI m_nameDisplay = default;
        [SerializeField] TextMeshProUGUI m_dialogueDisplay = default;
        [SerializeField] Animator m_boxAnim = default;
        Queue<string> m_dialogue;
        string m_currentLine;
        Coroutine m_displayingText;
        
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

        public void NextPage() 
        { 
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
        }

        void EndDialogue()
        {
            // end
            Debug.Log("End of talk.");
            m_boxAnim.SetBool("BoxOn", false);
        }
    }
}
