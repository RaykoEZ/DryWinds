using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.UI;
namespace Curry.Explore
{
    // A card that activates itself upon player drawing the card
    [Serializable]
    public class Encounter
    {
        [SerializeField] List<string> m_dialogues = default;
        protected virtual IEnumerator ActivateEffect(GameStateContext context) 
        {
            yield return new WaitForEndOfFrame();
        }
        public virtual Dialogue OnChoose(GameStateContext context) 
        {
            return new Dialogue(ActivateEffect(context), m_dialogues);
        }
    }
}