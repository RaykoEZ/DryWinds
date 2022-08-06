namespace Curry.Explore
{
    public abstract class RadialCrisis : Crisis
    {
        public virtual float StartRadius { get; protected set; }
        public virtual float MaxRadius { get; protected set; }
        // radius growth in uunit per sec
        public virtual float GrowthRate { get; protected set; }

        public RadialCrisis(float life, float intensity, float startRadius, float maxRadius, float growthRate ) : base(life, intensity)
        {
            StartRadius = startRadius;
            MaxRadius = maxRadius;
            GrowthRate = growthRate;
        }

    }
}
