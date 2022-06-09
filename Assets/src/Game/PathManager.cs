using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Curry.Events;
using Curry.Skill;
namespace Curry.Game
{
    public class PathManager : MonoBehaviour
    {
        [SerializeField] NavMeshSurface m_navMesh = default;
        [SerializeField] CurryGameEventListener m_onPathMake = default;
        Stack<PlayerPath> m_paths = new Stack<PlayerPath>();

        private void Awake()
        {
            m_onPathMake.Init();
            m_navMesh.BuildNavMeshAsync();
        }

        public void OnMakePath(EventInfo info) 
        {
            if (info.Payload == null || !info.Payload.ContainsKey("path")) 
            {
                return;
            }
            else if(info.Payload["path"] is PlayerPath path)
            {
                m_paths.Push(path);
                StartCoroutine(MakePath());            
            }
        }

        IEnumerator MakePath() 
        {
            AsyncOperation op = m_navMesh.BuildNavMeshAsync();
            yield return new WaitUntil(() => { return op.isDone; });
            Debug.Log("Navmesh done");
        }

        public void UndoPath() 
        {
            PlayerPath path = m_paths.Pop();
            path?.End();
        }
    }
}