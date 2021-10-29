using System.Collections.Generic;

namespace Curry.Game
{
    public interface IActionInput 
    { 
        Dictionary<string, object> Payload { get; }
    }
}
