using UnityEngine;
using System.Collections.Generic;

namespace Curry.Skill
{
    [RequireComponent(typeof(EdgeCollider2D))]
    public abstract class EdgeSkill : BaseDrawSkill
    {
        [SerializeField] protected EdgeCollider2D m_hitBox = default;

        protected override void UpdateHitBox(List<Vector2> verts)
        {
            m_hitBox.SetPoints(verts);
        }
    }
}
