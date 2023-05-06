﻿using Curry.Game;
using Curry.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public struct AbilityContent
    {
        [SerializeField] public string Name;
        [SerializeField] public string Description;
        // This sprite will be a grid pattern
        [SerializeField] public RangeMap RangePattern;
        [SerializeField] public Sprite Icon;
    }
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
