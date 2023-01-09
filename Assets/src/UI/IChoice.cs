using UnityEngine;

namespace Curry.UI
{
    public delegate void OnChoose(IChoice choice);
    public interface IChoice 
    {
        object Value { get; }
        event OnChoose OnChosen;
        event OnChoose OnUnchoose;
        void DisplayChoice(Transform parent);
        void Choose();
        void UnChoose();
    }
}