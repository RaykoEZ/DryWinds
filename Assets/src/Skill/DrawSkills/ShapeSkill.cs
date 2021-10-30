using System;
using System.Collections;
using UnityEngine;
using Curry.Game;
using System.Collections.Generic;

namespace Curry.Skill
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public abstract class ShapeSkill : BaseDrawSkill
    {
        [SerializeField] protected PolygonCollider2D m_hitBox = default;

        protected override void UpdateHitBox(List<Vector2> verts)
        {
            m_hitBox.SetPath(0, verts);
        }
    }

}
