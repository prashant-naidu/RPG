using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private UnityEvent m_OnHit;

        public void OnHit()
        {
            m_OnHit.Invoke();
        }
    }
}
