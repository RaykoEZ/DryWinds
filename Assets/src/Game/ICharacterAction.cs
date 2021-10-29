namespace Curry.Game
{
    public delegate void OnActionFinish<T>(ICharacterAction<T> action) where T : IActionInput;
    public interface ICharacterAction<T> where T : IActionInput
    {
        event OnActionFinish<T> OnFinish;
        bool IsUsable { get; }
        bool ActionInProgress { get; }
        ActionProperty Properties { get; }
        void Execute(T param);
        void Interrupt();
    }
}
