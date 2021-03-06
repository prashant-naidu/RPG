using GameDevTV.Utils;
using RPG.Attributes;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
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

        [Header("Patrol")]
        [SerializeField] private PatrolPath m_PatrolPath;
        public float WaypointDwellTime = 4f;

        [Header("Parameters")]
        public float ChaseDistance = 5f;
        public float SuspicionTime = 4f;
        public float AggrevatedCooldownTime = 5f;
        public float ShoutDistance = 5f;

        private GameObject m_PlayerGO;
        private LazyValue<Vector3> m_GuardPosition;
        private float m_TimeSincePlayerLastSeen = Mathf.Infinity;
        private float m_TimeSinceArrivedAtWaypoint = Mathf.Infinity;
        private float m_TimeSinceAggrevated = Mathf.Infinity;
        private float m_WaypointTolerance = 1f;
        private int m_CurrentWaypointIndex = 0;
        [Range(0,1)]
        private float m_PatrolSpeedFraction = 0.2f;

        private void Awake()
        {
            m_PlayerGO = GameObject.FindGameObjectWithTag("Player");
            m_GuardPosition = new LazyValue<Vector3>(GetInitialGuardPosition);
        }

        private Vector3 GetInitialGuardPosition()
        {
            return transform.position;
        }

        private void Start()
        {
            m_GuardPosition.ForceInit();
        }

        private void Update()
        {
            if (m_Health.IsDead) return;

            if (IsAggrevated() && m_Fighter.CanAttack(m_PlayerGO))
            {
                AttackBehaviour();
            }
            else if (m_TimeSincePlayerLastSeen < SuspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        public void Aggrevate()
        {
            m_TimeSinceAggrevated = 0;
        }

        private void UpdateTimers()
        {
            m_TimeSincePlayerLastSeen += Time.deltaTime;
            m_TimeSinceArrivedAtWaypoint += Time.deltaTime;
            m_TimeSinceAggrevated += Time.deltaTime;
        }

        private void AttackBehaviour()
        {
            m_TimeSincePlayerLastSeen = 0;
            m_Fighter.Attack(m_PlayerGO);

            AggrevateNearbyEnemies();
        }

        private void AggrevateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, ShoutDistance, Vector3.up, 0f);
            foreach (RaycastHit hit in hits)
            {
                AIController ai = hit.transform.gameObject.GetComponent<AIController>();

                if (ai != null)
                {
                    ai.Aggrevate();
                }
            }
        }

        private void SuspicionBehaviour()
        {
            m_ActionScheduler.CancelCurrentAction();
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = m_GuardPosition.value;

            if (m_PatrolPath != null)
            {
                if (AtWaypoint())
                {
                    m_TimeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            
            if (m_TimeSinceArrivedAtWaypoint > WaypointDwellTime)
            {
                m_Mover.StartMoveAction(nextPosition, m_PatrolSpeedFraction);
            }
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < m_WaypointTolerance;
        }

        private void CycleWaypoint()
        {
            m_CurrentWaypointIndex = m_PatrolPath.GetNextIndex(m_CurrentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return m_PatrolPath.GetWaypoint(m_CurrentWaypointIndex);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, ChaseDistance);
        }

        private bool IsAggrevated()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, m_PlayerGO.transform.position);
            return distanceToPlayer <= ChaseDistance || m_TimeSinceAggrevated < AggrevatedCooldownTime;
        }
    }
}
