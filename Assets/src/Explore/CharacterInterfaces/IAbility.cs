namespace Curry.Explore
{
    public interface IAbility 
    {
        AbilityContent GetContent();
    }
    public interface IDamageAbility
    {
        int Damage { get; }
        void AddDamage(int val);

    }
}
