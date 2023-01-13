using System;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.UI
{
    // Use this to prompt player to choose something
    [Serializable]
    public class ChoicePrompter
    {
        [SerializeField] ChoicePanelHandler m_choiceManagerRef = default;
        public void MakeChoice(ChoiceConditions conditions, List<IChoice> choice, OnChoiceFinish onFinish)
        {
            ChoiceContext context = new ChoiceContext(conditions, choice);
            m_choiceManagerRef?.BeginChoicePanel(context, onFinish);
        }
    }
}