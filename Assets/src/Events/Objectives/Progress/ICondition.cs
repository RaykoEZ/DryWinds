using System;

namespace Curry.Events
{
    public interface ICondition<T>
    {
        string Description { get; }
        T Target { get; }
        T Progress { get; }
        bool Achieved { get; }
        bool UpdateProgress(T progress);
    }
}
