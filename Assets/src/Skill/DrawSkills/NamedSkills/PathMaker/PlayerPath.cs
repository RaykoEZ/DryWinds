using System.Collections;
using UnityEngine;
using Curry.Util;
using Curry.Events;

namespace Curry.Skill
{
    public class PlayerPath : MonoBehaviour, ISkillObject<LineInput>
    {
        [SerializeField] Animator m_anim = default;
        [SerializeField] EdgeCollider2D m_collider = default;
        [SerializeField] LineRenderer m_line = default;
        [SerializeField] CurryGameEventSource m_onMakePath = default;
        public GameObject go { get { return gameObject; } }
        public void Begin(LineInput param)
        {
            if (param != null && param.Vertices.Count > 2)
            {
                GameUtil.RenderLine(param.Vertices, m_line, m_collider);
                StartCoroutine(PreparePath(param));
            }
        }

        IEnumerator PreparePath(LineInput param) 
        {
            m_anim.SetTrigger("Start");
            yield return new WaitUntil(() => 
            { return m_anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f; });
            EventInfo info = new EventInfo(param.Payload);
            info.Payload["path"] = this;
            m_onMakePath?.Broadcast(info);
        }

        public void End()
        {
            //for now
            Destroy(this);
        }
    }
}