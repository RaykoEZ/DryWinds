using System.Collections.Generic;

namespace Curry.Game
{
    public delegate void OnActionFinish();
    public interface ICharacterAction<T> where T : IActionParam
    {
        event OnActionFinish OnFinish;
        bool ActionInProgress { get; }
        void Execute(T param);
        void Interrupt();
    }

    public interface IActionParam 
    { 
        Dictionary<string, object> Payload { get; }
    }
}
