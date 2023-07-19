using TMPro;
using UnityEngine;

namespace Curry.Explore
{
    [RequireComponent(typeof(Animator))]
    public class CooldownAnimationHandler : MonoBehaviour 
    {
        [SerializeField] protected Animator m_anim = default;
        [SerializeField] protected TextMeshProUGUI m_countsownDisplay = default;
        public void OnCooldown(int currentCountdown, bool isOnCooldown = true) 
        {
            m_countsownDisplay.text = currentCountdown.ToString();
            m_anim.SetBool("cd", isOnCooldown);
        }
    }
}
