using UnityEngine;
using Curry.Explore;
using Curry.Events;
namespace Curry.UI
{
    public class TacticalCameraManager : CameraManager 
    {
        [SerializeField] CurryGameEventListener m_onCamFollow = default;
        [SerializeField] CurryGameEventListener m_onCardPlay = default;

        void Awake()
        {
            m_onCamFollow?.Init();
            m_onCardPlay?.Init();
        }

        public void OnFollowPlayer(EventInfo info)
        {
            if (info is PositionInfo pos)
            {
                FocusCamera(pos.WorldPosition);
            }
            else if(info is PlayerInfo player)
            {
                FocusCamera(player.PlayerStats.WorldPosition);
            }
        }
    }
}
