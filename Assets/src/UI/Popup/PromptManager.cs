using System;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.UI
{
    public enum EPromptType 
    { 
        Interact,
        Collect
    }

    public class PromptManager : MonoBehaviour
    {
        [SerializeField] Sprite m_default = default;
        [SerializeField] Sprite m_collect = default;
        [SerializeField] InteractPrompt m_initPrompt = default;

        Queue<InteractPrompt> m_availablePrompts = new Queue<InteractPrompt>();
        List<InteractPrompt> m_promptsInUse = new List<InteractPrompt>();

        void Awake()
        {
            m_availablePrompts.Enqueue(m_initPrompt);
        }

        public InteractPrompt GetPrompt(Action onClick, string title, EPromptType type = EPromptType.Interact) 
        {
            Sprite sprite;
            switch (type)
            {
                case EPromptType.Collect:
                    sprite = m_collect;
                    break;
                default:
                    sprite = m_default;
                    break;
            }
            InteractPrompt prompt = MakePrompt(onClick, title, sprite);
            return prompt;
        }

        protected InteractPrompt MakePrompt(Action onClick, string title, Sprite sprite) 
        {
            InteractPrompt prompt = m_availablePrompts.Count == 0 ?
                    Instantiate(m_initPrompt, transform) : m_availablePrompts.Dequeue();
            prompt.Prepare(onClick, title, sprite);
            prompt.OnHide += OnPromptHide;
            m_promptsInUse.Add(prompt);
            prompt.gameObject.SetActive(false);
            return prompt;
        }

        protected void OnPromptHide(InteractPrompt prompt) 
        {
            prompt.OnHide -= OnPromptHide;
            prompt.gameObject.SetActive(false);
            m_availablePrompts.Enqueue(prompt);
            m_promptsInUse.Remove(prompt);
        }
    }
}
