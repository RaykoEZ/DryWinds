using System.Collections.Generic;
using UnityEngine;

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