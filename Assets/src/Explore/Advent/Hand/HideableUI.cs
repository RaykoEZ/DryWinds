using UnityEngine;

namespace Curry.Explore
{
    [RequireComponent(typeof(Animator))]
    public class HideableUI : MonoBehaviour 
    {
        public virtual void Show() 
        {
            GetComponent<Animator>()?.SetBool("Show", true);
        }
        public virtual void Hide() 
        {
            GetComponent<Animator>()?.SetBool("Show", false);
        }
    }
}