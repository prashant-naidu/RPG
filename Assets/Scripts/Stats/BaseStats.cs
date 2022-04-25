using System;
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
        [SerializeField] private GameObject m_LevelUpEffectPrefabReference = null;

        private int m_CurrentLevel = 0;

        public event Action OnLevelUp;

        private void Start()
        {
            m_CurrentLevel = CalculateLevel();
            if (m_Experience != null)
            {
                m_Experience.OnExperienceGained += UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > m_CurrentLevel)
            {
                m_CurrentLevel = newLevel;
                LevelUpEffect();
                OnLevelUp();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(m_LevelUpEffectPrefabReference, transform);
        }

        public float GetStat(Stat stat)
        {
            return m_Progression.GetStat(stat, m_CharacterClass, GetLevel()) + GetAdditiveModifier(stat);
        }

        public int GetLevel()
        {
            if (m_CurrentLevel < 1)
            {
                m_CurrentLevel = CalculateLevel();
            }
            return m_CurrentLevel;
        }

        private float GetAdditiveModifier(Stat stat)
        {
            float total = 0;

            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditiveModifier(stat))
                {
                    total += modifier;
                }
            }

            return total;
        }

        private int CalculateLevel()
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