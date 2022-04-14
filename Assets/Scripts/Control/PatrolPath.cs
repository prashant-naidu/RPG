using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        private static float m_WaypointGizmoRadius = 1f;

        private void OnDrawGizmos()
        {
            for (int i=0; i < transform.childCount-1; i++)
            {
                Gizmos.DrawSphere(transform.GetChild(i).position, m_WaypointGizmoRadius);
            }
        }
    }
}
