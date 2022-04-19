using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private GameObject m_WeaponPrefab = null;
        [SerializeField] private AnimatorOverrideController m_WeaponAnimatorOverrideController = null;

        public void Spawn(Transform handTransform, Animator animator)
        {
            Instantiate(m_WeaponPrefab, handTransform);
            animator.runtimeAnimatorController = m_WeaponAnimatorOverrideController;
        }
    }
}