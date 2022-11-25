using System;
namespace Curry.Explore
{
    public struct EnemyId : IEquatable<EnemyId>
    {
        public string UnitId { get; private set; }
        public Guid InstanceId { get; private set; }
        public EnemyId(EnemyId id)
        {
            UnitId = id.UnitId;
            InstanceId = id.InstanceId;
        }
        public EnemyId(string unit = "")
        {
            UnitId = unit;
            InstanceId = Guid.NewGuid();
        }
        public override bool Equals(object obj)
        {
            return obj is EnemyId id && this == id;
        }
        public bool Equals(EnemyId other)
        {
            return this == other;
        }
        public static bool operator ==(EnemyId t1, EnemyId t2)
        {
            return t1.UnitId == t2.UnitId &&
                t1.InstanceId == t2.InstanceId;
        }
        public static bool operator !=(EnemyId t1, EnemyId t2)
        {
            return !(t1 == t2);
        }
        public override int GetHashCode()
        {
            return
                $"{UnitId}-{InstanceId}".GetHashCode();
        }
    }

}
