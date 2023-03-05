using UnityEngine;

namespace Curry.Explore
{
    [RequireComponent(typeof(Animator))]
    public class CooldownAnimationHandler : MonoBehaviour 
    { 
        public void OnCooldown(bool isOnCooldown = true) 
        {
            GetComponent<Animator>()?.SetBool("cooldown", isOnCooldown);
        }
    }
}
