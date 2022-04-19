using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [Header("Dependencies")]
        [SerializeField] private Animator m_Animator;
        [SerializeField] private ActionScheduler m_ActionScheduler;
        
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

        public void TakeDamage(float damage)
        {
            m_Health = Mathf.Max(m_Health - damage, 0f);
            if (m_Health == 0)
            {
                Die();
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