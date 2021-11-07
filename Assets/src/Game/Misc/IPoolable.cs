
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
        void ReturnToPool(object obj);
    }

    public interface IObjectPool<T> : IObjectPool where T : IPoolable
    {
        void ReturnToPool(T obj);
    }
}
