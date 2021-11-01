using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Util;
using Curry.Game;

namespace Curry.Skill
{
    [RequireComponent(typeof(EdgeCollider2D))]
    [RequireComponent(typeof(LineRenderer))]
    public abstract class EdgeSkill : BaseDrawSkill
    {
        [SerializeField] protected EdgeCollider2D m_hitBox = default;
        [SerializeField] protected LineRenderer m_lineRenderer = default;
        protected override void PrepareDrawEffect(List<Vector2> verts)
        {
            m_hitBox.SetPoints(verts);
            Vector3[] pos = VectorExtension.ToVector3Array(verts.ToArray());
            m_lineRenderer.positionCount = pos.Length;
            m_lineRenderer.SetPositions(pos);
        }
    }
}
