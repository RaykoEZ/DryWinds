using System;

namespace Curry.Explore
{
    public class ObjectId : IEquatable<ObjectId>
    {
        public string Id { get; private set; }

        public ObjectId(string unitId)
        {
            Id = unitId;

        }
        public ObjectId(UnityEngine.GameObject objectRef)
        {
            Id = objectRef.name;

        }
        public ObjectId(ObjectId tileId)
        {
            Id = tileId.Id;
        }
        public static bool operator ==(ObjectId t1, ObjectId t2)
        {
            return
                t1?.Id == t2?.Id;
        }
        public static bool operator !=(ObjectId t1, ObjectId t2)
        {
            return
                t1?.Id != t2?.Id;
        }

        public bool Equals(ObjectId obj)
        {
            return this == obj;
        }

        public override bool Equals(object obj)
        {
            if (obj is ObjectId Id)
            {
                return Equals(Id);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return
                $"{Id}".GetHashCode();
        }

    }
}