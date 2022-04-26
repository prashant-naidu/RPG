using GameDevTV.Utils;
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
        [SerializeField] private bool m_ShouldUseModifiers = false;

        private LazyValue<int> m_CurrentLevel;
        public int Level { get { return m_CurrentLevel.value; } }

        public event Action OnLevelUp;

        private void Awake()
        {
            m_CurrentLevel = new LazyValue<int>(GetInitialLevel);
        }

        private int GetInitialLevel()
        {
            return CalculateLevel();
        }

        private void Start()
        {
            m_CurrentLevel.ForceInit();
        }

        private void OnEnable()
        {
            if (m_Experience != null)
            {
                m_Experience.OnExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if (m_Experience != null)
            {
                m_Experience.OnExperienceGained -= UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > m_CurrentLevel.value)
            {
                m_CurrentLevel.value = newLevel;
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
            float baseStat = m_Progression.GetStat(stat, m_CharacterClass, m_CurrentLevel.value);

            return (baseStat + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat)/100f);
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if (!m_ShouldUseModifiers) return 0;

            float total = 0;

            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
            }

            return total;
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (!m_ShouldUseModifiers) return 0;

            float total = 0;

            foreach(IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float percentageModifier in provider.GetPercentageModifiers(stat))
                {
                    total += percentageModifier;
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