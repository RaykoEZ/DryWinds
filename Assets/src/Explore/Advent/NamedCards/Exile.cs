using UnityEngine;
namespace Curry.Explore
{
    public class Exile : Assault 
    {
        protected override void ActivateEffect(IPlayer user)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(Target - 5f * Vector3.forward, Vector3.forward, m_targetLayer);
            Vector2 userPos = user.CurrentStats.WorldPosition;
            foreach (RaycastHit2D hit in hits)
            {
                if (hit && hit.transform.TryGetComponent(out IEnemy enemy))
                {                
                    Vector2 diff = userPos - hit.rigidbody.position;
                    Vector2Int push = new Vector2Int((int)-diff.x, (int)-diff.y);
                    enemy.Move(push);
                    break;
                }
            }
            OnExpend();
        }
    }
}
