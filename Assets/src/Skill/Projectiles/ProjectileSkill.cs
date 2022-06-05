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
        [SerializeField] public float Recoil;
        // time (sec) for a projectile to weaken its power
        [SerializeField] public float DecayTimeInterval;
        // time to wait to shoot another projectile
        [SerializeField] public float RepeatTimeInterval;
        [SerializeField][Range(0f, 1f)] public float DecayCoeff;
        [SerializeField][Range(1, 69)] public int NumShotsToFire;

    }

    public abstract class ProjectileSkill : BaseSkill, IProjectileSkill<VectorInput>
    {
        [SerializeField] protected ProjectileSetting m_projectileSetting = default;
        [SerializeField] protected PrefabLoader m_assetLoader = default;
        public ISkillObject<VectorInput> Projectile { get; protected set; }
        protected List<Projectile> m_projectiles = new List<Projectile>();
        public override void Init(BaseCharacter user)
        {
            base.Init(user);
            m_assetLoader.OnLoadSuccess += (obj) =>
            {
                Projectile = obj.GetComponent<Projectile>();
                // Prepare projectile asset for shooting
                for (int i = 0; i < m_projectileSetting.NumShotsToFire; ++i)
                {
                    Projectile projectile =
                        m_instanceManager.GetInstanceFromAsset(Projectile.Self) as Projectile;

                    m_projectiles.Add(projectile);
                }
            };
            m_assetLoader.LoadAsset();

        }

        public virtual void Shoot(VectorInput param)
        {
            StartCoroutine(ShootInternal(param));
        }

        protected virtual IEnumerator ShootInternal(VectorInput param)
        {
            foreach(Projectile p in m_projectiles) 
            {
                // Shoot a projectile and wait
                p.Begin(param);
                yield return new WaitForSeconds(m_projectileSetting.RepeatTimeInterval);
            }
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