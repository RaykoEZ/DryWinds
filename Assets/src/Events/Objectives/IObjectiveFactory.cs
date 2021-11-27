using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Events
{

    public delegate void OnObjectiveComplete();
    public interface IObjective
    {
        IProgress<IComparable> Progress { get; }
        event OnObjectiveComplete OnComplete;
        void Init(GameEventManager eventManager);
    }

    public interface ObjectiveProperty
    {
    }

    public class TestProperty : ObjectiveProperty 
    { 
    } 

    public interface IObjectiveFactory<T> where T : ObjectiveProperty
    {
        IObjective Objective(T property);
    }


    public class Test : IObjectiveFactory<TestProperty>
    {
        public IObjective Objective(TestProperty property)
        {
            throw new NotImplementedException();
        }
    }
}
