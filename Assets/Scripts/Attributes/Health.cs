using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [Header("Dependencies")]
        [SerializeField] private Animator m_Animator;
        [SerializeField] private ActionScheduler m_ActionScheduler;
        [SerializeField] private BaseStats m_BaseStats;

        private float m_HealthPoints = -1f;
        private bool m_IsDead = false;
        public bool IsDead
        {
            get
            {
                return m_IsDead;
            }
        }

        private void Start()
        {
            if (m_HealthPoints < 0)
            {
                m_HealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
        }

        public float GetPercentage()
        {
            return (m_HealthPoints / m_BaseStats.GetStat(Stat.Health)) * 100f;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            m_HealthPoints = Mathf.Max(m_HealthPoints - damage, 0f);
            if (m_HealthPoints == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience != null)
            {
                experience.GainExperience(m_BaseStats.GetStat(Stat.ExperienceReward));
            }
        }

        private void Die()
        {
            if (m_IsDead) return;

            m_IsDead = true;

            m_Animator.SetTrigger("die");
            m_ActionScheduler.CancelCurrentAction();
        }

        public object CaptureState()
        {
            return m_HealthPoints;
        }

        public void RestoreState(object state)
        {
            m_HealthPoints = (float)state;

            if (m_HealthPoints <= 0)
            {
                Die();
            }
        }
    }
}