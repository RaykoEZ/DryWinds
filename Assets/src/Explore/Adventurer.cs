using UnityEngine;

namespace Curry.Explore
{
    // A basic character for adventure mode
    public class Adventurer : MonoBehaviour 
    {
        [Range(1,3)]
        [SerializeField] int m_scoutRange = default;
        public int ScoutRange { get { return m_scoutRange; } }
    }
}
