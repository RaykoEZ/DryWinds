using UnityEngine;

namespace Curry.Explore
{
    public class AdventSkaters : AdventCard
    {
        [SerializeField] GameObject m_spawnReference = default;

        private void Awake()
        {
            Activatable = true;
        }

        protected override void ActivateEffect(Adventurer user)
        {
            Activatable = false;
            Debug.Log("Skate activate: "+ user.name);
            OnExpend();
        }

        // On instance init
        public override void Prepare()
        {
            Activatable = true;
        }
    }
}
