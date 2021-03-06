using RPG.Attributes;
using RPG.Core;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [Header("Dependencies")]
        [SerializeField] private ActionScheduler m_ActionScheduler;
        [SerializeField] private NavMeshAgent m_NavMeshAgent;
        [SerializeField] private Animator m_Animator;
        [SerializeField] private Health m_Health;

        [Header("Parameters")]
        [SerializeField] private float m_MaxSpeed = 5.66f;
        [SerializeField] private float m_MaxNavPathLength = 40f;

        void Update()
        {
            m_NavMeshAgent.enabled = !m_Health.IsDead;

            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            m_ActionScheduler.StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            m_NavMeshAgent.destination = destination;
            m_NavMeshAgent.speed = m_MaxSpeed * Mathf.Clamp01(speedFraction);
            m_NavMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            m_NavMeshAgent.isStopped = true;
        }

        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);

            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (!IsValidNavPathLength(path)) return false;

            return true;
        }

        private bool IsValidNavPathLength(NavMeshPath path)
        {
            float distanceSum = 0f;

            if (path.corners.Length < 2) return true;

            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                distanceSum += Vector3.Distance(path.corners[i], path.corners[i + 1]);
                if (distanceSum > m_MaxNavPathLength)
                {
                    return false;
                }
            }

            return true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = m_NavMeshAgent.velocity;
            float speed = transform.InverseTransformDirection(velocity).z;

            m_Animator.SetFloat("forwardSpeed", speed);
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;
            m_NavMeshAgent.enabled = false;

            NavMeshHit closestHit;
            if (NavMesh.SamplePosition(position.GetVector(), out closestHit, 500, 1))
            {
                transform.position = closestHit.position;
            }

            m_NavMeshAgent.enabled = true;
            m_ActionScheduler.CancelCurrentAction();
        }
    }
}