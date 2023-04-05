﻿using Curry.Game;
using UnityEngine;

namespace Curry.Explore
{
    public delegate void OnCharacterUpdate(ICharacter character);
    public delegate void OnHpUpdate(int deltaVal, int newHP);
    public delegate void OnMovementBlocked(Vector3 blockedWorldPos);
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
        event OnMovementBlocked OnBlocked;
        void Reveal();
        void Hide();
        void Recover(int val);
        void TakeHit(int hitVal);
        void OnDefeated();
        void Despawn();
        void ApplyModifier(IStatModifier<TacticalStats> mod);
        void Move(Vector3 target);
        // returns if warp was successful
        bool Warp(Vector3 to);
    }
}
