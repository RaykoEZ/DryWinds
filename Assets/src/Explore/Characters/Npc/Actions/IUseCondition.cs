namespace Curry.Explore
{
    public interface IUseCondition
    { 
        bool Usable { get; }
        void Try(object arg, out bool used);
    }
    public interface ILimitedUse : IUseCondition
    {
        int UsesLeft { get; }
        void Refresh();
    }
}
