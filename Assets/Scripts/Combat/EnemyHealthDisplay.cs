using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private Fighter m_PlayerFighter;

        [Header("UI Elements")]
        [SerializeField] private Text m_HealthValueText;

        private void Update()
        {
            string textToDisplay = m_PlayerFighter.Target != null ? string.Format("{0:0}%", m_PlayerFighter.Target.GetPercentage()) : "No target";
            m_HealthValueText.text = textToDisplay;
        }
    }
}
