using System;

namespace Curry.Events
{
    public interface IProgress<T> where T: IComparable 
    { 
        T Target { get; }
        T Progress { get; }
        bool Achieved { get; }
        bool UpdateProgress(T progress);
    }
}
