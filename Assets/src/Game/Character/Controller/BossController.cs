using UnityEngine;

namespace Curry.Game
{
    public class BossController : NpcController
    {
        [SerializeField] BaseNpc m_boss = default;
        protected override BaseNpc Character { get { return m_boss; } }
    }
}
