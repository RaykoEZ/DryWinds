using System;
using UnityEngine;
namespace Curry.UI
{
    [Serializable]
    public struct Dialogue 
    {
        public string Name;
        [TextArea(3, 5)]
        public string[] DialogueLines;
    }
}
