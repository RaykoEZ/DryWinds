using System;

namespace Curry.Explore
{
    [Serializable]
    public struct DeploymentResource : IEquatable<DeploymentResource>
    {
        public int Food;
        public int Water;

        public override bool Equals(object obj)
        {
            return obj is DeploymentResource resource &&
                Equals(resource);
        }

        public bool Equals(DeploymentResource other)
        {
            return Food == other.Food &&
                   Water == other.Water;
        }

        public static bool operator <=(DeploymentResource lhs, DeploymentResource rhs)
        {
            return 
                    (lhs.Food < rhs.Food || lhs.Food == rhs.Food) &&
                    (lhs.Water < rhs.Water || lhs.Water == rhs.Water);
        }
        public static bool operator >=(DeploymentResource lhs, DeploymentResource rhs)
        {
            return
                    (lhs.Food > rhs.Food || lhs.Food == rhs.Food) &&
                    (lhs.Water > rhs.Water || lhs.Water == rhs.Water);
        }

        public static DeploymentResource operator +(DeploymentResource a, DeploymentResource b) 
        {
            DeploymentResource ret = new DeploymentResource();
            ret.Food = a.Food + b.Food;
            ret.Water = a.Water + b.Water;
            return ret;
        }
        public static DeploymentResource operator -(DeploymentResource a, DeploymentResource b)
        {
            DeploymentResource ret = new DeploymentResource();
            ret.Food = a.Food - b.Food;
            ret.Water = a.Water - b.Water;
            return ret;
        }

        public override int GetHashCode()
        {
            int hashCode = -1688808210;
            hashCode = hashCode * -1521134295 + Food.GetHashCode();
            hashCode = hashCode * -1521134295 + Water.GetHashCode();
            return hashCode;
        }
    }
}
