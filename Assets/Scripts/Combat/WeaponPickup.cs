using RPG.Attributes;
using RPG.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] private WeaponConfig m_Weapon = null;
        [SerializeField] private float m_RespawnTime = 5f;

        [SerializeField] private SphereCollider m_Collider = null;
        [SerializeField] private GameObject m_ModelGO = null;

        [Header("Temp Health Hack")]
        [SerializeField] private float m_HealthToRestore = 0;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                DoPickup(other.gameObject);
            }
        }

        private void DoPickup(GameObject subject)
        {
            if (m_Weapon != null)
            {
                subject.GetComponent<Fighter>().EquipWeapon(m_Weapon);
            }
            if (m_HealthToRestore > 0)
            {
                subject.GetComponent<Health>().Heal(m_HealthToRestore);
            }
            
            StartCoroutine(HideForSeconds(m_RespawnTime));
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            SetPickupVisibility(false);
            yield return new WaitForSeconds(seconds);
            SetPickupVisibility(true);
        }

        private void SetPickupVisibility(bool isVisible)
        {
            m_Collider.enabled = isVisible;
            m_ModelGO.SetActive(isVisible);
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                DoPickup(callingController.gameObject);
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}