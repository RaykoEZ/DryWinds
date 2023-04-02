namespace Curry.Explore
{
    public delegate void OnPlayerUpdate(IPlayer player);
    public delegate void OnTakeDamage(int damageVal, int newHP);
    public interface IPlayer : ICharacter
    {
        event OnTakeDamage TakeDamage;
        event OnPlayerUpdate OnDefeat;
        event OnPlayerUpdate OnReveal;
        event OnPlayerUpdate OnHide;
    }

}
