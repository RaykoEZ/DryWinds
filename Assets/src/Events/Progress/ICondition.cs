using System;

namespace Curry.Events
{
    public interface ICondition<T> where T : IComparable
    {
        string Description { get; }
        T Target { get; }
        T Progress { get; }
        bool Achieved { get; }
        bool UpdateProgress(T progress);
    }
}
