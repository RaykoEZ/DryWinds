using UnityEngine;
using Curry.Game;

namespace Curry.Explore
{
    public interface CrisisProperty<T> where T : Interactable
    {
        float Life { get; }
        float Intensity { get; }

        void OnEnterArea(T col);
        void OnExitArea(T col);
        void OnCrisisUpdate(float dt);

    }
}
