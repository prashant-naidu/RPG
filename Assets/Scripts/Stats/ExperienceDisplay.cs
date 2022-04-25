using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private Experience m_PlayerExperience;

        [Header("UI Elements")]
        [SerializeField] private Text m_ValueText;

        private void Update()
        {
            m_ValueText.text = string.Format("{0}", m_PlayerExperience.ExperiencePoints);
        }
    }
}