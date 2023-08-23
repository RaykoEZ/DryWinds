using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.VFX;
using Curry.Vfx;
using System;

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
        event OnCharacterUpdate OnMoveFinished;
        event OnMovementBlocked OnBlocked;
        string Name { get; }
        int MaxHp { get; }
        int CurrentHp { get; set; }
        int MoveRange { get; }
        int Speed { get; }
        Vector3 WorldPosition { get; }
        IReadOnlyList<AbilityContent> AbilityDetails { get; }
        Transform GetTransform();
        void Reveal();
        void Hide();
        void Recover(int val);
        void TakeHit(int hitVal);
        VfxManager.VfxHandle AddVfx(VisualEffectAsset vfx, TimelineAsset timeline);
        IEnumerator OnDefeated();
        void Despawn();
        // returns if warp was successful
        bool Warp(Vector3 to);
        void Move(Vector3 destination);
    }
}
