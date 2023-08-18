using Curry.Events;
using Curry.UI;
using System;
using System.Collections;
using UnityEngine;
namespace Curry.Explore
{
    public class ProvisionInfo : EventInfo 
    {
        protected ChoiceConditions m_conditions;
        public ChoiceConditions Conditions => m_conditions;
        public ProvisionInfo(ChoiceConditions cond) 
        {
            m_conditions = cond;
        }
    }
    [Serializable]
    public class Provision : Phase
    {
        [SerializeField] ChoiceConditions m_condition = default;
        [SerializeField] DeckManager m_deck = default;
        [SerializeField] CurryGameEventListener m_onProvision = default;
        protected override Type NextState { get; set; } = typeof(TurnStart);
        public override void Init()
        {
            m_onProvision?.Init();
            base.Init();
        }
        protected override IEnumerator Evaluate_Internal()
        {
            yield return StartCoroutine(Provision_Internal(m_condition));
        }
        protected IEnumerator Provision_Internal(ChoiceConditions conditions) 
        {
            StartInterrupt();
            yield return new WaitForEndOfFrame();
            m_deck.ChooseToAddFromInventory(
                conditions,
                cardPoolFilter: null,
                onChosen: TransitionTo);
            yield return null;
        }
        protected override void TransitionTo()
        {
            base.TransitionTo();
            EndInterrupt();
        }
        public void OnProvisionTrigger(EventInfo info) 
        {
            if (info is ProvisionInfo p) 
            {
                StartCoroutine(Provision_Internal(p.Conditions));
            }
            else 
            {
                StartCoroutine(Provision_Internal(m_condition));
            }
        }
        public void BeginProvision() 
        {
            StartCoroutine(Provision_Internal(m_condition));
        }
    }
}