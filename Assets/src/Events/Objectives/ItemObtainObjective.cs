using System;
using UnityEngine;
using Curry.Game;
using Curry.UI;
using System.Collections.Generic;

namespace Curry.Events
{
    public class ItemObtainObjective : GameObjective
    {
        [SerializeField] protected ItemObtained m_condition = default;
        [SerializeField] protected DialogueTrigger m_dialogueTrigger = default;
        [SerializeField] protected CurryGameEventListener m_onItemObtain = default;
        [TextArea]
        [SerializeField] protected List<string> m_dialogue = default;

        public override string Title { get { return "Get Title."; } }

        public override string Description { get { return "Get Description."; } }


        public override void Init()
        {
            m_onItemObtain?.Init();
        }

        public override void Shutdown()
        {
            m_onItemObtain?.Shutdown();
        }

        public virtual void OnItemObtain(EventInfo arg) 
        {
            if (arg == null) return;

            if(arg is ItemGain gain && m_condition.UpdateProgress(gain)) 
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
