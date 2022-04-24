﻿using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [Header("Dependencies")]
        [SerializeField] private ActionScheduler m_ActionScheduler;
        [SerializeField] private Mover m_Mover;
        [SerializeField] private Animator m_Animator;

        [Header("Other")]
        [SerializeField] private Transform m_RightHandTransform = null;
        [SerializeField] private Transform m_LeftHandTransform = null;
        [SerializeField] private Weapon m_DefaultWeapon = null;
        [SerializeField] private Weapon m_CurrentWeapon = null;

        private GameObject m_CurrentWeaponGO = null;

        [Header("Parameters")]
        public float TimeBetweenAttacks = 1f;
        
        private float m_TimeSinceLastAttack = Mathf.Infinity;
        private Health m_Target;
        public Health Target { get { return m_Target; } }

        private void Start()
        {
            if (m_CurrentWeapon == null)
            {
                EquipWeapon(m_DefaultWeapon);
            }
        }

        private void Update()
        {
            m_TimeSinceLastAttack += Time.deltaTime;

            if (m_Target == null) return;
            if (m_Target.IsDead) return;

            bool isInRange = Vector3.Distance(transform.position, m_Target.transform.position) < m_CurrentWeapon.Range;

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

        public void EquipWeapon(Weapon weapon)
        {
            if (m_CurrentWeaponGO != null)
            {
                Destroy(m_CurrentWeaponGO);
            }

            m_CurrentWeapon = weapon;
            m_CurrentWeaponGO = m_CurrentWeapon.Spawn(m_RightHandTransform, m_LeftHandTransform, m_Animator);
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
            if (m_Target == null) return;
            if (m_CurrentWeapon.HasProjectile)
            {
                m_CurrentWeapon.LaunchProjectile(m_RightHandTransform, m_LeftHandTransform, m_Target);
            }
            else
            {
                m_Target.TakeDamage(m_CurrentWeapon.Damage);
            }
        }

        // Animation Event
        private void Shoot()
        {
            Hit();
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

        public object CaptureState()
        {
            return m_CurrentWeapon.name;
        }

        public void RestoreState(object state)
        {
            Weapon weapon = Resources.Load<Weapon>((string)state);
            EquipWeapon(weapon);
        }
    }
}
