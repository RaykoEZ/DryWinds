using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.src.UI
{
    public class LevelLoader : MonoBehaviour
    {
        AsyncOperation m_loadSceneOp;
        public void LoadScene(int sceneIndex) 
        {
            StartCoroutine(LoadLevel_Internal(sceneIndex));
        }
        IEnumerator LoadLevel_Internal(int sceneIndex) 
        {
            m_loadSceneOp = SceneManager.LoadSceneAsync(sceneIndex);
            yield return null;           
        }
    }
}