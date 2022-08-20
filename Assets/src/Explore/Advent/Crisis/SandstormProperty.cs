using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Explore
{
    public class SandstormProperty : RadialCrisisProperty<BaseCharacter>
    {
        HashSet<BaseCharacter> m_inside = new HashSet<BaseCharacter>();
        public float Life { get; protected set; }
        public float Intensity { get; protected set; }
        public float StartRadius { get; protected set; }
        public float GrowthRate { get; protected set; }

        public SandstormProperty(float life, float intensity, 
            float startRadius, float growthRate)
        {
            Life = life;
            Intensity = intensity;
            StartRadius = startRadius;
            GrowthRate = growthRate;
        }

        public virtual void OnEnterArea(BaseCharacter col)
        {
            m_inside.Add(col);
            Debug.Log("Entering Crisis: " + col.gameObject);
        }

        public virtual void OnExitArea(BaseCharacter col)
        {
            m_inside.Remove(col);

            Debug.Log("Exiting Crisis: " + col.gameObject);
        }

        public virtual void OnCrisisUpdate(float dt)
        {
            Debug.Log("Update Crisis: " + dt);

        }
    }
}
