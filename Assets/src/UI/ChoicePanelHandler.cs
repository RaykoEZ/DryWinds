using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Curry.Game;

namespace Curry.UI
{
    public delegate void OnChoiceFinish(ChoiceResult result);
    public class ChoicePanelHandler : MonoBehaviour
    {
        [SerializeField] CanvasGroup m_viewToggle = default;
        [SerializeField] PanelUIHandler m_panelAnim = default;
        [SerializeField] TextMeshProUGUI m_title = default;
        [SerializeField] Transform m_content = default;
        [SerializeField] Button m_confirm = default;
        [SerializeField] Button m_cancel = default;
        [SerializeField] GameMessageTrigger m_error = default;
        event OnChoiceFinish OnChoiceComplete;
        protected bool m_inProgress = false;
        protected ChoiceContext m_currentContext;
        protected HashSet<IChoice> m_chosen = new HashSet<IChoice>();
        bool CanConfirm => m_chosen?.Count >= m_currentContext.Conditons.MinChoiceCount;
        bool CanChoose => m_chosen?.Count < m_currentContext.Conditons.MaxChoiceCount;
        string ChoiceDisplay => $"Choose to gain: {m_chosen.Count}/{m_currentContext.Conditons.MaxChoiceCount}";
        public virtual void BeginChoicePanel(ChoiceContext context, OnChoiceFinish onFinish) 
        {
            if (!m_inProgress) 
            {
                // Clear all leftover items
                foreach (Transform t in m_content)
                {
                    if (t.TryGetComponent(out PoolableBehaviour poolable)) 
                    {
                        poolable.ReturnToPool();
                    }
                    else 
                    {
                        Destroy(t.gameObject);
                    }
                }
                m_inProgress = true;
                OnChoiceComplete += onFinish;
                m_currentContext = context;
                m_title.text = ChoiceDisplay;
                m_confirm.interactable = false;
                m_cancel.interactable = context.Conditons.CanCancel;
                PrepareChoices();
                m_panelAnim.Show();
                m_viewToggle.alpha = 1f;
                m_viewToggle.blocksRaycasts = true;
            }
        }
        protected virtual void PrepareChoices() 
        {
            foreach (IChoice choice in m_currentContext.ChooseFrom) 
            {
                choice.OnChosen += OnChoose;
                choice.OnUnchoose += OnUnchoose;
                choice.DisplayChoice(m_content);
            }
        }
        protected virtual void Clear() 
        {
            m_panelAnim.Hide();
            // Hide view toggle
            m_viewToggle.alpha = 0f;
            m_viewToggle.blocksRaycasts = false;
            foreach (IChoice choice in m_currentContext.ChooseFrom)
            {
                choice.OnChosen -= OnChoose;
                choice.OnUnchoose -= OnUnchoose;
            }
            m_currentContext = new ChoiceContext();
            OnChoiceComplete = null;
            m_inProgress = false;
            m_chosen.Clear();
        }
        public virtual void ConfirmChoice() 
        {
            int numChosen = m_chosen.Count;
            bool tooFew = numChosen < m_currentContext.Conditons.MinChoiceCount;
            bool tooMany = numChosen > m_currentContext.Conditons.MaxChoiceCount;
            if (tooFew)
            {
                m_error.TriggerGameMessage(
                    ErrorMessages.s_chosenTooFew + $": Min: {m_currentContext.Conditons.MinChoiceCount}");
            }
            else if (tooMany) 
            {
                m_error.TriggerGameMessage(
                    ErrorMessages.s_chosenTooMany + $": Max: {m_currentContext.Conditons.MaxChoiceCount}");
            }
            else 
            {
                ChoiceResult result = new ChoiceResult(ChoiceStatus.Confirmed, m_currentContext.ChooseFrom, new List<IChoice>(m_chosen));
                OnChoiceComplete?.Invoke(result);
            }
            Clear();
        }
        public virtual void CancelChoice() 
        {
            ChoiceResult result = new ChoiceResult(ChoiceStatus.Cancelled, m_currentContext.ChooseFrom, new List<IChoice>());
            OnChoiceComplete?.Invoke(result);
            Clear();
        }
        protected virtual void OnChoose(IChoice chosen) 
        {
            if (CanChoose)
            {
                m_chosen.Add(chosen);
                // If we reach max choice count, disable all choice
                if (!CanChoose)
                {
                    SetChoiceChoosability(false);
                }
            }
            m_title.text = ChoiceDisplay;
            // Set confirm button if we chose 
            m_confirm.interactable = CanConfirm;
        }
        protected virtual void OnUnchoose(IChoice chosen) 
        {
            if (m_chosen.Remove(chosen)) 
            {
                SetChoiceChoosability(true);
                m_confirm.interactable = CanConfirm;
            }
            m_title.text = ChoiceDisplay;
        }
        protected virtual void SetChoiceChoosability(bool value) 
        {
            foreach (IChoice choice in m_currentContext.ChooseFrom)
            {
                choice.Choosable = value;
            }
        }
    }
}