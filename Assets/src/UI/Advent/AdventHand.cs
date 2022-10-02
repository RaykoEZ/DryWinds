using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Curry.Events;
namespace Curry.Explore
{
    public class AdventHand : MonoBehaviour
    {
        public void OnCardDraw(List<AdventCard> cards)
        {
            Debug.Log("Cards drawn");
        }
    }
}