using System.Collections.Generic;

namespace Curry.Events
{
    public class ObjectiveContainer 
    {
        protected List<IObjective> m_objectives = new List<IObjective>();
        public IReadOnlyList<IObjective> Objectives { get { return m_objectives; } }

        public void Add(IObjective objective)
        {
            if(objective == null) 
            {
                return;
            }
            m_objectives.Add(objective);
        }

        public void Add(List<IObjective> objectives)
        {
            if (objectives == null)
            {
                return;
            }
            m_objectives.AddRange(objectives);
        }

        public bool Remove(IObjective objective) 
        {
            return m_objectives.Remove(objective);
        }
    }
}
