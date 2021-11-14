using System;
using UnityEngine;

namespace Curry.Game
{
    [Serializable]
    public struct EntityProperty
    {
        public string Name;
        public string Description;
        public Sprite EntitySprite;
        public EntityProperty(string name, string desc, Sprite sprite)
        {
            Name = name;
            Description = desc;
            EntitySprite = sprite;
        }
    }
}
