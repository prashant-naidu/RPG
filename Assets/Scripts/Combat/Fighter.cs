using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [Header("Dependencies")]
        [SerializeField] private ActionScheduler m_ActionScheduler;
        [SerializeField] private Mover m_Mover;
        [SerializeField] private Animator m_Animator;

        [Header("Parameters")]
        public float WeaponRange = 2f;
        public float TimeBetweenAttacks = 1f;
        public float WeaponDamage = 5f;

        private Transform m_Target;
        private float m_TimeSinceLastAttack = 0f;

        private void Update()
        {
            m_TimeSinceLastAttack += Time.deltaTime;

            if (m_Target == null) return;

            bool isInRange = Vector3.Distance(transform.position, m_Target.position) < WeaponRange;

            if (!isInRange)
            {
                m_Mover.MoveTo(m_Target.position);
            }
            else
            {
                m_Mover.Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            if (m_TimeSinceLastAttack > TimeBetweenAttacks)
            {
                // This will trigger the Hit() event
                m_Animator.SetTrigger("attack");
                m_TimeSinceLastAttack = 0;
            }
        }

        // Animation Event
        private void Hit()
        {
            m_Target?.GetComponent<Health>().TakeDamage(WeaponDamage);
        }

        public void Attack(CombatTarget combatTarget)
        {
            m_ActionScheduler.StartAction(this);
            m_Target = combatTarget.transform;
        }

        public void Cancel()
        {
            m_Target = null;
        }
    }
}
