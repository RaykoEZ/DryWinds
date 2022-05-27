using System.Collections;
using UnityEngine;
using Curry.Game;

namespace Curry.Skill
{
    public interface ISkillObject<T> where T : IActionInput
    {
        GameObject Self { get; }
        void Begin(T param);
        void End();
    }

    #region Summoning
    public interface ISummonSkill<T> where T : IActionInput
    {
        ISkillObject<T> SummonObject { get; }
        void Summon(T param);
        void Unsummon();
    }
    #endregion


    #region Projectile

    public interface IProjectileSkill<T> where T : IActionInput 
    {
        ISkillObject<T> Projectile { get; }
        void Shoot(T param);
    }
    #endregion
}