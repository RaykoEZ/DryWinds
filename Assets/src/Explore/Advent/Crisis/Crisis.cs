using UnityEngine;
using Curry.Game;
namespace Curry.Explore
{
    public abstract class Crisis
    {
        public virtual float Life { get; protected set; }
        public virtual float Intensity { get; protected set; }

        public Crisis(float life, float intensity) 
        {
            Life = life;
            Intensity = intensity;
        }

        public abstract void OnEnterArea(Interactable col);
        public abstract void OnExitArea(Interactable col);
        public abstract void OnCrisisUpdate(float dt);

    }
}
