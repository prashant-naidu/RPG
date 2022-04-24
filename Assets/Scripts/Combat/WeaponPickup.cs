using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] private Weapon m_Weapon = null;
        [SerializeField] private float m_RespawnTime = 5f;

        [SerializeField] private SphereCollider m_Collider = null;
        [SerializeField] private GameObject m_ModelGO = null;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                other.GetComponent<Fighter>().EquipWeapon(m_Weapon);
                StartCoroutine(HideForSeconds(m_RespawnTime));
            }
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
    }
}