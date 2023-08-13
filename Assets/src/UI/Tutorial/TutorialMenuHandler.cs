using Curry.Explore;
using Curry.UI.Tutorial;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.UI.Tutorial
{
    [RequireComponent(typeof(HideableUI))]
    public class TutorialMenuHandler : MonoBehaviour
    {
        [SerializeField] List<PageTopicManager> m_topics = default;
        PageTopicManager m_currentlyChosenTopic;
        void OnEnable()
        {
            foreach (var item in m_topics)
            {
                item.OnChosen += OnTopicChosen;
            }
        }
        void OnDisable()
        {
            foreach (var item in m_topics)
            {
                item.OnChosen -= OnTopicChosen;
            }
        }
        void OnTopicChosen(PageTopicManager chosen) 
        {
            m_currentlyChosenTopic = chosen;
        }
        public void Show() 
        {
            GetComponent<HideableUI>()?.Show();
        }
        public void Hide() 
        {
            m_currentlyChosenTopic?.EndDisplay();
            GetComponent<HideableUI>()?.Hide();
        }
    }
}