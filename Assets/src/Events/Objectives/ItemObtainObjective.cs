using System;
using UnityEngine;

namespace Curry.Events
{
    public class ItemObtainObjective : GameObjective
    {
        [SerializeField] protected ItemObtained m_condition = default;
        public override ICondition<IComparable> ObjectiveCondition 
        { 
            get { return m_condition as ICondition<IComparable>; } 
        }
        public override void Init(GameEventManager eventManager)
        {
            eventManager.OnItemObtained += OnItemObtain; 
        }

        public override void Shutdown(GameEventManager eventManager)
        {
            eventManager.OnItemObtained -= OnItemObtain;
        }

        protected void OnItemObtain(object sender, ItemArgs arg) 
        {
            ItemGain gain = new ItemGain { Amount = 1, ItemSerialNumber = 0 };
            if (ObjectiveCondition.UpdateProgress(gain)) 
            {
                OnCompleteCallback();
            }
        }
    }

}
