using Curry.Game;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    public delegate void OnCharacterUpdate(ICharacter character);
    public delegate void OnHpUpdate(int deltaVal, int newHP);
    public interface ICharacter
    {
        event OnHpUpdate TakeDamage;
        event OnHpUpdate RecoverHp;
        event OnCharacterUpdate OnDefeat;
        event OnCharacterUpdate OnReveal;
        event OnCharacterUpdate OnHide;
        string Name { get; }
        int MaxHp { get; }
        int CurrentHp { get; }
        int MoveRange { get; }
        int Speed { get; }
        Vector3 WorldPosition { get; }
        ObjectVisibility Visibility { get; }
        IReadOnlyList<AbilityContent> AbilityDetails { get; }
        Transform GetTransform();
        void Reveal();
        void Hide();
        void Recover(int val);
        void TakeHit(int hitVal);
        void OnDefeated();
        void Despawn();
        // returns if warp was successful
        bool Warp(Vector3 to);
    }
}
