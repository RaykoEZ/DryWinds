using UnityEngine;

namespace Curry.Explore
{
    public class ObstacleAlertHandler : MonoBehaviour 
    {
        [SerializeField] Animator m_alertTileAnim = default;
        [SerializeField] AudioSource m_alertSound = default;
        public void AlertObstacle(Vector3 obstacleWorldPos, bool alertPlayer)
        {
            if (!alertPlayer)
            {
                return;
            }
            m_alertTileAnim.transform.position = obstacleWorldPos;
            m_alertTileAnim?.ResetTrigger("alertNow");
            m_alertTileAnim?.SetTrigger("alertNow");
            m_alertSound?.Play();
        }
    }
}
