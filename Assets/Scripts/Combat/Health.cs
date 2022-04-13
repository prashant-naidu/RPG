using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        public float health = 100f;

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
            health = Mathf.Max(health - damage, 0f);
            if (health == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (m_IsDead) return;

            GetComponent<Animator>().SetTrigger("die");
            m_IsDead = true;
        }
    }
}