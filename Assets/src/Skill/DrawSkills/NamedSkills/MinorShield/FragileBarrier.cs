using System.Collections;
using UnityEngine;
using Curry.Game;
using Curry.Util;

namespace Curry.Skill
{
    [RequireComponent(typeof(EdgeCollider2D))]
    [RequireComponent(typeof(LineRenderer))]
    public class FragileBarrier : FragileObject, ITimeLimit, ISkillObject<LineInput>
    {
        [SerializeField] protected LineRenderer m_lineRenderer = default;
        [SerializeField] protected EdgeCollider2D m_collider = default;
        [SerializeField] Animator m_anim = default;
        public float Duration { get; protected set; }
        public float TimeElapsed { get; protected set; }
        public virtual GameObject go { get { return gameObject; } }
        public virtual EdgeCollider2D HitBox { get { return m_collider; } }
        public virtual LineRenderer LineRenderer { get { return m_lineRenderer; } }

        public void Begin(LineInput param) 
        {
            if(param != null && param.Vertices.Count > 2) 
            {
                Duration = (float)param.Payload["duration"];
                GameUtil.RenderLine(param.Vertices, m_lineRenderer, m_collider);
                m_anim.SetTrigger("Start");
                StartCoroutine(Countdown());
            }
        }

        public virtual void End()
        {
            OnDefeat();
        }

        protected virtual IEnumerator Countdown()
        {
            while(TimeElapsed < Duration) 
            {
                TimeElapsed += Time.deltaTime;
                yield return null;
            }
            // Start defeat sequence
            Debug.Log("Barrier expired");
            m_anim.SetTrigger("End");
            yield return new WaitUntil(() => 
            { 
                return m_anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f; 
            });
            OnDefeat();
        }

        protected override void OnDefeat()
        {
            TimeElapsed = 0f;
            Duration = 0f;
            m_collider.points = new Vector2[] { };
            m_lineRenderer.positionCount = 0;
            m_lineRenderer.SetPositions(new Vector3[] { });
            base.OnDefeat();
        }
    }
}
