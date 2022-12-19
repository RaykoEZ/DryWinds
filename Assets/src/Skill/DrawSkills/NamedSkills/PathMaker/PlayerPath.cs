using System.Collections;
using UnityEngine;
using Curry.Util;

namespace Curry.Skill
{
    public class PlayerPath : MonoBehaviour, ISkillObject<LineInput>
    {
        [SerializeField] Animator m_anim = default;
        [SerializeField] LineRenderer m_line = default;
        public GameObject go { get { return gameObject; } }
        public void Begin(LineInput param)
        {
            if (param != null && param.Vertices.Count > 1)
            {
                GameUtil.RenderLine(param.Vertices, m_line);
                StartCoroutine(PreparePath(param));
            }
        }

        IEnumerator PreparePath(LineInput param) 
        {
            m_anim.SetTrigger("Start");
            yield return null;
        }

        public void End()
        {
            //for now
            Destroy(this);
        }
    }
}