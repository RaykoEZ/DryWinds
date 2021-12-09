using System;
using System.Collections.Generic;
using Curry.Game;

namespace Curry.Events
{

    public delegate void OnObjectiveComplete();
    public interface IObjective
    {
        ICondition<IComparable> ObjectiveCondition { get; }
        event OnObjectiveComplete OnComplete;
        void Init(GameEventManager eventManager);
        void Shutdown(GameEventManager eventManager);
    }

}
