using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Curry.UI
{
    [Serializable]
    public class Dialogue
    {
        public IEnumerator OnDialogueFinish { get; protected set; }
        public List<string> Text { get; protected set; }
        public Dialogue(IEnumerator OnDialogueEnd, List<string> text)
        {
            OnDialogueFinish = OnDialogueEnd;
            Text = text;
        }
    }
}
