using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using System;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [Header("Dependencies")]
        [SerializeField] private Animator m_Animator;
        [SerializeField] private ActionScheduler m_ActionScheduler;
        [SerializeField] private BaseStats m_BaseStats;
        
        [Header("Parameters")]
        [SerializeField] private float m_Health = 100f;

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
            m_Health = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return (m_Health / m_BaseStats.GetStat(Stat.Health)) * 100f;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            m_Health = Mathf.Max(m_Health - damage, 0f);
            if (m_Health == 0)
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
            return m_Health;
        }

        public void RestoreState(object state)
        {
            m_Health = (float)state;

            if (m_Health <= 0)
            {
                Die();
            }
        }
    }
}