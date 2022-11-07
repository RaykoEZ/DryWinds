namespace Curry.Explore
{
    public class EnemyAction : Phase
    {
        public override void Init()
        {
            NextState = typeof(TurnEnd);
        }

        protected override void Evaluate()
        {
            throw new System.NotImplementedException();
        }
    }

}