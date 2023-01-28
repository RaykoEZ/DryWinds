using Curry.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Curry.Explore
{
    [Serializable]
    public struct EncounterDetail 
    {
        public string Title;
        public string Description;
        public Sprite CoverImage;
        public List<EncounterOption> Choices;
    }
    [Serializable]
    public class EncounterOption : MonoBehaviour, IChoice
    {
        [SerializeField] string m_description = default;
        [SerializeField] Encounter m_encounter = default;
        public object Value => m_encounter;

        public bool Choosable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event OnChoose OnChosen;
        public event OnChoose OnUnchoose;

        public void Choose()
        {
            throw new NotImplementedException();
        }

        public void DisplayChoice(Transform parent)
        {
            throw new NotImplementedException();
        }

        public void UnChoose()
        {
            throw new NotImplementedException();
        }
    }


    [CreateAssetMenu(fileName = "BaseTerrain_", menuName = "Curry/Tiles", order = 1)]
    public class WorldTile : Tile
    {
        [SerializeField] protected string m_collectionId = default;
        [Range(0, 10)]
        [SerializeField] protected int m_diffculty = default;
        // Randomly draw from a deck with this ID
        public string CollectionId { get { return m_collectionId; } }
        public int Difficulty { get { return m_diffculty; } }

    }
}