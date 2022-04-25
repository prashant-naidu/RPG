using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
    class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] private float m_ExperiencePoints = 0;
        public float ExperiencePoints { get { return m_ExperiencePoints; } }

        public void GainExperience(float experience)
        {
            m_ExperiencePoints += experience;
        }

        public object CaptureState()
        {
            return m_ExperiencePoints;
        }

        public void RestoreState(object state)
        {
            m_ExperiencePoints = (float)state;
        }
    }
}
