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
        [SerializeField] protected EdgeCollider2D m_hitBox = default;
        [SerializeField] protected LineRenderer m_lineRenderer = default;

        public float Duration { get; protected set; }
        public float TimeElapsed { get; protected set; }
        public virtual GameObject Self { get { return gameObject; } }
        public virtual EdgeCollider2D HitBox { get { return m_hitBox; } }
        public virtual LineRenderer LineRenderer { get { return m_lineRenderer; } }

        public void OnSummon(RegionInput param) 
        {
            if(param != null && param.Vertices.Count > 2) 
            {
                Duration = (float)param.Payload["duration"];
                m_hitBox.SetPoints(param.Vertices);
                Vector3[] pos = VectorExtension.ToVector3Array(param.Vertices.ToArray());
                m_lineRenderer.positionCount = pos.Length;
                m_lineRenderer.SetPositions(pos);

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
            OnDefeat();
        }

        public override void OnDefeat(bool animate = false)
        {
            TimeElapsed = 0f;
            Duration = 0f;
            HitBox.points = new Vector2[] { };
            m_lineRenderer.positionCount = 0;
            m_lineRenderer.SetPositions(new Vector3[] { });
            base.OnDefeat(animate);
        }
    }
}
