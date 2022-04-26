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
        enum CursorType
        {
            None,
            Movement,
            Combat
        }
        
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
            if (InteractWithUI()) return;

            if (m_Health.IsDead)
            {
                SetCursor(CursorType.None);
                return;
            }

            if (InteractWithCombat()) return;

            if (InteractWithMovement()) return;

            SetCursor(CursorType.None);
        }

        private bool InteractWithUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach (RaycastHit hit in hits)
            {
                GameObject hitGO = hit.transform.gameObject;
                if (hitGO.tag == "CombatTarget" && m_Fighter.CanAttack(hitGO))
                {
                    if (Input.GetMouseButton(0))
                    {
                        m_Fighter.Attack(hitGO);
                    }
                    SetCursor(CursorType.Combat);
                    return true;
                }
            }
            return false;
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