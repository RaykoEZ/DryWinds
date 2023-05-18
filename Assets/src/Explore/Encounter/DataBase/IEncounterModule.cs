using System.Collections;
using System.Collections.Generic;

namespace Curry.Explore
{
    public interface IEncounterModule 
    {
        // For editor to serialize module fields from nested structures
        EncounterContent GetOutcomeContent();
    }
}