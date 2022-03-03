using System;

namespace Curry.Game
{
    public enum EntityType 
    {
        MiscItem = 0,
        Player = 1,
        Flora = 2,
        Fauna = 3,
        Npc = 4,
    }

    [Serializable]
    public struct EntityId : IComparable, IEquatable<EntityId>
    {
        public EntityType Type;
        public int SerialNumber;
        public string Value { get { return $"{Type}_{SerialNumber}"; } }

        public int CompareTo(object obj)
        {
            if(obj is EntityId id && id.Type == Type) 
            {
                return SerialNumber.CompareTo(id.SerialNumber);
            }
            return 1;
        }

        public static bool operator ==(EntityId t1, EntityId t2)
        {
            return
                t1.Type == t2.Type &&
                t1.SerialNumber == t2.SerialNumber;
        }

        public static bool operator !=(EntityId t1, EntityId t2)
        {
            return
                t1.Type != t2.Type ||
                t1.SerialNumber != t2.SerialNumber;
        }

        public bool Equals(EntityId other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (obj is EntityId Id)
            {
                return Equals(Id);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public bool IsItem() 
        {
            return Type == EntityType.MiscItem ||
                Type == EntityType.Flora ||
                Type == EntityType.Fauna;
        }

        public bool IsCharacter()
        {
            return Type == EntityType.Npc ||
                Type == EntityType.Player;
        }

        public bool IsCreature()
        {
            return Type != EntityType.MiscItem;
        }
    }
}
