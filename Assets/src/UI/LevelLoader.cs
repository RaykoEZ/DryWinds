using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.src.UI
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] Animator m_transition = default;
        AsyncOperation m_loadSceneOp;
        public void LoadScene(int sceneIndex) 
        {
            StartCoroutine(LoadLevel_Internal(sceneIndex));
        }
        IEnumerator LoadLevel_Internal(int sceneIndex) 
        {
            // start scene loading
            m_loadSceneOp = SceneManager.LoadSceneAsync(sceneIndex);
            m_loadSceneOp.allowSceneActivation = false;
            // play transition animaton
            m_transition?.SetTrigger("transition");
            yield return new WaitForSeconds(1f);
            // transition to new scene when loading and animation are done
            yield return new WaitUntil(() => m_loadSceneOp.progress >= 0.9f);
            m_loadSceneOp.allowSceneActivation = true;
        }
    }
}