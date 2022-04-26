using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;
using System;
using UnityEngine.EventSystems;
using UnityEngine.AI;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [Serializable]
        struct CursorMapping
        {
            public CursorType Type;
            public Texture2D Texture;
            public Vector2 Hotspot;
        }

        [Header("Cursor Mapping")]
        [SerializeField] private CursorMapping[] m_CursorMappings = null;

        [Header("Dependencies")]
        [SerializeField] private Mover m_Mover;
        [SerializeField] private Fighter m_Fighter;
        [SerializeField] private Health m_Health;

        [Header("Parameters")]
        [SerializeField] private float m_MaxNavMeshProjectionDistance = 1f;
        [SerializeField] private float m_MaxNavPathLength = 40f;

        // Update is called once per frame
        void Update()
        {
            if (InteractWithUI())
            {
                SetCursor(CursorType.UI);
                return;
            }

            if (m_Health.IsDead)
            {
                SetCursor(CursorType.None);
                return;
            }

            if (InteractWithComponent()) return;

            if (InteractWithMovement()) return;

            SetCursor(CursorType.None);
        }

        private bool InteractWithUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        private bool InteractWithComponent()
        {
            foreach (RaycastHit hit in RaycastAllSortedByDistance())
            {
                foreach(IRaycastable raycastable in hit.transform.GetComponents<IRaycastable>())
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        private RaycastHit[] RaycastAllSortedByDistance()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);

            return hits;
        }

        private bool InteractWithMovement()
        {
            //RaycastHit hit;
            //bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            Vector3 targetPosition;
            bool hasHit = RaycastNavMesh(out targetPosition);

            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    m_Mover.StartMoveAction(targetPosition, 1f);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private bool RaycastNavMesh(out Vector3 targetPosition)
        {
            targetPosition = new Vector3();

            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if (!hasHit) return false;

            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out navMeshHit, m_MaxNavMeshProjectionDistance, NavMesh.AllAreas);

            if (!hasCastToNavMesh) return false;

            targetPosition = navMeshHit.position;

            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path);

            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (!IsValidNavPathLength(path)) return false;

            return true;
        }

        private bool IsValidNavPathLength(NavMeshPath path)
        {
            float distanceSum = 0f;

            if (path.corners.Length < 2) return true;

            for (int i=0; i < path.corners.Length - 1; i++)
            {
                distanceSum += Vector3.Distance(path.corners[i], path.corners[i + 1]);
                if (distanceSum > m_MaxNavPathLength)
                {
                    return false;
                }
            }

            return true;
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.Texture, mapping.Hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in m_CursorMappings)
            {
                if (mapping.Type == type)
                {
                    return mapping;
                }
            }
            return m_CursorMappings[0];
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}