using System.Collections;
using UnityEngine;
using Curry.Game;
using Curry.Util;

namespace Curry.Skill
{
    public interface ISummonableObject<T> where T : IActionInput 
    {
        GameObject Self { get; }
        void OnSummon(T param);
    }

    [RequireComponent(typeof(EdgeCollider2D))]
    [RequireComponent(typeof(LineRenderer))]
    public class FragileBarrier : FragileObject, ITimeLimit, ISummonableObject<RegionInput>
    {
        [SerializeField] protected LineRenderer m_lineRenderer = default;
        [SerializeField] Animator m_anim = default;
        public float Duration { get; protected set; }
        public float TimeElapsed { get; protected set; }
        public virtual GameObject Self { get { return gameObject; } }
        public virtual EdgeCollider2D HitBox { get { return GetComponent<EdgeCollider2D>(); } }
        public virtual LineRenderer LineRenderer { get { return m_lineRenderer; } }

        public void OnSummon(RegionInput param) 
        {
            if(param != null && param.Vertices.Count > 2) 
            {
                Duration = (float)param.Payload["duration"];
                HitBox.SetPoints(param.Vertices);
                Vector3[] pos = VectorExtension.ToVector3Array(param.Vertices.ToArray());
                m_lineRenderer.positionCount = pos.Length;
                m_lineRenderer.SetPositions(pos);
                transform.parent = null;
                m_anim.SetTrigger("Start");
                StartCoroutine(Countdown());
            }
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
            HitBox.points = new Vector2[] { };
            m_lineRenderer.positionCount = 0;
            m_lineRenderer.SetPositions(new Vector3[] { });
            base.OnDefeat();
        }
    }
}
