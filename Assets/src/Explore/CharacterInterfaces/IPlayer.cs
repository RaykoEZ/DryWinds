namespace Curry.Explore
{
    public delegate void OnTakeDamage(int damageVal, int newHP);
    public interface IPlayer : ICharacter
    {
        event OnTakeDamage TakeDamage;
        AdventurerStats StartingStats { get; }
        AdventurerStats CurrentStats { get; }

    }

}
