using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        private static float m_WaypointGizmoRadius = 0.3f;

        private void OnDrawGizmos()
        {
            for (int i=0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(GetWaypoint(i), m_WaypointGizmoRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(GetNextIndex(i)));
            }
        }

        private int GetNextIndex(int i)
        {
            if (i + 1 == transform.childCount)
            {
                return 0;
            }
            return i + 1;
        }

        private Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}
