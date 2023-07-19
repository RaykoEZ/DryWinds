using System.Collections;
using UnityEngine;

namespace Curry.UI
{
    public class GameIntroduction : MonoBehaviour
    {
        [SerializeField] Animator m_anim = default;
        bool m_isReady = false;
        public bool IsReady => m_isReady;
        public void GameStartIntro() 
        {
            StartCoroutine(SetupIntro_Internal("startGame"));
        }
        public void GameEnd() 
        {
            StartCoroutine(SetupIntro_Internal("finishGame"));
        }
        IEnumerator SetupIntro_Internal(string animState) 
        {
            m_isReady = false;
            m_anim?.SetBool(animState, true);
            yield return new WaitWhile(()=>m_anim.GetBool(animState));
            yield return new WaitForEndOfFrame();
            m_isReady = true;
        }
    }
}