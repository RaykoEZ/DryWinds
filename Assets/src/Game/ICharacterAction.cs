using System.Collections.Generic;

namespace Curry.Game
{
    public delegate void OnActionFinish<T, T1>(ICharacterAction<T, T1> action) where T : IActionInput where T1 : IActionProperty;
    public interface ICharacterAction<T, T1> where T : IActionInput where T1 : IActionProperty
    {
        event OnActionFinish<T, T1> OnFinish;
        bool IsUsable { get; }
        bool ActionInProgress { get; }
        T1 Properties { get; }
        void Windup();
        void Execute(T param);
        void Interrupt();
    }

    public interface IActionInput 
    { 
        Dictionary<string, object> Payload { get; }
    }
}
