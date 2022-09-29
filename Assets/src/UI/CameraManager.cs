using System.Collections;
using UnityEngine;
using Curry.Util;

namespace Curry.UI
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] Camera m_camera = default;
        [SerializeField] float m_deltaTime = 0.2f;
        [SerializeField] float m_zoomHalfHieght = 3.5f;
        [SerializeField] float m_defaultHalfHieght = 5.0f;
        [SerializeField] CoroutineManager m_coroutineManager = default;

        public void FocusCamera(Vector3 dest)
        {
            m_coroutineManager.ScheduleCoroutine(OnCameraFocus(dest, m_zoomHalfHieght), interruptNow: true);
        }

        public void MoveCamera(Vector3 dest)
        {
            m_coroutineManager.ScheduleCoroutine(OnCameraFocus(dest, m_defaultHalfHieght), interruptNow: true);
        }

        public void UnFocusCamera()
        {
            m_coroutineManager.ScheduleCoroutine(OnCameraFocus(Vector3.zero, m_defaultHalfHieght), interruptNow: true);
        }

        IEnumerator OnCameraFocus(Vector3 dest, float halfHeight)
        {
            float deltaT = 0.0f;
            while (deltaT < 1.0f)
            {
                Vector3 newPos = Vector3.Lerp(transform.position, dest, deltaT);
                newPos.z = transform.position.z;
                transform.position = newPos;
                m_camera.orthographicSize = Mathf.Lerp(m_camera.orthographicSize, halfHeight, deltaT);

                deltaT += m_deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(1);
        }
    }
}
