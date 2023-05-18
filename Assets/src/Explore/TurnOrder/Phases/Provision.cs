using Curry.Events;
using Curry.UI;
using System;
using System.Collections;
using UnityEngine;
namespace Curry.Explore
{
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
            yield return new WaitForEndOfFrame();
            m_deck.ChooseToAddFromInventory(
                m_condition,
                cardPoolFilter: null,
                onChosen: TransitionTo);
            yield return null;
        }

        public void OnProvisionTrigger() 
        {
            StartCoroutine(Evaluate_Internal());
        }
    }
}