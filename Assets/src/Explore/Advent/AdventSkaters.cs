using UnityEngine;

namespace Curry.Explore
{
    public class AdventSkaters : AdventCard
    {
        protected void Start()
        {
            Activatable = true;
        }
        public override void Activate()
        {
            Debug.Log("Skate activate");
            Activatable = false;
            OnExpend();
        }
    }
}
