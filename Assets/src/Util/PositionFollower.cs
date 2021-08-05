using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Util
{
    public class PositionFollower : MonoBehaviour
    {
        [SerializeField] Transform m_target = default;
        // Update is called once per frame
        void Update()
        {
            gameObject.transform.position = m_target.position;
        }
    }
}
