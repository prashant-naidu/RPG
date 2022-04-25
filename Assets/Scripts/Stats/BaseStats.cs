using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] private int m_StartingLevel = 1;
        [SerializeField] private CharacterClass m_CharacterClass;
        [SerializeField] private Progression m_Progression = null;
        [SerializeField] private Experience m_Experience = null;

        public float GetStat(Stat stat)
        {
            return m_Progression.GetStat(stat, m_CharacterClass, GetLevel());
        }

        public int GetLevel()
        {
            if (m_Experience == null) return m_StartingLevel;

            int penultimateLevel = m_Progression.GetLevels(Stat.ExperienceToLevelUp, m_CharacterClass);
            for (int level = 1; level <= penultimateLevel; level++)
            {
                float XPToLevelUp = m_Progression.GetStat(Stat.ExperienceToLevelUp, m_CharacterClass, level);
                if (XPToLevelUp > m_Experience.ExperiencePoints)
                {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }
    }
}