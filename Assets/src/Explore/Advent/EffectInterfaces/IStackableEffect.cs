namespace Curry.Explore
{
    public interface IStackableEffect 
    {
        void AddStack(int addVal = 1);
        void SubtractStack(int subVal = 1);
        void ResetStack();
    }
}
