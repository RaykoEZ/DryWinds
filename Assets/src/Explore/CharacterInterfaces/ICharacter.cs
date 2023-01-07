using UnityEngine;

namespace Curry.Explore
{
    public delegate void OnMovementBlocked(Vector2 blockedWorldPos);
    public interface ICharacter 
    {
        event OnMovementBlocked OnBlocked;
        void Reveal();
        void Hide();
        void Recover(int val);
        void TakeHit(int hitVal);
        void OnDefeated();
        void Move(Vector2Int direction);
    }
}
