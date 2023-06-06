using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.U2D.Path;

namespace Curry.Explore 
{
    public class MoveToggle : MonoBehaviour
    {
        [SerializeField] GameObject m_player = default;
        [SerializeField] Toggle m_toggle = default;
        [SerializeField] TextMeshProUGUI m_label = default;
        [SerializeField] SelectionManager m_selector = default;
        public bool Interactable { get; protected set; } = true;
        void Start()
        {
        }
        void OnDestroy()
        {
        }
        public void SetInteractable(bool interactable) 
        {
            Interactable = interactable;
            Cancel();
        }
        public void OnMovementToggle(bool isOn) 
        {
            if (!Interactable) { return; }

            if (isOn) 
            {
                PromptMovementPosition();
            } 
            else 
            {
                Cancel();
            }
        }
        void Cancel() 
        {
            m_selector.OnCancel -= Cancel;
            m_toggle.isOn = false;
            m_label.text = "Move";
            m_selector?.CancelSelection();
        }
        void PromptMovementPosition() 
        {
            m_label.text = "Cancel";
            TileSelectionInfo info = new TileSelectionInfo(
                TileSelectionMode.Adventure,
                m_player,
                Camera.main.WorldToScreenPoint(m_player.transform.position));
            m_selector.OnSelectTile(info);
            m_selector.OnCancel += Cancel;
        }
    }
}