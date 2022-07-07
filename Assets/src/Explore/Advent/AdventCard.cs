using UnityEngine;
using Curry.Game;

namespace Curry.Explore
{
    public abstract class AdventCard : Interactable
    {
        [SerializeField] protected int m_id = 0;
        [SerializeField] protected string m_name = default;
        [SerializeField] protected string m_description = default;
        public int Id { get { return m_id; } }
        public string Name { get { return m_name; } }
        public string Description { get { return m_description; } }
        public abstract void Activate();
    }
}
