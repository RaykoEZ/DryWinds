using System;
using System.Collections.Generic;
using Curry.Game;

namespace Curry.Events
{
    public delegate void OnObjectiveComplete(IObjective completed);
    public delegate void OnObjectiveFail(IObjective completed);
    public interface IObjective
    {
        string Title { get; }
        string Description { get; }

        event OnObjectiveComplete OnComplete;
        event OnObjectiveFail OnFail;
        void Init();
        void Shutdown();
    }
}
