using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private Fighter m_Fighter;
        [SerializeField] private Health m_Health;
        [SerializeField] private Mover m_Mover;

        [Header("Parameters")]
        public float ChaseDistance = 5f;

        private GameObject m_PlayerGO;
        private Vector3 m_GuardPosition;

        private void Awake()
        {
            m_PlayerGO = GameObject.FindGameObjectWithTag("Player");
            m_GuardPosition = transform.position;
        }

        private void Update()
        {
            if (m_Health.IsDead) return;

            if (IsPlayerInAttackingRange() && m_Fighter.CanAttack(m_PlayerGO))
            {
                m_Fighter.Attack(m_PlayerGO);
            }
            else
            {
                m_Mover.StartMoveAction(m_GuardPosition);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, ChaseDistance);
        }

        private bool IsPlayerInAttackingRange()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, m_PlayerGO.transform.position);
            return distanceToPlayer <= ChaseDistance;
        }
    }
}
