using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Util
{
    public class CameraParentHandler : MonoBehaviour
    {
        [SerializeField] Transform m_defaultParent = default;
        [SerializeField] Camera m_cam = default;
        // Start is called before the first frame update
        void Start()
        {
            m_cam.gameObject.transform.SetParent(m_defaultParent);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
