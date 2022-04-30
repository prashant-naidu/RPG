using RPG.Attributes;
using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class PlayerHUD : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private RectTransform m_HpBar;
        [SerializeField] private RectTransform m_XpBar;
        [SerializeField] private TextMeshProUGUI m_HpText;
        [SerializeField] private TextMeshProUGUI m_XpText;
        [SerializeField] private TextMeshProUGUI m_LevelText;

        [Header("Dependencies")]
        [SerializeField] private Health m_PlayerHealth;
        [SerializeField] private Experience m_PlayerExperience;
        [SerializeField] private BaseStats m_PlayerBaseStats;

        private void Update()
        {
            m_HpText.text = string.Format("{0:0} / {1:0}", m_PlayerHealth.HealthPoints, m_PlayerHealth.MaxHealthPoints);
            m_HpBar.localScale = new Vector3(m_PlayerHealth.GetFraction(), 1f, 1f);
            m_XpText.text = string.Format("{0:0} / {1:0}", m_PlayerExperience.ExperiencePoints, m_PlayerBaseStats.GetStat(Stat.ExperienceToLevelUp));
            m_XpBar.localScale = new Vector3(GetPlayerExperiencePercentage(), 1f, 1f);
            m_LevelText.text = "Lv. " + m_PlayerBaseStats.Level.ToString();
        }

        private float GetPlayerExperiencePercentage()
        {
            return m_PlayerExperience.ExperiencePoints / m_PlayerBaseStats.GetStat(Stat.ExperienceToLevelUp);
        }
    }
}
