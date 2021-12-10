using System;
using UnityEngine;
using Curry.Game;
using Curry.UI;

namespace Curry.Events
{
    public class ItemObtainObjective : GameObjective
    {
        [SerializeField] protected ItemObtained m_condition = default;
        [SerializeField] protected DialogueTrigger m_dialogueTrigger = default;
        [SerializeField] protected Dialogue m_dialogue = default;

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

        protected override void OnCompleteCallback()
        {
            base.OnCompleteCallback();
            m_dialogueTrigger.TriggerDialogue(m_dialogue, true);
        }
    }

}
