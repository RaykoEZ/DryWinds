using System.Collections;
using UnityEngine;
using Curry.UI;

namespace Curry.Explore
{
    public class CardAudioHandler : MonoBehaviour 
    {
        [SerializeField] AudioManager m_audio = default;
        public void OnCardDraw(int numberDrawn) 
        {
            StartCoroutine(CardDraw_Internal(numberDrawn));
        }
        IEnumerator CardDraw_Internal(int numberDrawn) 
        { 
            for(int i = 0; i < numberDrawn; ++i) 
            {
                m_audio?.Play("cardDraw");
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}