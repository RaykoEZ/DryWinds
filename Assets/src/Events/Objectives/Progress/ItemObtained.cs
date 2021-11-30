using System;
using UnityEngine;

namespace Curry.Events
{
    [Serializable]
    public struct ItemGain
    {
        public int ItemSerialNumber;
        public int Amount;
    }

    [Serializable]
    public class ItemObtained : ICondition<ItemGain>
    {
        [SerializeField] int m_itemSerialNumber = default;
        [SerializeField] int m_amountNeeded = default;

        public virtual ItemGain Target => throw new NotImplementedException();

        public virtual ItemGain Progress => throw new NotImplementedException();

        public virtual bool Achieved => throw new NotImplementedException();

        public string Description => throw new NotImplementedException();

        public virtual bool UpdateProgress(ItemGain progress)
        {
            throw new NotImplementedException();
        }
    }
}
