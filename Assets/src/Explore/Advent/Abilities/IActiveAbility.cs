using System.Collections.Generic;

namespace Curry.Explore
{
    public interface IActiveAbility
    {
        int BaseDamage { get; }
        IReadOnlyList<IStatusAilment> OnHitEffects { get; }
        void AddOnHitEffect(IStatusAilment mod);
        void RemoveOnHitEffect(IStatusAilment mod);
    }
}
