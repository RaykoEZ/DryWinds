using System.Collections;
using UnityEngine;
using Curry.Util;
namespace Curry.UI
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] Camera m_camera = default;
        [Range(0.001f, 5f)]
        [SerializeField] float m_animTime = 0.2f;
        [SerializeField] float m_zoomOrthographSize = 3.5f;
        [SerializeField] float m_defaultOrthographSize = 5.0f;
        [SerializeField] CoroutineManager m_coroutineManager = default;
        Vector3 m_currentFocalPoint = Vector3.zero;
        public float AnimationTime => m_animTime;
        public void FocusCamera(Vector3 dest)
        {
            m_currentFocalPoint = dest;
            m_coroutineManager.ScheduleCoroutine(OnCameraFocus(dest, m_zoomOrthographSize), interruptNow: true);
        }

        public void MoveCamera(Vector3 dest)
        {
            m_currentFocalPoint = dest;
            m_coroutineManager.ScheduleCoroutine(OnCameraFocus(dest, m_defaultOrthographSize), interruptNow: true);       
        }

        public void UnFocusCamera()
        {
            if (!Mathf.Approximately(m_camera.orthographicSize, m_defaultOrthographSize))
            {
                m_coroutineManager.ScheduleCoroutine(OnCameraFocus(m_currentFocalPoint, m_defaultOrthographSize), interruptNow: true);
            }
        }

        IEnumerator OnCameraFocus(Vector3 dest, float halfHeight)
        {
            float dt = 0.0f;
            while (dt <= 1.0f)
            {
                Vector3 newPos = Vector3.Lerp(transform.position, dest, dt/m_animTime);
                newPos.z = transform.position.z;
                transform.position = newPos;
                m_camera.orthographicSize = Mathf.Lerp(m_camera.orthographicSize, halfHeight, dt);
                dt += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
