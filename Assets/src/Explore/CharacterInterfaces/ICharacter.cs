using UnityEngine;

namespace Curry.Explore
{
    public delegate void OnMovementBlocked(Vector2 blockedWorldPos);
    public interface ICharacter 
    {
        string Name { get; }
        int MaxHp { get; }
        int CurrentHp { get; }
        int MoveRange { get; }
        Vector3 WorldPosition { get; }
        ObjectVisibility Visibility { get; }
        event OnMovementBlocked OnBlocked;
        void Reveal();
        void Hide();
        void Recover(int val);
        void TakeHit(int hitVal);
        void OnDefeated();
        void OnMovementBlocked(ICharacter blocking);
        void Move(Vector3 target);
        // returns if warp was successful
        bool Warp(Vector3 to);
    }
}
