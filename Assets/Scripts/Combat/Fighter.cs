using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using System;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [Header("Dependencies")]
        [SerializeField] private ActionScheduler m_ActionScheduler;
        [SerializeField] private Mover m_Mover;
        [SerializeField] private Animator m_Animator;

        [Header("Other")]
        [SerializeField] private Transform m_HandTransform = null;
        [SerializeField] private Weapon m_Weapon = null;

        [Header("Parameters")]
        public float TimeBetweenAttacks = 1f;

        private Health m_Target;
        private float m_TimeSinceLastAttack = Mathf.Infinity;

        private void Start()
        {
            SpawnWeapon();
        }

        private void Update()
        {
            m_TimeSinceLastAttack += Time.deltaTime;

            if (m_Target == null) return;
            if (m_Target.IsDead) return;

            bool isInRange = Vector3.Distance(transform.position, m_Target.transform.position) < m_Weapon.Range;

            if (!isInRange)
            {
                m_Mover.MoveTo(m_Target.transform.position, 1f);
            }
            else
            {
                m_Mover.Cancel();
                AttackBehaviour();
            }
        }

        private void SpawnWeapon()
        {
            if (m_Weapon != null)
            {
                m_Weapon.Spawn(m_HandTransform, m_Animator);
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(m_Target.transform);

            if (m_TimeSinceLastAttack > TimeBetweenAttacks)
            {
                // This will trigger the Hit() event
                m_Animator.ResetTrigger("stopAttack");
                m_Animator.SetTrigger("attack");
                m_TimeSinceLastAttack = 0;
            }
        }

        // Animation Event
        private void Hit()
        {
            m_Target?.TakeDamage(m_Weapon.Damage);
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }

            Health targetToTest = combatTarget.GetComponent<Health>();
            return !targetToTest.IsDead;
        }

        public void Attack(GameObject combatTarget)
        {
            m_ActionScheduler.StartAction(this);
            m_Target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttackAnimation();
            m_Target = null;
            m_Mover.Cancel();
        }

        private void StopAttackAnimation()
        {
            m_Animator.ResetTrigger("attack");
            m_Animator.SetTrigger("stopAttack");
        }
    }
}
