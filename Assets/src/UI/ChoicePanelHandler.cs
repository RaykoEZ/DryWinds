using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Curry.Explore;
using TMPro;
namespace Curry.UI
{
    // The fields to fill the choice panel
    // (e.g: the pool of items to choose from, title, max/min to choose...etc)
    public struct ChoiceContext 
    { 
    
    }
    public interface IChoice 
    {
        object Value { get; }
        void DisplayChoice(Transform parent);
        void Choose();
        void UnChoose();
    }
    public struct ChoiceResult
    {
        public ChoiceStatus Status;
        public IReadOnlyList<IChoice> Choices;
        public enum ChoiceStatus
        {
            Confirmed,
            Cancelled
        }
    }

    public delegate void OnChoiceFinish(ChoiceResult result);
    public class ChoicePanelHandler : MonoBehaviour
    {
        [SerializeField] Animator m_panelAnim = default;
        [SerializeField] TextMeshProUGUI m_title = default;
        [SerializeField] Transform m_content = default;
        [SerializeField] Button m_confirm = default;
        [SerializeField] Button m_cancel = default;
        event OnChoiceFinish OnChoiceComplete;
        public void BeginChoicePanel(ChoiceContext context) { }
        public void ConfirmChoice() { }
        public void CancelChoice() { }
    }
}