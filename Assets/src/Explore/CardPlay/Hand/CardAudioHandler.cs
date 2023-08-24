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
            StartCoroutine(CardAudio_Internal(numberDrawn, "cardDraw"));
        }
        public void OnCardConsume() 
        {
            StartCoroutine(CardAudio_Internal(1, "cardConsume"));
        }
        IEnumerator CardAudio_Internal(int numberDrawn, string audioName) 
        { 
            for(int i = 0; i < numberDrawn; ++i) 
            {
                m_audio?.Play(audioName);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}