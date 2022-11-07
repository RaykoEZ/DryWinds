namespace Curry.Explore
{
    public class TurnEnd : Phase
    {
        public override void Init()
        {
            NextState = typeof(TurnStart);
        }

        protected override void Evaluate()
        {
            throw new System.NotImplementedException();
        }
    }

}