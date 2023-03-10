using UnityEngine;
using Curry.Explore;
using Curry.Events;
namespace Curry.UI
{
    public class TacticalCameraManager : CameraManager 
    {
        [SerializeField] CurryGameEventListener m_onCamFollow = default;

        void Awake()
        {
            m_onCamFollow?.Init();
        }

        public void OnFollowPlayer(EventInfo info)
        {
            if (info is PositionInfo pos)
            {
                FocusCamera(pos.WorldPosition);
            }
            else if(info is Explore.CharacterInfo player)
            {
                FocusCamera(player.Character.WorldPosition);
            }
        }
    }
}
