using System;
using UnityEngine;

namespace Curry.Events
{
    [Serializable]
    public class ItemGain : IComparable
    {
        public int ItemSerialNumber;
        public int Amount;

        public int CompareTo(object obj)
        {
            if (obj is ItemGain gain && gain.ItemSerialNumber == ItemSerialNumber)
            {
                return ItemSerialNumber.CompareTo(gain.ItemSerialNumber);
            }
            return 1;
        }
    }

    [Serializable]
    public class ItemObtained : ICondition<ItemGain>
    {
        [SerializeField] ItemGain m_targetItemGain = default;
        protected ItemGain m_current;

        public virtual ItemGain Target
        {
            get { return m_targetItemGain; }
        }

        public virtual ItemGain Progress
        {
            get { return m_current; }
        }

        public virtual bool Achieved { 
            get { 
                return Progress.ItemSerialNumber == Target.ItemSerialNumber && 
                    Progress.Amount >= Target.Amount;
            }
        }

        public string Description { get { return $"Collect Item: {Target.ItemSerialNumber} x {Target.Amount}"; } }

        public ItemObtained(ItemGain target) 
        {
            m_targetItemGain = target;
            m_current = new ItemGain 
            { Amount = 0, ItemSerialNumber = target.ItemSerialNumber };
        }

        public virtual bool UpdateProgress(ItemGain progress)
        {
            if(m_current == null) 
            {
                m_current = new ItemGain { Amount = 0, ItemSerialNumber = m_targetItemGain.ItemSerialNumber };
            }

            if(progress.ItemSerialNumber == Target.ItemSerialNumber) 
            {
                m_current.Amount += progress.Amount;
            }

            return Achieved;
        }
    }
}
