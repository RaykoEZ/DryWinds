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

        protected abstract void OnEnter();
        protected abstract void OnExit();
        protected abstract void OnCrisisUpdate(float dt);

    }
}
