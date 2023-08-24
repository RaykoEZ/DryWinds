
namespace Curry.Game
{
    public interface IPoolable
    {
        IObjectPool Origin { get; set; }
        void Prepare();
        void ReturnToPool();
    }
    public interface IObjectPool
    {
        void Reclaim(object obj);
    }

    public interface IObjectPool<T> : IObjectPool where T : IPoolable
    {
        void ReclaimInstance(T obj);
    }
}
