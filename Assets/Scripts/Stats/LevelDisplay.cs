using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private BaseStats m_PlayerBaseStats;

        [Header("UI Elements")]
        [SerializeField] private Text m_ValueText;

        private void Update()
        {
            m_ValueText.text = m_PlayerBaseStats.Level.ToString();
        }
    }
}
