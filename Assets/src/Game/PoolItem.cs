using UnityEngine;

namespace Curry.Game
{
    public class PoolItem
    {
        public bool IsActive 
        { 
            get { return m_obj.activeInHierarchy; } 
        }

        GameObject m_obj;
        public GameObject Obj { get { return m_obj; } }

        public PoolItem(GameObject objRef, Transform parent) 
        {
            m_obj = Object.Instantiate(objRef, parent);
            SetActive(false);
        }

        public void SetActive(bool setActive = true) 
        {
            m_obj?.SetActive(setActive);
        }
    }
}
