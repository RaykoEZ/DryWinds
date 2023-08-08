using Curry.Explore;
using UnityEngine;

namespace Curry.UI.Tutorial
{
    [RequireComponent(typeof(HideableUI))]
    public class PageIcon : MonoBehaviour 
    {
        [SerializeField] HideableUI m_anim = default;
        void Start()
        {
            m_anim?.Hide();
        }
        public void Highlight() 
        {
            m_anim?.Show();
        }
        public void StopHighlight() 
        {
            m_anim?.Hide();
        }
    }
}