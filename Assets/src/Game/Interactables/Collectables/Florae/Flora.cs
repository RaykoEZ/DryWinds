using UnityEngine;

namespace Curry.Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Flora : BaseItem, IFlammable
    {
        protected override void OnCollect(Player player)
        {
            base.OnCollect(player);
        }

        public void OnTouchFire(IElementSource source)
        {
            Debug.Log("on fire");
        }

        protected override void OnTakeDamage(float damage, int partDamage = 0)
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
