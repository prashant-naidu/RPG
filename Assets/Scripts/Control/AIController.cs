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
        [SerializeField] private ActionScheduler m_ActionScheduler;

        [Header("Parameters")]
        public float ChaseDistance = 5f;
        public float SuspicionTime = 4f;

        private GameObject m_PlayerGO;
        private Vector3 m_GuardPosition;
        private float m_TimeSincePlayerLastSeen = Mathf.Infinity;

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
                m_TimeSincePlayerLastSeen = 0;
                AttackBehaviour();
            }
            else if (m_TimeSincePlayerLastSeen < SuspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                GuardBehaviour();
            }

            m_TimeSincePlayerLastSeen += Time.deltaTime;
        }
        private void AttackBehaviour()
        {
            m_Fighter.Attack(m_PlayerGO);
        }

        private void SuspicionBehaviour()
        {
            m_ActionScheduler.CancelCurrentAction();
        }

        private void GuardBehaviour()
        {
            m_Mover.StartMoveAction(m_GuardPosition);
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
