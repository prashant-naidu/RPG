using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] m_ProgressionCharacterClasses = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            foreach (ProgressionCharacterClass progressionCharacterClass in m_ProgressionCharacterClasses)
            {
                if (progressionCharacterClass.CharacterClass != characterClass) continue;

                foreach (ProgressionStat progressionStat in progressionCharacterClass.Stats)
                {
                    if (progressionStat.Stat != stat) continue;
                    if (progressionStat.Levels.Length < level) continue;

                    return progressionStat.Levels[level - 1];
                }
            }

            return 0;
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

