using Curry.Explore;
using UnityEngine;

namespace Curry.UI
{
    [RequireComponent(typeof(HideableUI))]
    public class UIPage : MonoBehaviour 
    {
        protected HideableUI UIAnim => GetComponent<HideableUI>();
        protected virtual void Start() 
        {
            UIAnim?.Hide();
        }
        public virtual void Play()
        {
            UIAnim?.Show();
        }
        public virtual void Stop() 
        {
            UIAnim?.Hide();
        }
    }
}