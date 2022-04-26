using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private Health m_PlayerHealth;

        [Header("UI Elements")]
        [SerializeField] private Text m_HealthValueText;

        private void Update()
        {
            m_HealthValueText.text = string.Format("{0:0} / {1}", m_PlayerHealth.HealthPoints, m_PlayerHealth.MaxHealthPoints);
        }
    }
}
