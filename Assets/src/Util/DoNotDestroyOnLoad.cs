using UnityEngine;

namespace Curry.Util
{
    public class DoNotDestroyOnLoad : MonoBehaviour
    {
        // Start is called before the first frame update
        void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}
