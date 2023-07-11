using System;
using System.Collections.Generic;
using UnityEngine;
namespace Curry.Explore
{
    public partial class EnemyManager
    {
        protected class EnemyContainer
        {
            #region Enemy Comparer class
            // Comparer<TacticalEnemy> for priority sorting 
            protected class EnemyPriorityComparer : IComparer<IEnemy>
            {
                public int Compare(IEnemy x, IEnemy y)
                {
                    if (x.Id == y.Id)
                    {
                        return 0;
                    }
                    if (x == null)
                    {
                        if (y == null)
                        {
                            // If x is null and y is null, they're
                            // equal.
                            return 0;
                        }
                        else
                        {
                            // If x is null and y is not null, y
                            // is greater.
                            return -1;
                        }
                    }
                    else
                    {
                        // If x is not null...
                        //
                        if (y == null)
                        // ...and y is null, x is greater.
                        {
                            return 1;
                        }
                        else
                        {
                            // ...and y is not null, return countdown difference
                            return x.Speed - y.Speed;
                        }
                    }
                }
            }
            #endregion
            List<IEnemy> m_activeEnemies = new List<IEnemy>();
            HashSet<IEnemy> m_toRemove = new HashSet<IEnemy>();
            HashSet<IEnemy> m_toAdd = new HashSet<IEnemy>();
            EnemyPriorityComparer m_priorityComparer = new EnemyPriorityComparer();
            public IReadOnlyList<IEnemy> ActiveEnemies { get => m_activeEnemies; }

            // add and remove scheduled updates to the enemy list
            public void UpdateActivity()
            {
                foreach (IEnemy e in m_toAdd)
                {
                    m_activeEnemies.Add(e);
                }
                foreach (IEnemy e in m_toRemove)
                {
                    m_activeEnemies.Remove(e);
                }
                m_toAdd.Clear();
                m_toRemove.Clear();
            }
            public void SortActiveEnemyPriorities()
            {
                m_activeEnemies.Sort(m_priorityComparer);
            }
            public void ScheduleAdd(IEnemy add) 
            {
                m_toAdd.Add(add);
            }
            public void ScheduleRemove(IEnemy remove) 
            {
                m_toRemove.Add(remove);
            }
        }
    }
}