using UnityEngine;

namespace Curry.Explore
{
    public class AdventSkaters : AdventCard
    {
        [SerializeField] GameObject m_spawnReference = default;
        protected void Start()
        {
            Activatable = true;
        }

        protected override void ActivateEffect(Explorer user)
        {
            Debug.Log("Skate activate: "+ user.name);
            OnExpend();
        }
    }
}
