using RPG.Combat;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private Fighter m_Fighter;

        [Header("Parameters")]
        public float WeaponRange = 2f;
        public float TimeBetweenAttacks = 1f;
        public float WeaponDamage = 5f;
        public float ChaseDistance = 5f;

        private GameObject m_PlayerGO;

        private void Awake()
        {
            m_PlayerGO = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update()
        {
            if (IsPlayerInAttackingRange() && m_Fighter.CanAttack(m_PlayerGO))
            {
                m_Fighter.Attack(m_PlayerGO);
            }
            else
            {
                m_Fighter.Cancel();
            }
        }

        private bool IsPlayerInAttackingRange()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, m_PlayerGO.transform.position);
            return distanceToPlayer <= ChaseDistance;
        }
    }
}
