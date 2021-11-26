using UnityEngine;

namespace Curry.Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Flora : BaseItem, IFlammable, IFragile
    {
        protected override void OnCollect(Player player)
        {
            base.OnCollect(player);
        }

        public void OnTouchFire(IElementSource source)
        {
            Debug.Log("on fire");
        }

        public override void OnTakeDamage(float damage)
        {
            Debug.Log("Flora is damaged");
        }

        protected override bool OnActivate(BaseCharacter hit)
        {
            Debug.Log($"{Property.Name} used");
            return true;
        }
    }
}
