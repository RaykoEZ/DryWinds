using System;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Util
{
    [CreateAssetMenu(fileName = "GameConditionDatabase", menuName = "Curry/GameConditionDatabase", order = 1)]
    public class GameConditionDatabase : ScriptableObject
    {
        [Header("Existing Game Condition Flags")]
        [SerializeField] string[] m_definedConditions = new string[32];
        public IReadOnlyList<string> ExistingFlags => m_definedConditions;
        public int NameToFieldIndex(string name)
        {
            List<string> temp = new List<string>(ExistingFlags);
            if (string.IsNullOrEmpty(name) || 
                temp == null || temp.Count == 0 || !temp.Contains(name))
            {
                return -1;
            }
            else
            {
                return temp.IndexOf(name);
            }
        }
        public int FieldToMask(int field)
        {
            if (field == -1) return -1;
            int mask = 0;
            for (int i = 0; i < ExistingFlags.Count; i++)
            {
                if ((field & (1 << i)) != 0)
                {
                    mask |= 1 << NameToFieldIndex(ExistingFlags[i]);
                }
                else
                {
                    mask &= ~(1 << NameToFieldIndex(ExistingFlags[i]));
                }
            }

            return mask;
        }
        public int MaskToField(int flag)
        {
            if (flag == -1) return -1;
            int field = 0;
            int index = 0;
            for (int i = 0; i < ExistingFlags.Count; ++i)
            {
                index = NameToFieldIndex(ExistingFlags[i]);
                if (index >= 0 && (flag & (1 << index)) != 0)
                {
                    field |= 1 << i;
                }
            }
            return field;
        }
    }
}