using System;
using UnityEngine;

namespace Curry.Game
{
    [Serializable]
    public struct EntityProperty
    {
        public string Name { get; }
        public string Description { get; }
        public Sprite ItemSprite { get; }

        public EntityProperty(string name, string desc, Sprite sprite)
        {
            Name = name;
            Description = desc;
            ItemSprite = sprite;
        }
    }
}
