using System.Collections;
namespace Curry.Explore
{
    public interface IEncounterModule 
    {
        // For editor to serialize module fields from nested structures
        public string[] SerializePropertyNames { get; }
        IEnumerator Activate(GameStateContext context);
    }
}