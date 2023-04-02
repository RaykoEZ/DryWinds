using System.Collections;

namespace Curry.Explore
{
    // For cards that goes away after used once/number of uses
    public interface IConsumable
    { 
        IEnumerator OnExpend();
    }
    // For cards that have a time countdown before becoming usable again
    public interface ICooldown
    { 
        bool IsOnCooldown { get; }
        int CooldownTime { get; set; }
        int Current { get; set; }
        void TrggerCooldown();
        void Tick(int dt, out bool isOnCoolDown);
    }
}
