namespace Curry.Game
{
    public delegate void OnActionFinish<T>(ICharacterAction<T> action) where T : IActionInput;
    public interface ICharacterAction<T> where T : IActionInput
    {
        event OnActionFinish<T> OnFinish;
        bool IsUsable { get; }
        bool OnCooldown { get; }
        ActionProperty Properties { get; }
        void OnEnter(T param);
        void Interrupt();
    }
}
