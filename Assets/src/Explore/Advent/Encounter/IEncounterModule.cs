using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Curry.Explore
{
    public interface IEncounterModule 
    {
        IEnumerator Activate(GameStateContext context);
    }
    public interface IHealModule : IEncounterModule 
    {
#if UNITY_EDITOR
        // For editor to serialize Healing module fields from nested structures
        public string ModuleName { get; }
#endif
        public HealingModule HealProperty { get; }

    }
    public interface IHurtModule : IEncounterModule 
    {
#if UNITY_EDITOR
        // For editor to serialize Healing module fields from nested structures
        public string ModuleName { get; }
#endif
        public DealDamageTo DamageProperty { get; }
    }
    public interface IAddToHandModule : IEncounterModule 
    {
#if UNITY_EDITOR
        // For editor to serialize Healing module fields from nested structures
        public string ModuleName { get; }
#endif
    }
}