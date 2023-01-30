using System;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Explore
{
    [Serializable]
    public struct EncounterDetail 
    {
        public string Title;
        public string Description;
        public Sprite CoverImage;
        public List<EncounterOption> Choices;
    }
}