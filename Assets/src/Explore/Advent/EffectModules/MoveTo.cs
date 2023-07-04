using Curry.Events;
using System;
using System.Collections;
using UnityEngine;
namespace Curry.Explore
{
    [Serializable]
    public class MoveTo : PropertyAttribute
    {
        public void ApplyEffect(ICharacter moveThis, Vector3 destination, MovementManager movement, IEnumerator onMoveFinish = null)
        {
            movement?.MoveCharacter(moveThis, destination, false, onMoveFinish);
        }
    }
}