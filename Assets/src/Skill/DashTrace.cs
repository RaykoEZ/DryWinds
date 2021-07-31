using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Skill
{
    public class DashTrace : BaseTrace
    {
        protected virtual void Start() 
        {
            // Our dash does not do physics
            EdgeCollider.isTrigger = true;
        }

        public virtual void OnDraw(Vector2 origin, Vector2 targetPos) 
        {
            base.OnDraw(origin, 0f);
            float length = Vector2.Distance(targetPos, origin);
            base.OnDraw(targetPos, length);
        }

        protected override void OnClear()
        {
            m_isDecaying = false;
            m_decayTimer = 0f;
            LineRenderer.positionCount = 0;
            m_drawnVert.Clear();
            m_drawnPositions.Clear();
            m_segmentLengths.Clear();

            if (m_isMakingCollider)
            {
                EdgeCollider.Reset();
            }
        }

    }
}
