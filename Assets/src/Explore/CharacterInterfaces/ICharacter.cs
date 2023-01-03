using UnityEngine;

namespace Curry.Explore
{
    public interface ICharacter 
    {
        void Reveal();
        void Hide();
        void Recover(int val);
        void TakeHit(int hitVal);
        void Move(Vector2Int direction);
    }
}
