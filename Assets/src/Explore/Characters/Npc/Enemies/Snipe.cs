using System.Collections;

namespace Curry.Explore
{
    public class Snipe : BaseAbility, IEnemyReaction
    {
        public bool CanReact(IEnemy user)
        {
            return user.SpotsTarget;
        }
        public IEnumerator OnPlayerAction(IEnemy enemy)
        {
            yield return (enemy as IRangedUnit)?.FireWeapon();
        }
    }
}
