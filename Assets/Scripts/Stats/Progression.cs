using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] m_ProgressionCharacterClasses = null;

        private Dictionary<CharacterClass, Dictionary<Stat, float[]>> m_LookupTable = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookup();

            float[] levels = m_LookupTable[characterClass][stat];

            if (levels.Length < level) return 0;

            return levels[level - 1];
        }

        private void BuildLookup()
        {
            if (m_LookupTable != null) return;

            m_LookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (ProgressionCharacterClass progressionCharacterClass in m_ProgressionCharacterClasses)
            {
                var statTable = new Dictionary<Stat, float[]>();

                foreach (ProgressionStat progressionStat in progressionCharacterClass.Stats)
                {
                    statTable[progressionStat.Stat] = progressionStat.Levels;
                }

                m_LookupTable[progressionCharacterClass.CharacterClass] = statTable;
            }
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass CharacterClass;
            public ProgressionStat[] Stats;
        }

        [System.Serializable]
        class ProgressionStat
        {
            public Stat Stat;
            public float[] Levels;
        }
    }
}

