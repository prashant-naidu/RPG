using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        public float ChaseDistance = 5f;

        private GameObject m_PlayerGO;

        private void Awake()
        {
            m_PlayerGO = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update()
        {
            if (Vector3.Distance(transform.position, m_PlayerGO.transform.position) <= ChaseDistance)
            {
                print("Chase");
            }
        }
    }
}
