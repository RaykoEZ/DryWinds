using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Util
{
    public class GravityHandler : MonoBehaviour
    {
        [SerializeField] float GravityScalar = 9.8f;
        // Start is called before the first frame update
        void Start()
        {
            Physics2D.gravity = new Vector2(0, -GravityScalar);
        }
    }
}
