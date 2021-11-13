using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace Curry.UI
{
    public delegate void OnPromptHide(InteractPrompt prompt);
    public class InteractPrompt : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI m_title = default;
        [SerializeField] Button m_button = default;
        [SerializeField] Image m_icon = default;
        public event OnPromptHide OnHide;
        public virtual void Prepare(Action onClick, string title, Sprite sprite) 
        {
            m_title.text = title;
            UnityAction onComplete = () =>
            {
                m_button.onClick.RemoveAllListeners();
                onClick.Invoke();
                Hide();
            };
            m_button.onClick.AddListener(onComplete);

            m_icon.sprite = sprite;
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide() 
        {
            OnHide?.Invoke(this);
        }
    }
}
