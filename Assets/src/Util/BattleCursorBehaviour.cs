using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Curry.Util
{
    // Not using for now
    public class BattleCursorBehaviour : MonoBehaviour
    {
        [SerializeField] SpriteRenderer Sprite = default;
        public bool CollisionActivated { get { return m_collisionOn; } }

        bool m_collisionOn = false;

        void OnMouseDown()
        {
            BattleMode(true);
        }


        void OnMouseUp()
        {
            BattleMode(false);
        }

        void Update()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mousePos.z = 0.0f;
            transform.position = mousePos;
        }

        void BattleMode(bool isOn) 
        {
            Sprite.enabled = isOn;
            m_collisionOn = isOn;
        }

    }
}

