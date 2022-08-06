using UnityEngine;
using Curry.Game;

namespace Curry.Explore
{
    public class Sandstorm : RadialCrisis
    {
        public Sandstorm(float life, float intensity, float startRadius, float maxRadius, float growthRate) : 
            base(life, intensity, startRadius, maxRadius, growthRate)
        {
        }

        public override void OnEnterArea(Interactable col)
        {
            Debug.Log("Entering Crisis: " + col.gameObject);
        }

        public override void OnExitArea(Interactable col)
        {
            Debug.Log("Exiting Crisis: " + col.gameObject);
        }

        public override void OnCrisisUpdate(float dt)
        {
        }
    }
}
