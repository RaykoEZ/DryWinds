using UnityEngine;
using TMPro;
using Curry.Game;
namespace Curry.Explore
{
    public class GameStateController : MonoBehaviour
    {
        [SerializeField] Adventurer m_player = default;
        [SerializeField] Animator m_gameResult = default;
        [SerializeField] ObjectiveManager m_objectives = default;
        [SerializeField] TextMeshProUGUI m_resultText = default;
        void Start() 
        {
            m_gameResult.gameObject.SetActive(false);
            m_player.OnDefeat += OnPlayerDefeat;
            m_objectives.AllCriticalComplete += OnGameCleared;
        }
        void OnPlayerDefeat(IPlayer player) 
        {
            m_resultText.text = "Game Over";
            UpdateResultPanel();
        }
        void OnGameCleared()
        {
            m_resultText.text = "Commission Completed";
            UpdateResultPanel();
        }

        void UpdateResultPanel() 
        {
            m_gameResult.gameObject.SetActive(true);
            m_gameResult.SetBool("GameOver", true);
        }

        void OnProceedToResult() 
        {
            m_gameResult.SetBool("GameOver", false);
            m_gameResult.gameObject.SetActive(false);
        }
    }
}
