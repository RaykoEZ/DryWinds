using Curry.Game;
using Curry.UI;
using Curry.Util;
using System;
using System.Collections;
using UnityEngine;
namespace Curry.Explore
{
    public class TurnStart : Phase
    {
        [SerializeField] MovementManager m_movementManager = default;
        protected override Type NextState { get; set; } = typeof(PlayerAction);
        protected override IEnumerator Evaluate_Internal()
        {
            m_movementManager.AddMoveCounter();
            TransitionTo();
            yield return null;
        }
    }
}