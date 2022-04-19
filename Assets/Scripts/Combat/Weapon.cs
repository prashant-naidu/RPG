using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [Header("References")]
        [SerializeField] private GameObject m_EquippedPrefab = null;
        [SerializeField] private AnimatorOverrideController m_WeaponAnimatorOverrideController = null;

        [Header("Parameters")]
        [SerializeField] private float m_Damage = 5f;
        public float Damage { get { return m_Damage; } }

        [SerializeField] private float m_Range = 2f;
        public float Range { get { return m_Range; } }

        [SerializeField] private bool m_IsRightHanded = true;

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (m_EquippedPrefab != null)
            {
                Instantiate(m_EquippedPrefab, m_IsRightHanded ? rightHand : leftHand);
            }
            if (m_WeaponAnimatorOverrideController != null)
            {
                animator.runtimeAnimatorController = m_WeaponAnimatorOverrideController;
            }
        }
    }
}