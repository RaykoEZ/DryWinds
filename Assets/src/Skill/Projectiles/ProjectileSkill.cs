using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Skill;
using Curry.Util;

namespace Curry.Game
{

    [Serializable]
    public struct ProjectileSetting 
    {
        [SerializeField] public float InitForce;
        [SerializeField] public float InitDamage;
        [SerializeField] public float InitKnockback;
        [SerializeField] public float Recoil;
        // time (sec) for a projectile to weaken its power
        [SerializeField] public float DecayTimeInterval;
        [SerializeField] [Range(0f, 1f)] public float DecayCoeff;
        [SerializeField][Range(1, 69)] public int NumShotsToFire;
        [SerializeField][Range(1, 69)] public int NumShotsPerSec;

    }

    public abstract class ProjectileSkill : BaseSkill, IProjectileSkill<VectorInput>
    {
        [SerializeField] protected ProjectileSetting m_setting = default;
        [SerializeField] protected PrefabLoader m_assetLoader = default;
        public ISkillObject<VectorInput> Projectile { get; protected set; }
        protected List<Projectile> m_projectiles;

        public override void Init(BaseCharacter user)
        {
            base.Init(user);
            m_assetLoader.OnLoadSuccess += (obj) =>
            {
                Projectile = obj.GetComponent<Projectile>();
            };
            m_assetLoader.LoadAsset();
        }


        public virtual void Shoot(VectorInput param)
        {
            throw new System.NotImplementedException();
        }

        protected override IEnumerator ExecuteInternal(IActionInput target)
        {
            if (target is VectorInput input)
            {
                Shoot(input);
            }
            // Start animation
            yield return null;
        }
    }
}