using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;
using System;
using UnityEngine.EventSystems;

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
            foreach (RaycastHit hit in RaycastAllSorted())
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

        private RaycastHit[] RaycastAllSorted()
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
            RaycastHit hit;
            if (Physics.Raycast(GetMouseRay(), out hit))
            {
                if (Input.GetMouseButton(0))
                {
                    m_Mover.StartMoveAction(hit.point, 1f);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
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