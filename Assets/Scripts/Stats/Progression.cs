using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] m_CharacterClasses = null;

        [System.Serializable]
        class ProgressionCharacterClass
        {
            [SerializeField] private CharacterClass m_CharacterClass;
            [SerializeField] private float[] m_HealthProgression;
        }
    }
}

