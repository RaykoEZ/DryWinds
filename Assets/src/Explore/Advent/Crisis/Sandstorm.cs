namespace Curry.Explore
{
    public class Sandstorm : RadialCrisis
    {
        public Sandstorm(float life, float intensity, float startRadius, float maxRadius, float growthRate) : 
            base(life, intensity, startRadius, maxRadius, growthRate)
        {
        }

        protected override void OnEnter()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnExit()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnCrisisUpdate(float dt)
        {
            throw new System.NotImplementedException();
        }
    }
}
